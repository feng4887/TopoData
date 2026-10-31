//=============================================================================
// 对齐 TopoData：auDAServer/BLL/Scheduler.cs 的启动序列与 frmMain.cs 的启停调用
//               （采集引擎在 Runtime/hiServer.dll）
// Description: 采集服务：装载点表并启停共享引擎中的各通道驱动。
//=============================================================================
// Hands the driver lifecycle to the shared legacy engine layer (Runtime/hiServer.dll), so the
// OPC UA / Profinet S7 / Modbus implementations stay identical to the service runtime.
using System.Globalization;
using auDASLib;
using auDAServer;

namespace TopoView.Services;

/// <summary>
/// 采集服务：装载点表并启停共享引擎（ItemPool）中的各通道驱动。
/// </summary>
public sealed class AcquisitionService : IAsyncDisposable
{
    private readonly SemaphoreSlim _lifecycle = new(1, 1);
    private WorkspaceProfile? _profile;
    public bool IsRunning { get; private set; }
    /// <summary>引擎实时值池（点位完整名称 → 实时值）。</summary>
    public IEnumerable<RealTimeValue> Values => ItemPool.ValuePool.Values;

    /// <summary>
    /// 校验并发布点表后启动全部启用通道（与旧版 Scheduler.Start 相同的调用序列）；
    /// 免费版设备数与单设备点位数上限在此检查。
    /// </summary>
    public async Task StartAsync(WorkspaceProfile profile)
    {
        await _lifecycle.WaitAsync();
        try
        {
            if (IsRunning) return;
            ConfigurationStore.Normalize(profile);
            ConfigurationStore.Validate(profile);
            _profile = profile;
            ServerCfg.Instance = profile.Server;
            var devices = profile.Server.cDevices.Where(d => d.iActive == 1).ToList();
            if (devices.Count == 0) throw new InvalidOperationException("请先添加并启用设备。");
            if (DACtrl.free_Version && devices.Count > DACtrl.free_Cannels)
                throw new InvalidOperationException($"当前版本最多启用 {DACtrl.free_Cannels} 个设备。");
            foreach (var device in devices)
            {
                var count = profile.Tags.Count(t => t.CannelID == device.CannelID);
                if (count == 0) throw new InvalidOperationException($"{device.CannelID} 尚未配置点位。");
                if (DACtrl.free_Version && count > DACtrl.free_CannelTags)
                    throw new InvalidOperationException($"{device.CannelID} 超过当前版本的 {DACtrl.free_CannelTags} 点限制。");
            }
            // Same calls the legacy Scheduler made: publish the tag table, then start every enabled
            // channel. Drivers connect and sample on their own worker threads.
            await Task.Run(() =>
            {
                DataImport.dicCannelTags.Clear();
                DataImport.dicTagPool.Clear();
                DataImport.ImportCannelTags(profile.Tags);
                // The shared simulator keeps realtime values that already exist, so seed the configured
                // defaults first (the legacy desktop client did the same); otherwise simulator points
                // would all come up as plain type zero values.
                ItemPool.ValuePool.Clear();
                foreach (var device in devices.Where(d => d.IsSimulator))
                    foreach (var tag in profile.Tags.Where(t => t.CannelID == device.CannelID))
                        ItemPool.ValuePool[tag.TagId] = new RealTimeValue
                        {
                            Cannel = tag.CannelID, ValueName = tag.TagId, Timestamp = DateTime.Now, Quality = 192, uom = tag.UOM,
                            RealValue = ParseValue(tag.DataType,
                                string.IsNullOrWhiteSpace(tag.DefaultValue) ? DefaultText(tag.DataType) : tag.DefaultValue)
                        };
                ItemPool.Instance.Start(profile.Tags);
            });
            IsRunning = true;
        }
        catch
        {
            await Task.Run(() => ItemPool.Instance.Stop());
            throw;
        }
        finally { _lifecycle.Release(); }
    }

    /// <summary>停止全部通道驱动，并将实时值质量置 0（旧版停止语义）。</summary>
    public async Task StopAsync()
    {
        await _lifecycle.WaitAsync();
        try { await Task.Run(() => ItemPool.Instance.Stop()); }
        finally { _lifecycle.Release(); }
        IsRunning = false;
        foreach (var value in ItemPool.ValuePool.Values) value.Quality = 0;
    }

    /// <summary>通道驱动是否处于连接状态（仿真设备视为恒连接）。</summary>
    public bool IsConnected(string channel) => IsRunning &&
        (_profile?.Server.cDevices.Any(d => d.CannelID == channel && d.iActive == 1 && d.IsSimulator) == true ||
         ChannelConnected(channel));

    /// <summary>Reads the live connection flag of the channel's driver, if one is running.</summary>
    private static bool ChannelConnected(string channel)
    {
        try
        {
            return ItemPool.Instance.ConnectionPool.TryGetValue(channel, out var value) && value switch
            {
                UAOperator op => op.IsConnected,
                Sharp7Operator op => op.IsConnected,
                ModbusTcpOperator op => op.IsConnected,
                ModbusRTUOperator op => op.IsConnected,
                _ => false
            };
        }
        catch (Exception) { return false; }
    }

    /// <summary>Validates the complete batch before performing any device writes.</summary>
    public async Task<int> WriteAsync(IEnumerable<(string TagId, string Value)> requested)
    {
        if (!IsRunning || _profile == null) throw new InvalidOperationException("采集服务尚未启动。");
        var batch = requested.Select(item =>
        {
            if (!ItemPool.TagPool.TryGetValue(item.TagId, out var tag))
                throw new InvalidOperationException($"点位不存在：{item.TagId}");
            if (tag.RW == 1) throw new InvalidOperationException($"只读点位禁止写入：{item.TagId}");
            var device = _profile.Server.cDevices.Single(d => d.CannelID == tag.CannelID);
            if (device.iActive != 1 || !IsConnected(device.CannelID))
                throw new InvalidOperationException($"设备未启用或未连接：{device.CannelID}");
            return (Tag: tag, Value: ParseValue(tag.DataType, item.Value));
        }).ToList();
        var result = await Task.Run(() => ItemPool.Instance.WriteData(new WriteValueQueue
        {
            WriteValues = batch.Select(item => new WriteValue
            {
                CannelID = item.Tag.CannelID, ValueName = item.Tag.TagId, Address = item.Tag.Address,
                DataType = item.Tag.DataType, value = item.Value
            }).ToList()
        }));
        if (result.lastError != 0 || !string.IsNullOrWhiteSpace(result.ErrorMessage))
            throw new IOException($"写入失败（错误码 {result.lastError}）：{result.ErrorMessage}");
        return result.count;
    }

    /// <summary>按点表数据类型把文本解析为运行值（布尔、字符串、整型、浮点）。</summary>
    public static object ParseValue(int type, string value) => type switch
    {
        0 => value.Trim().ToLowerInvariant() switch { "1" or "true" => true, "0" or "false" => false,
            _ => throw new FormatException("布尔值必须为 true、false、1 或 0。") },
        1 or 2 => value,
        3 => byte.Parse(value, CultureInfo.InvariantCulture),
        4 => short.Parse(value, CultureInfo.InvariantCulture),
        5 => ushort.Parse(value, CultureInfo.InvariantCulture),
        6 => int.Parse(value, CultureInfo.InvariantCulture),
        10 => float.Parse(value, CultureInfo.InvariantCulture),
        11 => double.Parse(value, CultureInfo.InvariantCulture),
        12 => long.Parse(value, CultureInfo.InvariantCulture),
        _ => throw new FormatException("不支持的数据类型。")
    };

    private static string DefaultText(int type) => type is 1 or 2 ? "" : type == 0 ? "false" : "0";

    public async ValueTask DisposeAsync() { await StopAsync(); _lifecycle.Dispose(); }
}
