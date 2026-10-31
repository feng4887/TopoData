//=============================================================================
// 对齐 TopoData：auDASLib 配置目录约定（pubDefine.Folder）与五个 XML 配置文件
//               （hiTopoCfg / hiTopoTagDef / hiTopoRecipeDef / hiTopoPythonScriptDef /
//               mqtt_config），与旧 TopoData 共用同一组配置文件。
// Description: XML 配置读写、校验、打包导出与旧配置导入。
//=============================================================================
// Reads and writes the TopoData XML contract. The default directory is OS dependent
// (see AppContextServices.DefaultConfigDirectory): Windows keeps TopoData's shared
// production folder, Linux uses the Cfg folder next to the application executable.
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using auDASLib;

namespace TopoView.Services;

/// <summary>一组完整配置的内存模型：设备、点表、配方、脚本与 MQTT。</summary>
public sealed class WorkspaceProfile
{
    public ServerCfg Server { get; set; } = new();
    public List<DBTag> Tags { get; set; } = [];
    public master_recipe Recipes { get; set; } = new();
    public PythonScriptConfig Scripts { get; set; } = new();
    public MqttServerConfig Mqtt { get; set; } = new();
}

// The XML name and property names match TopoData's mqtt_config.xml; the defaults are the values
// MqttOperator.LoadConfig wrote when the legacy application created mqtt_config.xml for the first time.
public sealed class MqttServerConfig
{
    public bool EnableMqtt { get; set; }
    public bool UseSecurity { get; set; }
    public string Server { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 1883;
    public string Username { get; set; } = "admin";
    public string Password { get; set; } = "password";
    public string ClientId { get; set; } = "RealTimePublisher";
    public string TopicPrefix { get; set; } = "industrial/realtime";
    public int PublishIntervalMs { get; set; } = 2000;
    public bool PublishSingleTag { get; set; }
    public bool Retain { get; set; }
}

/// <summary>XML 配置读写器：与旧 TopoData 共用同一组配置文件、目录与格式约定。</summary>
public sealed class ConfigurationStore(string directory)
{
    /// <summary>旧版五个配置文件名，顺序与 Load/Save 的读写一一对应。</summary>
    public static readonly string[] FileNames =
        ["hiTopoCfg.xml", "hiTopoTagDef.xml", "hiTopoRecipeDef.xml", "hiTopoPythonScriptDef.xml", "mqtt_config.xml"];
    public string DirectoryPath { get; } = Path.GetFullPath(directory);

    /// <summary>Loads a complete profile; malformed files are reported instead of overwritten.</summary>
    public WorkspaceProfile Load()
    {
        var profile = CreateEmpty();
        profile.Server  = ReadFile(FileNames[0], profile.Server);
        profile.Tags    = ReadFile(FileNames[1], profile.Tags);
        profile.Recipes = ReadFile(FileNames[2], profile.Recipes);
        profile.Scripts = ReadFile(FileNames[3], profile.Scripts);
        profile.Mqtt    = ReadFile(FileNames[4], profile.Mqtt);
        Normalize(profile);
        return profile;
    }

    /// <summary>空配置模板：默认本地 SQLite 库，REST 与 MQTT 均关闭。</summary>
    public WorkspaceProfile CreateEmpty() => new()
    {
        Server = new ServerCfg
        {
            Localization = "zh", RestUri1 = "http://127.0.0.1:8093", RestEnable = false,
            SqlConfigInfo = new SQLCfg
            {
                DbType = "SqlLite", DbName = Path.Combine(DirectoryPath, "topoview.db"),
                ServerName = "localhost", User = "", Psw = "", Port = 0
            }
        }
    };

    /// <summary>Atomically replaces each XML file and retains the preceding version as .bak.</summary>
    public void Save(WorkspaceProfile profile)
    {
        Normalize(profile);
        Validate(profile);
        Directory.CreateDirectory(DirectoryPath);
        WriteFile(FileNames[0], profile.Server);
        WriteFile(FileNames[1], profile.Tags);
        WriteFile(FileNames[2], profile.Recipes);
        WriteFile(FileNames[3], profile.Scripts);
        WriteFile(FileNames[4], profile.Mqtt);
    }

    /// <summary>保存前校验：设备驱动类型、标识与采样参数，点表完整性与唯一性，MQTT 端口与周期。</summary>
    public static void Validate(WorkspaceProfile profile)
    {
        var channels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var device in profile.Server.cDevices)
        {
            if (device.devType is not (DeviceType.OPCUA or DeviceType.ProfinetS7 or DeviceType.ModbusTCP or DeviceType.ModbusRTU))
                throw new InvalidDataException($"设备 {device.CannelID} 的驱动不受支持，请先移除该设备。");
            if (string.IsNullOrWhiteSpace(device.CannelID) || device.CannelID.Contains('.') || !channels.Add(device.CannelID))
                throw new InvalidDataException("设备标识不能为空、重复或包含英文句点。");
            // The legacy page always wrote 20 ms / 10 samples and the drivers clamp the interval to
            // 50..3000 ms, so existing TopoData configurations must keep passing this check.
            if (device.SampleInterval < 20 || device.SampleQty < 1)
                throw new InvalidDataException($"{device.CannelID}：采样周期至少 20 ms，采样数量必须大于零。");
        }
        var tagNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var tag in profile.Tags)
        {
            if (!channels.Contains(tag.CannelID) || string.IsNullOrWhiteSpace(tag.TagName) || string.IsNullOrWhiteSpace(tag.Address))
                throw new InvalidDataException("每个点位必须具有有效设备、名称及地址。");
            if (string.IsNullOrEmpty(DBTag.DataTypeString(tag.DataType)) || !tagNames.Add(tag.TagId))
                throw new InvalidDataException($"点位 {tag.TagId} 重复或数据类型无效。");
            if (tag.RW is < 0 or > 2) throw new InvalidDataException($"{tag.TagId} 的读写类型必须为 0、1 或 2。");
        }
        if (profile.Mqtt.Port is < 1 or > 65535 || profile.Mqtt.PublishIntervalMs < 100)
            throw new InvalidDataException("MQTT 端口或发布周期无效。");
    }

    /// <summary>先保存当前配置，再把五个 XML 打包为 ZIP，用于跨机器迁移。</summary>
    public void ExportZip(WorkspaceProfile profile, string path)
    {
        Save(profile);
        var temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var archive = ZipFile.Open(temporary, ZipArchiveMode.Create))
                foreach (var name in FileNames)
                    archive.CreateEntryFromFile(Path.Combine(DirectoryPath, name), name);
            File.Move(temporary, path, true);
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }

    /// <summary>Imports whitelisted XML entries only; OPC DA is explicitly excluded.</summary>
    public (WorkspaceProfile Profile, string Message) Import(string path)
    {
        var staging = Path.Combine(DirectoryPath, "import-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(staging);
        try
        {
            if (Directory.Exists(path))
            {
                foreach (var name in FileNames)
                {
                    var source = Path.Combine(path, name);
                    if (File.Exists(source)) File.Copy(source, Path.Combine(staging, name));
                }
            }
            else
            {
                using var archive = ZipFile.OpenRead(path);
                foreach (var name in FileNames)
                {
                    var entries = archive.Entries.Where(e => e.Name == name).ToList();
                    if (entries.Count > 1) throw new InvalidDataException($"压缩包包含重复配置：{name}");
                    if (entries.Count == 1)
                    {
                        if (entries[0].Length > 32 * 1024 * 1024) throw new InvalidDataException("配置文件超过 32 MB。");
                        entries[0].ExtractToFile(Path.Combine(staging, name));
                    }
                }
            }
            if (!File.Exists(Path.Combine(staging, FileNames[0])))
                throw new InvalidDataException("所选配置中没有 hiTopoCfg.xml。");
            var profile = new ConfigurationStore(staging).Load();
            var excluded = profile.Server.cDevices.Where(d => d.devType is not
                (DeviceType.OPCUA or DeviceType.ProfinetS7 or DeviceType.ModbusTCP or DeviceType.ModbusRTU)).ToList();
            var excludedIds = excluded.Select(d => d.CannelID).ToHashSet(StringComparer.OrdinalIgnoreCase);
            profile.Server.cDevices.RemoveAll(d => excludedIds.Contains(d.CannelID));
            profile.Tags.RemoveAll(t => excludedIds.Contains(t.CannelID));
            bool IsExcluded(string? tag) => !string.IsNullOrWhiteSpace(tag) && excludedIds.Any(id => tag.StartsWith(id + ".", StringComparison.OrdinalIgnoreCase));
            foreach (var task in profile.Server.dataSamples.Where(t => t.Samples.Any(s => excludedIds.Contains(s.CannelID) || IsExcluded(s.TagID)) ||
                IsExcluded(t.wo) || IsExcluded(t.SampleTrig.dataSampleTrig_OnTrue.TagName) || IsExcluded(t.SampleTrig.dataSampleTrig_OnTrueOnFalse.TagName) ||
                IsExcluded(t.SampleTrig.dataSampleTrig_TimeWindow.TagName) || t.SampleTrig.ExpressionTags.Any(t => IsExcluded(t.TagId))))
                task.bActive = false;
            foreach (var script in profile.Scripts.Scripts.Where(s => excludedIds.Any(id =>
                s.Trigger.TagName.StartsWith(id + ".", StringComparison.OrdinalIgnoreCase) ||
                s.Trigger.ExpressionTags.Any(t => excludedIds.Contains(t.CannelID)))))
                script.bActive = false;
            // Import is configuration-only. Starting services always remains a separate action.
            profile.Server.AutoStart = false;
            Save(profile);
            return (profile, excluded.Count == 0 ? "配置导入成功。" :
                $"配置导入成功；已排除不支持的设备：{string.Join("、", excludedIds)}，关联存储任务已禁用。请检查脚本和配方引用。");
        }
        finally
        {
            // Only delete the uniquely created staging directory inside this profile.
            if (Path.GetDirectoryName(staging) == DirectoryPath) Directory.Delete(staging, true);
        }
    }

    /// <summary>离线演示配置：一个内存仿真设备、四个点位和一个演示配方（--demo 专用，不接触生产配置）。</summary>
    public WorkspaceProfile CreateDemo()
    {
        var profile = CreateEmpty();
        profile.Server.cDevices.Add(new ConnctDevice
        {
            CannelID = "Reactor01", Description = "离线仿真设备", IsSimulator = true, devType = DeviceType.OPCUA
        });
        foreach (var (name, type, unit, value) in new[]
        {
            ("Temperature", 10, "°C", "25"), ("Pressure", 10, "bar", "1.2"),
            ("Running", 0, "", "false"), ("Batch", 1, "", "DEMO-001")
        })
            profile.Tags.Add(new DBTag { CannelID = "Reactor01", TagName = name, Address = "ns=2;s=" + name,
                DataType = type, UOM = unit, Description = name, DefaultValue = value });
        profile.Recipes.equipment_recipes.Add(new equipment_recipe
        {
            equipment_id = "Recipe01", equipment_name = "演示配方", description = "只作用于仿真内存点位",
            recipe_status = RecipeStatus.InUse,
            recipe_data = [new recipe_data { parameter_name = "目标温度", tag_name = "Reactor01.Temperature", data_type = "float", value = "35", uom = "°C" }]
        });
        Normalize(profile);
        return profile;
    }

    /// <summary>补齐缺省集合与触发子对象，并把点表 TagId 规范为“设备.名称”。</summary>
    public static void Normalize(WorkspaceProfile profile)
    {
        // The shared acquisition engines resolve database types with case-sensitive spellings
        // ("SqlLite", "MySql"); normalise the display values used by the settings page so both
        // sides read the same configuration.
        var sql = profile.Server.SqlConfigInfo;
        if (sql is not null)
        {
            if (sql.DbType?.Contains("lite", StringComparison.OrdinalIgnoreCase) == true) sql.DbType = "SqlLite";
            else if (string.Equals(sql.DbType, "MySQL", StringComparison.OrdinalIgnoreCase)) sql.DbType = "MySql";
        }
        profile.Server.cDevices ??= [];
        profile.Server.dataSamples ??= [];
        profile.Recipes.equipment_recipes ??= [];
        profile.Scripts.Scripts ??= [];
        foreach (var task in profile.Server.dataSamples)
        {
            task.Samples ??= [];
            task.SampleTrig ??= new DataSampleTrig();
            task.SampleTrig.ExpressionTags ??= [];
            task.SampleTrig.dataSampleTrig_OnTrue ??= new DataSampleTrig_OnTrue();
            task.SampleTrig.dataSampleTrig_OnTrueOnFalse ??= new DataSampleTrig_OnTrueOnFalse();
            task.SampleTrig.dataSampleTrig_TimeWindow ??= new DataSampleTrig_TimeWindow();
        }
        foreach (var script in profile.Scripts.Scripts) { script.Trigger ??= new PythonScriptTrigger(); script.Trigger.TagName ??= ""; script.Trigger.ExpressionTags ??= []; }
        foreach (var tag in profile.Tags) tag.TagId = $"{tag.CannelID}.{tag.TagName}";
    }

    /// <summary>Resolves a stored tag reference to its full tag id. Legacy TopoData files store the
    /// complete "Cannel.TagName" id in TagName with TagID and CannelID left empty, while TopoView
    /// stores the full id in TagID; both forms must resolve to the same canonical tag key.</summary>
    public static string ResolveTagId(string? tagId, string? tagName, string? cannelId) =>
        !string.IsNullOrWhiteSpace(tagId) ? tagId
        : !string.IsNullOrWhiteSpace(tagName) && tagName.Contains('.') ? tagName
        : $"{cannelId}.{tagName}";

    private T ReadFile<T>(string name, T fallback)
    {
        var path = Path.Combine(DirectoryPath, name);
        if (!File.Exists(path)) return fallback;
        using var reader = XmlReader.Create(path, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null });
        return (T?)new XmlSerializer(typeof(T)).Deserialize(reader) ?? throw new InvalidDataException($"无法读取 {name}");
    }

    private void WriteFile<T>(string name, T value)
    {
        var path = Path.Combine(DirectoryPath, name);
        var temporary = path + ".tmp";
        using (var writer = XmlWriter.Create(temporary, new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) }))
            new XmlSerializer(typeof(T)).Serialize(writer, value);
        if (File.Exists(path)) File.Copy(path, path + ".bak", true);
        File.Move(temporary, path, true);
    }
}
