// Offline integration checks. No production profile or physical device is accessed.
using System.Net;
using System.Data;
using System.Globalization;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Xml.Serialization;
using System.IO.Compression;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Controls;
using auDASLib;
using auDAServer;
using Opc.Ua;
using TopoView;
using TopoView.Services;
using TopoView.Views;

internal static class Program
{
    private static readonly string Root = GetTestDirectory();
    private static int _passed;

    private static string GetTestDirectory()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "TopoView.csproj")))
            directory = directory.Parent;
        if (directory == null) throw new InvalidOperationException("TopoView project directory was not found.");
        return Path.Combine(directory.FullName, "artifacts", "tests");
    }

    [STAThread]
    public static int Main(string[] arguments)
    {
        Directory.CreateDirectory(Root);
        try
        {
            if (!arguments.Contains("--ui-only"))
                RunBusinessChecksAsync(arguments.FirstOrDefault()).GetAwaiter().GetResult();
            RunLegacySchemaChecksAsync().GetAwaiter().GetResult();
            RunUiChecks();
            Console.WriteLine($"PASS: {_passed} checks. Artifacts: {Root}");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException("FAIL: " + message);
        Console.WriteLine("PASS: " + message);
        _passed++;
    }

    private static async Task ExpectAsync<T>(Func<Task> action, string message) where T : Exception
    {
        try { await action(); }
        catch (T) { Check(true, message); return; }
        throw new InvalidOperationException("FAIL: " + message);
    }

    private static async Task RunBusinessChecksAsync(string? python)
    {
        // The acquisition engines and models must come from the shared assemblies (Runtime/hiServer.dll,
        // hiLib, Log4), never from sources compiled into TopoView; this guards that integration.
        Check(typeof(ItemPool).Assembly.GetName().Name == "hiServer" && typeof(ItemPool).Assembly.Location.EndsWith("hiServer.dll", StringComparison.OrdinalIgnoreCase),
            "Acquisition engines resolve from the shared hiServer assembly");
        Check(typeof(DBTag).Assembly.GetName().Name == "hiLib" && typeof(StdLog4.Log4Helper).Assembly.GetName().Name == "Log4",
            "Models and logging resolve from the shared hiLib / Log4 assemblies");
        var session = Path.Combine(Root, Guid.NewGuid().ToString("N"));
        var store = new ConfigurationStore(session);
        var profile = store.CreateDemo();
        store.Save(profile);
        var loaded = store.Load();
        Check(loaded.Tags.Count == 4 && loaded.Server.cDevices.Single().IsSimulator, "Legacy XML round trip preserves devices and tags");
        Check(loaded.Tags.All(t => t.TagId == t.CannelID + "." + t.TagName), "Canonical tag identifiers are retained");
        var excel = Path.Combine(session, "tags.xlsx");
        TagExchangeService.WriteTags(excel, loaded.Tags);
        var importedTags = TagExchangeService.ReadTags(excel, "Reactor01");
        Check(importedTags.Count == 4 && importedTags[0].DataType == loaded.Tags[0].DataType, "Excel tag import/export preserves types");
        var zip = Path.Combine(session, "profile.zip");
        store.ExportZip(profile, zip);
        store.ExportZip(profile, zip);
        Check(File.Exists(zip), "Export safely replaces an explicitly selected archive");
        var destination = new ConfigurationStore(Path.Combine(session, "imported"));
        var imported = destination.Import(zip);
        Check(imported.Profile.Tags.Count == 4, "Whitelisted ZIP import is compatible with TopoData configuration");

        var legacy = Path.Combine(session, "legacy");
        Directory.CreateDirectory(legacy);
        profile.Server.cDevices.Add(new ConnctDevice { CannelID = "LegacyDA", devType = DeviceType.OPCDA });
        profile.Tags.Add(new DBTag { CannelID = "LegacyDA", TagName = "DA", TagId = "LegacyDA.DA", Address = "DA", DataType = 10 });
        using (var writer = File.Create(Path.Combine(legacy, "hiTopoCfg.xml")))
            new XmlSerializer(typeof(ServerCfg)).Serialize(writer, profile.Server);
        using (var writer = File.Create(Path.Combine(legacy, "hiTopoTagDef.xml")))
            new XmlSerializer(typeof(List<DBTag>)).Serialize(writer, profile.Tags);
        var filtered = destination.Import(legacy);
        Check(filtered.Profile.Server.cDevices.Count == 1 && filtered.Profile.Tags.Count == 4 && filtered.Message.Contains("LegacyDA"), "OPC DA is excluded with a visible import report");
        Check(File.ReadAllText(Path.Combine(legacy, "hiTopoCfg.xml")).Contains("LegacyDA"), "Import leaves the source configuration untouched");
        Check(!filtered.Profile.Server.AutoStart, "Import never installs a login startup entry");
        var brokenDirectory = Path.Combine(session, "broken");
        Directory.CreateDirectory(brokenDirectory);
        var brokenFile = Path.Combine(brokenDirectory, "hiTopoCfg.xml");
        File.WriteAllText(brokenFile, "<broken>");
        await using (var broken = new AppContextServices(["--config-dir", brokenDirectory]))
        {
            await ExpectAsync<InvalidOperationException>(() => { broken.Save(); return Task.CompletedTask; }, "Malformed profiles block saving instead of overwriting existing XML");
            Check(File.ReadAllText(brokenFile) == "<broken>", "Malformed source XML is preserved");
        }
        // SQL Server connections must trust the server certificate like the legacy clients and the
        // shared engine's own connection string; strict validation stays available via preferences.
        var legacySql = store.CreateDemo();
        legacySql.Server.SqlConfigInfo.DbType = "Microsoft SQL Server";
        Check(new DatabaseService(() => legacySql).CreateConnection().ConnectionString.Contains("Trust Server Certificate=True", StringComparison.OrdinalIgnoreCase),
            "SQL Server connections trust the server certificate by default like the legacy clients");
        var preferenceDirectory = Path.Combine(session, "preferences");
        Directory.CreateDirectory(preferenceDirectory);
        File.WriteAllText(Path.Combine(preferenceDirectory, "preferences.json"), "{\"TrustDatabaseCertificate\":false}");
        await using (var preferences = new AppContextServices(["--config-dir", preferenceDirectory]))
            Check(!preferences.Database.TrustServerCertificate, "An explicit certificate preference overrides the legacy default");
        var duplicateZip = Path.Combine(session, "duplicate.zip");        using (var archive = ZipFile.Open(duplicateZip, ZipArchiveMode.Create))
        {
            archive.CreateEntryFromFile(Path.Combine(legacy, "hiTopoCfg.xml"), "a/hiTopoCfg.xml");
            archive.CreateEntryFromFile(Path.Combine(legacy, "hiTopoCfg.xml"), "b/hiTopoCfg.xml");
        }
        await ExpectAsync<InvalidDataException>(() => { destination.Import(duplicateZip); return Task.CompletedTask; }, "Ambiguous duplicate XML entries are rejected");

        await using var context = new AppContextServices(["--config-dir", Path.Combine(session, "runtime"), "--demo"]);
        var sample = new DataSample
        {
            TaskName = "Cycle", TableName = "ProcessData", EquipmentName = "Reactor01", bActive = true,
            Samples =
            [
                new DSTbDefine { Column = "Temperature", DataType = "float", TagID = "Reactor01.Temperature", TagName = "Temperature", CannelID = "Reactor01", UOM = "°C" },
                // Legacy TopoData columns store the full tag id in TagName with TagID and CannelID empty.
                new DSTbDefine { Column = "Pressure", DataType = "float", TagName = "Reactor01.Pressure" }
            ],
            SampleTrig = new DataSampleTrig { Type = 1, dataSampleTrig_TimeWindow = new DataSampleTrig_TimeWindow { TimeWindow = 400, TagName = "" } }
        };
        context.Profile.Server.dataSamples.Add(sample);
        await context.StartAsync();
        Check(context.IsRunning && context.Acquisition.Values.Count() == 4, "Simulator starts without connecting to hardware");
        var written = await context.Acquisition.WriteAsync([("Reactor01.Temperature", "42.5")]);
        Check(written == 1 && Convert.ToDouble(ItemPool.ValuePool["Reactor01.Temperature"].RealValue) == 42.5, "Typed writes update the correct simulator point");
        await ExpectAsync<FormatException>(() => context.Acquisition.WriteAsync([("Reactor01.Temperature", "invalid")]), "Invalid typed writes are rejected");
        ItemPool.TagPool["Reactor01.Pressure"].RW = 1;
        await ExpectAsync<InvalidOperationException>(() => context.Acquisition.WriteAsync([("Reactor01.Pressure", "3")]), "Read-only points reject writes");
        Check(ScriptService.Evaluate("temp > 40", [new DBTag { TagName = "temp", TagId = "Reactor01.Temperature" }]), "Flee expressions resolve realtime aliases");
        // diaExpress stored the alias in TagName and the full tag reference in Address.
        Check(ScriptService.Evaluate("temp > 40", [new DBTag { TagName = "temp", Address = "Reactor01.Temperature" }]),
            "Legacy expression bindings resolve through their stored Address");
        ScriptService.ValidateExpression("temp > 40", [new DBTag { TagName = "temp", Address = "Reactor01.Temperature" }], loaded.Tags);
        Check(true, "Legacy expression bindings validate against configured tags");
        await Task.Delay(1100);
        var history = await context.Database.QueryAsync("ProcessData", DateTime.Today.AddDays(-1), DateTime.Now.AddMinutes(1), "");
        Check(history.Rows.Count > 0 && history.Columns.Contains("Temperature") && history.Columns.Contains("Pressure"),
            "Cyclic sampling persists real values to SQLite");
        Check(Math.Abs(Convert.ToDouble(history.Rows[0]["Pressure"]) - 1.2) < 0.0001, "Legacy column references resolve to the same realtime tags at runtime");
        Check(ConfigurationStore.ResolveTagId("Reactor01.Pressure", "Pressure", "Reactor01") == "Reactor01.Pressure" &&
              ConfigurationStore.ResolveTagId(null, "Reactor01.Pressure", null) == "Reactor01.Pressure" &&
              ConfigurationStore.ResolveTagId("", "Pressure", "Reactor01") == "Reactor01.Pressure",
            "Tag references resolve for both TopoView and legacy TopoData column formats");
        TagExchangeService.WriteTable(Path.Combine(session, "history.xlsx"), history);
        Check(File.Exists(Path.Combine(session, "history.xlsx")), "Historical query results export to Excel");
        await ExpectAsync<InvalidDataException>(() => context.Database.QueryAsync("x; DROP TABLE ProcessData", DateTime.Today, DateTime.Now, ""), "SQL identifiers cannot inject statements");

        var endpoint = new TcpListener(IPAddress.Loopback, 0);
        endpoint.Start();
        var port = ((IPEndPoint)endpoint.LocalEndpoint).Port;
        endpoint.Stop();
        var apiProfile = store.CreateDemo();
        apiProfile.Server.RestEnable = true;
        apiProfile.Server.RestUri1 = $"http://127.0.0.1:{port}";
        await using (var api = new WebApiService())
        {
            await api.StartAsync(apiProfile, context.Acquisition, CancellationToken.None);
            using var http = new HttpClient { BaseAddress = new Uri(apiProfile.Server.RestUri1) };
            var response = await http.PostAsJsonAsync("/api/Data/GetRealtimeValues", new[] { "Reactor01.Temperature" });
            Check(response.IsSuccessStatusCode && (await response.Content.ReadAsStringAsync()).Contains("42.5"), "Existing realtime HTTP route returns simulator data");
            var writeResponse = await http.PostAsJsonAsync("/api/Data/WriteRealtimeValues", new[] { new { CannelID = "Reactor01", ValueName = "Reactor01.Temperature", value = 43.25 } });
            Check(writeResponse.IsSuccessStatusCode && Convert.ToDouble(ItemPool.ValuePool["Reactor01.Temperature"].RealValue) == 43.25, "Existing HTTP write route accepts JSON values");
            var recipeResponse = await http.PostAsJsonAsync("/api/Recipe/DownloadRecipe", "演示配方");
            Check(recipeResponse.IsSuccessStatusCode && Convert.ToDouble(ItemPool.ValuePool["Reactor01.Temperature"].RealValue) == 35, "Recipe HTTP route validates and writes a parameter batch");
            var status = await http.GetStringAsync("/api/Status/GetServerStatus");
            Check(status.Contains("appStartTime", StringComparison.OrdinalIgnoreCase), "Legacy server status route remains available");
        }
        if (!string.IsNullOrWhiteSpace(python))
        {
            context.Scripts.PythonExecutable = python;
            var output = await context.Scripts.RunAsync(new PythonScriptItem { ScriptText = "print(Read[Reactor01.Temperature])" });
            Check(output.Contains("35"), "Python reads resolve the current realtime snapshot");
            // PythonScriptJob wrote the resolved source to a .py file and exported HITOPO_* trigger data.
            var envOutput = await context.Scripts.RunAsync(new PythonScriptItem { ScriptName = "EnvScript",
                ScriptText = "import os;print(__file__);print(os.environ.get('HITOPO_TRIGGER_NAME',''))" },
                trigger: new ScriptService.TriggerContext("Reactor01.Running", "true", DateTime.Now));
            Check(envOutput.Contains(".py") && envOutput.Contains("Reactor01.Running"),
                "Python runs from a script file with the legacy HITOPO_* trigger environment");
            context.Scripts.Timeout = TimeSpan.FromMilliseconds(250);
            await ExpectAsync<TimeoutException>(() => context.Scripts.RunAsync(new PythonScriptItem { ScriptText = "import time; time.sleep(10)" }), "An explicit Python timeout terminates the child process");
            // The legacy runtime had no implicit deadline; clearing the preference restores that.
            context.Scripts.Timeout = null;
        }
        var series = new DataSample { TaskName = "Series", TableName = "Unused", bSave2ProcessTable = true, wo = "Reactor01.Batch",
            Samples = [new DSTbDefine { Column = "Running", TagID = "Reactor01.Running", TagName = "Running", CannelID = "Reactor01", DataType = "bool" }] };
        await context.Database.CreateTableAsync(series);
        await context.Acquisition.WriteAsync([("Reactor01.Running", "true")]);
        await context.Database.StoreSampleAsync(series, CancellationToken.None);
        var seriesRows = await context.Database.QueryAsync("tb_au_pubhisvalues", DateTime.Today, DateTime.Now.AddMinutes(1), "Reactor01.Running");
        Check(seriesRows.Rows.Count == 1 && Convert.ToBoolean(seriesRows.Rows[0]["bvalue"]) &&
            Convert.ToString(seriesRows.Rows[0]["tagn"]) == "Running" && Convert.ToString(seriesRows.Rows[0]["wo_no"]) == "DEMO-001",
            "Time-series sampling preserves boolean values, the configured column name and the work order");
        await using (var connection = context.Database.CreateConnection())
        {
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM tb_au_workorder WHERE wo_no = 'DEMO-001'";
            Check(Convert.ToInt64(await command.ExecuteScalarAsync()) == 1, "Stored samples maintain the legacy work-order table");
        }
        await context.StopAsync();
        Check(!context.IsRunning && context.Acquisition.Values.All(v => v.Quality == 0), "Stopping marks realtime values bad and cancels workers");
        await context.StartAsync();
        Check(context.IsRunning && context.Acquisition.Values.Count() == 4, "Stop/start reuses neither old workers nor stale point collections");
        await context.StopAsync();
        sample.SampleTrig.Type = 0;
        sample.SampleTrig.dataSampleTrig_OnTrue.TagName = "Reactor01.Running";
        var baseline = (await context.Database.QueryAsync("ProcessData", DateTime.Today, DateTime.Now.AddMinutes(1), "")).Rows.Count;
        if (!string.IsNullOrWhiteSpace(python))
            context.Profile.Scripts.Scripts.Add(new PythonScriptItem { ScriptName = "EdgeScript", bActive = true,
                ScriptText = "print('EDGE_SCRIPT_OK')", Trigger = new PythonScriptTrigger { TagName = "Reactor01.Running" } });
        await context.StartAsync();
        await Task.Delay(350);
        await context.Acquisition.WriteAsync([("Reactor01.Running", "true")]);
        await Task.Delay(800);
        var firstEdge = (await context.Database.QueryAsync("ProcessData", DateTime.Today, DateTime.Now.AddMinutes(1), "")).Rows.Count;
        Check(firstEdge == baseline + 1, "Rising-edge storage captures exactly one row while the trigger stays true");
        if (!string.IsNullOrWhiteSpace(python)) Check(context.LogEntries.Any(l => l.Contains("EDGE_SCRIPT_OK")), "Boolean trigger starts the configured Python script");
        await context.Acquisition.WriteAsync([("Reactor01.Running", "false")]);
        await Task.Delay(350);
        await context.Acquisition.WriteAsync([("Reactor01.Running", "true")]);
        await Task.Delay(600);
        Check((await context.Database.QueryAsync("ProcessData", DateTime.Today, DateTime.Now.AddMinutes(1), "")).Rows.Count == baseline + 2, "A second rising edge creates exactly one additional row");
        ItemPool.ValuePool["Reactor01.Running"].Quality = 0;
        Check(!ScriptService.ReadCondition("Reactor01.Running"), "Bad trigger quality is not interpreted as a trigger or a falling edge");
        await context.StopAsync();
        context.Profile.Scripts.Scripts.Clear();
        sample.SampleTrig.dataSampleTrig_OnTrue.TagName = "Missing.Trigger";
        await ExpectAsync<InvalidDataException>(context.StartAsync, "Invalid active trigger blocks startup before drivers run");
        Check(!context.IsRunning, "Failed startup rolls back runtime state");
        sample.SampleTrig.dataSampleTrig_OnTrue.TagName = "Reactor01.Running";
        await context.StartAsync();
        Check(context.IsRunning, "Runtime starts successfully after correcting invalid configuration");
        await context.StopAsync();
        Check(!Directory.EnumerateFiles(session, "*", SearchOption.AllDirectories).Any(p => p.Contains("OpcDa", StringComparison.OrdinalIgnoreCase)), "Offline checks create no OPC DA configuration");
        await RunModbusChecksAsync(store);
        // The shared engine layer (Runtime/hiServer.dll) auto-accepts untrusted server certificates
        // like the legacy clients; the shipped client configuration must keep that behaviour and the
        // SDK must complete validation for a self-signed server without any extra handler.
        var ua = new UAClientHelperAPI();
        static ApplicationConfiguration ClientConfigOf(UAClientHelperAPI helper) =>
            (ApplicationConfiguration)typeof(UAClientHelperAPI).GetField("mApplicationConfig", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(helper)!;
        static void RemoveFromUserRootStore(string thumbprint)
        {
            try
            {
                using var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                foreach (var cert in store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false)) store.Remove(cert);
            }
            catch (Exception) { /* cleanup only; the acceptance check already ran */ }
        }
        var uaConfig = ClientConfigOf(ua);
        Check(uaConfig.SecurityConfiguration.AutoAcceptUntrustedCertificates, "OPC UA client auto-accepts untrusted server certificates");
        using (var key = RSA.Create(2048))
        {
            var request = new CertificateRequest(new X500DistinguishedName("CN=TopoTestOpcServer"), key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation |
                X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DataEncipherment, false));
            request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
                [new Oid("1.3.6.1.5.5.7.3.1"), new Oid("1.3.6.1.5.5.7.3.2")], false));
            var subjectNames = new SubjectAlternativeNameBuilder();
            subjectNames.AddUri(new Uri("urn:topotest:opcserver"));
            request.CertificateExtensions.Add(subjectNames.Build());
            using var serverCertificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
            await uaConfig.CertificateValidator.Update(uaConfig);
            Exception? validationFailure = null;
            try { uaConfig.CertificateValidator.Validate(serverCertificate); }
            catch (Exception ex) { validationFailure = ex; }
            RemoveFromUserRootStore(serverCertificate.Thumbprint);
            Check(validationFailure == null, "Self-signed OPC UA server certificate passes validation without user action: " + validationFailure?.Message);
        }
        ua.Disconnect();
        ua.Disconnect();
        Check(true, "OPC UA disconnect is safe before any connection");
    }

    private static async Task RunModbusChecksAsync(ConfigurationStore store)
    {
        await using var server = new LoopbackModbusServer();
        var profile = store.CreateEmpty();
        profile.Server.cDevices.Add(new ConnctDevice { CannelID = "Loopback", devType = DeviceType.ModbusTCP, IsSimulator = false,
            IpCfg = new IpCfg { Host = "127.0.0.1", Port = server.Port, Rack = 1, Slot = 1 }, SampleInterval = 100 });
        profile.Tags.Add(new DBTag { CannelID = "Loopback", TagName = "Register", Address = "40001", DataType = 5 });
        ConfigurationStore.Normalize(profile);
        await using var acquisition = new AcquisitionService();
        await acquisition.StartAsync(profile);
        async Task WaitForValue(int expected)
        {
            using var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(6));
            while (!ItemPool.ValuePool.TryGetValue("Loopback.Register", out var value) || value.Quality != 192 || Convert.ToInt32(value.RealValue) != expected)
                await Task.Delay(50, deadline.Token);
        }
        await WaitForValue(123);
        Check(acquisition.IsConnected("Loopback"), "Managed Modbus TCP driver reads a real loopback protocol response");
        await acquisition.WriteAsync([("Loopback.Register", "456")]);
        await WaitForValue(456);
        Check(server.Value == 456, "Modbus write reaches the server and is confirmed by readback");
        await acquisition.StopAsync();
        await acquisition.StartAsync(profile);
        await WaitForValue(456);
        Check(acquisition.IsConnected("Loopback"), "Managed Modbus TCP driver restarts and reconnects cleanly");
        await acquisition.StopAsync();
    }

    private static void RunUiChecks()
    {
        AppBuilder.Configure<App>().UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false })
            .UseSkia().WithInterFont().SetupWithoutStarting();
        UiParityChecks.Run(Check);
        RunDesignerChecks();
        RunHistoryColumnChecks();
        RunDialogChecks();
        var context = new AppContextServices(["--config-dir", Path.Combine(Root, "ui"), "--demo"]);
        // 诊断页渲染前注入仿真实时值，使截图与真实运行状态一致（含 Quality 红绿灯）；
        // ImportCannelTags 仅填充共享点表字典，不启动任何采集服务。
        auDASLib.DataImport.ImportCannelTags(context.Profile.Tags);
        var realtimeStamp = DateTime.Now;
        auDAServer.ItemPool.ValuePool["Reactor01.Temperature"] = new auDASLib.RealTimeValue
            { ValueName = "Reactor01.Temperature", RealValue = 25.6d, Quality = 192, Timestamp = realtimeStamp, uom = "°C" };
        auDAServer.ItemPool.ValuePool["Reactor01.Pressure"] = new auDASLib.RealTimeValue
            { ValueName = "Reactor01.Pressure", RealValue = 1.23d, Quality = 64, Timestamp = realtimeStamp, uom = "bar" };
        auDAServer.ItemPool.ValuePool["Reactor01.Running"] = new auDASLib.RealTimeValue
            { ValueName = "Reactor01.Running", RealValue = false, Quality = 0, Timestamp = realtimeStamp, uom = "" };
        auDAServer.ItemPool.ValuePool["Reactor01.Batch"] = new auDASLib.RealTimeValue
            { ValueName = "Reactor01.Batch", RealValue = "DEMO-001", Quality = 192, Timestamp = realtimeStamp, uom = "" };
        var window = new frmMain(context);
            window.Show();
        foreach (var page in new[] { "devices", "storage", "recipes", "scripts", "diagnostics", "history", "settings", "about" })
        {
            window.ShowPage(page);
            window.UpdateLayout();
            Dispatcher.UIThread.RunJobs();
            Thread.Sleep(220);
            AvaloniaHeadlessPlatform.ForceRenderTimerTick();
            using var frame = window.CaptureRenderedFrame();
            if (frame == null) throw new InvalidOperationException("No rendered frame for " + page);
            frame.Save(Path.Combine(Root, "topoview-" + page + ".png"));
            Check(frame.PixelSize.Width >= 1000, "Native Avalonia page renders: " + page);
        }
        Check(!context.IsRunning, "Opening and navigating the UI does not start acquisition");
        RunEditorEventChecks(context);
        // Regression: the discover click used to read the endpoint TextBox inside Task.Run, which raises
        // an Avalonia cross-thread exception before any network work starts. An empty address must instead
        // fail as a URI error in the worker thread, and no control may be touched off the UI thread.
        var devices = new ucCannelCfg(context);
        var endpointBox = devices.FindControl<ucDriverOPCUA>("ucOPCUA")!.FindControl<TextBox>("tbEndPoint")!;
        endpointBox.Text = "";
        var discovery = (Task)typeof(ucCannelCfg).GetMethod("DiscoverAsync", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(devices, null)!;
        using (var discoveryDeadline = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            while (!discovery.IsCompleted && !discoveryDeadline.IsCancellationRequested)
            {
                Dispatcher.UIThread.RunJobs();
                Thread.Sleep(20);
            }
        var discoveryFailure = discovery.Exception?.GetBaseException();
        Check(discoveryFailure is UriFormatException, "Empty OPC UA endpoint address is rejected inside the discovery worker");
        Check(discoveryFailure is not InvalidOperationException crossThread || !crossThread.Message.Contains("cannot access this object", StringComparison.OrdinalIgnoreCase),
            "Endpoint discovery never touches UI controls from the worker thread");
        window.FindControl<ComboBox>("cbLanguage")!.SelectedIndex = 1;
        Dispatcher.UIThread.RunJobs();
        Check(Equals(window.FindControl<NavigationViewItem>("accordionControlAbout")!.Content, UiText.Translate("关于")), "Interface language changes without changing user data");
        window.FindControl<ComboBox>("cbLanguage")!.SelectedIndex = 0;
        Dispatcher.UIThread.RunJobs();
        Check(Equals(window.FindControl<Button>("barButtonImport")!.Content, "导入"),
            "Static AXAML captions restore Chinese after an English round trip");
        window.FindControl<ComboBox>("cbLanguage")!.SelectedIndex = 1;
        Dispatcher.UIThread.RunJobs();
        window.Width = 1000;
        window.ShowPage("devices");
        window.UpdateLayout();
        Dispatcher.UIThread.RunJobs();
        Thread.Sleep(250);
        AvaloniaHeadlessPlatform.ForceRenderTimerTick();
        using (var frame = window.CaptureRenderedFrame()) frame!.Save(Path.Combine(Root, "topoview-compact-en.png"));
        // The collapsed rail must keep the navigation usable with icons alone: no item caption is
        // drawn, and every caption stays reachable as the hover text of its icon.
        var nav = window.FindControl<NavigationView>("accordionControl1")!;
        var collapsed = nav.MenuItems.OfType<NavigationViewItem>().ToList();
        Check(nav.PaneDisplayMode == NavigationViewPaneDisplayMode.LeftCompact && !nav.IsPaneOpen &&
            collapsed.Count == 8 && collapsed.All(item => item.IconSource != null &&
                item.Bounds.Width <= nav.CompactPaneLength + 1 &&
                item.GetVisualDescendants().OfType<TextBlock>()
                    .Where(label => Equals(label.Text, item.Content)).All(label => label.Bounds.Width < 1) &&
                Equals(ToolTip.GetTip(item), item.Content)),
            "Collapsed navigation rail shows icons with captions only in the hover text");
        Check(window.Width == 1000 && !context.IsRunning, "Compact English layout renders without starting any service");
        // The headless test process owns all resources and exits after saving render artifacts.
        window.Hide();
    }

    /// <summary>Loads real AXAML previews while guarding against accidental service ownership.</summary>
    private static void RunDesignerChecks()
    {
        var previewDirectory = Path.Combine(Root, "designer-" + Guid.NewGuid().ToString("N"));
        var previousDirectory = Environment.GetEnvironmentVariable("TOPOVIEW_CONFIG_DIR");
        // A regression that opens a default profile must remain inside the test directory.
        Environment.SetEnvironmentVariable("TOPOVIEW_CONFIG_DIR", previewDirectory);
        try
        {
            var contextField = typeof(BusinessPage).GetField("_context",
                BindingFlags.Instance | BindingFlags.NonPublic)!;
            var pageControls = new (Type PageType, string ControlName)[]
            {
                (typeof(ucCannelCfg), "gridControl1"),
                (typeof(ucDataTableCfg), "gridControl1"),
                (typeof(ucRecipe), "gridControl1"),
                (typeof(ucScripts), "memoEditScriptText"),
                (typeof(ucDiagnose), "gridControl1"),
                (typeof(ucHisQuery), "gridControl1"),
                (typeof(ucSystemCfg), "cbDB"),
                (typeof(ucAbout), "memoEdit1")
            };
            foreach (var (pageType, controlName) in pageControls)
            {
                Check(pageType.GetConstructor(Type.EmptyTypes) != null,
                    "Public designer constructor is available: " + pageType.Name);
                var page = (BusinessPage)Activator.CreateInstance(pageType)!;
                page.Refresh();
                page.Commit();
                Check(page.Content is Control && contextField.GetValue(page) == null &&
                    page.FindControl<Control>(controlName) != null,
                    "AXAML preview loads named controls without runtime services: " + pageType.Name);
            }

            var shell = new frmMain();
            var shellContext = typeof(frmMain).GetField("_context",
                BindingFlags.Instance | BindingFlags.NonPublic)!;
            Check(shellContext.GetValue(shell) == null &&
                shell.FindControl<ComboBox>("cbLanguage") != null &&
                shell.FindControl<TextBlock>("toolStrip_ServiceStatus") != null,
                "frmMain designer constructor owns no runtime context");
            shell.Show();
            RenderUi(shell);
            using (var frame = shell.CaptureRenderedFrame())
            {
                Check(frame != null, "Service-free frmMain preview renders natively");
                frame!.Save(Path.Combine(Root, "topoview-designer.png"));
            }
            shell.Close();

            var browser = new diaOPCUATagSelect();
            browser.Show();
            RenderUi(browser);
            var helperField = typeof(diaOPCUATagSelect).GetField("_helper",
                BindingFlags.Instance | BindingFlags.NonPublic)!;
            Check(helperField.GetValue(browser) == null,
                "Opening the OPC UA browser preview creates no client or certificate store");
            browser.Close();
            Check(!Directory.Exists(previewDirectory),
                "Designer construction writes no profile files or directories");
        }
        finally
        {
            Environment.SetEnvironmentVariable("TOPOVIEW_CONFIG_DIR", previousDirectory);
        }
    }

    /// <summary>Exercises the AXAML column template with arbitrary database column names.</summary>
    private static void RunHistoryColumnChecks()
    {
        var history = new ucHisQuery();
        var grid = history.FindControl<DataGrid>("gridControl1")!;
        using var table = new DataTable();
        table.Columns.Add("Custom]Temperature", typeof(double));
        table.Columns.Add("Batch Number", typeof(string));
        table.Rows.Add(25.6, "B001");
        HistoryColumns.SetSource(grid, table);
        Check(grid.Columns.Count == 2 &&
            Equals(grid.Columns[0].Header, "Custom]Temperature") &&
            Equals(grid.Columns[1].Header, "Batch Number"),
            "Historical columns preserve database names containing brackets and spaces");
        Check(grid.Columns.All(column => column is DataGridTextColumn && column.IsReadOnly),
            "Historical result columns remain read-only after loading their AXAML template");
        var host = new Window { Width = 1000, Height = 650, Content = history };
        host.Show();
        RenderUi(host);
        var renderedText = grid.GetVisualDescendants().OfType<TextBlock>()
            .Select(label => label.Text).ToArray();
        Check(renderedText.Contains(25.6.ToString(CultureInfo.CurrentCulture)) &&
            renderedText.Contains("B001"),
            "Historical cells bind the actual values for arbitrary column names");
        using (var frame = host.CaptureRenderedFrame())
            frame!.Save(Path.Combine(Root, "topoview-history-schema.png"));

        using var empty = new DataTable();
        empty.Columns.Add("Empty]Column", typeof(string));
        HistoryColumns.SetSource(grid, empty);
        Check(grid.Columns.Count == 1 && Equals(grid.Columns[0].Header, "Empty]Column") &&
            grid.ItemsSource is DataView { Count: 0 },
            "Empty historical results replace the previous schema without stale rows");
        HistoryColumns.SetSource(grid, null);
        Check(grid.Columns.Count == 0 && grid.ItemsSource == null,
            "Clearing historical results removes both rows and columns");
        // The original timestamps are DateTimeOffsetEdit controls; the port must keep a calendar and a
        // clock reachable, and both must feed the field the query reads. The parts live in the flyout,
        // so they are reached through its content instead of the visual tree.
        var start = history.FindControl<LegacyDateTimeEdit>("dtStime")!;
        var dropdown = ((Flyout)start.FindControl<Button>("picker")!.Flyout!).Content as Control;
        var calendar = dropdown?.GetLogicalDescendants().OfType<Avalonia.Controls.Calendar>().FirstOrDefault();
        var clock = dropdown?.GetLogicalDescendants().OfType<TimePicker>().FirstOrDefault();
        var picked = new DateTime(2026, 1, 2, 3, 4, 0);
        if (calendar != null) calendar.SelectedDate = picked.Date;
        if (clock != null) clock.SelectedTime = picked.TimeOfDay;
        Dispatcher.UIThread.RunJobs();
        Check(calendar != null && clock != null && start.Value == picked &&
            start.FindControl<TextBox>("editor")!.Text == "2026-01-02 03:04:00",
            "Historical timestamps edit through a calendar and clock dropdown");
        host.Close();
    }

    /// <summary>Checks named AXAML buttons still dispatch to the existing editor behavior.</summary>
    private static void RunEditorEventChecks(AppContextServices context)
    {
        var devices = new ucCannelCfg(context);
        var tags = devices.FindControl<DataGrid>("gridControl1")!;
        var originalTagCount = context.Profile.Tags.Count;
        var added = tags.ItemsSource.Cast<TagEditorRow>().Last();
        Check(string.IsNullOrEmpty(added.Name) && string.IsNullOrEmpty(added.Address),
            "Legacy tag grid exposes its bottom new-row editor without an invented add button");
        added.Address = "ns=2;s=DesignerAdjustedTag";
        added.Name   = "DesignerAdjustedTag";
        added.Type   = "bool";
        added.Access = "只读";
        devices.Commit();
        Check(context.Profile.Tags.Any(tag => tag.TagName == "DesignerAdjustedTag" &&
            tag.DataType == 0 && tag.RW == 1) && context.Profile.Tags.Count == originalTagCount + 1,
            "Point editor commits the same typed values after AXAML conversion");
        context.Profile.Tags.RemoveAll(tag => tag.TagName == "DesignerAdjustedTag");

        var recipes = new ucRecipe(context);
        var recipesHost = new Window { Width = 1200, Height = 800, Content = recipes };
        recipesHost.Show();
        RenderUi(recipesHost);
        var recipe = context.Profile.Recipes.equipment_recipes.First();
        var originalName = recipe.equipment_name;
        var originalParameters = recipe.recipe_data.ToList();
        var nameEditor = recipes.FindControl<TextBox>("tbTaskName")!;
        nameEditor.Text = "Designer adjusted recipe";
        recipes.Commit();
        Check(recipe.equipment_name == "Designer adjusted recipe",
            "Named recipe input commits edited metadata");
        var parameters = recipes.FindControl<DataGrid>("gridControl1")!;
        parameters.SelectedItem = parameters.ItemsSource.Cast<recipe_data>().First();
        recipes.FindControl<Button>("btColumnDelete")!
            .RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        ConfirmOpenDialog(recipesHost);
        recipes.Commit();
        Check(recipe.recipe_data.Count == originalParameters.Count - 1,
            "Named delete-parameter button removes the selected recipe row");
        recipesHost.Close();
        recipe.equipment_name = originalName;
        recipe.recipe_data    = originalParameters;
        Check(!context.IsRunning,
            "Editor events modify isolated configuration without starting acquisition");
    }

    private static void RenderUi(Window window)
    {
        window.UpdateLayout();
        Dispatcher.UIThread.RunJobs();
        Thread.Sleep(220);
        AvaloniaHeadlessPlatform.ForceRenderTimerTick();
    }

    /// <summary>Checks the custom AXAML dialog receives its Fluent template and button events.</summary>
    private static void RunDialogChecks()
    {
        var owner = new frmMain();
        owner.Show();
        var dialog = new MessageDialog
        {
            Title = "确认操作", PrimaryButtonText = "确认", CloseButtonText = "取消"
        };
        dialog.SetMessage("AXAML dialog content");
        var result = dialog.ShowAsync(owner);
        RenderUi(dialog);
        Check(dialog.GetVisualDescendants().OfType<TextBlock>()
            .Any(label => label.Text == "AXAML dialog content"),
            "AXAML message dialog renders its content with the Fluent theme");
        using (var frame = dialog.CaptureRenderedFrame())
            frame!.Save(Path.Combine(Root, "topoview-dialog.png"));
        Check(dialog.Owner == owner && !dialog.CanResize &&
            dialog.SystemDecorations == SystemDecorations.None,
            "Message dialogs are borderless modal windows movable by their title bar");
        var confirm = dialog.GetVisualDescendants().OfType<Button>()
            .FirstOrDefault(button => Equals(button.Content, "确认"));
        Check(confirm != null, "AXAML message dialog exposes its confirmation button");
        confirm!.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        for (var attempt = 0; attempt < 6 && !result.IsCompleted; attempt++) RenderUi(owner);
        Check(result.IsCompletedSuccessfully &&
            result.Result == FluentAvalonia.UI.Controls.ContentDialogResult.Primary,
            "AXAML dialog confirmation preserves the modal result contract");

        var prompt = new MessageDialog
            { Title = "参数输入", PrimaryButtonText = "确定", CloseButtonText = "取消" };
        prompt.SetInput("12.5");
        var promptResult = prompt.ShowAsync(owner);
        RenderUi(prompt);
        var input = prompt.FindControl<TextBox>("txtValue")!;
        Check(input.IsVisible && input.Text == "12.5",
            "AXAML input dialog displays the caller's initial value");
        input.Text = "37.8";
        CancelDialog(prompt, owner);
        Check(promptResult.IsCompletedSuccessfully &&
            promptResult.Result == ContentDialogResult.None && prompt.InputValue == "37.8",
            "AXAML input dialog retains edited text while cancel returns no confirmation");

        var profile = new ConfigurationStore(Path.Combine(Root, "dialog-profile")).CreateDemo();
        var selector = new diaTagSelect(profile);
        var selectionResult = selector.ShowDialog<List<DBTag>>(owner);
        RenderUi(selector);
        selector.FindControl<TextBox>("tbTnFilter")!.Text = "Pressure";
        selector.FindControl<Button>("btTnFilter")!.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        RenderUi(selector);
        var tagGrid = selector.FindControl<DataGrid>("listView1")!;
        tagGrid.SelectedItem = tagGrid.ItemsSource!.Cast<object>().Single();
        Check(selector.TagName == "Reactor01.Pressure",
            "Legacy tag filter button selects the original configured tag");
        selector.FindControl<Button>("btSelect")!.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        RenderUi(owner);
        Check(selectionResult.IsCompletedSuccessfully && selectionResult.Result.Single().TagId == "Reactor01.Pressure",
            "Legacy tag selection button returns the selected tag");
        var cancelled = new diaTagSelect(profile);
        var cancelledResult = cancelled.ShowDialog<List<DBTag>>(owner);
        cancelled.Close();
        RenderUi(owner);
        Check(cancelledResult.IsCompletedSuccessfully && cancelledResult.Result == null,
            "Closing the legacy tag selector cancels without modifying configuration");
        RunLegacyDialogChecks(owner, profile);
        owner.Close();
    }

    private static async Task RunLegacySchemaChecksAsync()
    {
        var store = new ConfigurationStore(Path.Combine(Root, "schema-" + Guid.NewGuid().ToString("N")));
        var profile = store.CreateDemo();
        profile.Server.SqlConfigInfo.DbType = "SQLite";
        Directory.CreateDirectory(store.DirectoryPath);
        profile.Server.SqlConfigInfo.DbName = Path.Combine(store.DirectoryPath, "legacy-schema.db");
        var database = new DatabaseService(() => profile);
        var sample = new DataSample
        {
            TaskName = "LegacySchema", TableName = "LegacySamples", bSave2ProcessTable = false,
            Samples = [new DSTbDefine { Column = "Value", DataType = "float", IsNullable = true,
                isInternalValue = true, DefaultValue = "0" }]
        };
        profile.Server.dataSamples.Add(sample);
        await database.SynchronizeTableAsync(sample);
        sample.Samples.Add(new DSTbDefine { Column = "LegacyAdded", DataType = "string", IsNullable = true, isInternalValue = true });
        await database.SynchronizeTableAsync(sample);
        var result = await database.QueryAsync(sample.TableName, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1), "");
        Check(result.Columns.Contains("LegacyAdded"), "Legacy synchronize-table action adds a missing SQLite field");
        await database.DeleteSampleColumnAsync(sample, "LegacyAdded");
        result = await database.QueryAsync(sample.TableName, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1), "");
        Check(!result.Columns.Contains("LegacyAdded"), "Legacy delete-DB-column action removes only the selected SQLite field");
        await ExpectAsync<InvalidOperationException>(() => database.DeleteSampleColumnAsync(sample, "Datetime"),
            "Legacy schema action rejects unconfigured reserved database columns");
    }

    private static void RunLegacyDialogChecks(Window owner, WorkspaceProfile profile)
    {
        var tag = profile.Tags.First(tag => tag.RW != 1);
        var column = new diaColumnSelect(profile);
        var columnResult = column.ShowDialog<DSTbDefine>(owner);
        column.FindControl<TextBox>("tbAtribute")!.Text = "ConstantColumn";
        column.FindControl<TextBox>("tbDefaultValue")!.Text = "Batch001";
        column.FindControl<ComboBox>("cbDataType")!.SelectedItem = "string";
        column.FindControl<RadioButton>("rbInternal")!.IsChecked = true;
        column.FindControl<Button>("btOK")!.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        RenderUi(owner);
        Check(columnResult.IsCompletedSuccessfully && columnResult.Result is
            { Column: "ConstantColumn", isInternalValue: true, DefaultValue: "Batch001" },
            "Legacy column dialog returns a configured constant without changing source tags");
        var parameter = new diaRecipeTagSelect(profile, new recipe_data
        {
            parameter_name = "RecipeParameter", tag_name = tag.TagId,
            data_type = DBTag.DataTypeString(tag.DataType), value = tag.DataType == 0 ? "false" : "0"
        });
        var parameterResult = parameter.ShowDialog<recipe_data>(owner);
        parameter.FindControl<TextBox>("tbDefaultValue")!.Text = tag.DataType == 0 ? "true" : "42";
        parameter.FindControl<Button>("btOK")!.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        RenderUi(owner);
        Check(parameterResult.IsCompletedSuccessfully && parameterResult.Result.tag_name == tag.TagId,
            "Legacy recipe parameter dialog validates and returns the selected binding");
        var write = new diaWrite { TagName = tag.TagId, UOM = tag.UOM };
        var writeResult = write.ShowDialog<bool>(owner);
        write.FindControl<Button>("btCancel")!.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        RenderUi(owner);
        Check(writeResult.IsCompletedSuccessfully && !writeResult.Result,
            "Legacy write dialog closes without submitting a device write");
        var check = new diaExpressCheck(new ExpressionResult("a > 10",
            [new DBTag { TagName = "a", TagId = tag.TagId, DataType = 6 }]));
        var rows = check.FindControl<DataGrid>("dataGridView1")!.ItemsSource.Cast<ExpressionValueRow>().ToList();
        rows[0].Value = "11";
        Check(check.Evaluate(), "Legacy expression check evaluates supplied typed values");
    }

    private static void CancelDialog(Control dialog, Window owner)
    {
        var cancel = dialog.GetVisualDescendants().OfType<Button>()
            .First(button => Equals(button.Content, "取消"));
        cancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        for (var attempt = 0; attempt < 3; attempt++) RenderUi(owner);
    }

    /// <summary>Clicks the primary button of the confirmation dialog the view just opened.</summary>
    private static void ConfirmOpenDialog(Window owner)
    {
        for (var attempt = 0; attempt < 3; attempt++) RenderUi(owner);
        var dialog = owner.OwnedWindows.OfType<TopoView.Views.MessageDialog>().FirstOrDefault();
        var confirm = dialog?.GetVisualDescendants().OfType<Button>()
            .FirstOrDefault(button => Equals(button.Content, "确认"));
        confirm?.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        for (var attempt = 0; attempt < 3; attempt++) RenderUi(owner);
    }
}
