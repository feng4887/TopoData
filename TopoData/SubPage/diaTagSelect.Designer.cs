using System.Drawing;
using System.Windows.Forms;

namespace TopoData.SubPage
{
    partial class diaTagSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(diaTagSelect));
            panel1 = new Panel();
            btTnFilter = new DevExpress.XtraEditors.SimpleButton();
            tbTnFilter = new DevExpress.XtraEditors.TextEdit();
            label1 = new DevExpress.XtraEditors.LabelControl();
            btSelect = new DevExpress.XtraEditors.SimpleButton();
            listView1 = new ListView();
            sNum = new ColumnHeader();
            sCannel = new ColumnHeader();
            sTagName = new ColumnHeader();
            sDataType = new ColumnHeader();
            sUOM = new ColumnHeader();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbTnFilter.Properties).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btTnFilter);
            panel1.Controls.Add(tbTnFilter);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btSelect);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(812, 32);
            panel1.TabIndex = 0;
            // 
            // btTnFilter
            // 
            btTnFilter.ImageOptions.Image = (Image)resources.GetObject("btTnFilter.ImageOptions.Image");
            btTnFilter.Location = new Point(298, 7);
            btTnFilter.Margin = new Padding(3, 2, 3, 2);
            btTnFilter.Name = "btTnFilter";
            btTnFilter.Size = new Size(28, 19);
            btTnFilter.TabIndex = 3;
            btTnFilter.Click += btTnFilter_Click;
            // 
            // tbTnFilter
            // 
            tbTnFilter.Location = new Point(90, 7);
            tbTnFilter.Margin = new Padding(3, 2, 3, 2);
            tbTnFilter.Name = "tbTnFilter";
            tbTnFilter.Size = new Size(202, 20);
            tbTnFilter.TabIndex = 2;
            // 
            // label1
            // 
            label1.Location = new Point(37, 10);
            label1.Name = "label1";
            label1.Size = new Size(36, 14);
            label1.TabIndex = 1;
            label1.Text = "变量名";
            // 
            // btSelect
            // 
            btSelect.Location = new Point(700, 6);
            btSelect.Margin = new Padding(3, 2, 3, 2);
            btSelect.Name = "btSelect";
            btSelect.Size = new Size(75, 21);
            btSelect.TabIndex = 0;
            btSelect.Text = "选择";
            btSelect.Click += btSelect_Click;
            // 
            // listView1
            // 
            listView1.BackColor = SystemColors.Window;
            listView1.BorderStyle = BorderStyle.FixedSingle;
            listView1.Columns.AddRange(new ColumnHeader[] { sNum, sCannel, sTagName, sDataType, sUOM });
            listView1.Dock = DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Location = new Point(0, 32);
            listView1.Margin = new Padding(3, 2, 3, 2);
            listView1.Name = "listView1";
            listView1.Size = new Size(812, 339);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            // 
            // sNum
            // 
            sNum.Text = "序号";
            // 
            // sCannel
            // 
            sCannel.Text = "通道";
            sCannel.Width = 120;
            // 
            // sTagName
            // 
            sTagName.Text = "变量名";
            sTagName.Width = 150;
            // 
            // sDataType
            // 
            sDataType.Text = "数据类型";
            sDataType.Width = 70;
            // 
            // sUOM
            // 
            sUOM.Text = "单位";
            sUOM.TextAlign = HorizontalAlignment.Center;
            // 
            // diaTagSelect
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(812, 371);
            Controls.Add(listView1);
            Controls.Add(panel1);
            IconOptions.Icon = (Icon)resources.GetObject("diaTagSelect.IconOptions.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "diaTagSelect";
            Text = "选择";
            Load += diaTagSelect_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbTnFilter.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btSelect;
        private ListView listView1;
        private ColumnHeader sNum;
        private ColumnHeader sCannel;
        private ColumnHeader sTagName;
        private ColumnHeader sUOM;
        private ColumnHeader sDataType;
        private DevExpress.XtraEditors.SimpleButton btTnFilter;
        private DevExpress.XtraEditors.TextEdit tbTnFilter;
        private DevExpress.XtraEditors.LabelControl label1;
    }
}