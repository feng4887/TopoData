#region Reference
using auDASLib;
using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.model;
using TopoData.Properties;

#endregion 

//=============================================================================
// Austar Group - AAS
// (c)Copyright (2021) All Rights Reserved
//----------------------------------------------------------------------------- 
//作者：杜金旺
//日期：2025-12-3
//版本：
//=============================================================================
namespace TopoData
{
    public partial class diaOPCUATagSelect : DevExpress.XtraEditors.XtraForm
    {

        /// <summary>
        /// Fields & deifination
        /// </summary>
        #region Fields
        public Session mySession;
        private Subscription mySubscription;
        ///// <summary>
        ///// Opc客户端的核心类
        ///// </summary>
        private UAClientHelperAPI myClientHelperAPI;
        private EndpointDescription mySelectedEndpoint;

        private List<MonitoredItem> myMonitoredItems;
        private List<String> myRegisteredNodeIdStrings;
        private ReferenceDescriptionCollection myReferenceDescriptionCollection;

        /// <summary>
        /// tag view for List view
        /// </summary>
        private Dictionary<string, DBTag> myTagView = new Dictionary<string, DBTag>();

        public string EndpointUrl { get; set; }
        public string SecurityMode { get; set; }
        public string SecurityPolicy { get; set; }
        public string ApplicationName { get; set; }
        public string User { get; set; } = "";
        public string Psw { get; set; } = "";
        public bool UsePsw { get; set; } = false;

        #endregion

        #region Form

        public diaOPCUATagSelect()
        {
            InitializeComponent();
            myClientHelperAPI = new UAClientHelperAPI();
            myRegisteredNodeIdStrings = new List<String>();
        }

        private void diaOPCUATagSelect_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            //必须设置
            nodeTreeView.IsMulSelect = true;
            nodeTreeView.ImageList = this.imageList1;
            discoveryTextBox.Text = "[" + ApplicationName + "] " + " [" + SecurityMode + "] " + " [" + SecurityPolicy + "] " + " [" + EndpointUrl + "]";
            connectUAServer();
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            Label1.Text = Resources.ServerNode;
            //tabPage1.Text = Resources.Config;
            TagName.Text = Resources.Attribute;
            Value.Text = Resources.Value;
            MenuGetSubItems.Text = Resources.MenuGetSubItems;
            MenuGetThisItem.Text = Resources.GetThisItem;
        }
        #endregion //Initial

        #region connect

        /// <summary>
        /// OPC UA connected
        /// </summary>
        private bool IsConnected
        {
            get
            {
                if (mySession != null && !mySession.Disposed)
                    return true;
                else
                    return false;
            }
        }

        private async Task connectUAServer()
        {
            //Check if sessions exists; 
            if (IsConnected)
            {
                MessageBox.Show("OPC UA Server already connected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                try
                {
                    //Register mandatory events (cert and keep alive)
                    //-----------------------
                    myClientHelperAPI.KeepAliveNotification += new KeepAliveEventHandler(Notification_KeepAlive);
                   // myClientHelperAPI.CertificateValidationNotification += new CertificateValidationEventHandler(Notification_ServerCertificate);
                    
                    mySelectedEndpoint = GetEndpoint(EndpointUrl, SecurityMode, SecurityPolicy);
                    //Check for a selected endpoint
                    if (mySelectedEndpoint != null)
                    {
                        //Call connect
                        //-----------------------
                        await myClientHelperAPI.Connect(mySelectedEndpoint, UsePsw, User, Psw);
                        //Extract the session object for further direct session interactions
                        mySession = myClientHelperAPI.Session;

                        if (mySession != null)
                        {
                            //UI settings
                            BindTree();
                        }
                    }
                    else
                    {
                        MessageBox.Show("在连接服务器前倾选择一个 endpoint 。", "错误");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误");
                }
            }
        }

        public void CloseSession()
        {
            if (IsConnected)
            {
                myClientHelperAPI.Disconnect();
                mySession = null;
                ResetUI();
            }
        }

        /// <summary>
        /// 获取单个Endpoint
        /// </summary>
        /// <param name="UAParamter"></param>
        /// <returns></returns>
        public EndpointDescription GetEndpoint(string discoveryUrl, string SecurMode, string securPolicy)
        {
            //The local discovery URL for the discovery server
            EndpointDescription ed = new EndpointDescription();
            try
            {
                ApplicationDescriptionCollection servers = myClientHelperAPI.FindServers(discoveryUrl);
                foreach (ApplicationDescription ad in servers)
                {
                    foreach (string url in ad.DiscoveryUrls)
                    {
                        EndpointDescriptionCollection endpoints = myClientHelperAPI.GetEndpoints(url);
                        foreach (EndpointDescription ep in endpoints)
                        {
                            string securityPolicy = ep.SecurityPolicyUri.Remove(0, 42);
                            if ((SecurMode == ep.SecurityMode.ToString()) && (securPolicy == securityPolicy.ToString()))
                            {
                                ed = ep;

                                //ClearScreen();
                                //Console.WriteLine("[Conn(" + myConnctDevice.CannelID.ToString() + ")]OPC UA Found :[" + ad.ApplicationName + "] " + " [" + ep.SecurityMode + "] " + " [" + securityPolicy + "] " + " [" + ep.EndpointUrl + "]");
                                break;
                            }
                            //endpointListView.Items.Add("[" + ad.ApplicationName + "] " + " [" + ep.SecurityMode + "] " + " [" + securityPolicy + "] " + " [" + ep.EndpointUrl + "]").Tag = ep;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ed;
        }

        /// <summary>
        /// Auto connection here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notification_KeepAlive(Opc.Ua.Client.ISession sender, KeepAliveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                //this.BeginInvoke(new KeepAliveEventHandler(Notification_KeepAlive), sender, e);
                return;
            }

            try
            {
                // check for events from discarded sessions.
                if (!Object.ReferenceEquals(sender, mySession))
                {
                    return;
                }

                // check for disconnected session.
                if (!ServiceResult.IsGood(e.Status))
                {
                    // try reconnecting using the existing session state
                    mySession.Reconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                ResetUI();
            }
        }
        #endregion //connect

        /// <summary>
        /// Private methods for UI handling
        /// </summary>
        #region PrivateMethods

        /// <summary>
        /// Reset UI
        /// </summary>
        private void ResetUI()
        {
            descriptionListView.Items.Clear();
            nodeTreeView.Nodes.Clear();
            myReferenceDescriptionCollection = null;
        }

        #endregion

        #region Tree

        private List<TreeNode> nodeList = new List<TreeNode>();

        private void FetchNode(TreeNode node)
        {
            if (!string.IsNullOrEmpty(node.Text))
                nodeList.Add(node);
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                FetchNode(node.Nodes[i]);
            }
        }
        private void BindTree()
        {
            if (myReferenceDescriptionCollection == null)
            {
                try
                {
                    myReferenceDescriptionCollection = myClientHelperAPI.BrowseRoot();
                    foreach (ReferenceDescription refDesc in myReferenceDescriptionCollection)
                    {
                        nodeTreeView.Nodes.Add(refDesc.DisplayName.ToString()).Tag = refDesc;
                        foreach (TreeNode node in nodeTreeView.Nodes)
                        {
                            node.Nodes.Add("");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void nodeTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();

            ReferenceDescriptionCollection referenceDescriptionCollection;
            ReferenceDescription refDesc = (ReferenceDescription)e.Node.Tag;

            try
            {
                referenceDescriptionCollection = myClientHelperAPI.BrowseNode(refDesc);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message, "Error");
                return;
            }

            //foreach (ReferenceDescription tempRefDesc in referenceDescriptionCollection)
            //{
            //    e.Node.Nodes.Add(tempRefDesc.DisplayName.ToString()).Tag = tempRefDesc;
            //}

            foreach (ReferenceDescription tempRefDesc in referenceDescriptionCollection)
            {
                // 创建节点并绑定 Tag
                var childNode = new TreeNode(tempRefDesc.DisplayName.ToString()) { Tag = tempRefDesc };

                // 尝试根据类型/数据类型设定图标（容错）
                try
                {
                    Node node = myClientHelperAPI.ReadNode(tempRefDesc.NodeId.ToString());
                    string iconKey = GetIconKeyForNode(tempRefDesc, node);

                    if (nodeTreeView.ImageList != null && !string.IsNullOrEmpty(iconKey) && nodeTreeView.ImageList.Images.ContainsKey(iconKey))
                    {
                        childNode.ImageKey = childNode.SelectedImageKey = iconKey;
                    }
                    else
                    {
                        childNode.ImageIndex = childNode.SelectedImageIndex = GetDefaultIconIndex(tempRefDesc.NodeClass);
                    }
                }
                catch
                {
                    childNode.ImageIndex = childNode.SelectedImageIndex = GetDefaultIconIndex(tempRefDesc.NodeClass);
                }

                e.Node.Nodes.Add(childNode);
            }

            // 保留占位子节点，延迟加载时使用
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Nodes.Add("");
            }
        }

        // 根据 ReferenceDescription + Node 推断图标 key
        private string GetIconKeyForNode(ReferenceDescription refDesc, Node node)
        {
            if (refDesc == null) return "icon_folder";

            // 根据节点类别优先选择
            if (refDesc.NodeClass == NodeClass.Variable)
            {
                try
                {
                    var variableNode = node.DataLock as VariableNode;
                    if (variableNode != null)
                    {
                        // 读取数据类型显示名（通过 session）
                        IList<NodeId> nodeIds = new List<NodeId> { new NodeId(variableNode.DataType) };
                        IList<string> displayNames = null;
                        IList<ServiceResult> errors = null;
                        mySession.ReadDisplayName(nodeIds, out displayNames, out errors);

                        string dtype = displayNames?.FirstOrDefault()?.ToLower() ?? "";

                        if (dtype.Contains("bool")) return "icon_bool";
                        if (dtype.Contains("int") || dtype.Contains("uint") || dtype.Contains("short") || dtype.Contains("long") || dtype.Contains("word")) return "icon_int";
                        if (dtype.Contains("float") || dtype.Contains("double") || dtype.Contains("real")) return "icon_number";
                        if (dtype.Contains("string") || dtype.Contains("wstring") || dtype.Contains("utf8")) return "icon_string";

                        return "icon_variable";
                    }
                }
                catch
                {
                    return "icon_variable";
                }
            }

            if (refDesc.NodeClass == NodeClass.Object) return "icon_object";
            if (refDesc.NodeClass == NodeClass.Method) return "icon_method";
            // 其它情况当作文件夹/分支处理
            return "icon_folder";
        }

        // 备用：当不存在 ImageKey 时使用的默认 ImageIndex（按你的 ImageList 顺序调整）
        private int GetDefaultIconIndex(NodeClass nodeClass)
        {
            return nodeClass switch
            {
                NodeClass.Variable => 1,
                NodeClass.Object => 2,
                NodeClass.Method => 3,
                _ => 4,
            };
        }

        private void nodeTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            descriptionListView.Items.Clear();
            try
            {
                ReferenceDescription refDesc = (ReferenceDescription)e.Node.Tag;
                Node node = myClientHelperAPI.ReadNode(refDesc.NodeId.ToString());
                VariableNode variableNode = new VariableNode();

                string[] row1 = new string[] { "Node Id", refDesc.NodeId.ToString() };
                string[] row2 = new string[] { "Namespace Index", refDesc.NodeId.NamespaceIndex.ToString() };
                string[] row3 = new string[] { "Identifier Type", refDesc.NodeId.IdType.ToString() };
                string[] row4 = new string[] { "Identifier", refDesc.NodeId.Identifier.ToString() };
                string[] row5 = new string[] { "Browse Name", refDesc.BrowseName.ToString() };
                string[] row6 = new string[] { "Display Name", refDesc.DisplayName.ToString() };
                string[] row7 = new string[] { "Node Class", refDesc.NodeClass.ToString() };
                string[] row8 = new string[] { "Description", "null" };
                try { row8 = new string[] { "Description", node.Description?.ToString() }; }
                catch { row8 = new string[] { "Description", "null" }; }
                string[] row9 = new string[] { "Type Definition", refDesc.TypeDefinition.ToString() };
                string[] row10 = new string[] { "Write Mask", node.WriteMask.ToString() };
                string[] row11 = new string[] { "User Write Mask", node.UserWriteMask.ToString() };
                if (node.NodeClass == NodeClass.Variable)
                {
                    variableNode = (VariableNode)node.DataLock;
                    IList<NodeId> nodeIds = new List<NodeId>();
                    IList<string> displayNames = new List<string>();
                    IList<ServiceResult> errors = new List<ServiceResult>();
                    NodeId nodeId = new NodeId(variableNode.DataType);
                    nodeIds.Add(nodeId);
                    mySession.ReadDisplayName(nodeIds, out displayNames, out errors);

                    string[] row12 = new string[] { "Data Type", displayNames[0] };
                    string[] row13 = new string[] { "Value Rank", variableNode.ValueRank.ToString() };
                    string[] row14 = new string[] { "Array Dimensions", variableNode.ArrayDimensions.Capacity.ToString() };
                    string[] row15 = new string[] { "Access Level", variableNode.AccessLevel.ToString() };
                    string[] row16 = new string[] { "Minimum Sampling Interval", variableNode.MinimumSamplingInterval.ToString() };
                    string[] row17 = new string[] { "Historizing", variableNode.Historizing.ToString() };

                    object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, row13, row14, row15, row16, row17 };
                    foreach (string[] rowArray in rows)
                    {

                        descriptionListView.Items.Add(new ListViewItem(rowArray));
                    }
                }
                else
                {
                    object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11 };
                    foreach (string[] rowArray in rows)
                    {

                        descriptionListView.Items.Add(new ListViewItem(rowArray));
                    }
                }

                //descriptionGridView.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }


        // 异步展开所有子节点（可取消，UI 操作通过 nodeTreeView.Invoke 执行）
        private CancellationTokenSource? _expandCts;

        /// <summary>
        /// 异步展开所有子节点的菜单子函数
        /// </summary>
        /// <param name="root"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task ExpandAllChildNodeAsync(TreeNode? root, CancellationToken ct = default)
        {
            if (root == null) return;

            // 使用栈避免深度递归带来的栈溢出
            var stack = new Stack<TreeNode>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                ct.ThrowIfCancellationRequested();

                var node = stack.Pop();

                // 在 UI 线程执行展开
                if (nodeTreeView.InvokeRequired)
                {
                    nodeTreeView.Invoke((Action)(() => node.Expand()));
                }
                else
                {
                    node.Expand();
                }

                // 读取子节点集合的快照（必须在 UI 线程访问 Nodes）
                List<TreeNode> children;
                if (nodeTreeView.InvokeRequired)
                {
                    List<TreeNode>? tmp = null;
                    nodeTreeView.Invoke((Action)(() => tmp = node.Nodes.Cast<TreeNode>().ToList()));
                    children = tmp ?? new List<TreeNode>();
                }
                else
                {
                    children = node.Nodes.Cast<TreeNode>().ToList();
                }

                // 反向入栈以保持从左到右的展开顺序
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    stack.Push(children[i]);
                }

                // 让出 UI 线程，避免长时间占用（可根据需要调整延迟）
                await Task.Delay(10, ct);
            }
        }

        /// <summary>
        /// 异步展开所有子节点的菜单点击事件
        /// 注意：事件处理器可以是 async void（WinForms 事件），但尽量短小并处理异常与取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MenuExpendSubItems_Click(object sender, EventArgs e)
        {
            try
            {
                // 取消之前的展开任务（可选）
                _expandCts?.Cancel();
                _expandCts = new CancellationTokenSource();

                var node = nodeTreeView.CurrentNode;
                if (node == null) return;

                // 异步展开（不会阻塞 UI）
                await ExpandAllChildNodeAsync(node, _expandCts.Token);
            }
            catch (OperationCanceledException)
            {
                // 用户取消，静默返回
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void MenuGetAllSubItems_Click(object sender, EventArgs e)
        {
            try
            { 
                nodeList.Clear();
                //if (nodeTreeView.SelectedNodes != null)
                //{ List<WITPLMMultiSelectTreeView> xxx = nodeTreeView.SelectedNodes; }
                //foreach(var x in nodeTreeView.SelectedNodes)
                //    FetchNode(x);List<TreeNode>
                object[] x = (nodeTreeView.SelectedNodes.ToArray());
                foreach (var j in x)
                {
                    TreeNode node = j as TreeNode;
                    FetchNode(node);
                }
                //int xx = nodeList.Count;

                foreach (TreeNode xx in nodeList)
                {
                    ReferenceDescription refDesc = (ReferenceDescription)xx.Tag;
                    Node node = myClientHelperAPI.ReadNode(refDesc.NodeId.ToString());
                    VariableNode variableNode = new VariableNode();
                    string[] row1 = new string[] { "Node Id", refDesc.NodeId.ToString() };

                    if (!myTagView.Keys.Contains(refDesc.NodeId.ToString()))
                    {
                        string datatype = "";
                        if (refDesc.NodeClass == NodeClass.Variable)
                        {
                            variableNode = (VariableNode)node.DataLock;
                            IList<string> displayNames = new List<string>();
                            IList<ServiceResult> errors = new List<ServiceResult>();
                            IList<NodeId> nodeIds = new List<NodeId>();
                            NodeId nodeId = new NodeId(variableNode.DataType);
                            nodeIds.Add(nodeId);
                            mySession.ReadDisplayName(nodeIds, out displayNames, out errors);
                            datatype = displayNames[0];
                        }
                        if (!string.IsNullOrEmpty(datatype))
                            myTagView.Add(refDesc.NodeId.ToString(), new DBTag
                            {
                                Address = refDesc.NodeId.ToString(),
                                TagName = refDesc.DisplayName.ToString(),
                                DataType = DBTag.GetDataType(datatype),
                                Description = node.Description?.ToString()
                            });
                        //if (!string.IsNullOrEmpty(datatype))
                        //    myTagView.Add(refDesc.NodeId.ToString(), new DBTag { TagId = refDesc.NodeId.ToString(), TagName = refDesc.DisplayName.ToString(), DataType = DBTag.GetDataType(datatype) });
                    }
                }

                if (myTagView.Count > 0)
                {
                    OnTagEvent(new DBTagEventArgs("Add new tags", DeviceType.OPCUA, myTagView.Values.ToList()));
                }            
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        /// <summary>
        /// 菜单-获取下一级子节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGetSubItems_Click(object sender, EventArgs e)
        {
            try
            { 
                myTagView.Clear();

                System.Collections.ArrayList al = nodeTreeView.SelectedNodes;
                List<string> xi = new List<string>();
                foreach (TreeNode xas in al)
                {
                    foreach (TreeNode xx in xas.Nodes)
                    {
                        if (null == xx.Tag)
                        {
                            MessageBox.Show(" Not find subtags, plz try to expand the node. ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                        ReferenceDescription refDesc = (ReferenceDescription)xx.Tag;
                        Node node = myClientHelperAPI.ReadNode(refDesc.NodeId.ToString());
                        VariableNode variableNode = new VariableNode();
                        string[] row1 = new string[] { "Node Id", refDesc.NodeId.ToString() };


                        if (!myTagView.Keys.Contains(refDesc.NodeId.ToString()))
                        {
                            string datatype = "";
                            if (refDesc.NodeClass == NodeClass.Variable)
                            {
                                variableNode = (VariableNode)node.DataLock;
                                IList<string> displayNames = new List<string>();
                                IList<ServiceResult> errors = new List<ServiceResult>();
                                IList<NodeId> nodeIds = new List<NodeId>();
                                NodeId nodeId = new NodeId(variableNode.DataType);
                                nodeIds.Add(nodeId);
                                mySession.ReadDisplayName(nodeIds, out displayNames, out errors);
                                datatype = displayNames[0];
                            }
                            if (!string.IsNullOrEmpty(datatype))
                                myTagView.Add(refDesc.NodeId.ToString(), new DBTag { Address = refDesc.NodeId.ToString(),
                                    TagName = refDesc.DisplayName.ToString(),
                                    DataType = DBTag.GetDataType(datatype),
                                    Description = node.Description?.ToString()
                                });
                        }
                    }
                }

                if (myTagView.Count > 0)
                {
                    OnTagEvent(new DBTagEventArgs("Add new tags", DeviceType.OPCUA, myTagView.Values.ToList()));
                }           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        /// <summary>
        /// 菜单-获取当前节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGetThisItem_Click(object sender, EventArgs e)
        {
            try
            {
                myTagView.Clear();

                ReferenceDescription refDesc = (ReferenceDescription)nodeTreeView.CurrentNode.Tag;
                Node node = myClientHelperAPI.ReadNode(refDesc.NodeId.ToString());
                VariableNode variableNode = new VariableNode();
                string[] row1 = new string[] { "Node Id", refDesc.NodeId.ToString() };

                if (!myTagView.Keys.Contains(refDesc.NodeId.ToString()))
                {
                    string datatype = "";
                    if (refDesc.NodeClass == NodeClass.Variable)
                    {
                        variableNode = (VariableNode)node.DataLock;
                        IList<string> displayNames = new List<string>();
                        IList<ServiceResult> errors = new List<ServiceResult>();
                        IList<NodeId> nodeIds = new List<NodeId>();
                        NodeId nodeId = new NodeId(variableNode.DataType);
                        nodeIds.Add(nodeId);
                        mySession.ReadDisplayName(nodeIds, out displayNames, out errors);
                        datatype = displayNames[0];
                    }
                    //if (!string.IsNullOrEmpty(datatype))
                    //    myTagView.Add(refDesc.NodeId.ToString(), new DBTag { TagId = refDesc.NodeId.ToString(), TagName = refDesc.NodeId.Identifier.ToString(), DataType = DBTag.GetDataType(datatype) });

                    if (!string.IsNullOrEmpty(datatype))
                        myTagView.Add(refDesc.NodeId.ToString(), new DBTag
                        {
                            Address = refDesc.NodeId.ToString(),
                            TagName = refDesc.DisplayName.ToString(),
                            DataType = DBTag.GetDataType(datatype),
                            Description = node.Description?.ToString()
                        });

                    if (myTagView.Count > 0)
                    {
                       OnTagEvent(new DBTagEventArgs("Add new tags", DeviceType.OPCUA, myTagView.Values.ToList()));
                    }
                }            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
        #endregion //tree

        #region 事件
        // 用EventHandler<woEventArgs>委托定义事件
        public event EventHandler<DBTagEventArgs> TagEvent;

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTagEvent(DBTagEventArgs e)
        {
            TagEvent?.Invoke(this, e);
        }
        #endregion //事件
    }
}
