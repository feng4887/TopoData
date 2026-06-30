using MQTTnet;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace auDASLib
{
    public class MqttServerConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether MQTT support is enabled.
        /// </summary>
        public bool EnableMqtt { get; set; } = true;
        public bool UseSecurity { get; set; } = false;
        public string Server { get; set; } = "localhost";
        public int Port { get; set; } = 1883;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string ClientId { get; set; } = "topoDataPublisher";
        public string TopicPrefix { get; set; } = "industrial/realtime";

        public int PublishIntervalMs { get; set; } = 1000;

        /// <summary>
        /// Single-value publishing mode; 
        /// if true, each value is published individually to its corresponding topic;  
        /// if false, values are batched and published to a single unified topic，like "industrial/realtime".
        /// </summary>
        public bool PublishSingleTag { get; set; } = false;
        public bool Retain { get; set; } = false;
    }

    public sealed class MqttHelper : IAsyncDisposable
    {
        private IMqttClient? _client;
        private MqttClientOptions? _options;
        private readonly SemaphoreSlim _connectLock = new(1, 1);
        private bool _manualDisconnect;

        public event Action<string>? OnStatus;
        public event Action<string, Exception>? OnError;

        public bool IsConnected => _client?.IsConnected == true;

        public async Task<bool> ConnectAsync(MqttServerConfig config, CancellationToken token = default)
        {
            await _connectLock.WaitAsync(token);

            try
            {
                if (_client?.IsConnected == true)
                    return true;

                _manualDisconnect = false;

                // MQTTnet 5.x 用 MqttClientFactory，不再用 MqttFactory
                var factory = new MqttClientFactory();
                _client = factory.CreateMqttClient();

                var builder = new MqttClientOptionsBuilder()
                    .WithClientId(config.ClientId)
                    .WithTcpServer(config.Server, config.Port)
                    .WithCleanSession();

                if (config.UseSecurity && !string.IsNullOrWhiteSpace(config.Username))
                {
                    builder.WithCredentials(config.Username, config.Password);
                }

                _options = builder.Build();

                _client.ConnectedAsync += e =>
                {
                    OnStatus?.Invoke($"MQTT连接成功：{config.Server}:{config.Port}");
                    return Task.CompletedTask;
                };

                _client.DisconnectedAsync += async e =>
                {
                    OnStatus?.Invoke("MQTT连接断开");

                    if (_manualDisconnect)
                        return;

                    await ReconnectLoopAsync(token);
                };

                var result = await _client.ConnectAsync(_options, token);

                return result.ResultCode == MqttClientConnectResultCode.Success;
            }
            catch (Exception ex)
            {
                OnError?.Invoke("MQTT连接失败", ex);
                return false;
            }
            finally
            {
                _connectLock.Release();
            }
        }

        private async Task ReconnectLoopAsync(CancellationToken token)
        {
            if (_client == null || _options == null)
                return;

            while (!_manualDisconnect && !_client.IsConnected && !token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(5000, token);
                    await _client.ConnectAsync(_options, token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke("MQTT重连失败", ex);
                }
            }
        }

        public async Task<bool> PublishAsync(
            string topic,
            string payload,
            bool retain = false,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce,
            CancellationToken token = default)
        {
            if (_client?.IsConnected != true)
                return false;

            try
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel(qos)
                    .WithRetainFlag(retain)
                    .Build();

                await _client.PublishAsync(message, token);
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"MQTT发布失败，Topic={topic}", ex);
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            _manualDisconnect = true;

            if (_client?.IsConnected == true)
            {
                await _client.DisconnectAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
            _connectLock.Dispose();
            _client?.Dispose();
        }
    }

    public sealed class MqttOperator : IAsyncDisposable
    {
        private MqttServerConfig _config;
        private MqttHelper _mqtt = new();
        private object _dataLock = new();
        private SemaphoreSlim _publishLock = new(1, 1);
        private CancellationTokenSource? _cts;
        private Task? _publishTask;
        private List<RealTimeValue> _currentData = new();

        #region 单例者
        static public MqttOperator Instance
        {
            get
            {
                if (_Instance == null)
                    return new MqttOperator();
                else
                    return _Instance;
            }
        }

        static private MqttOperator _Instance = new MqttOperator();
        #endregion //单例者

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public event Action<string>? OnStatusChanged;
        public event Action<string, Exception>? OnError;

        public MqttOperator()
        {
        }

        public MqttServerConfig LoadConfig(string configPath)
        {
            var serializer = new XmlSerializer(typeof(MqttServerConfig));
            if (!File.Exists(configPath))
            {
                var config = new MqttServerConfig
                {
                    Server = "127.0.0.1",
                    Port = 1883,
                    Username = "admin",
                    Password = "password",
                    ClientId = "RealTimePublisher",
                    TopicPrefix = "industrial/realtime",
                    PublishIntervalMs = 2000,
                    PublishSingleTag = false,
                    Retain = false,
                    EnableMqtt = false,
                    UseSecurity = false
                };

                using var writer = new StreamWriter(configPath);
                serializer.Serialize(writer, config);
            }

            using var reader = new StreamReader(configPath);
            return (MqttServerConfig)serializer.Deserialize(reader)!;
        }

        public async Task Start(string configPath, CancellationToken token = default)
        {
            _config = LoadConfig(configPath);
            if (_config == null || !_config.EnableMqtt)
                return;

            _mqtt.OnStatus += msg => OnStatusChanged?.Invoke(msg);
            _mqtt.OnError += (msg, ex) => OnError?.Invoke(msg, ex);
            var ok = await _mqtt.ConnectAsync(_config, token);
            OnStatusChanged?.Invoke(ok ? "MQTT初始化成功" : "MQTT初始化失败");
            if (!ok) //启动失败
                return;

            _cts = new CancellationTokenSource();
            _publishTask = Task.Run(() => PublishLoopAsync(_config.PublishIntervalMs,_cts.Token));
            OnStatusChanged?.Invoke($"开始发布数据，间隔：{_config.PublishIntervalMs}ms");
        }

        private async Task PublishLoopAsync(int pubWindow,CancellationToken token)
        {
            if (pubWindow < 500)
                pubWindow = 500;

            while (!token.IsCancellationRequested)
            {
                if (!_mqtt.IsConnected)
                {
                    await Task.Delay(1000, token);
                    continue;
                }

                try
                {
                    List<RealTimeValue> data = new List<RealTimeValue>();
                    lock (_dataLock)
                    {
                        List<string> cannels = ServerCfg.Instance.cDevices.Where(it => it.iActive == 1).Select(x => x.CannelID).ToList();
                        if (cannels == null || cannels.Count == 0)
                            return;

                        List<string> TagIDs = new List<string>();

                        foreach (var v in cannels)
                        {
                            if (DataImport.dicCannelTags.ContainsKey(v))
                                TagIDs.AddRange(DataImport.dicCannelTags[v].Select(it => it.TagId).ToList());
                        }

                        //------------------------------------------------------
                        //To do: filter function
                        //TagIDs = PubFunction.FilterTagIDs(TagIDs, _strFilter);

                        if (TagIDs == null || TagIDs.Count == 0)
                            return;

                        try
                        {
                            foreach (var item in TagIDs)
                            {
                                RealTimeValue realTimeValue = new RealTimeValue();
                                var vl = auDAServer.ItemPool.ValuePool.TryGetValue(item, out realTimeValue);
                                if (vl && realTimeValue != null)
                                    data.Add(realTimeValue);
                            }
                        }
                        catch (Exception ex)
                        {
                          //  Debug.WriteLine(ex.Message);
                        }
                    }

                    if (data.Count == 0)
                    {
                        await Task.Delay(1000, token);
                        continue;                  
                    }

                    if (_config.PublishSingleTag)
                    {
                        foreach (var item in data)
                        {
                            if (string.IsNullOrWhiteSpace(item.ValueName))
                                continue;

                            var singlePayload = item;

                            string singleJson = JsonSerializer.Serialize(singlePayload, JsonOptions);
                            string singleTopic = $"{item.ValueName}";

                            await _mqtt.PublishAsync(
                                singleTopic,
                                singleJson,
                                _config.Retain,
                                MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce,
                                token);
                        }
                    }
                    else
                    {
                        // 批量发布
                        var batchPayload = new
                        {
                            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            count = data.Count,
                            values = data
                        };

                        string batchJson = JsonSerializer.Serialize(batchPayload, JsonOptions);
                        string batchTopic = $"{_config.TopicPrefix}";

                        await _mqtt.PublishAsync(
                            batchTopic,
                            batchJson,
                            _config.Retain,
                            MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce,
                            token);
                    }

                    OnStatusChanged?.Invoke($"已发布 {data.Count} 条实时数据");
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
            //        OnError?.Invoke("发布数据失败", ex);
                }
                finally
                {
                }
                await Task.Delay(pubWindow, token);
            }
        }

        public async Task Stop()
        {
            if (_cts == null)
                return;

            _cts.Cancel();

            try
            {
                if (_publishTask != null)
                    await _publishTask;
            }
            catch (OperationCanceledException)
            {
            }

            _cts.Dispose();
            _cts = null;
            _publishTask = null;

            OnStatusChanged?.Invoke("停止发布数据");
        }

        public void UpdateData(IEnumerable<RealTimeValue>? newData)
        {
            lock (_dataLock)
            {
                _currentData = newData?.ToList() ?? new List<RealTimeValue>();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await Stop();
            await _mqtt.DisposeAsync();
            _publishLock.Dispose();
        }
    }
}