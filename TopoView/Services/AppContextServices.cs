//=============================================================================
// 对齐 TopoData：frmMain.cs 的服务装配与启停编排；
//               WebAPI 随程序启动对应旧 Program.cs 的启动时机
// Description: 应用上下文：配置、采集、数据库、脚本与 WebAPI 的统一持有者。
//=============================================================================
// One owner for profile state, service start/stop, diagnostics and user preferences.
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using auDASLib;

namespace TopoView.Services;

/// <summary>
/// 应用上下文：配置档案、采集、数据库、脚本与 WebAPI 服务的统一持有者与启停编排。
/// </summary>
public sealed class AppContextServices : IAsyncDisposable
{
    private CancellationTokenSource? _running;
    private Task[] _workers = [];
    private readonly SemaphoreSlim _lifecycle = new(1, 1);
    private readonly object _logLock = new();
    private string _savedProfile = "";
    private bool _savedAutoStart;
    private bool _savedRestEnable;
    private readonly List<string> _logEntries = [];
    public event Action? StateChanged;
    public event Action<string>? Message;
    public ConfigurationStore Store { get; }
    public WorkspaceProfile Profile { get; private set; }
    public AcquisitionService Acquisition { get; } = new();
    public DatabaseService Database { get; }
    public ScriptService Scripts { get; }
    public WebApiService WebApi { get; } = new();
    public bool IsRunning => Acquisition.IsRunning;
    public bool IsDemo { get; }
    public string StartupError { get; private set; } = "";
    public string LastMessage { get; private set; } = "服务已停止";
    public IReadOnlyList<string> LogEntries { get { lock (_logLock) return _logEntries.ToArray(); } }
    public bool HasChanges => ProfileContent() != _savedProfile;

    public AppContextServices(string[] arguments)
    {
        IsDemo = arguments.Contains("--demo");
        // Windows keeps TopoData's shared production configuration folder so both applications
        // read and write the same XML files; Linux reads the Cfg folder next to the executable.
        // --demo must stay isolated from production.
        var directory = Environment.GetEnvironmentVariable("TOPOVIEW_CONFIG_DIR") ??
            (IsDemo ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TopoView", "DemoConfig")
                    : DefaultConfigDirectory());
        var index = Array.IndexOf(arguments, "--config-dir");
        if (index >= 0 && index + 1 < arguments.Length) directory = arguments[index + 1];
        Store = new ConfigurationStore(directory);
        try { Profile = IsDemo ? Store.CreateDemo() : Store.Load(); }
        catch (Exception ex) { Profile = Store.CreateEmpty(); StartupError = ex.Message; }
        Database = new DatabaseService(() => Profile);
        // Old TopoData keeps its .py files in Config\PythonScripts (auDAServer PythonScriptOperater);
        // filename-referenced scripts must resolve against the same folder.
        Scripts = new ScriptService(Path.Combine(Store.DirectoryPath, "PythonScripts"));
        var preferences = Path.Combine(Store.DirectoryPath, "preferences.json");
        if (File.Exists(preferences))
            try
            {
                var settings = JsonSerializer.Deserialize<Preferences>(File.ReadAllText(preferences));
                if (!string.IsNullOrWhiteSpace(settings?.PythonExecutable)) Scripts.PythonExecutable = settings.PythonExecutable;
                // Absent preference keeps the legacy default (trust the server certificate).
                Database.TrustServerCertificate = settings?.TrustDatabaseCertificate ?? true;
            }
            catch (Exception ex) { Log("偏好设置读取失败：" + ex.Message); }
        StdLog4.Log4Helper.Sink = Log;
        // Legacy TopoData brought the WebAPI up when the application opened (Program.cs started it as
        // soon as a tag definition file existed); it kept running across start/stop of acquisition.
        if (Profile.Server.RestEnable && Profile.Tags.Count > 0)
            _ = Task.Run(async () =>
            {
                try { await WebApi.StartAsync(Profile, Acquisition, CancellationToken.None, () => Profile); Log("WebAPI 已随程序启动。"); }
                catch (Exception ex) { Log("WebAPI 启动失败：" + ex.Message); }
            });
        _savedProfile = ProfileContent();
        _savedAutoStart = Profile.Server.AutoStart;
        _savedRestEnable = Profile.Server.RestEnable;
    }

    /// <summary>修改配置前的状态检查：采集运行中或原配置读取失败时阻止保存。</summary>
    public void EnsureStopped()
    {
        if (IsRunning) throw new InvalidOperationException("请先停止采集服务，再修改配置。");
        if (!string.IsNullOrEmpty(StartupError)) throw new InvalidOperationException("原配置读取失败。请先导入有效配置，避免覆盖原文件。\n" + StartupError);
    }

    /// <summary>保存配置与偏好，联动登录自启；关闭 REST 时立即释放端口（启用需重启程序）。</summary>
    public void Save()
    {
        EnsureStopped();
        Store.Save(Profile);
        if (_savedAutoStart != Profile.Server.AutoStart)
        {
            DesktopStartupService.SetEnabled(Profile.Server.AutoStart, Store.DirectoryPath);
            _savedAutoStart = Profile.Server.AutoStart;
        }
        Directory.CreateDirectory(Store.DirectoryPath);
        File.WriteAllText(Path.Combine(Store.DirectoryPath, "preferences.json"), JsonSerializer.Serialize(new Preferences(Scripts.PythonExecutable, Database.TrustServerCertificate)));
        // Enabling REST needs a restart (the legacy client also started it once at application open);
        // disabling it takes effect immediately so the port is released.
        if (_savedRestEnable && !Profile.Server.RestEnable)
        {
            _savedRestEnable = false;
            _ = Task.Run(async () =>
            {
                try { await WebApi.DisposeAsync(); Log("WebAPI 已停止。"); }
                catch (Exception ex) { Log("WebAPI 停止失败：" + ex.Message); }
            });
        }
        _savedProfile = ProfileContent();
        Notify("配置已保存。");
        StateChanged?.Invoke();
    }

    /// <summary>导入旧版配置（ZIP 或 Config 目录）并切换运行档案；导入不继承登录自启。</summary>
    public void Import(string path)
    {
        if (IsRunning) throw new InvalidOperationException("请先停止服务。");
        var imported = Store.Import(path);
        if (_savedAutoStart) DesktopStartupService.SetEnabled(false, Store.DirectoryPath);
        _savedAutoStart = false;
        Profile = imported.Profile;
        StartupError = "";
        _savedProfile = ProfileContent();
        Notify(imported.Message);
        StateChanged?.Invoke();
    }

    /// <summary>启动采集：保存配置、校验任务与脚本，以配置快照启动采集、调度、MQTT 与历史清理。</summary>
    public async Task StartAsync()
    {
        await _lifecycle.WaitAsync();
        try
        {
            if (IsRunning) return;
            Save();
            // Runtime receives a detached snapshot so edits cannot mutate a running driver.
            var options = new JsonSerializerOptions { IncludeFields = true };
            var snapshot = JsonSerializer.Deserialize<WorkspaceProfile>(JsonSerializer.Serialize(Profile, options), options)!;
            _running = new CancellationTokenSource();
            var jobs = new RuntimeJobs(Database, Scripts, Log);
            await jobs.InitializeAsync(snapshot, _running.Token);
            await Acquisition.StartAsync(snapshot);
            // The WebAPI normally starts with the application; this idempotent call only catches a
            // server that was not startable at construction (e.g. REST was enabled afterwards).
            await WebApi.StartAsync(snapshot, Acquisition, _running.Token, () => Profile);
            _workers = [ObserveWorker(jobs.RunAsync(snapshot, _running.Token), _running.Token),
                ObserveWorker(new MqttService(Acquisition, Log).RunAsync(snapshot.Mqtt, _running.Token), _running.Token),
                ObserveWorker(Database.RunRetentionLoopAsync(Log, _running.Token), _running.Token)];
            Notify(IsDemo ? "离线仿真服务已启动。" : "采集服务已启动；连接状态请查看参数诊断。");
            StateChanged?.Invoke();
        }
        catch
        {
            if (_running != null) await _running.CancelAsync();
            await Task.WhenAll(_workers);
            _workers = [];
            await Acquisition.StopAsync();
            _running?.Dispose();
            _running = null;
            StateChanged?.Invoke();
            throw;
        }
        finally { _lifecycle.Release(); }
    }

    /// <summary>停止采集与后台任务；WebAPI 保持监听（与旧版一致）。</summary>
    public async Task StopAsync()
    {
        await _lifecycle.WaitAsync();
        try
        {
            if (_running != null) await _running.CancelAsync();
            // Like the legacy desktop client, stopping acquisition leaves the WebAPI online.
            await Task.WhenAll(_workers);
            _workers = [];
            await Acquisition.StopAsync();
            _running?.Dispose();
            _running = null;
            Notify("采集服务已停止。");
            StateChanged?.Invoke();
        }
        finally { _lifecycle.Release(); }
    }

    /// <summary>记录并向界面推送一条消息。</summary>
    public void Notify(string message) { Log(message); Message?.Invoke(message); }

    /// <summary>记录运行日志：诊断页显示；启用系统日志时同时写入配置目录 Logs 下的日志文件。</summary>
    public void Log(string message)
    {
        var line = $"{DateTime.Now:HH:mm:ss}  {message}";
        lock (_logLock)
        {
            LastMessage = message;
            _logEntries.Add(line);
            if (_logEntries.Count > 500) _logEntries.RemoveAt(0);
            if (Profile?.Server.EnableLog4 == true)
            {
                try
                {
                    var path = Path.Combine(Store.DirectoryPath, "Logs");
                    Directory.CreateDirectory(path);
                    File.AppendAllText(Path.Combine(path, $"topoview-{DateTime.Now:yyyyMMdd}.log"), line + Environment.NewLine);
                }
                catch (IOException) { /* Diagnostics must not terminate driver threads. */ }
                catch (UnauthorizedAccessException) { /* The visible log remains available. */ }
            }
        }
    }

    private async Task ObserveWorker(Task task, CancellationToken token)
    {
        try { await task; }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { }
        catch (Exception ex) { Notify("后台任务失败：" + ex.Message); }
    }

    private string ProfileContent()
    {
        var content = JsonSerializer.SerializeToNode(new { Profile, Scripts.PythonExecutable, Database.TrustServerCertificate }, new JsonSerializerOptions { IncludeFields = true });
        static void RemoveTimestamps(JsonNode? node)
        {
            if (node is JsonObject obj)
            {
                obj.Remove("last_modify_time");
                foreach (var entry in obj) RemoveTimestamps(entry.Value);
            }
            else if (node is JsonArray array) foreach (var item in array) RemoveTimestamps(item);
        }
        RemoveTimestamps(content);
        return content!.ToJsonString();
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        // The WebAPI no longer stops with acquisition, so the application must release it here.
        await WebApi.DisposeAsync();
        await Acquisition.DisposeAsync();
        _lifecycle.Dispose();
        StdLog4.Log4Helper.Sink = null;
    }

    private sealed record Preferences(string? PythonExecutable = null, bool? TrustDatabaseCertificate = null);

    /// <summary>Windows 沿用 TopoData 的共享生产配置目录；Linux 在应用可执行文件同级 Cfg 文件夹读取。</summary>
    private static string DefaultConfigDirectory() =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? auDASLib.pubDefine.Folder
            : Path.Combine(AppContext.BaseDirectory, "Cfg");
}
