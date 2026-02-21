namespace TopoData.SubPage
{
    partial class ucRecipe
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucRecipe));
            panelControl3 = new DevExpress.XtraEditors.PanelControl();
            groupControl3 = new DevExpress.XtraEditors.GroupControl();
            btColumnUpdate = new DevExpress.XtraEditors.SimpleButton();
            btColumnDelete = new DevExpress.XtraEditors.SimpleButton();
            btColumnAdd = new DevExpress.XtraEditors.SimpleButton();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            Column = new DevExpress.XtraGrid.Columns.GridColumn();
            TagName = new DevExpress.XtraGrid.Columns.GridColumn();
            DataType = new DevExpress.XtraGrid.Columns.GridColumn();
            UOM = new DevExpress.XtraGrid.Columns.GridColumn();
            DefaultValue = new DevExpress.XtraGrid.Columns.GridColumn();
            navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            DataTableList = new DevExpress.XtraNavBar.NavBarGroup();
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            tbDescription = new DevExpress.XtraEditors.MemoEdit();
            label2 = new DevExpress.XtraEditors.LabelControl();
            tbTaskName = new DevExpress.XtraEditors.TextEdit();
            label1 = new DevExpress.XtraEditors.LabelControl();
            cbActive = new DevExpress.XtraEditors.ToggleSwitch();
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            barManager1 = new DevExpress.XtraBars.BarManager(components);
            bar2 = new DevExpress.XtraBars.Bar();
            barButtonAdd = new DevExpress.XtraBars.BarButtonItem();
            barButtonRemove = new DevExpress.XtraBars.BarButtonItem();
            barButtonItemSave = new DevExpress.XtraBars.BarButtonItem();
            bar3 = new DevExpress.XtraBars.Bar();
            barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            imageList1 = new System.Windows.Forms.ImageList(components);
            ((System.ComponentModel.ISupportInitialize)panelControl3).BeginInit();
            panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl3).BeginInit();
            groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)navBarControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbDescription.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbTaskName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbActive.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            SuspendLayout();
            // 
            // panelControl3
            // 
            panelControl3.Controls.Add(groupControl3);
            panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl3.Location = new System.Drawing.Point(103, 216);
            panelControl3.Name = "panelControl3";
            panelControl3.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            panelControl3.Size = new System.Drawing.Size(885, 479);
            panelControl3.TabIndex = 9;
            // 
            // groupControl3
            // 
            groupControl3.Controls.Add(btColumnUpdate);
            groupControl3.Controls.Add(btColumnDelete);
            groupControl3.Controls.Add(btColumnAdd);
            groupControl3.Controls.Add(gridControl1);
            groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl3.Location = new System.Drawing.Point(11, 11);
            groupControl3.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            groupControl3.Name = "groupControl3";
            groupControl3.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            groupControl3.Size = new System.Drawing.Size(863, 457);
            groupControl3.TabIndex = 2;
            // 
            // btColumnUpdate
            // 
            btColumnUpdate.Location = new System.Drawing.Point(615, 64);
            btColumnUpdate.Name = "btColumnUpdate";
            btColumnUpdate.Size = new System.Drawing.Size(64, 28);
            btColumnUpdate.TabIndex = 13;
            btColumnUpdate.Text = "更新";
            btColumnUpdate.Click += btColumnUpdate_Click;
            // 
            // btColumnDelete
            // 
            btColumnDelete.Location = new System.Drawing.Point(615, 98);
            btColumnDelete.Name = "btColumnDelete";
            btColumnDelete.Size = new System.Drawing.Size(64, 28);
            btColumnDelete.TabIndex = 12;
            btColumnDelete.Text = "删除";
            btColumnDelete.Click += btColumnDelete_Click;
            // 
            // btColumnAdd
            // 
            btColumnAdd.Location = new System.Drawing.Point(615, 31);
            btColumnAdd.Name = "btColumnAdd";
            btColumnAdd.Size = new System.Drawing.Size(64, 28);
            btColumnAdd.TabIndex = 11;
            btColumnAdd.Text = "添加";
            btColumnAdd.Click += btColumnAdd_Click;
            // 
            // gridControl1
            // 
            gridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            gridControl1.Location = new System.Drawing.Point(11, 32);
            gridControl1.MainView = gridView1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new System.Drawing.Size(579, 414);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { Column, TagName, DataType, UOM, DefaultValue });
            gridView1.DetailHeight = 300;
            gridView1.GridControl = gridControl1;
            gridView1.IndicatorWidth = 30;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 686;
            gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // Column
            // 
            Column.Caption = "RecipeParameter";
            Column.FieldName = "parameter_name";
            Column.MinWidth = 17;
            Column.Name = "Column";
            Column.Visible = true;
            Column.VisibleIndex = 0;
            Column.Width = 154;
            // 
            // TagName
            // 
            TagName.Caption = "TagName";
            TagName.FieldName = "tag_name";
            TagName.MinWidth = 17;
            TagName.Name = "TagName";
            TagName.Visible = true;
            TagName.VisibleIndex = 1;
            TagName.Width = 183;
            // 
            // DataType
            // 
            DataType.Caption = "DataType";
            DataType.FieldName = "data_type";
            DataType.MinWidth = 17;
            DataType.Name = "DataType";
            DataType.Visible = true;
            DataType.VisibleIndex = 2;
            DataType.Width = 115;
            // 
            // UOM
            // 
            UOM.Caption = "UOM";
            UOM.FieldName = "uom";
            UOM.MinWidth = 17;
            UOM.Name = "UOM";
            UOM.Visible = true;
            UOM.VisibleIndex = 3;
            UOM.Width = 64;
            // 
            // DefaultValue
            // 
            DefaultValue.Caption = "DefaultValue";
            DefaultValue.FieldName = "value";
            DefaultValue.MinWidth = 17;
            DefaultValue.Name = "DefaultValue";
            DefaultValue.Visible = true;
            DefaultValue.VisibleIndex = 4;
            DefaultValue.Width = 152;
            // 
            // navBarControl1
            // 
            navBarControl1.ActiveGroup = DataTableList;
            navBarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] { DataTableList });
            navBarControl1.Location = new System.Drawing.Point(0, 24);
            navBarControl1.Name = "navBarControl1";
            navBarControl1.OptionsNavPane.ExpandedWidth = 103;
            navBarControl1.OptionsNavPane.ShowOverflowButton = false;
            navBarControl1.OptionsNavPane.ShowOverflowPanel = false;
            navBarControl1.OptionsNavPane.ShowSplitter = false;
            navBarControl1.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            navBarControl1.Size = new System.Drawing.Size(103, 671);
            navBarControl1.TabIndex = 6;
            navBarControl1.Text = "navBarControl1";
            // 
            // DataTableList
            // 
            DataTableList.Caption = "存储列表";
            DataTableList.Expanded = true;
            DataTableList.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsList;
            DataTableList.Name = "DataTableList";
            DataTableList.NavigationPaneVisible = false;
            // 
            // groupControl1
            // 
            groupControl1.Controls.Add(tbDescription);
            groupControl1.Controls.Add(label2);
            groupControl1.Controls.Add(tbTaskName);
            groupControl1.Controls.Add(label1);
            groupControl1.Controls.Add(cbActive);
            groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl1.Location = new System.Drawing.Point(11, 11);
            groupControl1.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            groupControl1.Name = "groupControl1";
            groupControl1.Padding = new System.Windows.Forms.Padding(77, 77, 77, 77);
            groupControl1.Size = new System.Drawing.Size(863, 170);
            groupControl1.TabIndex = 2;
            // 
            // tbDescription
            // 
            tbDescription.Location = new System.Drawing.Point(123, 73);
            tbDescription.Name = "tbDescription";
            tbDescription.Size = new System.Drawing.Size(247, 45);
            tbDescription.TabIndex = 37;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(54, 92);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(24, 12);
            label2.TabIndex = 36;
            label2.Text = "备注";
            // 
            // tbTaskName
            // 
            tbTaskName.EditValue = "";
            tbTaskName.Location = new System.Drawing.Point(123, 40);
            tbTaskName.Name = "tbTaskName";
            tbTaskName.Size = new System.Drawing.Size(247, 18);
            tbTaskName.TabIndex = 31;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(54, 46);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 12);
            label1.TabIndex = 25;
            label1.Text = "配方名";
            // 
            // cbActive
            // 
            cbActive.EditValue = null;
            cbActive.Location = new System.Drawing.Point(98, 134);
            cbActive.Name = "cbActive";
            cbActive.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            cbActive.Properties.OffText = "禁用";
            cbActive.Properties.OnText = "启用";
            cbActive.Size = new System.Drawing.Size(100, 18);
            cbActive.TabIndex = 35;
            // 
            // panelControl1
            // 
            panelControl1.Controls.Add(groupControl1);
            panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            panelControl1.Location = new System.Drawing.Point(103, 24);
            panelControl1.Name = "panelControl1";
            panelControl1.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            panelControl1.Size = new System.Drawing.Size(885, 192);
            panelControl1.TabIndex = 7;
            // 
            // barManager1
            // 
            barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] { bar2, bar3 });
            barManager1.DockControls.Add(barDockControlTop);
            barManager1.DockControls.Add(barDockControlBottom);
            barManager1.DockControls.Add(barDockControlLeft);
            barManager1.DockControls.Add(barDockControlRight);
            barManager1.DockWindowTabFont = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            barManager1.Form = this;
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { barButtonAdd, barButtonRemove, barButtonItemSave });
            barManager1.MainMenu = bar2;
            barManager1.MaxItemId = 3;
            barManager1.StatusBar = bar3;
            // 
            // bar2
            // 
            bar2.BarName = "Main menu";
            bar2.DockCol = 0;
            bar2.DockRow = 0;
            bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, barButtonAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, barButtonRemove, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, barButtonItemSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph) });
            bar2.OptionsBar.MultiLine = true;
            bar2.OptionsBar.UseWholeRow = true;
            bar2.Text = "Main menu";
            // 
            // barButtonAdd
            // 
            barButtonAdd.Caption = "Add";
            barButtonAdd.Id = 0;
            barButtonAdd.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barButtonAdd.ImageOptions.Image");
            barButtonAdd.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("barButtonAdd.ImageOptions.LargeImage");
            barButtonAdd.Name = "barButtonAdd";
            barButtonAdd.ItemClick += barButtonAdd_ItemClick;
            // 
            // barButtonRemove
            // 
            barButtonRemove.Caption = "Delete";
            barButtonRemove.Id = 1;
            barButtonRemove.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barButtonRemove.ImageOptions.Image");
            barButtonRemove.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("barButtonRemove.ImageOptions.LargeImage");
            barButtonRemove.Name = "barButtonRemove";
            barButtonRemove.ItemClick += barButtonRemove_ItemClick;
            // 
            // barButtonItemSave
            // 
            barButtonItemSave.Caption = "Save";
            barButtonItemSave.Id = 2;
            barButtonItemSave.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barButtonItemSave.ImageOptions.Image");
            barButtonItemSave.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("barButtonItemSave.ImageOptions.LargeImage");
            barButtonItemSave.Name = "barButtonItemSave";
            barButtonItemSave.ItemClick += barButtonItemSave_ItemClick;
            // 
            // bar3
            // 
            bar3.BarName = "Status bar";
            bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            bar3.DockCol = 0;
            bar3.DockRow = 0;
            bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            bar3.OptionsBar.AllowQuickCustomization = false;
            bar3.OptionsBar.DrawDragBorder = false;
            bar3.OptionsBar.UseWholeRow = true;
            bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            barDockControlTop.CausesValidation = false;
            barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            barDockControlTop.Location = new System.Drawing.Point(0, 0);
            barDockControlTop.Manager = barManager1;
            barDockControlTop.Size = new System.Drawing.Size(988, 24);
            // 
            // barDockControlBottom
            // 
            barDockControlBottom.CausesValidation = false;
            barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            barDockControlBottom.Location = new System.Drawing.Point(0, 695);
            barDockControlBottom.Manager = barManager1;
            barDockControlBottom.Size = new System.Drawing.Size(988, 20);
            // 
            // barDockControlLeft
            // 
            barDockControlLeft.CausesValidation = false;
            barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            barDockControlLeft.Manager = barManager1;
            barDockControlLeft.Size = new System.Drawing.Size(0, 671);
            // 
            // barDockControlRight
            // 
            barDockControlRight.CausesValidation = false;
            barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            barDockControlRight.Location = new System.Drawing.Point(988, 24);
            barDockControlRight.Manager = barManager1;
            barDockControlRight.Size = new System.Drawing.Size(0, 671);
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "Action_Edit_32x32.png");
            imageList1.Images.SetKeyName(1, "Action_Document_Object_Inplace_32x32.png");
            imageList1.Images.SetKeyName(2, "Meter.png");
            // 
            // ucRecipe
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panelControl3);
            Controls.Add(panelControl1);
            Controls.Add(navBarControl1);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            Name = "ucRecipe";
            Size = new System.Drawing.Size(988, 715);
            Load += ucRecipe_Load;
            ((System.ComponentModel.ISupportInitialize)panelControl3).EndInit();
            panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)groupControl3).EndInit();
            groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)navBarControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbDescription.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbTaskName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbActive.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)barManager1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SimpleButton btColumnUpdate;
        private DevExpress.XtraEditors.SimpleButton btColumnDelete;
        private DevExpress.XtraEditors.SimpleButton btColumnAdd;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn Column;
        private DevExpress.XtraGrid.Columns.GridColumn TagName;
        private DevExpress.XtraGrid.Columns.GridColumn DataType;
        private DevExpress.XtraGrid.Columns.GridColumn UOM;
        private DevExpress.XtraGrid.Columns.GridColumn DefaultValue;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup DataTableList;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit tbTaskName;
        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.ToggleSwitch cbActive;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.MemoEdit tbDescription;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonAdd;
        private DevExpress.XtraBars.BarButtonItem barButtonRemove;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSave;
    }
}
