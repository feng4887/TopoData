namespace TopoData.Page
{
    partial class ucDiagnose
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
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleIconSet formatConditionRuleIconSet1 = new DevExpress.XtraEditors.FormatConditionRuleIconSet();
            DevExpress.XtraEditors.FormatConditionIconSet formatConditionIconSet1 = new DevExpress.XtraEditors.FormatConditionIconSet();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon1 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon2 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon3 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon4 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucDiagnose));
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            TagName = new DevExpress.XtraGrid.Columns.GridColumn();
            Value = new DevExpress.XtraGrid.Columns.GridColumn();
            Datetime = new DevExpress.XtraGrid.Columns.GridColumn();
            Quality = new DevExpress.XtraGrid.Columns.GridColumn();
            UOM = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            imageCollection1 = new DevExpress.Utils.ImageCollection(components);
            repositoryItemImageComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            cbRecipes = new DevExpress.XtraEditors.ComboBoxEdit();
            btDownloadRecipe = new DevExpress.XtraEditors.SimpleButton();
            label2 = new DevExpress.XtraEditors.LabelControl();
            btTnFilter = new DevExpress.XtraEditors.SimpleButton();
            tbTnFilter = new DevExpress.XtraEditors.TextEdit();
            label1 = new DevExpress.XtraEditors.LabelControl();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemImageComboBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imageCollection1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemImageComboBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cbRecipes.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbTnFilter.Properties).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.Location = new System.Drawing.Point(10, 73);
            gridControl1.MainView = gridView1;
            gridControl1.Name = "gridControl1";
            gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemImageComboBox1, repositoryItemImageComboBox2 });
            gridControl1.Size = new System.Drawing.Size(646, 291);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { TagName, Value, Datetime, Quality, UOM });
            gridView1.DetailHeight = 300;
            gridFormatRule1.Column = Quality;
            gridFormatRule1.Name = "Format0";
            formatConditionIconSet1.CategoryName = "Shapes";
            formatConditionIconSetIcon1.PredefinedName = "TrafficLights4_1.png";
            formatConditionIconSetIcon1.Value = new decimal(new int[] { 75, 0, 0, 0 });
            formatConditionIconSetIcon1.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSetIcon2.PredefinedName = "TrafficLights4_2.png";
            formatConditionIconSetIcon2.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSetIcon3.PredefinedName = "TrafficLights4_3.png";
            formatConditionIconSetIcon3.Value = new decimal(new int[] { 50, 0, 0, int.MinValue });
            formatConditionIconSetIcon3.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSetIcon4.PredefinedName = "TrafficLights4_4.png";
            formatConditionIconSetIcon4.Value = new decimal(new int[] { 100, 0, 0, int.MinValue });
            formatConditionIconSetIcon4.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon1);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon2);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon3);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon4);
            formatConditionIconSet1.Name = "TrafficLights4";
            formatConditionIconSet1.ValueType = DevExpress.XtraEditors.FormatConditionValueType.Number;
            formatConditionRuleIconSet1.IconSet = formatConditionIconSet1;
            gridFormatRule1.Rule = formatConditionRuleIconSet1;
            gridView1.FormatRules.Add(gridFormatRule1);
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsEditForm.PopupEditFormWidth = 686;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.DoubleClick += gridView1_DoubleClick;
            // 
            // TagName
            // 
            TagName.Caption = "TagName";
            TagName.FieldName = "ValueName";
            TagName.MinWidth = 17;
            TagName.Name = "TagName";
            TagName.Visible = true;
            TagName.VisibleIndex = 0;
            TagName.Width = 103;
            // 
            // Value
            // 
            Value.Caption = "Value";
            Value.FieldName = "RealValue";
            Value.MinWidth = 17;
            Value.Name = "Value";
            Value.Visible = true;
            Value.VisibleIndex = 1;
            Value.Width = 106;
            // 
            // Datetime
            // 
            Datetime.Caption = "Timestamp";
            Datetime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss.fff";
            Datetime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Datetime.FieldName = "Timestamp";
            Datetime.MaxWidth = 154;
            Datetime.MinWidth = 17;
            Datetime.Name = "Datetime";
            Datetime.Visible = true;
            Datetime.VisibleIndex = 2;
            Datetime.Width = 154;
            // 
            // Quality
            // 
            Quality.Caption = "Quality";
            Quality.FieldName = "Quality";
            Quality.MinWidth = 17;
            Quality.Name = "Quality";
            Quality.Visible = true;
            Quality.VisibleIndex = 3;
            Quality.Width = 44;
            // 
            // UOM
            // 
            UOM.Caption = "uom";
            UOM.FieldName = "uom";
            UOM.MinWidth = 17;
            UOM.Name = "UOM";
            UOM.Visible = true;
            UOM.VisibleIndex = 4;
            UOM.Width = 47;
            // 
            // repositoryItemImageComboBox1
            // 
            repositoryItemImageComboBox1.AutoHeight = false;
            repositoryItemImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1";
            repositoryItemImageComboBox1.SmallImages = imageCollection1;
            // 
            // imageCollection1
            // 
            imageCollection1.ImageStream = (DevExpress.Utils.ImageCollectionStreamer)resources.GetObject("imageCollection1.ImageStream");
            imageCollection1.Images.SetKeyName(0, "checkbox_32x32.png");
            imageCollection1.Images.SetKeyName(1, "warning_32x32.png");
            // 
            // repositoryItemImageComboBox2
            // 
            repositoryItemImageComboBox2.AutoHeight = false;
            repositoryItemImageComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemImageComboBox2.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] { new DevExpress.XtraEditors.Controls.ImageComboBoxItem("192", "192", 0), new DevExpress.XtraEditors.Controls.ImageComboBoxItem("0", "0", 1) });
            repositoryItemImageComboBox2.Name = "repositoryItemImageComboBox2";
            repositoryItemImageComboBox2.SmallImages = imageCollection1;
            // 
            // panelControl1
            // 
            panelControl1.Controls.Add(cbRecipes);
            panelControl1.Controls.Add(btDownloadRecipe);
            panelControl1.Controls.Add(label2);
            panelControl1.Controls.Add(btTnFilter);
            panelControl1.Controls.Add(tbTnFilter);
            panelControl1.Controls.Add(label1);
            panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            panelControl1.Location = new System.Drawing.Point(9, 9);
            panelControl1.Name = "panelControl1";
            panelControl1.Size = new System.Drawing.Size(825, 35);
            panelControl1.TabIndex = 1;
            // 
            // cbRecipes
            // 
            cbRecipes.Location = new System.Drawing.Point(453, 9);
            cbRecipes.Name = "cbRecipes";
            cbRecipes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbRecipes.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbRecipes.Size = new System.Drawing.Size(217, 18);
            cbRecipes.TabIndex = 13;
            cbRecipes.QueryPopUp += cbRecipes_QueryPopUp;
            // 
            // btDownloadRecipe
            // 
            btDownloadRecipe.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btDownloadRecipe.ImageOptions.Image");
            btDownloadRecipe.Location = new System.Drawing.Point(694, 6);
            btDownloadRecipe.Name = "btDownloadRecipe";
            btDownloadRecipe.Size = new System.Drawing.Size(42, 25);
            btDownloadRecipe.TabIndex = 5;
            btDownloadRecipe.Click += btDownloadRecipe_Click;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(398, 12);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(36, 12);
            label2.TabIndex = 3;
            label2.Text = "Recipe";
            // 
            // btTnFilter
            // 
            btTnFilter.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btTnFilter.ImageOptions.Image");
            btTnFilter.Location = new System.Drawing.Point(310, 6);
            btTnFilter.Name = "btTnFilter";
            btTnFilter.Size = new System.Drawing.Size(42, 25);
            btTnFilter.TabIndex = 2;
            btTnFilter.Click += btTnFilter_Click;
            // 
            // tbTnFilter
            // 
            tbTnFilter.Location = new System.Drawing.Point(88, 9);
            tbTnFilter.Name = "tbTnFilter";
            tbTnFilter.Size = new System.Drawing.Size(207, 18);
            tbTnFilter.TabIndex = 1;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(16, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(48, 12);
            label1.TabIndex = 0;
            label1.Text = "Tag Name";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // ucDiagnose
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Controls.Add(panelControl1);
            Name = "ucDiagnose";
            Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            Size = new System.Drawing.Size(843, 412);
            Load += ucDiagnose_Load;
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemImageComboBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)imageCollection1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemImageComboBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cbRecipes.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbTnFilter.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton btTnFilter;
        private DevExpress.XtraEditors.TextEdit tbTnFilter;
        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn TagName;
        private DevExpress.XtraGrid.Columns.GridColumn Value;
        private DevExpress.XtraGrid.Columns.GridColumn Datetime;
        private DevExpress.XtraGrid.Columns.GridColumn Quality;
        private DevExpress.XtraGrid.Columns.GridColumn UOM;
        private DevExpress.XtraEditors.SimpleButton btDownloadRecipe;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.ComboBoxEdit cbRecipes;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox2;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
    }
}
