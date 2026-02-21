using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;

namespace auDASLib
{
    public delegate void TreeNodeDoubleClickHandle(object sender, EventArgs args);
    public delegate void TreeNodeChangedHandle(object sender, EventArgs args);

    /// <summary>
    /// WITPLMMultiSelectTreeView 的摘要说明。
    /// </summary>
    [Serializable]
    public class WITPLMMultiSelectTreeView : TreeView
    {
        public event TreeNodeDoubleClickHandle TreeNodeDoubleClick;
        public event TreeNodeChangedHandle TreeNodeChanged;

        private ArrayList selectedNodes = new ArrayList();
        private bool isMulSelect = false;
        private TreeNode currentNode = null;

        public WITPLMMultiSelectTreeView()
        {
        }

        public virtual void Initialize()
        {
            this.Sorted = false;
            this.ShowRootLines = false;
            this.Indent = 15;
            this.ItemHeight = 18;
            // this.ImageList = PLMService.ResourceService.ImageList;
        }

        /// <summary>
        /// 获取当前节点
        /// </summary>
        public TreeNode CurrentNode
        {
            get
            {
                return currentNode;
            }
        }

        /// <summary>
        /// 获取所选节点组
        /// </summary>
        public ArrayList SelectedNodes
        {
            get
            {
                return selectedNodes;
            }
        }

        /// <summary>
        /// 判断是否是多选
        /// </summary>
        public bool IsMulSelect
        {
            get
            {
                return isMulSelect;
            }
            set
            {
                isMulSelect = value;
            }
        }

        /// <summary>
        /// 显示节点菜单
        /// </summary>
        protected virtual void ShowTreeNodeContextMenu()
        { }

        /// <summary>
        /// 显示树菜单
        /// </summary>
        protected virtual void ShowTreeViewContextMenu()
        { }

        /// <summary>
        /// 检测鼠标单击事件
        /// </summary>
        /// <param name= "e "> </param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            TreeNode node = GetNodeAt(e.X, e.Y);
            if (node != null)
            {
                if (isMulSelect)
                {
                    if (!(selectedNodes.Count == 1 && selectedNodes[0] == node))
                    {
                        if (((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0))
                            MulSelectNode(node, e.Button == MouseButtons.Right);
                        else
                            SingleSelectNode(node);
                    }
                }
                else
                    SetCurrentNode(node);
            }

            if (e.Button == MouseButtons.Right)
            {
                if (node == null)
                    ShowTreeViewContextMenu();
                else
                    ShowTreeNodeContextMenu();
            }
            else if (e.Clicks == 2 && node != null)
            {
                if (TreeNodeDoubleClick != null)
                    TreeNodeDoubleClick(node, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 节点选择改变后执行
        /// </summary>
        /// <param name= "e "> </param>
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            SetCurrentNode(e.Node);
            base.OnAfterCheck(e);
        }

        /// <summary>
        /// 复选
        /// </summary>
        /// <param name= "node "> </param>
        /// <param name= "mustSelect "> </param>
        private void MulSelectNode(TreeNode node, bool mustSelect)
        {
            if (selectedNodes.Contains(node) && !mustSelect)
            {
                selectedNodes.Remove(node);
                LowlightNode(node);
                SetCurrentNode((TreeNode)selectedNodes[selectedNodes.Count - 1]);
            }
            else
            {
                selectedNodes.Add(node);
                HighlightNode(node);
                SetCurrentNode(node);
            }
        }

        /// <summary>
        /// 单选
        /// </summary>
        /// <param name= "node "> </param>
        private void SingleSelectNode(TreeNode node)
        {
            foreach (TreeNode nd in SelectedNodes)
            {
                nd.BackColor = BackColor;
                nd.ForeColor = ForeColor;
            }
            SelectedNodes.Clear();
            SelectedNodes.Add(node);
            HighlightNode(node);
            SetCurrentNode(node);
        }

        /// <summary>
        /// 设置当前节点
        /// </summary>
        /// <param name= "node "> </param>
        private void SetCurrentNode(TreeNode node)
        {
            if (isMulSelect)
                SelectedNode = null;
            if (currentNode != node)
            {
                currentNode = node as TreeNode;
                if (TreeNodeChanged != null)
                    TreeNodeChanged(currentNode, EventArgs.Empty);
                // if(currentNode.Component != null)
                // currentNode.Component.FireOperation(new ComponentOperationArgs(ComponentOperation.Selected));
            }
        }

        /// <summary>
        /// 取消当前节点的高亮显示
        /// </summary>
        /// <param name= "node "> </param>
        private void LowlightNode(TreeNode node)
        {
            node.BackColor = BackColor;
            node.ForeColor = SystemColors.ControlText;
        }

        /// <summary>
        /// 高亮显示节点
        /// </summary>
        /// <param name= "node "> </param>
        private void HighlightNode(TreeNode node)
        {
            node.BackColor = SystemColors.Highlight;
            node.ForeColor = SystemColors.HighlightText;
        }

    }
}