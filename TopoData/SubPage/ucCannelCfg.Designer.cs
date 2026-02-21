namespace TopoData.Page
{
    partial class ucCannelCfg
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucCannelCfg));
            imageList1 = new System.Windows.Forms.ImageList(components);
            navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            panelControl2 = new DevExpress.XtraEditors.PanelControl();
            groupControl2 = new DevExpress.XtraEditors.GroupControl();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            TagName = new DevExpress.XtraGrid.Columns.GridColumn();
            Address = new DevExpress.XtraGrid.Columns.GridColumn();
            DataType = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            UOM = new DevExpress.XtraGrid.Columns.GridColumn();
            Description = new DevExpress.XtraGrid.Columns.GridColumn();
            btExport = new DevExpress.XtraEditors.SimpleButton();
            btImport = new DevExpress.XtraEditors.SimpleButton();
            btConfirm = new DevExpress.XtraEditors.SimpleButton();
            tbDescription = new DevExpress.XtraEditors.MemoEdit();
            labelControl3 = new DevExpress.XtraEditors.LabelControl();
            pnCannel = new DevExpress.XtraEditors.PanelControl();
            cbCannel = new DevExpress.XtraEditors.ComboBoxEdit();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            tbCannelID = new DevExpress.XtraEditors.TextEdit();
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            cbActive = new DevExpress.XtraEditors.ToggleSwitch();
            btDelete = new DevExpress.XtraEditors.SimpleButton();
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)navBarControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControl2).BeginInit();
            panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl2).BeginInit();
            groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemComboBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbDescription.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pnCannel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbCannel.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbCannelID.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cbActive.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageSize = new System.Drawing.Size(16, 16);
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // navBarControl1
            // 
            navBarControl1.ActiveGroup = navBarGroup1;
            navBarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] { navBarGroup1 });
            navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] { navBarItem1 });
            navBarControl1.Location = new System.Drawing.Point(0, 0);
            navBarControl1.Name = "navBarControl1";
            navBarControl1.OptionsNavPane.ExpandedWidth = 110;
            navBarControl1.OptionsNavPane.ShowOverflowButton = false;
            navBarControl1.OptionsNavPane.ShowOverflowPanel = false;
            navBarControl1.OptionsNavPane.ShowSplitter = false;
            navBarControl1.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            navBarControl1.Size = new System.Drawing.Size(110, 508);
            navBarControl1.TabIndex = 0;
            navBarControl1.Text = "navBarControl1";
            // 
            // navBarGroup1
            // 
            navBarGroup1.Caption = "设备列表";
            navBarGroup1.Expanded = true;
            navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsList;
            navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] { new DevExpress.XtraNavBar.NavBarItemLink(navBarItem1) });
            navBarGroup1.Name = "navBarGroup1";
            navBarGroup1.NavigationPaneVisible = false;
            // 
            // navBarItem1
            // 
            navBarItem1.Caption = "我的设备";
            navBarItem1.ImageOptions.LargeImageSize = new System.Drawing.Size(32, 32);
            navBarItem1.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("navBarItem1.ImageOptions.SvgImage");
            navBarItem1.Name = "navBarItem1";
            // 
            // panelControl2
            // 
            panelControl2.Controls.Add(groupControl2);
            panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl2.Location = new System.Drawing.Point(110, 246);
            panelControl2.Name = "panelControl2";
            panelControl2.Padding = new System.Windows.Forms.Padding(9);
            panelControl2.Size = new System.Drawing.Size(826, 262);
            panelControl2.TabIndex = 8;
            // 
            // groupControl2
            // 
            groupControl2.Controls.Add(gridControl1);
            groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl2.Location = new System.Drawing.Point(11, 11);
            groupControl2.Name = "groupControl2";
            groupControl2.Size = new System.Drawing.Size(804, 240);
            groupControl2.TabIndex = 4;
            groupControl2.Text = "点表";
            // 
            // gridControl1
            // 
            gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControl1.Location = new System.Drawing.Point(2, 23);
            gridControl1.MainView = gridView1;
            gridControl1.Name = "gridControl1";
            gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemComboBox1 });
            gridControl1.Size = new System.Drawing.Size(800, 215);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { TagName, Address, DataType, UOM, Description });
            gridView1.DetailHeight = 300;
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
            gridView1.CustomColumnDisplayText += gridView1_CustomColumnDisplayText;
            // 
            // TagName
            // 
            TagName.Caption = "TagName";
            TagName.FieldName = "TagName";
            TagName.MinWidth = 17;
            TagName.Name = "TagName";
            TagName.Visible = true;
            TagName.VisibleIndex = 0;
            TagName.Width = 170;
            // 
            // Address
            // 
            Address.Caption = "Address";
            Address.FieldName = "Address";
            Address.MinWidth = 17;
            Address.Name = "Address";
            Address.Visible = true;
            Address.VisibleIndex = 1;
            Address.Width = 219;
            // 
            // DataType
            // 
            DataType.Caption = "DataType";
            DataType.ColumnEdit = repositoryItemComboBox1;
            DataType.FieldName = "DataType";
            DataType.MinWidth = 17;
            DataType.Name = "DataType";
            DataType.Visible = true;
            DataType.VisibleIndex = 2;
            DataType.Width = 77;
            // 
            // repositoryItemComboBox1
            // 
            repositoryItemComboBox1.AutoHeight = false;
            repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // UOM
            // 
            UOM.Caption = "UOM";
            UOM.FieldName = "UOM";
            UOM.MinWidth = 17;
            UOM.Name = "UOM";
            UOM.Visible = true;
            UOM.VisibleIndex = 3;
            UOM.Width = 64;
            // 
            // Description
            // 
            Description.Caption = "Description";
            Description.FieldName = "Description";
            Description.MinWidth = 17;
            Description.Name = "Description";
            Description.Visible = true;
            Description.VisibleIndex = 4;
            Description.Width = 285;
            // 
            // btExport
            // 
            btExport.Location = new System.Drawing.Point(154, 179);
            btExport.Name = "btExport";
            btExport.Size = new System.Drawing.Size(64, 29);
            btExport.TabIndex = 10;
            btExport.Text = "导出";
            btExport.Click += btExport_Click;
            // 
            // btImport
            // 
            btImport.Location = new System.Drawing.Point(84, 179);
            btImport.Name = "btImport";
            btImport.Size = new System.Drawing.Size(64, 29);
            btImport.TabIndex = 9;
            btImport.Text = "导入";
            btImport.Click += btImport_Click;
            // 
            // btConfirm
            // 
            btConfirm.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btConfirm.Appearance.Options.UseBackColor = true;
            btConfirm.Location = new System.Drawing.Point(15, 179);
            btConfirm.Name = "btConfirm";
            btConfirm.Size = new System.Drawing.Size(64, 29);
            btConfirm.TabIndex = 8;
            btConfirm.Text = "保存";
            btConfirm.Click += btConfirm_Click;
            // 
            // tbDescription
            // 
            tbDescription.Location = new System.Drawing.Point(98, 88);
            tbDescription.Name = "tbDescription";
            tbDescription.Size = new System.Drawing.Size(187, 45);
            tbDescription.TabIndex = 7;
            // 
            // labelControl3
            // 
            labelControl3.Location = new System.Drawing.Point(18, 94);
            labelControl3.Name = "labelControl3";
            labelControl3.Size = new System.Drawing.Size(48, 12);
            labelControl3.TabIndex = 6;
            labelControl3.Text = "设备备注";
            // 
            // pnCannel
            // 
            pnCannel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pnCannel.Location = new System.Drawing.Point(316, 35);
            pnCannel.Margin = new System.Windows.Forms.Padding(0);
            pnCannel.Name = "pnCannel";
            pnCannel.Padding = new System.Windows.Forms.Padding(10);
            pnCannel.Size = new System.Drawing.Size(482, 150);
            pnCannel.TabIndex = 5;
            // 
            // cbCannel
            // 
            cbCannel.Location = new System.Drawing.Point(98, 58);
            cbCannel.Name = "cbCannel";
            cbCannel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbCannel.Properties.Items.AddRange(new object[] { "OPC UA", "Siemens Profinet" });
            cbCannel.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbCannel.Size = new System.Drawing.Size(187, 18);
            cbCannel.TabIndex = 4;
            cbCannel.SelectedIndexChanged += cbCannel_SelectedIndexChanged;
            // 
            // labelControl2
            // 
            labelControl2.Location = new System.Drawing.Point(18, 63);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new System.Drawing.Size(36, 12);
            labelControl2.TabIndex = 2;
            labelControl2.Text = "驱动名";
            // 
            // labelControl1
            // 
            labelControl1.Location = new System.Drawing.Point(18, 35);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new System.Drawing.Size(36, 12);
            labelControl1.TabIndex = 1;
            labelControl1.Text = "设备名";
            // 
            // tbCannelID
            // 
            tbCannelID.EditValue = "我的设备";
            tbCannelID.Location = new System.Drawing.Point(98, 30);
            tbCannelID.Name = "tbCannelID";
            tbCannelID.Size = new System.Drawing.Size(187, 18);
            tbCannelID.TabIndex = 0;
            // 
            // groupControl1
            // 
            groupControl1.Controls.Add(cbActive);
            groupControl1.Controls.Add(btDelete);
            groupControl1.Controls.Add(btExport);
            groupControl1.Controls.Add(btImport);
            groupControl1.Controls.Add(btConfirm);
            groupControl1.Controls.Add(tbDescription);
            groupControl1.Controls.Add(labelControl3);
            groupControl1.Controls.Add(pnCannel);
            groupControl1.Controls.Add(cbCannel);
            groupControl1.Controls.Add(labelControl2);
            groupControl1.Controls.Add(labelControl1);
            groupControl1.Controls.Add(tbCannelID);
            groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl1.Location = new System.Drawing.Point(11, 11);
            groupControl1.Name = "groupControl1";
            groupControl1.Size = new System.Drawing.Size(804, 224);
            groupControl1.TabIndex = 3;
            groupControl1.Text = "设备配置";
            // 
            // cbActive
            // 
            cbActive.EditValue = null;
            cbActive.Location = new System.Drawing.Point(18, 144);
            cbActive.Name = "cbActive";
            cbActive.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            cbActive.Properties.OffText = "禁用";
            cbActive.Properties.OnText = "启用";
            cbActive.Size = new System.Drawing.Size(100, 18);
            cbActive.TabIndex = 36;
            // 
            // btDelete
            // 
            btDelete.Location = new System.Drawing.Point(223, 179);
            btDelete.Name = "btDelete";
            btDelete.Size = new System.Drawing.Size(64, 29);
            btDelete.TabIndex = 11;
            btDelete.Text = "删除";
            btDelete.Click += btDelete_Click;
            // 
            // panelControl1
            // 
            panelControl1.Controls.Add(groupControl1);
            panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            panelControl1.Location = new System.Drawing.Point(110, 0);
            panelControl1.Name = "panelControl1";
            panelControl1.Padding = new System.Windows.Forms.Padding(9);
            panelControl1.Size = new System.Drawing.Size(826, 246);
            panelControl1.TabIndex = 7;
            // 
            // ucCannelCfg
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panelControl2);
            Controls.Add(panelControl1);
            Controls.Add(navBarControl1);
            Name = "ucCannelCfg";
            Size = new System.Drawing.Size(936, 508);
            Load += ucCannelCfg_Load;
            ((System.ComponentModel.ISupportInitialize)navBarControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControl2).EndInit();
            panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)groupControl2).EndInit();
            groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemComboBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbDescription.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)pnCannel).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbCannel.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbCannelID.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cbActive.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btExport;
        private DevExpress.XtraEditors.SimpleButton btImport;
        private DevExpress.XtraEditors.SimpleButton btConfirm;
        private DevExpress.XtraEditors.MemoEdit tbDescription;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.PanelControl pnCannel;
        private DevExpress.XtraEditors.ComboBoxEdit cbCannel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit tbCannelID;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btDelete;
        private DevExpress.XtraGrid.Columns.GridColumn TagName;
        private DevExpress.XtraGrid.Columns.GridColumn Address;
        private DevExpress.XtraGrid.Columns.GridColumn DataType;
        private DevExpress.XtraGrid.Columns.GridColumn UOM;
        private DevExpress.XtraGrid.Columns.GridColumn Description;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.ToggleSwitch cbActive;
    }
}
