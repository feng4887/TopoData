using System.Drawing;
using System.Windows.Forms;
using auDASLib;
namespace TopoData
{
    partial class diaOPCUATagSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(diaOPCUATagSelect));
            splitContainer1 = new SplitContainer();
            nodeTreeView = new WITPLMMultiSelectTreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            MenuGetThisItem = new ToolStripMenuItem();
            MenuGetSubItems = new ToolStripMenuItem();
            MenuGetAllSubItems = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            MenuExpendSubItems = new ToolStripMenuItem();
            panel3 = new DevExpress.XtraEditors.PanelControl();
            descriptionListView = new auDAManager.NoFlashListView();
            TagName = new ColumnHeader();
            Value = new ColumnHeader();
            panel1 = new DevExpress.XtraEditors.PanelControl();
            discoveryTextBox = new DevExpress.XtraEditors.TextEdit();
            endpointLabel = new DevExpress.XtraEditors.LabelControl();
            contextMenuStripTaglist = new ContextMenuStrip(components);
            removeItemsToolStripMenuItem = new ToolStripMenuItem();
            imageList1 = new ImageList(components);
            Label1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panel3).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panel1).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)discoveryTextBox.Properties).BeginInit();
            contextMenuStripTaglist.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 55);
            splitContainer1.Margin = new Padding(2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(nodeTreeView);
            splitContainer1.Panel1.Controls.Add(panel3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(descriptionListView);
            splitContainer1.Size = new Size(698, 363);
            splitContainer1.SplitterDistance = 186;
            splitContainer1.TabIndex = 0;
            // 
            // nodeTreeView
            // 
            nodeTreeView.ContextMenuStrip = contextMenuStrip1;
            nodeTreeView.Dock = DockStyle.Fill;
            nodeTreeView.IsMulSelect = false;
            nodeTreeView.Location = new Point(0, 24);
            nodeTreeView.Margin = new Padding(3, 2, 3, 2);
            nodeTreeView.Name = "nodeTreeView";
            nodeTreeView.Size = new Size(184, 337);
            nodeTreeView.TabIndex = 0;
            nodeTreeView.BeforeExpand += nodeTreeView_BeforeExpand;
            nodeTreeView.BeforeSelect += nodeTreeView_BeforeSelect;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { MenuGetThisItem, MenuGetSubItems, MenuGetAllSubItems, toolStripSeparator4, MenuExpendSubItems });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(205, 130);
            // 
            // MenuGetThisItem
            // 
            MenuGetThisItem.Name = "MenuGetThisItem";
            MenuGetThisItem.Size = new Size(204, 30);
            MenuGetThisItem.Text = "获取当前节点";
            MenuGetThisItem.Click += MenuGetThisItem_Click;
            // 
            // MenuGetSubItems
            // 
            MenuGetSubItems.Image = (Image)resources.GetObject("MenuGetSubItems.Image");
            MenuGetSubItems.Name = "MenuGetSubItems";
            MenuGetSubItems.Size = new Size(204, 30);
            MenuGetSubItems.Text = "获取所有下一级子节点";
            MenuGetSubItems.Click += MenuGetSubItems_Click;
            // 
            // MenuGetAllSubItems
            // 
            MenuGetAllSubItems.Name = "MenuGetAllSubItems";
            MenuGetAllSubItems.Size = new Size(204, 30);
            MenuGetAllSubItems.Text = "获取所有子节点";
            MenuGetAllSubItems.Click += MenuGetAllSubItems_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(201, 6);
            // 
            // MenuExpendSubItems
            // 
            MenuExpendSubItems.Name = "MenuExpendSubItems";
            MenuExpendSubItems.Size = new Size(204, 30);
            MenuExpendSubItems.Text = "展开所有子节点";
            MenuExpendSubItems.Click += MenuExpendSubItems_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(Label1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(184, 24);
            panel3.TabIndex = 0;
            // 
            // descriptionListView
            // 
            descriptionListView.BackColor = SystemColors.Window;
            descriptionListView.BorderStyle = BorderStyle.FixedSingle;
            descriptionListView.Columns.AddRange(new ColumnHeader[] { TagName, Value });
            descriptionListView.Dock = DockStyle.Fill;
            descriptionListView.FullRowSelect = true;
            descriptionListView.GridLines = true;
            descriptionListView.Location = new Point(0, 0);
            descriptionListView.Margin = new Padding(3, 2, 3, 2);
            descriptionListView.MultiSelect = false;
            descriptionListView.Name = "descriptionListView";
            descriptionListView.Size = new Size(506, 361);
            descriptionListView.TabIndex = 4;
            descriptionListView.UseCompatibleStateImageBehavior = false;
            descriptionListView.View = View.Details;
            // 
            // TagName
            // 
            TagName.Text = "属性";
            TagName.Width = 270;
            // 
            // Value
            // 
            Value.Text = "参数值";
            Value.Width = 500;
            // 
            // panel1
            // 
            panel1.Controls.Add(discoveryTextBox);
            panel1.Controls.Add(endpointLabel);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(698, 55);
            panel1.TabIndex = 16;
            // 
            // discoveryTextBox
            // 
            discoveryTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            discoveryTextBox.Location = new Point(81, 19);
            discoveryTextBox.Name = "discoveryTextBox";
            discoveryTextBox.Properties.Appearance.BackColor = Color.GhostWhite;
            discoveryTextBox.Properties.Appearance.Options.UseBackColor = true;
            discoveryTextBox.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            discoveryTextBox.Properties.ReadOnly = true;
            discoveryTextBox.Size = new Size(600, 18);
            discoveryTextBox.TabIndex = 17;
            // 
            // endpointLabel
            // 
            endpointLabel.Location = new Point(11, 21);
            endpointLabel.Margin = new Padding(3, 0, 3, 0);
            endpointLabel.Name = "endpointLabel";
            endpointLabel.Size = new Size(60, 12);
            endpointLabel.TabIndex = 16;
            endpointLabel.Text = "Endpoint：";
            // 
            // contextMenuStripTaglist
            // 
            contextMenuStripTaglist.ImageScalingSize = new Size(20, 20);
            contextMenuStripTaglist.Items.AddRange(new ToolStripItem[] { removeItemsToolStripMenuItem });
            contextMenuStripTaglist.Name = "contextMenuStripTaglist";
            contextMenuStripTaglist.Size = new Size(125, 26);
            // 
            // removeItemsToolStripMenuItem
            // 
            removeItemsToolStripMenuItem.Name = "removeItemsToolStripMenuItem";
            removeItemsToolStripMenuItem.Size = new Size(124, 22);
            removeItemsToolStripMenuItem.Text = "删除标签";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "icons8-database-administrator-116.png");
            imageList1.Images.SetKeyName(1, "Tag_16x16.png");
            imageList1.Images.SetKeyName(2, "ModelEditor_Class_Object.png");
            imageList1.Images.SetKeyName(3, "ModelEditor_Default.png");
            imageList1.Images.SetKeyName(4, "Properties_16x16.png");
            imageList1.Images.SetKeyName(5, "icons8-voltmeter-16.png");
            imageList1.Images.SetKeyName(6, "Action_StateMachine.png");
            imageList1.Images.SetKeyName(7, "BO_StateMachine.png");
            // 
            // Label1
            // 
            Label1.Location = new Point(11, 6);
            Label1.Margin = new Padding(3, 0, 3, 0);
            Label1.Name = "Label1";
            Label1.Size = new Size(24, 12);
            Label1.TabIndex = 17;
            Label1.Text = "节点";
            // 
            // diaOPCUATagSelect
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(698, 418);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            IconOptions.Icon = (Icon)resources.GetObject("diaOPCUATagSelect.IconOptions.Icon");
            Margin = new Padding(2);
            Name = "diaOPCUATagSelect";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tag Selection";
            FormClosing += frmMain_FormClosing;
            Load += diaOPCUATagSelect_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)panel3).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)panel1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)discoveryTextBox.Properties).EndInit();
            contextMenuStripTaglist.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panel1;
        private DevExpress.XtraEditors.TextEdit discoveryTextBox;
        private DevExpress.XtraEditors.LabelControl endpointLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private auDASLib.WITPLMMultiSelectTreeView nodeTreeView;
        private DevExpress.XtraEditors.PanelControl panel3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuGetSubItems;
        private System.Windows.Forms.ToolStripMenuItem MenuGetThisItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTaglist;
        private System.Windows.Forms.ToolStripMenuItem removeItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem MenuExpendSubItems;
        private System.Windows.Forms.ToolStripMenuItem MenuGetAllSubItems;
        private ImageList imageList1;
        private auDAManager.NoFlashListView descriptionListView;
        private ColumnHeader TagName;
        private ColumnHeader Value;
        private DevExpress.XtraEditors.LabelControl Label1;
    }
}

