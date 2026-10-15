//=============================================================================
// Austar Group
// (c)Copyright (2026) All Rights Reserved
//说明：OPC DA 点位枚举与选择对话框。
//-----------------------------------------------------------------------------
//作者：杜金旺
//日期：2026-07-17
//版本：1.0
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using auDASLib;
using OpcDaNetCore.Contracts;
using OpcDaNetCore.Factory;
using OpcDaNetCore.ValueObjects;
using TopoData.Properties;
using TopoData.model;

namespace TopoData
{
    public partial class diaOPCDATagSelect : Form
    {
        private const string BrowseGroupName = "TopoDataBrowse";
        private readonly CancellationTokenSource _formCancellation = new CancellationTokenSource();
        private IOpcDaService? _client;
        private bool _readGroupCreated;

        public string HostName { get; set; } = "localhost";
        public string Endpoint { get; set; } = "";

        public diaOPCDATagSelect()
        {
            InitializeComponent();
        }

        private async void diaOPCDATagSelect_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            nodeTreeView.IsMulSelect = true;
            nodeTreeView.ImageList = imageList1;
            discoveryTextBox.Text = $"[{HostName}] [{Endpoint}]";
            await ConnectDAServerAsync();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        /// <summary>
        /// 应用界面语言资源。
        /// </summary>
        public void ApplyLanguage()
        {
            label1.Text = Resources.ServerNode;
            tabPage1.Text = Resources.Config;
            TagName.Text = Resources.Attribute;
            Value.Text = Resources.Value;
            endpointLabel.Text = "OPC DA Server：";
        }

        /// <summary>
        /// 连接 OPC DA 服务并加载根节点。
        /// </summary>
        private async Task ConnectDAServerAsync()
        {
            if (_client?.IsConnected == true)
                return;

            if (string.IsNullOrWhiteSpace(HostName) || string.IsNullOrWhiteSpace(Endpoint))
            {
                MessageBox.Show("OPC DA 主机名或 Server 名称为空。", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UseWaitCursor = true;
            try
            {
                _client = await new OpcDaFactory()
                    .WithIp(HostName.Trim())
                    .WithServerName(Endpoint.Trim())
                    .BuildAsync(_formCancellation.Token);

                await BindRootAsync(_formCancellation.Token);
            }
            catch (OperationCanceledException)
            {
                // 窗体关闭时取消连接或浏览，不再显示错误。
            }
            catch (Exception ex)
            {
                ResetUI();
                MessageBox.Show($"连接 OPC DA Server 失败：{ex.Message}", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        /// <summary>
        /// 断开 OPC DA 连接并释放 COM 资源。
        /// </summary>
        public void CloseSession()
        {
            _formCancellation.Cancel();
            try
            {
                _client?.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OPC DA dispose failed: {ex.Message}");
            }
            finally
            {
                _client = null;
                _readGroupCreated = false;
            }
        }

        private void ResetUI()
        {
            descriptionListView.Items.Clear();
            nodeTreeView.Nodes.Clear();
        }

        /// <summary>
        /// 加载 OPC DA 根地址空间。
        /// </summary>
        private async Task BindRootAsync(CancellationToken cancellationToken)
        {
            IOpcDaService client = _client ?? throw new InvalidOperationException("OPC DA server is not connected.");
            IEnumerable<BrowseItem> items = await client.BrowseNodeAsync(null, cancellationToken);

            nodeTreeView.BeginUpdate();
            try
            {
                nodeTreeView.Nodes.Clear();
                foreach (BrowseItem item in items.OrderBy(x => x.Name, StringComparer.CurrentCultureIgnoreCase))
                    nodeTreeView.Nodes.Add(CreateTreeNode(item));
            }
            finally
            {
                nodeTreeView.EndUpdate();
            }
        }

        private static TreeNode CreateTreeNode(BrowseItem item)
        {
            var node = new TreeNode(item.Name)
            {
                Tag = item,
                ImageIndex = item.HasChildren ? 0 : 1,
                SelectedImageIndex = item.HasChildren ? 0 : 1
            };

            if (item.HasChildren)
                node.Nodes.Add(new TreeNode { Tag = null });

            return node;
        }

        private async void nodeTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!HasPlaceholder(e.Node))
                return;

            e.Cancel = true;
            try
            {
                await EnsureChildrenLoadedAsync(e.Node, _formCancellation.Token);
                e.Node.Expand();
            }
            catch (OperationCanceledException)
            {
                // 窗体关闭时取消浏览。
            }
            catch (Exception ex)
            {
                MessageBox.Show($"浏览 OPC DA 节点失败：{ex.Message}", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool HasPlaceholder(TreeNode node)
        {
            return node.Nodes.Count == 1 && node.Nodes[0].Tag == null;
        }

        private async Task EnsureChildrenLoadedAsync(TreeNode node, CancellationToken cancellationToken)
        {
            if (!HasPlaceholder(node))
                return;
            if (node.Tag is not BrowseItem parent)
                return;

            IOpcDaService client = _client ?? throw new InvalidOperationException("OPC DA server is not connected.");
            IEnumerable<BrowseItem> children = await client.BrowseNodeAsync(parent.Id, cancellationToken);

            node.Nodes.Clear();
            foreach (BrowseItem child in children.OrderBy(x => x.Name, StringComparer.CurrentCultureIgnoreCase))
                node.Nodes.Add(CreateTreeNode(child));
        }

        private async void nodeTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is not BrowseItem item)
                return;

            descriptionListView.Items.Clear();
            AddDescription("Name", item.Name);
            AddDescription("Item ID", item.Id);
            AddDescription("Has Children", item.HasChildren.ToString());

            if (item.HasChildren || _client == null)
                return;

            try
            {
                ItemDataValue? value = await ReadItemAsync(item.Id);
                AddDescription("Data Type", GetDataTypeName(value?.Value));
                AddDescription("Value", value?.Value?.ToString() ?? "");
            }
            catch (Exception ex)
            {
                AddDescription("Read Error", ex.Message);
            }
        }

        private void AddDescription(string name, string value)
        {
            var row = new ListViewItem(name);
            row.SubItems.Add(value);
            descriptionListView.Items.Add(row);
        }

        private async Task<ItemDataValue?> ReadItemAsync(string itemId)
        {
            IOpcDaService client = _client ?? throw new InvalidOperationException("OPC DA server is not connected.");
            EnsureReadGroup(client, itemId);
            return await Task.Run(() => client.Read(BrowseGroupName, itemId).FirstOrDefault(), _formCancellation.Token);
        }

        private void EnsureReadGroup(IOpcDaService client, string firstItemId)
        {
            if (_readGroupCreated)
                return;

            client.AddItems(BrowseGroupName, firstItemId);
            _readGroupCreated = true;
        }

        private async void MenuGetThisItem_Click(object sender, EventArgs e)
        {
            if (nodeTreeView.CurrentNode?.Tag is not BrowseItem item)
                return;

            try
            {
                await AddBrowseItemsAsync(new[] { item });
            }
            catch (OperationCanceledException)
            {
                // 窗体关闭时取消读取。
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取 OPC DA 点位失败：{ex.Message}", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void MenuGetSubItems_Click(object sender, EventArgs e)
        {
            try
            {
                var leaves = new List<BrowseItem>();
                foreach (TreeNode node in GetSelectedNodes())
                {
                    if (node.Tag is not BrowseItem item || !item.HasChildren)
                        continue;

                    IOpcDaService client = _client ?? throw new InvalidOperationException("OPC DA server is not connected.");
                    IEnumerable<BrowseItem> children = await client.BrowseNodeAsync(item.Id, _formCancellation.Token);
                    leaves.AddRange(children.Where(x => !x.HasChildren));
                }

                await AddBrowseItemsAsync(leaves);
            }
            catch (OperationCanceledException)
            {
                // 窗体关闭时取消浏览。
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取 OPC DA 子节点失败：{ex.Message}", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void MenuGetAllSubItems_Click(object sender, EventArgs e)
        {
            try
            {
                var leaves = new List<BrowseItem>();
                foreach (TreeNode node in GetSelectedNodes())
                {
                    if (node.Tag is BrowseItem item)
                        leaves.AddRange(await BrowseLeafItemsAsync(item, _formCancellation.Token));
                }

                await AddBrowseItemsAsync(leaves);
            }
            catch (OperationCanceledException)
            {
                // 窗体关闭时取消浏览。
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取 OPC DA 全部子节点失败：{ex.Message}", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<List<BrowseItem>> BrowseLeafItemsAsync(BrowseItem root, CancellationToken cancellationToken)
        {
            IOpcDaService client = _client ?? throw new InvalidOperationException("OPC DA server is not connected.");
            var leaves = new List<BrowseItem>();
            var pending = new Stack<BrowseItem>();
            pending.Push(root);

            while (pending.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                BrowseItem item = pending.Pop();
                if (!item.HasChildren)
                {
                    leaves.Add(item);
                    continue;
                }

                IEnumerable<BrowseItem> children = await client.BrowseNodeAsync(item.Id, cancellationToken);
                foreach (BrowseItem child in children)
                    pending.Push(child);
            }

            return leaves;
        }

        private async void MenuExpendSubItems_Click(object sender, EventArgs e)
        {
            TreeNode? root = nodeTreeView.CurrentNode;
            if (root == null)
                return;

            try
            {
                var pending = new Stack<TreeNode>();
                pending.Push(root);
                while (pending.Count > 0)
                {
                    _formCancellation.Token.ThrowIfCancellationRequested();
                    TreeNode node = pending.Pop();
                    await EnsureChildrenLoadedAsync(node, _formCancellation.Token);
                    node.Expand();

                    for (int i = node.Nodes.Count - 1; i >= 0; i--)
                        pending.Push(node.Nodes[i]);
                }
            }
            catch (OperationCanceledException)
            {
                // 窗体关闭时取消展开。
            }
            catch (Exception ex)
            {
                MessageBox.Show($"展开 OPC DA 节点失败：{ex.Message}", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<TreeNode> GetSelectedNodes()
        {
            var nodes = nodeTreeView.SelectedNodes.Cast<TreeNode>().ToList();
            if (nodes.Count == 0 && nodeTreeView.CurrentNode != null)
                nodes.Add(nodeTreeView.CurrentNode);

            return nodes;
        }

        /// <summary>
        /// 读取叶子点位并通过事件发送给通道配置窗体。
        /// </summary>
        private async Task AddBrowseItemsAsync(IEnumerable<BrowseItem> browseItems)
        {
            BrowseItem[] items = browseItems
                .Where(x => x != null && !x.HasChildren && !string.IsNullOrWhiteSpace(x.Id))
                .GroupBy(x => x.Id, StringComparer.OrdinalIgnoreCase)
                .Select(x => x.First())
                .ToArray();

            if (items.Length == 0)
            {
                MessageBox.Show("当前选择中没有可添加的 OPC DA 点位。", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IOpcDaService client = _client ?? throw new InvalidOperationException("OPC DA server is not connected.");
            EnsureReadGroup(client, items[0].Id);
            string[] addresses = items.Select(x => x.Id).ToArray();

            Dictionary<string, object?> values;
            try
            {
                IEnumerable<ItemDataValue> readValues = await Task.Run(
                    () => client.Read(BrowseGroupName, addresses), _formCancellation.Token);
                values = readValues
                    .GroupBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(x => x.Key, x => x.First().Value, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine($"OPC DA data type read failed: {ex.Message}");
                values = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            }

            List<DBTag> tags = items.Select(item =>
            {
                values.TryGetValue(item.Id, out object? value);
                return new DBTag
                {
                    TagName = item.Name,
                    Address = item.Id,
                    DataType = GetDataType(value),
                    UOM = "",
                    Description = ""
                };
            }).ToList();

            OnTagEvent(new DBTagEventArgs("Add new tags", DeviceType.OPCDA, tags));
        }

        private static int GetDataType(object? value)
        {
            const int ArrayDataTypeOffset = 100;

            return value switch
            {
                bool          => DBTag.GetDataType("bool"),
                string        => DBTag.GetDataType("string"),
                byte or sbyte => DBTag.GetDataType("byte"),
                short         => DBTag.GetDataType("int16"),
                ushort        => DBTag.GetDataType("uint16"),
                int or uint   => DBTag.GetDataType("int32"),
                float  => DBTag.GetDataType("float"),
                double or decimal => DBTag.GetDataType("double"),
                long or ulong     => DBTag.GetDataType("int64"),
                bool[]            => DBTag.GetDataType("bool")       + ArrayDataTypeOffset,
                string[]          => DBTag.GetDataType("string")     + ArrayDataTypeOffset,
                byte[] or sbyte[]     => DBTag.GetDataType("byte")   + ArrayDataTypeOffset,
                short[]               => DBTag.GetDataType("int16")  + ArrayDataTypeOffset,
                ushort[]              => DBTag.GetDataType("uint16") + ArrayDataTypeOffset,
                int[] or uint[]       => DBTag.GetDataType("int32")  + ArrayDataTypeOffset,
                float[]               => DBTag.GetDataType("float")  + ArrayDataTypeOffset,
                double[] or decimal[] => DBTag.GetDataType("double") + ArrayDataTypeOffset,
                long[] or ulong[]     => DBTag.GetDataType("int64")  + ArrayDataTypeOffset,
                _ => -1
            };
        }

        private static string GetDataTypeName(object? value)
        {
            int dataType = GetDataType(value);
            return dataType < 0 ? value?.GetType().Name ?? "" : DBTag.DataTypeString(dataType);
        }

        public event EventHandler<DBTagEventArgs>? TagEvent;

        protected virtual void OnTagEvent(DBTagEventArgs e)
        {
            TagEvent?.Invoke(this, e);
        }
    }
}
