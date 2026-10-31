//=============================================================================
// 对齐 TopoData：Public/WebServer.cs（WebAPIBuilder）与 Controllers/DataController、
//               RecipeController、StatusController、CommandController、OPCUAController
// Description: 随程序启动的兼容 WebAPI 宿主，保持旧版路由契约与响应语义。
//=============================================================================
// Keeps TopoData's realtime and recipe HTTP route contracts on the new runtime, including the
// legacy empty-array bodies, plain-text error messages and lenient per-item write behaviour.
using System.Globalization;
using System.Text.Json;
using auDASLib;
using auDAServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TopoView.Services;

/// <summary>
/// 兼容 WebAPI 宿主：实时值读写、状态查询、配方下载与 OPC UA 端点发现。
/// </summary>
public sealed class WebApiService : IAsyncDisposable
{
    private WebApplication? _application;
    // Routes resolve the live profile so an imported configuration is visible without a restart.
    private Func<WorkspaceProfile> _profile = () => throw new InvalidOperationException("WebAPI 尚未启动。");
    // Legacy GetServerStatus echoed the engine's process start time; the service is now created with
    // the application, so the construction time is the equivalent moment.
    private readonly DateTime _startedAt = DateTime.Now;
    /// <summary>WebAPI 是否正在监听。</summary>
    public bool IsRunning => _application != null;

    /// <summary>
    /// 启动 WebAPI 服务（对应 WebAPIBuilder.StartService）：按系统配置的 REST 地址监听；
    /// 未启用 REST 或已在运行时不做任何事。停止采集不影响监听。
    /// </summary>
    public async Task StartAsync(WorkspaceProfile profile, AcquisitionService acquisition, CancellationToken token,
        Func<WorkspaceProfile>? profileSource = null)
    {
        if (_application != null) return;
        if (!profile.Server.RestEnable) return;
        _profile = profileSource ?? (() => profile);
        var uri = new Uri(profile.Server.RestUri1);
        if (uri.Scheme != "http" && uri.Scheme != "https") throw new InvalidOperationException("WebAPI 地址必须使用 HTTP 或 HTTPS。");
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions { Args = [], ApplicationName = typeof(WebApiService).Assembly.FullName });
        builder.Logging.ClearProviders();
        builder.WebHost.UseUrls(uri.GetLeftPart(UriPartial.Authority));
        var application = builder.Build();
        // DataController.GetRealtimeValues：读数据函数（点位完整名称清单）。
        application.MapPost("/api/Data/GetRealtimeValues", (string[] names) => Results.Ok(acquisition.Values
            .Where(value => names.Contains(value.ValueName)).ToList()));
        // 状态查询：新运行状态及各设备连接状态。
        application.MapGet("/api/Status/GetStatus", () => Results.Ok(new { IsRunning = acquisition.IsRunning, Timestamp = DateTime.Now,
            Devices = _profile().Server.cDevices.Select(d => new { d.CannelID, IsConnected = acquisition.IsConnected(d.CannelID) }) }));
        // StatusController.GetServerStatus：旧版授权状态结构；免费版恒为未试用、不超时。
        application.MapGet("/api/Status/GetServerStatus", () => Results.Ok(new ServerStatus
            { isTtrail = false, TotalTrailHours = 3, AppRunTimeOut = false, AppStartTime = _startedAt }));
        // OPCUAController.GetUAEndpoints：读取 OPC UA 服务端点。
        application.MapGet("/api/OPCUA/GetUAEndpoints", async (string uri) =>
        {
            try { return Results.Ok(await Task.Run(() => auDAServer.UAOperator.GetUACfgs(uri))); }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }
        });
        // DataController.WriteRealtimeValues（原 WriteValue）：写数据函数。
        application.MapPost("/api/Data/WriteRealtimeValues", async (HttpRequest request) =>
        {
            try
            {
                var values = await request.ReadFromJsonAsync<List<WriteValue>>();
                // Legacy DataController skipped unusable entries silently and always answered 200 [].
                foreach (var item in values ?? [])
                {
                    if (item == null || string.IsNullOrWhiteSpace(item.ValueName)) continue;
                    var text = item.value is JsonElement element
                        ? element.ValueKind == JsonValueKind.String ? element.GetString() ?? "" : element.GetRawText()
                        : Convert.ToString((object?)item.value, CultureInfo.InvariantCulture) ?? "";
                    try { await acquisition.WriteAsync([(item.ValueName, text)]); }
                    catch { /* one invalid entry never failed the whole legacy request */ }
                }
                return Results.Ok(Array.Empty<RealTimeValue>());
            }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }
        });
        // RecipeController.DownloadRecipe：下载配方到设备。
        application.MapPost("/api/Recipe/DownloadRecipe", async (HttpRequest request) =>
        {
            try
            {
                var name = await JsonSerializer.DeserializeAsync<string>(request.Body);
                if (string.IsNullOrWhiteSpace(name)) return Results.NotFound("Recipe Name is empty.");
                var recipe = (_profile().Recipes.equipment_recipes ?? []).FirstOrDefault(r =>
                    string.Equals(r.equipment_id, name, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(r.equipment_name, name, StringComparison.OrdinalIgnoreCase));
                if (recipe == null) return Results.NotFound($"Recipe '{name}' not found.");
                if (recipe.recipe_status != RecipeStatus.InUse) return Results.BadRequest($"Recipe '{name}' Status is not Active.");
                var items = (recipe.recipe_data ?? []).Where(p => !string.IsNullOrWhiteSpace(p.tag_name)).ToList();
                if (items.Count == 0) return Results.BadRequest("No writable parameters found in the recipe.");
                foreach (var group in items.GroupBy(p => ItemPool.TagPool.TryGetValue(p.tag_name, out var tag) ? tag.CannelID ?? "" : ""))
                    try { await acquisition.WriteAsync(group.Select(p => (p.tag_name, p.value ?? ""))); }
                    catch { /* legacy continued with the remaining channels after a failed queue */ }
                return Results.Ok(Array.Empty<RealTimeValue>());
            }
            catch (Exception ex) { return Results.BadRequest(ex.Message); }
        });
        // The legacy CommandController routes were intentional no-ops kept for client compatibility.
        application.MapPost("/api/Command/StartCannel", () => Results.Ok());
        application.MapPost("/api/Command/StopCannel", () => Results.Ok());
        application.MapPost("/api/Command/AddCannel", () => Results.Ok());
        try { await application.StartAsync(token); _application = application; }
        catch { await application.DisposeAsync(); throw; }
    }

    /// <summary>停止并释放 WebAPI 监听。</summary>
    public async ValueTask DisposeAsync()
    {
        if (_application == null) return;
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await _application.StopAsync(timeout.Token);
        await _application.DisposeAsync();
        _application = null;
    }
}
