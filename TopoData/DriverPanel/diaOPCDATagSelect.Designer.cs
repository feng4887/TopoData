using System.Drawing;
using System.Windows.Forms;
using auDASLib;

namespace TopoData
{
    partial class diaOPCDATagSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(diaOPCDATagSelect));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            panel2 = new Panel();
            splitContainer1 = new SplitContainer();
            nodeTreeView = new WITPLMMultiSelectTreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            MenuGetThisItem = new ToolStripMenuItem();
            MenuGetSubItems = new ToolStripMenuItem();
            MenuGetAllSubItems = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            MenuExpendSubItems = new ToolStripMenuItem();
            panel3 = new Panel();
            label1 = new Label();
            descriptionListView = new auDAManager.NoFlashListView();
            TagName = new ColumnHeader();
            Value = new ColumnHeader();
            statusStrip1 = new StatusStrip();
            panel1 = new Panel();
            discoveryTextBox = new TextBox();
            endpointLabel = new Label();
            contextMenuStripTaglist = new ContextMenuStrip(components);
            removeItemsToolStripMenuItem = new ToolStripMenuItem();
            imageList1 = new ImageList(components);
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            panel3.SuspendLayout();
            panel1.SuspendLayout();
            contextMenuStripTaglist.SuspendLayout();
            SuspendLayout();
            //
            // tabControl1
            //
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(2, 3, 2, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(871, 516);
            tabControl1.TabIndex = 0;
            //
            // tabPage1
            //
            tabPage1.Controls.Add(panel2);
            tabPage1.Controls.Add(statusStrip1);
            tabPage1.Controls.Add(panel1);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Margin = new Padding(2, 3, 2, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(2, 3, 2, 3);
            tabPage1.Size = new Size(863, 486);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "配置";
            tabPage1.UseVisualStyleBackColor = true;
            //
            // panel2
            //
            panel2.Controls.Add(splitContainer1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(2, 81);
            panel2.Margin = new Padding(2, 3, 2, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(859, 380);
            panel2.TabIndex = 18;
            //
            // splitContainer1
            //
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(2, 3, 2, 3);
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
            splitContainer1.Size = new Size(859, 380);
            splitContainer1.SplitterDistance = 230;
            splitContainer1.TabIndex = 0;
            //
            // nodeTreeView
            //
            nodeTreeView.ContextMenuStrip = contextMenuStrip1;
            nodeTreeView.Dock = DockStyle.Fill;
            nodeTreeView.IsMulSelect = false;
            nodeTreeView.Location = new Point(0, 29);
            nodeTreeView.Name = "nodeTreeView";
            nodeTreeView.Size = new Size(228, 349);
            nodeTreeView.TabIndex = 0;
            nodeTreeView.BeforeExpand += nodeTreeView_BeforeExpand;
            nodeTreeView.AfterSelect += nodeTreeView_AfterSelect;
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
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(2, 3, 2, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(228, 29);
            panel3.TabIndex = 0;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Font = new Font("黑体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(14, 8);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(63, 14);
            label1.TabIndex = 0;
            label1.Text = "服务节点";
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
            descriptionListView.MultiSelect = false;
            descriptionListView.Name = "descriptionListView";
            descriptionListView.Size = new Size(623, 378);
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
            // statusStrip1
            //
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Location = new Point(2, 461);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 10, 0);
            statusStrip1.Size = new Size(859, 22);
            statusStrip1.TabIndex = 17;
            statusStrip1.Text = "statusStrip1";
            //
            // panel1
            //
            panel1.Controls.Add(discoveryTextBox);
            panel1.Controls.Add(endpointLabel);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(2, 3);
            panel1.Margin = new Padding(2, 3, 2, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(859, 78);
            panel1.TabIndex = 16;
            //
            // discoveryTextBox
            //
            discoveryTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            discoveryTextBox.BackColor = Color.Lavender;
            discoveryTextBox.Location = new Point(18, 38);
            discoveryTextBox.Margin = new Padding(4);
            discoveryTextBox.Name = "discoveryTextBox";
            discoveryTextBox.ReadOnly = true;
            discoveryTextBox.Size = new Size(826, 23);
            discoveryTextBox.TabIndex = 17;
            //
            // endpointLabel
            //
            endpointLabel.AutoSize = true;
            endpointLabel.Location = new Point(21, 11);
            endpointLabel.Margin = new Padding(4, 0, 4, 0);
            endpointLabel.Name = "endpointLabel";
            endpointLabel.Size = new Size(72, 17);
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
            // diaOPCDATagSelect
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(871, 516);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 3, 2, 3);
            Name = "diaOPCDATagSelect";
            StartPosition = FormStartPosition.CenterParent;
            Text = "OPC DA Tag selection";
            FormClosing += frmMain_FormClosing;
            Load += diaOPCDATagSelect_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            panel2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            contextMenuStripTaglist.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox discoveryTextBox;
        private System.Windows.Forms.Label endpointLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private auDASLib.WITPLMMultiSelectTreeView nodeTreeView;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
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
    }
}
