//=============================================================================
// 对齐 TopoData：Public/MqttOperator.cs（MqttServerConfig、MqttHelper、MqttOperator）
// Description: MQTT 实时值发布服务，保持旧版报文契约与发布周期语义。
//=============================================================================
// MQTT publisher reproducing TopoData MqttOperator's payload contract: camel-case JSON,
// "yyyy-MM-dd HH:mm:ss" batch timestamps, QoS 1, a 500 ms floor and no empty publications.
using System.Text.Json;
using auDASLib;
using MQTTnet;
using MQTTnet.Protocol;

namespace TopoView.Services;

/// <summary>
/// MQTT 实时值发布服务，对应旧版 MqttOperator 单例的发布循环。
/// </summary>
public sealed class MqttService(AcquisitionService acquisition, Action<string> log)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    /// <summary>
    /// 连接 Broker 并循环发布实时值：单值模式逐点发布到各自 Topic，
    /// 批量模式发布到统一主题；未启用 MQTT 时直接返回。
    /// </summary>
    public async Task RunAsync(MqttServerConfig config, CancellationToken token)
    {
        if (!config.EnableMqtt) return;
        using var client = new MqttClientFactory().CreateMqttClient();
        var builder = new MqttClientOptionsBuilder().WithClientId(config.ClientId).WithTcpServer(config.Server, config.Port).WithCleanSession();
        // MqttHelper only attached credentials when a user name was actually configured.
        if (config.UseSecurity && !string.IsNullOrWhiteSpace(config.Username)) builder.WithCredentials(config.Username, config.Password);
        var options = builder.Build();
        var lastError = "";
        try
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (!client.IsConnected)
                    {
                        await client.ConnectAsync(options, token);
                        log("MQTT 已连接。");
                    }
                    // The legacy publisher only announced points of channels enabled in hiTopoCfg.xml.
                    var active = ServerCfg.Instance?.cDevices?.Where(d => d.iActive == 1).Select(d => d.CannelID).ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];
                    var values = acquisition.Values.Where(v => active.Contains(v.Cannel)).ToList();
                    if (values.Count == 0)
                    {
                        await Task.Delay(1000, token);
                        continue;
                    }
                    if (config.PublishSingleTag)
                    {
                        foreach (var value in values)
                        {
                            if (string.IsNullOrWhiteSpace(value.ValueName)) continue;
                            await client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(value.ValueName)
                                .WithPayload(JsonSerializer.Serialize(value, JsonOptions))
                                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                                .WithRetainFlag(config.Retain).Build(), token);
                        }
                    }
                    else
                        await client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(config.TopicPrefix)
                            .WithPayload(JsonSerializer.Serialize(new
                            {
                                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                count = values.Count,
                                values
                            }, JsonOptions))
                            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                            .WithRetainFlag(config.Retain).Build(), token);
                    lastError = "";
                    // MqttOperator clamped every configured interval below 500 ms.
                    await Task.Delay(Math.Max(500, config.PublishIntervalMs), token);
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested) { break; }
                catch (Exception ex)
                {
                    if (lastError != ex.Message) log("MQTT：" + ex.Message);
                    lastError = ex.Message;
                    await Task.Delay(5000, token);
                }
            }
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { }
        finally
        {
            if (client.IsConnected)
                try { await client.DisconnectAsync(new MqttClientDisconnectOptions(), CancellationToken.None); }
                catch (Exception ex) { log("MQTT 断开：" + ex.Message); }
        }
    }

    /// <summary>
    /// 系统配置中的 MQTT 连接测试：8 秒内完成连接并立即断开。
    /// </summary>
    public static async Task TestAsync(MqttServerConfig config)
    {
        using var client = new MqttClientFactory().CreateMqttClient();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(8));
        var builder = new MqttClientOptionsBuilder().WithTcpServer(config.Server, config.Port)
            .WithClientId("topoview-test-" + Guid.NewGuid().ToString("N"));
        if (config.UseSecurity) builder.WithCredentials(config.Username, config.Password);
        await client.ConnectAsync(builder.Build(), timeout.Token);
        await client.DisconnectAsync(new MqttClientDisconnectOptions(), timeout.Token);
    }
}
