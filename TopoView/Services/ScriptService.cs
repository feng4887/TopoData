//=============================================================================
// 对齐 TopoData：Public/PythonScriptOperater.cs（PythonScriptOperater、PythonScriptJob）
//               与 Public/PythonScriptExecutor.cs
// Description: Python 脚本执行服务，保持旧版落盘、触发环境变量与执行语义。
//=============================================================================
// Runs Python exactly like TopoData's PythonScriptJob: resolved source written to a .py file,
// HITOPO_* trigger variables in the environment and no implicit execution deadline.
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Flee.PublicTypes;
using auDASLib;
using auDAServer;

namespace TopoView.Services;

/// <summary>
/// Python 脚本执行服务：对应旧版 PythonScriptOperater/PythonScriptJob 的
/// 脚本落盘、触发上下文注入与进程执行。
/// </summary>
public sealed class ScriptService(string workingDirectory)
{
    /// <summary>触发上下文：触发名称、触发值与触发时间，注入为 HITOPO_* 环境变量。</summary>
    public sealed record TriggerContext(string Name, string Value, DateTime Time);

    private const string DefaultWindowsExecutable = "python.exe";
    private const string DefaultUnixExecutable = "python3";

    /// <summary>The interpreter selected in system settings; null keeps the legacy resolution chain.</summary>
    public string? PythonExecutable { get; set; }
    /// <summary>Legacy runtime had no deadline; only an explicit value terminates runaway scripts.</summary>
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// 解析脚本内容并写入配置目录 PythonScripts 下的 .py 文件，随后以外部解释器执行；
    /// 标准输出与标准错误合并返回，退出码非 0 视为失败。
    /// </summary>
    public async Task<string> RunAsync(PythonScriptItem script, CancellationToken token = default, TriggerContext? trigger = null)
    {
        if (string.IsNullOrWhiteSpace(script.ScriptText)) throw new InvalidOperationException("脚本内容不能为空。");
        var source = await ReadScriptContentAsync(script.ScriptText, token);
        if (!PythonScriptReadResolver.TryResolve(source, out var resolved, out var error))
            throw new InvalidOperationException(error);
        Directory.CreateDirectory(workingDirectory);
        var scriptPath = Path.Combine(workingDirectory, GetSafeScriptName(script.ScriptName) + ".py");
        await File.WriteAllTextAsync(scriptPath, resolved, new UTF8Encoding(false), token);
        var start = new ProcessStartInfo(ResolvePythonExecutable())
        {
            WorkingDirectory = workingDirectory, UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8, StandardErrorEncoding = Encoding.UTF8
        };
        // -X utf8 makes the child emit UTF-8 so the redirected decoders above stay correct on Windows.
        start.ArgumentList.Add("-u");
        start.ArgumentList.Add("-X");
        start.ArgumentList.Add("utf8");
        start.ArgumentList.Add(scriptPath);
        start.Environment["HITOPO_SCRIPT_NAME"] = script.ScriptName ?? "";
        start.Environment["HITOPO_TRIGGER_NAME"] = trigger?.Name ?? "";
        start.Environment["HITOPO_TRIGGER_VALUE"] = trigger?.Value ?? "";
        start.Environment["HITOPO_TRIGGER_TIME"] = trigger is { Time: var time } && time != DateTime.MinValue
            ? time.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
            : "";
        using var process = new Process { StartInfo = start };
        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(token);
        if (Timeout is { } limit) deadline.CancelAfter(limit);
        if (!process.Start()) throw new IOException("无法启动 Python 进程。");
        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        try
        {
            await process.WaitForExitAsync(deadline.Token);
            var output = await outputTask;
            var errors = await errorTask;
            if (process.ExitCode != 0) throw new InvalidOperationException($"Python 退出码 {process.ExitCode}\n{errors}");
            return output + (string.IsNullOrWhiteSpace(errors) ? "" : "\n" + errors);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited) process.Kill(true);
            await process.WaitForExitAsync(CancellationToken.None);
            if (!token.IsCancellationRequested && Timeout is { } cancelledLimit)
                throw new TimeoutException($"Python 执行超过 {cancelledLimit.TotalSeconds:0} 秒，已终止。");
            throw;
        }
    }

    /// <summary>Mirrors PythonScriptOperater.ResolvePythonExecutablePath: preference, then
    /// HITOPO_PYTHON_EXE, then a bundled python folder, then the operating system PATH.</summary>
    private string ResolvePythonExecutable()
    {
        if (!string.IsNullOrWhiteSpace(PythonExecutable)) return PythonExecutable;
        var environment = Environment.GetEnvironmentVariable("HITOPO_PYTHON_EXE");
        if (!string.IsNullOrWhiteSpace(environment)) return environment.Trim('"');
        var bundled = Path.Combine(AppContext.BaseDirectory, "python", "python.exe");
        if (File.Exists(bundled)) return bundled;
        return OperatingSystem.IsWindows() ? DefaultWindowsExecutable : DefaultUnixExecutable;
    }

    /// <summary>Mirrors PythonScriptJob.ReadScriptContent/ResolveScriptPath: a single-line ".py"
    /// reference loads that file, resolved against the app directory and the configuration root.</summary>
    private async Task<string> ReadScriptContentAsync(string scriptText, CancellationToken token)
    {
        var text = scriptText.Trim();
        var candidate = text.Trim('"');
        if (!text.Contains('\r') && !text.Contains('\n') && candidate.EndsWith(".py", StringComparison.OrdinalIgnoreCase))
        {
            var root = Directory.GetParent(workingDirectory)?.FullName;
            foreach (var directory in new[] { AppContext.BaseDirectory, workingDirectory, root })
            {
                if (string.IsNullOrEmpty(directory)) continue;
                var path = Path.IsPathRooted(candidate) ? candidate : Path.Combine(directory, candidate);
                if (File.Exists(path)) return await File.ReadAllTextAsync(path, Encoding.UTF8, token);
            }
        }
        return scriptText;
    }

    private static string GetSafeScriptName(string? scriptName)
    {
        var name = string.IsNullOrWhiteSpace(scriptName) ? "PythonScript" : scriptName.Trim();
        foreach (var item in Path.GetInvalidFileNameChars()) name = name.Replace(item, '_');
        return name;
    }

    /// <summary>Legacy trigger semantics: a missing or bad-quality point simply does not trigger,
    /// and any value is converted to bool like PythonScriptJob.ConvertToBool.</summary>
    public static bool ReadCondition(string tagId) => ReadCondition(tagId, out _);

    public static bool ReadCondition(string tagId, out string? valueText)
    {
        valueText = null;
        if (string.IsNullOrWhiteSpace(tagId)) return false;
        if (!ItemPool.ValuePool.TryGetValue(tagId, out var value) || value?.Quality != 192) return false;
        valueText = Convert.ToString(value.RealValue, CultureInfo.InvariantCulture) ?? "";
        return ConvertToBool(value.RealValue);
    }

    private static bool ConvertToBool(object? value)
    {
        if (value == null) return false;
        if (value is bool boolValue) return boolValue;
        if (value is string stringValue)
        {
            if (bool.TryParse(stringValue, out var parsed)) return parsed;
            return double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var number) &&
                Math.Abs(number) > double.Epsilon;
        }
        if (value is IConvertible)
            try { return Math.Abs(Convert.ToDouble(value, CultureInfo.InvariantCulture)) > double.Epsilon; }
            catch (FormatException) { return false; }
        return false;
    }

    /// <summary>校验触发表达式：绑定别名必须解析到已配置点位，表达式必须可编译为布尔值。</summary>
    public static void ValidateExpression(string expression, IEnumerable<DBTag> bindings, IReadOnlyList<DBTag> tags)
    {
        if (string.IsNullOrWhiteSpace(expression)) throw new InvalidDataException("触发表达式不能为空。");
        var context = new ExpressionContext();
        foreach (var binding in bindings)
        {
            var id = BindingReference(binding);
            var tag = tags.FirstOrDefault(t => t.TagId == id)
                ?? throw new InvalidDataException($"表达式引用点位不存在：{id}");
            context.Variables.Add(binding.TagName, AcquisitionService.ParseValue(tag.DataType, tag.DataType is 1 or 2 ? "" : tag.DataType == 0 ? "false" : "0"));
        }
        context.CompileGeneric<bool>(expression);
    }

    /// <summary>diaExpress stored the tag reference in Address and the alias in TagName; older
    /// configurations must keep resolving through that field.</summary>
    public static string BindingReference(DBTag binding) =>
        !string.IsNullOrWhiteSpace(binding.Address) ? binding.Address
        : ConfigurationStore.ResolveTagId(binding.TagId, binding.TagName, binding.CannelID);

    /// <summary>Legacy evaluation feeds 0 for missing or bad-quality points instead of failing.</summary>
    public static bool Evaluate(string expression, IEnumerable<DBTag> bindings)
    {
        if (string.IsNullOrWhiteSpace(expression)) throw new InvalidOperationException("表达式不能为空。");
        var context = new ExpressionContext();
        foreach (var binding in bindings)
        {
            var key = BindingReference(binding);
            context.Variables.Add(binding.TagName, ItemPool.ValuePool.TryGetValue(key, out var value) && value?.Quality == 192
                ? (object)value.RealValue!
                : 0);
        }
        return context.CompileGeneric<bool>(expression).Evaluate();
    }
}
