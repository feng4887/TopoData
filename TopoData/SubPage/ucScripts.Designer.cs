namespace TopoData.SubPage
{
    partial class ucScripts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucScripts));
            imageList1 = new System.Windows.Forms.ImageList(components);
            barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            barManager1 = new DevExpress.XtraBars.BarManager(components);
            bar2 = new DevExpress.XtraBars.Bar();
            barButtonAdd = new DevExpress.XtraBars.BarButtonItem();
            barButtonRemove = new DevExpress.XtraBars.BarButtonItem();
            barButtonItemSave = new DevExpress.XtraBars.BarButtonItem();
            barButtonRun = new DevExpress.XtraBars.BarButtonItem();
            bar3 = new DevExpress.XtraBars.Bar();
            barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            tbDescription = new DevExpress.XtraEditors.MemoEdit();
            tbTaskName = new DevExpress.XtraEditors.TextEdit();
            label1 = new DevExpress.XtraEditors.LabelControl();
            cbActive = new DevExpress.XtraEditors.ToggleSwitch();
            DataTableList = new DevExpress.XtraNavBar.NavBarGroup();
            navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            panelControl2 = new DevExpress.XtraEditors.PanelControl();
            groupControl2 = new DevExpress.XtraEditors.GroupControl();
            memoEditScriptText = new ScintillaNET.Scintilla();
            panelControl3 = new DevExpress.XtraEditors.PanelControl();
            btAddReadTag = new DevExpress.XtraEditors.SimpleButton();
            panel2 = new System.Windows.Forms.Panel();
            tbTagName1 = new DevExpress.XtraEditors.TextEdit();
            label3 = new DevExpress.XtraEditors.LabelControl();
            btTagSelect1 = new DevExpress.XtraEditors.SimpleButton();
            btExpress = new DevExpress.XtraEditors.SimpleButton();
            cbExpress = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbDescription.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbTaskName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbActive.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)navBarControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControl2).BeginInit();
            panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl2).BeginInit();
            groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelControl3).BeginInit();
            panelControl3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbTagName1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbExpress.Properties).BeginInit();
            SuspendLayout();
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
            // barDockControlLeft
            // 
            barDockControlLeft.CausesValidation = false;
            barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            barDockControlLeft.Manager = barManager1;
            barDockControlLeft.Size = new System.Drawing.Size(0, 820);
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
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { barButtonAdd, barButtonRemove, barButtonItemSave, barButtonRun });
            barManager1.MainMenu = bar2;
            barManager1.MaxItemId = 4;
            barManager1.StatusBar = bar3;
            // 
            // bar2
            // 
            bar2.BarName = "Main menu";
            bar2.DockCol = 0;
            bar2.DockRow = 0;
            bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, barButtonAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, barButtonRemove, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, barButtonItemSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph), new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, barButtonRun, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph) });
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
            // barButtonRun
            // 
            barButtonRun.Caption = "Run";
            barButtonRun.Id = 3;
            barButtonRun.Name = "barButtonRun";
            barButtonRun.ItemClick += barButtonRun_ItemClick;
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
            barDockControlTop.Size = new System.Drawing.Size(914, 24);
            // 
            // barDockControlBottom
            // 
            barDockControlBottom.CausesValidation = false;
            barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            barDockControlBottom.Location = new System.Drawing.Point(0, 844);
            barDockControlBottom.Manager = barManager1;
            barDockControlBottom.Size = new System.Drawing.Size(914, 20);
            // 
            // barDockControlRight
            // 
            barDockControlRight.CausesValidation = false;
            barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            barDockControlRight.Location = new System.Drawing.Point(914, 24);
            barDockControlRight.Manager = barManager1;
            barDockControlRight.Size = new System.Drawing.Size(0, 820);
            // 
            // panelControl1
            // 
            panelControl1.Controls.Add(groupControl1);
            panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            panelControl1.Location = new System.Drawing.Point(103, 24);
            panelControl1.Name = "panelControl1";
            panelControl1.Padding = new System.Windows.Forms.Padding(9);
            panelControl1.Size = new System.Drawing.Size(811, 229);
            panelControl1.TabIndex = 11;
            // 
            // groupControl1
            // 
            groupControl1.Controls.Add(labelControl1);
            groupControl1.Controls.Add(tbDescription);
            groupControl1.Controls.Add(tbTaskName);
            groupControl1.Controls.Add(label1);
            groupControl1.Controls.Add(cbActive);
            groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl1.Location = new System.Drawing.Point(11, 11);
            groupControl1.Margin = new System.Windows.Forms.Padding(9);
            groupControl1.Name = "groupControl1";
            groupControl1.Padding = new System.Windows.Forms.Padding(77);
            groupControl1.Size = new System.Drawing.Size(789, 207);
            groupControl1.TabIndex = 2;
            // 
            // labelControl1
            // 
            labelControl1.Location = new System.Drawing.Point(72, 76);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new System.Drawing.Size(24, 12);
            labelControl1.TabIndex = 38;
            labelControl1.Text = "备注";
            // 
            // tbDescription
            // 
            tbDescription.Location = new System.Drawing.Point(161, 76);
            tbDescription.Name = "tbDescription";
            tbDescription.Size = new System.Drawing.Size(247, 45);
            tbDescription.TabIndex = 37;
            // 
            // tbTaskName
            // 
            tbTaskName.EditValue = "";
            tbTaskName.Location = new System.Drawing.Point(161, 43);
            tbTaskName.Name = "tbTaskName";
            tbTaskName.Size = new System.Drawing.Size(247, 18);
            tbTaskName.TabIndex = 31;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(72, 46);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 12);
            label1.TabIndex = 25;
            label1.Text = "配方名";
            // 
            // cbActive
            // 
            cbActive.EditValue = null;
            cbActive.Location = new System.Drawing.Point(161, 137);
            cbActive.Name = "cbActive";
            cbActive.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            cbActive.Properties.OffText = "禁用";
            cbActive.Properties.OnText = "启用";
            cbActive.Size = new System.Drawing.Size(100, 18);
            cbActive.TabIndex = 35;
            // 
            // DataTableList
            // 
            DataTableList.Caption = "存储列表";
            DataTableList.Expanded = true;
            DataTableList.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsList;
            DataTableList.Name = "DataTableList";
            DataTableList.NavigationPaneVisible = false;
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
            navBarControl1.Size = new System.Drawing.Size(103, 820);
            navBarControl1.TabIndex = 10;
            navBarControl1.Text = "Python scripts";
            // 
            // panelControl2
            // 
            panelControl2.Controls.Add(groupControl2);
            panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl2.Location = new System.Drawing.Point(103, 253);
            panelControl2.Name = "panelControl2";
            panelControl2.Padding = new System.Windows.Forms.Padding(9);
            panelControl2.Size = new System.Drawing.Size(811, 591);
            panelControl2.TabIndex = 12;
            // 
            // groupControl2
            // 
            groupControl2.Controls.Add(btAddReadTag);
            groupControl2.Controls.Add(memoEditScriptText);
            groupControl2.Controls.Add(panelControl3);
            groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl2.Location = new System.Drawing.Point(11, 11);
            groupControl2.Margin = new System.Windows.Forms.Padding(9);
            groupControl2.Name = "groupControl2";
            groupControl2.Padding = new System.Windows.Forms.Padding(77);
            groupControl2.Size = new System.Drawing.Size(789, 569);
            groupControl2.TabIndex = 2;
            // 
            // memoEditScriptText
            // 
            memoEditScriptText.AutocompleteListSelectedBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            memoEditScriptText.LexerName = null;
            memoEditScriptText.Location = new System.Drawing.Point(14, 173);
            memoEditScriptText.Name = "memoEditScriptText";
            memoEditScriptText.Size = new System.Drawing.Size(761, 393);
            memoEditScriptText.TabIndex = 41;
            // 
            // panelControl3
            // 
            panelControl3.Controls.Add(panel2);
            panelControl3.Controls.Add(btExpress);
            panelControl3.Controls.Add(cbExpress);
            panelControl3.Location = new System.Drawing.Point(3, 21);
            panelControl3.Name = "panelControl3";
            panelControl3.Size = new System.Drawing.Size(783, 105);
            panelControl3.TabIndex = 40;
            // 
            // btAddReadTag
            // 
            btAddReadTag.Location = new System.Drawing.Point(680, 144);
            btAddReadTag.Name = "btAddReadTag";
            btAddReadTag.Size = new System.Drawing.Size(95, 23);
            btAddReadTag.TabIndex = 37;
            btAddReadTag.Text = "+ Read";
            btAddReadTag.Click += btAddReadTag_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(tbTagName1);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(btTagSelect1);
            panel2.Location = new System.Drawing.Point(64, 18);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(507, 32);
            panel2.TabIndex = 14;
            // 
            // tbTagName1
            // 
            tbTagName1.EditValue = "";
            tbTagName1.Location = new System.Drawing.Point(94, 8);
            tbTagName1.Name = "tbTagName1";
            tbTagName1.Size = new System.Drawing.Size(186, 18);
            tbTagName1.TabIndex = 38;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(4, 12);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(60, 12);
            label3.TabIndex = 2;
            label3.Text = "bool触发点";
            // 
            // btTagSelect1
            // 
            btTagSelect1.Location = new System.Drawing.Point(288, 8);
            btTagSelect1.Name = "btTagSelect1";
            btTagSelect1.Size = new System.Drawing.Size(37, 20);
            btTagSelect1.TabIndex = 3;
            btTagSelect1.Text = "...";
            btTagSelect1.Click += btTagSelect1_Click;
            // 
            // btExpress
            // 
            btExpress.Location = new System.Drawing.Point(182, 66);
            btExpress.Name = "btExpress";
            btExpress.Size = new System.Drawing.Size(95, 23);
            btExpress.TabIndex = 15;
            btExpress.Text = "表达式";
            btExpress.Click += btExpress_Click;
            // 
            // cbExpress
            // 
            cbExpress.Location = new System.Drawing.Point(68, 69);
            cbExpress.Name = "cbExpress";
            cbExpress.Properties.Caption = "使用表达式";
            cbExpress.Size = new System.Drawing.Size(99, 20);
            cbExpress.TabIndex = 36;
            // 
            // ucScripts
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panelControl2);
            Controls.Add(panelControl1);
            Controls.Add(navBarControl1);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            Name = "ucScripts";
            Size = new System.Drawing.Size(914, 864);
            ((System.ComponentModel.ISupportInitialize)barManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbDescription.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbTaskName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbActive.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)navBarControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControl2).EndInit();
            panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)groupControl2).EndInit();
            groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)panelControl3).EndInit();
            panelControl3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbTagName1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbExpress.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem barButtonAdd;
        private DevExpress.XtraBars.BarButtonItem barButtonRemove;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSave;
        private DevExpress.XtraBars.BarButtonItem barButtonRun;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit tbDescription;
        private DevExpress.XtraEditors.TextEdit tbTaskName;
        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.ToggleSwitch cbActive;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup DataTableList;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.CheckEdit cbExpress;
        private DevExpress.XtraEditors.SimpleButton btExpress;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.TextEdit tbTagName1;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.SimpleButton btTagSelect1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btAddReadTag;
        private ScintillaNET.Scintilla memoEditScriptText;
    }
}
