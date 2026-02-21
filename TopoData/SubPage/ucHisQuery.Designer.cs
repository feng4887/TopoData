namespace TopoData.Page
{
    partial class ucHisQuery
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
            splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            dtStime = new DevExpress.XtraEditors.DateTimeOffsetEdit();
            label3 = new DevExpress.XtraEditors.LabelControl();
            dtEtime = new DevExpress.XtraEditors.DateTimeOffsetEdit();
            btExport = new DevExpress.XtraEditors.SimpleButton();
            btSearch = new DevExpress.XtraEditors.SimpleButton();
            label4 = new DevExpress.XtraEditors.LabelControl();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel1).BeginInit();
            splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel2).BeginInit();
            splitContainerControl1.Panel2.SuspendLayout();
            splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radioGroup1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtStime.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtEtime.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            SuspendLayout();
            // 
            // splitContainerControl1
            // 
            splitContainerControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerControl1.Horizontal = false;
            splitContainerControl1.Location = new System.Drawing.Point(9, 9);
            splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            splitContainerControl1.Panel1.Controls.Add(groupControl1);
            splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            splitContainerControl1.Panel2.Controls.Add(gridControl1);
            splitContainerControl1.Panel2.Text = "Panel2";
            splitContainerControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            splitContainerControl1.Size = new System.Drawing.Size(1185, 621);
            splitContainerControl1.SplitterPosition = 75;
            splitContainerControl1.TabIndex = 0;
            // 
            // groupControl1
            // 
            groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            groupControl1.Controls.Add(radioGroup1);
            groupControl1.Controls.Add(dtStime);
            groupControl1.Controls.Add(label3);
            groupControl1.Controls.Add(dtEtime);
            groupControl1.Controls.Add(btExport);
            groupControl1.Controls.Add(btSearch);
            groupControl1.Controls.Add(label4);
            groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            groupControl1.Location = new System.Drawing.Point(0, 0);
            groupControl1.Name = "groupControl1";
            groupControl1.ShowCaption = false;
            groupControl1.Size = new System.Drawing.Size(1181, 75);
            groupControl1.TabIndex = 8;
            groupControl1.Text = "groupControl1";
            // 
            // radioGroup1
            // 
            radioGroup1.Location = new System.Drawing.Point(302, 8);
            radioGroup1.Name = "radioGroup1";
            radioGroup1.Properties.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            radioGroup1.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] { new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "宽表", true, null, "wide_table"), new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "时序表", true, null, "realtime_table") });
            radioGroup1.Size = new System.Drawing.Size(167, 62);
            radioGroup1.TabIndex = 6;
            // 
            // dtStime
            // 
            dtStime.EditValue = null;
            dtStime.Location = new System.Drawing.Point(110, 16);
            dtStime.Name = "dtStime";
            dtStime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dtStime.Properties.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            dtStime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtStime.Properties.EditFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            dtStime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dtStime.Properties.MaskSettings.Set("mask", "yyyy/MM/dd HH:mm:ss");
            dtStime.Size = new System.Drawing.Size(161, 18);
            dtStime.TabIndex = 2;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(48, 23);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(48, 12);
            label3.TabIndex = 0;
            label3.Text = "开始时间";
            // 
            // dtEtime
            // 
            dtEtime.EditValue = null;
            dtEtime.Location = new System.Drawing.Point(110, 45);
            dtEtime.Name = "dtEtime";
            dtEtime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dtEtime.Properties.MaskSettings.Set("mask", "yyyy/MM/dd HH:mm:ss");
            dtEtime.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            dtEtime.Size = new System.Drawing.Size(161, 18);
            dtEtime.TabIndex = 3;
            // 
            // btExport
            // 
            btExport.Location = new System.Drawing.Point(609, 28);
            btExport.Name = "btExport";
            btExport.Size = new System.Drawing.Size(64, 31);
            btExport.TabIndex = 5;
            btExport.Text = "导出";
            btExport.Click += btExport_Click;
            // 
            // btSearch
            // 
            btSearch.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btSearch.Appearance.Options.UseBackColor = true;
            btSearch.Location = new System.Drawing.Point(529, 28);
            btSearch.Name = "btSearch";
            btSearch.Size = new System.Drawing.Size(64, 31);
            btSearch.TabIndex = 4;
            btSearch.Text = "查询";
            btSearch.Click += btSearch_Click;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(48, 47);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(48, 12);
            label4.TabIndex = 1;
            label4.Text = "结束时间";
            // 
            // gridControl1
            // 
            gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControl1.Location = new System.Drawing.Point(0, 0);
            gridControl1.MainView = gridView1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new System.Drawing.Size(1181, 532);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.DetailHeight = 300;
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 686;
            gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // ucHisQuery
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerControl1);
            Name = "ucHisQuery";
            Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            Size = new System.Drawing.Size(1203, 639);
            Load += ucHisQuery_Load;
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel1).EndInit();
            splitContainerControl1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel2).EndInit();
            splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1).EndInit();
            splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)radioGroup1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtStime.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtEtime.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.LabelControl label4;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.DateTimeOffsetEdit dtStime;
        private DevExpress.XtraEditors.SimpleButton btExport;
        private DevExpress.XtraEditors.SimpleButton btSearch;
        private DevExpress.XtraEditors.DateTimeOffsetEdit dtEtime;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}
