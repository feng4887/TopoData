// Device configuration follows TopoData ucCannelCfg and its four protocol panels.
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using auDASLib;
using auDAServer;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class ucCannelCfg : BusinessPage
{
    private static readonly string[] Protocols = ["OPC UA", "Siemens Profinet", "Modbus TCP", "Modbus RTU"];
    private static readonly DeviceType[] ProtocolTypes = [DeviceType.OPCUA, DeviceType.ProfinetS7, DeviceType.ModbusTCP, DeviceType.ModbusRTU];
    private readonly ObservableCollection<TagEditorRow> _tagRows = [];
    private ConnctDevice? _currentDevice;

    public ucCannelCfg()
    {
        InitializeComponent();
        gridControl1.ItemsSource = _tagRows;
        cbCannel.ItemsSource = Protocols;
        cbCannel.SelectedIndex = 0;
        ucOPCUA.DiscoverRequested += async (sender, _) => await RunActionAsync(sender, DiscoverAsync);
        ucOPCUA.BrowseRequested += async (sender, _) => await RunActionAsync(sender, BrowseAsync);
        EnsureNewRow();
        UiText.Apply(this);
    }

    public ucCannelCfg(AppContextServices context) : this()
    {
        InitializeRuntime(context);
        Refresh();
    }

    public override void Refresh()
    {
        if (!HasRuntimeContext) return;
        _currentDevice = Context.Profile.Server.cDevices.FirstOrDefault();
        var device = _currentDevice;
        tbCannelID.Text = device?.CannelID ?? "";
        tbCannelID.IsReadOnly = device != null;
        navBarItem1.Content = device?.CannelID ?? UiText.Translate("我的设备");
        tbDescription.Text = device?.Description ?? "";
        cbCannel.SelectedIndex = Math.Max(0, Array.IndexOf(ProtocolTypes, device?.devType ?? DeviceType.OPCUA));
        cbActive.IsChecked = device?.iActive == 1;
        cbIsSimulator.IsChecked = device?.IsSimulator == true;
        ucOPCUA.LoadConfig(device?.UAParamter ?? new UACfg { SecurityMode = "None", securityPolicy = "None" });
        var ip = device?.IpCfg ?? new IpCfg { Port = 502, Rack = 0, Slot = 1 };
        ucProfinet.LoadConfig(ip);
        ucModbusTCP.LoadConfig(ip);
        ucModbusRTU.LoadConfig(device?.ComCfg ?? new ComCfg(), ip);
        _tagRows.Clear();
        if (device != null)
            foreach (var tag in Context.Profile.Tags.Where(tag => tag.CannelID == device.CannelID))
                _tagRows.Add(new TagEditorRow(tag));
        EnsureNewRow();
        UpdateProtocol();
    }

    public override void Commit()
    {
        if (!HasRuntimeContext || Context.IsRunning) return;
        gridControl1.CommitEdit();
        var id = tbCannelID.Text?.Trim() ?? "";
        if (_currentDevice == null && id.Length == 0) return;
        if (id.Length == 0 || id.Contains('.')) throw new InvalidDataException("设备名不能为空或包含英文句点。");
        var tags = _tagRows.Where(row => !IsEmpty(row)).Select(row => row.ToTag(id)).ToList();
        if (tags.GroupBy(tag => tag.TagId, StringComparer.OrdinalIgnoreCase).Any(group => group.Count() > 1))
            throw new InvalidDataException("点位名称重复。");
        var device = _currentDevice ?? new ConnctDevice
        {
            CannelID = id, IpCfg = new IpCfg { Port = 502, Slot = 1 },
            UAParamter = new UACfg { SecurityMode = "None", securityPolicy = "None" }
        };
        var type = ProtocolTypes[Math.Max(0, cbCannel.SelectedIndex)];
        if (type == DeviceType.ProfinetS7 && !PubFunction.IsValidIP(ucProfinet.Host))
            throw new InvalidDataException("IP 地址错误。");
        if (type == DeviceType.ModbusTCP && !PubFunction.IsValidIP(ucModbusTCP.Host))
            throw new InvalidDataException("IP 地址错误。");
        var ip = type == DeviceType.ProfinetS7 ? ReadProfinetConfig(device.IpCfg)
            : type == DeviceType.ModbusTCP ? ucModbusTCP.ReadConfig()
            : type == DeviceType.ModbusRTU ? ucModbusRTU.ReadAddressConfig(device.IpCfg) : device.IpCfg;
        var serial = type == DeviceType.ModbusRTU ? ucModbusRTU.ReadSerialConfig() : device.ComCfg;
        var ua = type == DeviceType.OPCUA ? ucOPCUA.ReadConfig() : device.UAParamter;
        device.Description = tbDescription.Text ?? "";
        device.devType = type;
        device.iActive = cbActive.IsChecked == true ? 1 : 0;
        device.IsSimulator = cbIsSimulator.IsChecked == true;
        // 原版每个驱动分支都写死这两项；OPC UA/OPC DA 的 Address 记为 "2"，其余为 "1"。
        device.Address = type == DeviceType.OPCUA ? "2" : "1";
        device.SampleInterval = 20;
        device.SampleQty = 10;
        device.IpCfg = ip; device.ComCfg = serial; device.UAParamter = ua;
        if (_currentDevice == null) Context.Profile.Server.cDevices.Add(device);
        _currentDevice = device;
        LimitFreeVersionTags(tags);
        Context.Profile.Tags.RemoveAll(tag => tag.CannelID == device.CannelID);
        Context.Profile.Tags.AddRange(tags);
        EnsureNewRow();
    }

    /// <summary>Mirrors the original Profinet rack/slot parse error message.</summary>
    private IpCfg ReadProfinetConfig(IpCfg original)
    {
        try { return ucProfinet.ReadConfig(original); }
        catch (InvalidDataException) { throw new InvalidDataException("解析profinet rack/slot地址错误。"); }
    }

    /// <summary>Mirrors the free-version tag limit applied before saving the tag table.</summary>
    private static void LimitFreeVersionTags(List<DBTag> tags)
    {
        if (!DACtrl.free_Version || tags.Count <= DACtrl.free_CannelTags) return;
        for (var i = tags.Count - 1; i >= DACtrl.free_CannelTags; i--) tags.RemoveAt(i);
    }

    private static bool IsEmpty(TagEditorRow row) => string.IsNullOrWhiteSpace(row.Name) && string.IsNullOrWhiteSpace(row.Address);
    private void EnsureNewRow()
    {
        if (_tagRows.Count == 0 || !IsEmpty(_tagRows[^1])) _tagRows.Add(new TagEditorRow());
    }
    private void gridControl1_RowEditEnded(object? sender, DataGridRowEditEndedEventArgs e) => EnsureNewRow();
    private void gridControl1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Insert) return;
        EnsureNewRow();
        gridControl1.SelectedItem = _tagRows[^1];
        gridControl1.ScrollIntoView(_tagRows[^1], gridControl1.Columns[0]);
        gridControl1.BeginEdit();
        e.Handled = true;
    }
    private void cbCannel_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ucOPCUA != null) UpdateProtocol();
    }
    private void UpdateProtocol()
    {
        ucOPCUA.IsVisible = cbCannel.SelectedIndex == 0;
        ucProfinet.IsVisible = cbCannel.SelectedIndex == 1;
        ucModbusTCP.IsVisible = cbCannel.SelectedIndex == 2;
        ucModbusRTU.IsVisible = cbCannel.SelectedIndex == 3;
    }
    private async void btConfirm_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        if (string.IsNullOrWhiteSpace(tbCannelID.Text)) throw new InvalidDataException("设备名不能为空。");
        var overLimit = DACtrl.free_Version &&
            _tagRows.Count(row => !IsEmpty(row)) > DACtrl.free_CannelTags;
        SaveChanges();
        if (overLimit)
            await Ui.MessageAsync(this, "信息",
                $"免费版本导入变量点数量不能超过 {DACtrl.free_CannelTags} 个。");
    });
    private async void btImport_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, ImportTagsAsync);
    private async void btExport_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, ExportTagsAsync);
    private async void btDelete_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, DeleteTagsAsync);
    private async Task DeleteTagsAsync()
    {
        var selected = gridControl1.SelectedItems.Cast<TagEditorRow>().Where(row => !IsEmpty(row)).ToList();
        if (selected.Count == 0 || !await Ui.ConfirmAsync(this, $"确认删除选中的 {selected.Count} 行？")) return;
        foreach (var tag in selected) _tagRows.Remove(tag);
        EnsureNewRow();
    }
    /// <summary>Mirrors btImport_Click: the imported sheet replaces the channel's current tags.</summary>
    private async Task ImportTagsAsync()
    {
        Commit();
        if (_currentDevice == null) throw new InvalidOperationException("请先保存设备配置。");
        var path = await Ui.OpenAsync(this, "导入点表", "xlsx");
        if (path == null) return;
        var imported = await Task.Run(() => TagExchangeService.ReadTags(path, _currentDevice.CannelID));
        imported = imported.Where(tag => !string.IsNullOrWhiteSpace(tag.TagName))
            .OrderBy(tag => tag.TagName).ToList();
        LimitFreeVersionTags(imported);
        // 原版导入是整表替换：先移除该通道的旧点位，再写入表格内容并立即保存点表。
        _tagRows.Clear();
        foreach (var tag in imported) _tagRows.Add(new TagEditorRow(tag));
        EnsureNewRow();
        Context.Profile.Tags.RemoveAll(tag => tag.CannelID == _currentDevice.CannelID);
        Context.Profile.Tags.AddRange(imported);
        Context.Save();
        await Ui.MessageAsync(this, "信息", $"导入变量点{imported.Count}个。");
    }

    /// <summary>Mirrors btExport_Click: warn on an empty tag table, then report the exported count.</summary>
    private async Task ExportTagsAsync()
    {
        Commit();
        if (_currentDevice == null) return;
        var tags = Context.Profile.Tags
            .Where(tag => tag.CannelID == _currentDevice.CannelID).ToList();
        if (tags.Count == 0 && !await Ui.ConfirmAsync(this, "System will export demo tag list."))
            return;

        var path = await Ui.SaveAsync(this, "导出点表", _currentDevice.CannelID + "-tags.xlsx", "xlsx");
        if (path == null) return;
        var rows = tags.Count > 0 ? tags : DemoTagList();
        await Task.Run(() => TagExchangeService.WriteTags(path, rows));
        await Ui.MessageAsync(this, "信息", $"共导出 {rows.Count} 条数据。");
    }

    /// <summary>Demo rows used by the original export when the channel has no tags.</summary>
    private List<DBTag> DemoTagList() => cbCannel.SelectedIndex switch
    {
        1 =>
        [
            new DBTag { TagName = "A_trg", Address = "DB300.DBX0.0", DataType = 0, UOM = "-" },
            new DBTag { TagName = "A_Line", Address = "DB300.DBD4", DataType = 6, UOM = "-" },
            new DBTag { TagName = "D_NetWeight", Address = "DB300.DBD48", DataType = 10, UOM = "-" },
            new DBTag { TagName = "Temp1", Address = "DB301.string0.10", DataType = 1, UOM = "-" },
            new DBTag { TagName = "Temp2", Address = "DB301.wstring12.10", DataType = 2, UOM = "-" },
            new DBTag { TagName = "state", Address = "DB302.DBW0", DataType = 5, UOM = "-" }
        ],
        2 or 3 =>
        [
            new DBTag { TagName = "HR1", Address = "40001", DataType = 6, UOM = "-", Description = "Holding Register 1" },
            new DBTag { TagName = "HR2", Address = "40001", DataType = 5, UOM = "-", Description = "Holding Register 2" },
            new DBTag { TagName = "HR3", Address = "40001", DataType = 11, UOM = "-", Description = "Holding Register 3" },
            new DBTag { TagName = "HR4", Address = "40001", DataType = 10, UOM = "-", Description = "Holding Register 4" },
            new DBTag { TagName = "HR5", Address = "40001", DataType = 4, UOM = "-", Description = "Holding Register 5" },
            new DBTag { TagName = "HR6", Address = "40001", DataType = 12, UOM = "-", Description = "Holding Register 6" },
            new DBTag { TagName = "HR7", Address = "40001", DataType = 5, UOM = "-", Description = "Holding Register 7" },
            new DBTag { TagName = "IR1", Address = "30001", DataType = 5, UOM = "-", Description = "Input Register" },
            new DBTag { TagName = "Input1", Address = "10001", DataType = 0, UOM = "-", Description = "Input Status" },
            new DBTag { TagName = "Coil1", Address = "00001", DataType = 0, UOM = "-", Description = "Coil Status" }
        ],
        _ =>
        [
            new DBTag { TagName = "cip_CipPress", Address = "ns=2;s=模拟器示例.函数.Random1", DataType = 10, UOM = "-" },
            new DBTag { TagName = "cip_CipTemp", Address = "ns=2;s=模拟器示例.函数.Random2", DataType = 10, UOM = "-" },
            new DBTag { TagName = "pkg_Bucket", Address = "ns=2;s=模拟器示例.函数.Sine1", DataType = 1, UOM = "-" }
        ]
    };
    private async Task DiscoverAsync()
    {
        var url = ucOPCUA.EndpointUrl;
        var endpoints = await Task.Run(() => new UAClientHelperAPI().GetEndpoints(url));
        if (endpoints.Count == 0) throw new InvalidOperationException("没有发现可用端点。");
        ucOPCUA.SetEndpoints(endpoints);
    }
    private async Task BrowseAsync()
    {
        Commit();
        if (_currentDevice == null) return;
        var browser = new diaOPCUATagSelect(_currentDevice);
        await browser.ShowDialog((Window)TopLevel.GetTopLevel(this)!);
        foreach (var row in _tagRows.Where(IsEmpty).ToList()) _tagRows.Remove(row);
        // 原版 UcOPCUA_TagEvent 的合并规则：入表前先按名称去重并跳过已有名称（均不区分
        // 大小写），避免保存时才因"点位名称重复"被卡住。
        var existingNames = new HashSet<string>(
            _tagRows.Select(row => row.Name), StringComparer.OrdinalIgnoreCase);
        var incoming = browser.SelectedTags
            .Where(tag => !string.IsNullOrEmpty(tag.TagName))
            .GroupBy(tag => tag.TagName, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First());
        foreach (var tag in incoming.Where(tag => existingNames.Add(tag.TagName)))
            _tagRows.Add(new TagEditorRow(tag));
        EnsureNewRow();
    }
}

public sealed class TagEditorRow
{
    public static readonly string[] AccessModes = ["读写", "只读", "只写"];
    public static readonly string[] Types = ["bool", "string", "wstring", "byte", "int16", "word", "int", "float", "double", "int64"];
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string Type { get; set; } = "float";
    public string Unit { get; set; } = "";
    public string Description { get; set; } = "";
    public string Access { get; set; } = "读写";
    public string DefaultValue { get; set; } = "";
    private readonly DBTag? _source;

    public TagEditorRow() { }

    public TagEditorRow(DBTag tag)
    {
        _source = tag;
        Name = tag.TagName;
        Address = tag.Address;
        Type = DBTag.DataTypeString(tag.DataType);
        Unit = tag.UOM;
        Description = tag.Description;
        DefaultValue = tag.DefaultValue;
        Access = tag.RW == 1 ? "只读" : tag.RW == 2 ? "只写" : "读写";
    }

    public DBTag ToTag(string channel)
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Address) || DBTag.GetDataType(Type) < 0)
            throw new InvalidDataException("点位名称、地址和数据类型必须完整。");
        return new DBTag
        {
            Class = _source?.Class ?? 0,
            isInternalValue = _source?.isInternalValue ?? false,
            RegID = _source?.RegID ?? "",
            ListviewIndex = _source?.ListviewIndex ?? 0,
            CannelID = channel,
            TagName = Name.Trim(),
            TagId = channel + "." + Name.Trim(),
            Address = Address.Trim(),
            DataType = DBTag.GetDataType(Type),
            UOM = Unit ?? "",
            Description = Description ?? "",
            DefaultValue = DefaultValue ?? "",
            RW = Access == "只读" ? 1 : Access == "只写" ? 2 : 0
        };
    }
}
