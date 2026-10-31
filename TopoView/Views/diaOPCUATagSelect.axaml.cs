// OPC UA tree and context menus correspond to TopoData.DriverPanel.diaOPCUATagSelect.
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using auDAServer;
using Opc.Ua;

namespace TopoView.Views;

public sealed partial class diaOPCUATagSelect : Window
{
    private readonly ConnctDevice? _device;
    private readonly ObservableCollection<UaAttributeRow> _attributes = [];
    private readonly ObservableCollection<UaTreeNode> _roots = [];
    private readonly List<DBTag> _selected = [];
    private UAClientHelperAPI? _helper;
    private bool _isBusy;
    private bool _isClosed;
    public IReadOnlyList<DBTag> SelectedTags => _selected;

    public diaOPCUATagSelect()
    {
        InitializeComponent();
        descriptionListView.ItemsSource = _attributes;
        nodeTreeView.ItemsSource = _roots;
        nodeTreeView.AddHandler(TreeViewItem.ExpandedEvent, NodeExpanded);
        UiText.Apply(this);
    }
    public diaOPCUATagSelect(ConnctDevice device) : this()
    {
        _device = device;
        var cfg = device.UAParamter;
        discoveryTextBox.Text = $"[{cfg.ApplicationName}] [{cfg.SecurityMode}] [{cfg.securityPolicy}] [{cfg.EndpointUrl}]";
    }
    private async void Window_Opened(object? sender, EventArgs e)
    {
        if (Design.IsDesignMode || _device == null) return;
        await RunAsync(ConnectAsync);
    }
    private void Window_Closed(object? sender, EventArgs e)
    {
        _isClosed = true;
        try { _helper?.Disconnect(); }
        catch (Exception ex) { discoveryTextBox.Text = ex.Message; }
    }
    private async Task RunAsync(Func<Task> action)
    {
        if (_device == null || _isBusy || _isClosed || Design.IsDesignMode) return;
        _isBusy = true;
        nodeTreeView.IsEnabled = false;
        try { await action(); }
        catch (Exception ex) { discoveryTextBox.Text = ex.Message; }
        finally { _isBusy = false; nodeTreeView.IsEnabled = true; }
    }
    private async Task ConnectAsync()
    {
        if (_device == null) return;
        var helper = _helper ??= new UAClientHelperAPI();
        var cfg = _device.UAParamter;
        var endpoints = await Task.Run(() => helper.GetEndpoints(cfg.EndpointUrl));
        var endpoint = endpoints.FirstOrDefault(endpoint => endpoint.SecurityMode.ToString() == cfg.SecurityMode &&
            endpoint.SecurityPolicyUri.Split('#').Last() == cfg.securityPolicy.Split('#').Last())
            ?? throw new InvalidOperationException("没有找到与设备安全配置一致的端点，请先选择端点。");
        await helper.Connect(endpoint, cfg.UseUserLogIn, cfg.User, cfg.Psw, "TopoViewBrowser");
        if (_isClosed) { helper.Disconnect(); return; }
        foreach (var node in await Task.Run(() => helper.BrowseRoot())) _roots.Add(new UaTreeNode(node));
    }
    private async void NodeExpanded(object? sender, RoutedEventArgs e)
    {
        if (e.Source is TreeViewItem { DataContext: UaTreeNode node })
            await RunAsync(() => LoadChildrenAsync(node));
    }
    private async Task LoadChildrenAsync(UaTreeNode node)
    {
        if (node.Node == null || node.IsLoaded) return;
        var helper = GetConnectedHelper();
        var children = await Task.Run(() => helper.BrowseNode(node.Node));
        node.Children.Clear();
        foreach (var child in children) node.Children.Add(new UaTreeNode(child));
        node.IsLoaded = true;
    }
    private async void nodeTreeView_AfterSelect(object? sender, SelectionChangedEventArgs e) => await RunAsync(async () =>
    {
        if (nodeTreeView.SelectedItem is not UaTreeNode { Node: { } reference }) return;
        var id = NodeIdText(reference);
        var node = await Task.Run(() => GetConnectedHelper().ReadNode(id));
        _attributes.Clear();
        _attributes.Add(new UaAttributeRow("NodeId", id));
        _attributes.Add(new UaAttributeRow("名称", node.DisplayName.Text));
        _attributes.Add(new UaAttributeRow("节点类型", node.NodeClass.ToString()));
        if (node is VariableNode variable)
        {
            _attributes.Add(new UaAttributeRow("数据类型", variable.DataType.ToString()));
            _attributes.Add(new UaAttributeRow("访问权限", variable.AccessLevel.ToString()));
        }
    });
    private async void MenuGetThisItem_Click(object? sender, RoutedEventArgs e) => await RunAsync(async () =>
    {
        if (nodeTreeView.SelectedItem is UaTreeNode { Node: { } node }) await AddVariableAsync(node);
    });
    private async void MenuGetSubItems_Click(object? sender, RoutedEventArgs e) => await RunAsync(async () =>
    {
        if (nodeTreeView.SelectedItem is not UaTreeNode selected) return;
        await LoadChildrenAsync(selected);
        foreach (var child in selected.Children)
            if (child.Node != null) await AddVariableAsync(child.Node);
    });
    private async void MenuGetAllSubItems_Click(object? sender, RoutedEventArgs e) => await RunAsync(() => TraverseSelectedAsync(true));
    private async void MenuExpendSubItems_Click(object? sender, RoutedEventArgs e) => await RunAsync(() => TraverseSelectedAsync(false));
    private async Task TraverseSelectedAsync(bool collect)
    {
        if (nodeTreeView.SelectedItem is not UaTreeNode selected) return;
        var seen = new HashSet<string>();
        async Task Visit(UaTreeNode entry, int depth)
        {
            if (depth > 32 || seen.Count >= 10000) throw new InvalidOperationException("浏览达到安全上限，请缩小范围。");
            if (entry.Node == null || !seen.Add(NodeIdText(entry.Node))) return;
            if (collect) await AddVariableAsync(entry.Node);
            await LoadChildrenAsync(entry);
            if (!collect) entry.IsExpanded = true;
            foreach (var child in entry.Children) await Visit(child, depth + 1);
        }
        await Visit(selected, 0);
    }
    private void removeItemsToolStripMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        if (nodeTreeView.SelectedItem is UaTreeNode { Node: { } node } && _helper != null)
            _selected.RemoveAll(tag => tag.Address == NodeIdText(node));
    }
    private async Task AddVariableAsync(ReferenceDescription reference)
    {
        if (reference.NodeClass != NodeClass.Variable || _device == null) return;
        var id = NodeIdText(reference);
        if (_selected.Any(tag => tag.Address == id)) return;
        var helper = GetConnectedHelper();
        if (await Task.Run(() => helper.ReadNode(id)) is not VariableNode variable) return;
        var type = TypeInfo.GetBuiltInType(variable.DataType);
        var dataType = type switch
        {
            BuiltInType.Boolean => 0,
            BuiltInType.Byte => 3,
            BuiltInType.Int16 => 4,
            BuiltInType.UInt16 => 5,
            BuiltInType.Int32 => 6,
            BuiltInType.Float => 10,
            BuiltInType.Double => 11,
            BuiltInType.Int64 => 12,
            _ => 1
        };
        var name = reference.BrowseName.Name;
        if (_selected.Any(tag => tag.TagName == name)) name += "_" + _selected.Count;
        _selected.Add(new DBTag
        {
            CannelID = _device.CannelID,
            TagName = name,
            TagId = _device.CannelID + "." + name,
            Address = id,
            DataType = dataType,
            UOM = "",
            Description = reference.DisplayName.Text,
            RW = (variable.AccessLevel & AccessLevels.CurrentWrite) != 0 ? 0 : 1
        });
    }

    private UAClientHelperAPI GetConnectedHelper() => _helper
        ?? throw new InvalidOperationException("OPC UA 连接尚未建立。");

    private string NodeIdText(ReferenceDescription node)
    {
        var helper = GetConnectedHelper();
        return ExpandedNodeId.ToNodeId(node.NodeId, helper.Session.NamespaceUris).ToString();
    }
}

public sealed class UaTreeNode : INotifyPropertyChanged
{
    private bool _isExpanded;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ReferenceDescription? Node { get; }
    public string Label => Node?.DisplayName.Text ?? "…";
    public bool IsLoaded { get; set; }
    public bool IsExpanded
    {
        get => _isExpanded;
        set { _isExpanded = value; PropertyChanged?.Invoke(this, new(nameof(IsExpanded))); }
    }
    public ObservableCollection<UaTreeNode> Children { get; } = [];
    public UaTreeNode(ReferenceDescription? node)
    {
        Node = node;
        if (node != null) Children.Add(new UaTreeNode(null));
    }
}

public sealed record UaAttributeRow(string Name, string Value);
