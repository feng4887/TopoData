using System.Drawing;
using System.Windows.Forms;

namespace TopoData.SubPage
{
    partial class diaExpress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(diaExpress));
            richTextBox1 = new RichTextBox();
            panel1 = new Panel();
            groupBox2 = new GroupBox();
            btCheck = new DevExpress.XtraEditors.SimpleButton();
            btAblut = new DevExpress.XtraEditors.SimpleButton();
            btSelectTag = new DevExpress.XtraEditors.SimpleButton();
            groupBox1 = new GroupBox();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            btRemoveTag = new DevExpress.XtraEditors.SimpleButton();
            btAddTag = new DevExpress.XtraEditors.SimpleButton();
            btClose = new DevExpress.XtraEditors.SimpleButton();
            btSave = new DevExpress.XtraEditors.SimpleButton();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(8, 18);
            richTextBox1.Margin = new Padding(3, 2, 3, 2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(377, 87);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(btClose);
            panel1.Controls.Add(btSave);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(562, 254);
            panel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btCheck);
            groupBox2.Controls.Add(btAblut);
            groupBox2.Controls.Add(btSelectTag);
            groupBox2.Controls.Add(richTextBox1);
            groupBox2.Location = new Point(10, 131);
            groupBox2.Margin = new Padding(3, 2, 3, 2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 2, 3, 2);
            groupBox2.Size = new Size(445, 110);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "表达式";
            // 
            // btCheck
            // 
            btCheck.ImageOptions.Image = (Image)resources.GetObject("btCheck.ImageOptions.Image");
            btCheck.Location = new Point(391, 45);
            btCheck.Margin = new Padding(3, 2, 3, 2);
            btCheck.Name = "btCheck";
            btCheck.Size = new Size(38, 24);
            btCheck.TabIndex = 8;
            btCheck.Click += btCheck_Click;
            // 
            // btAblut
            // 
            btAblut.ImageOptions.Image = (Image)resources.GetObject("btAblut.ImageOptions.Image");
            btAblut.Location = new Point(391, 72);
            btAblut.Margin = new Padding(3, 2, 3, 2);
            btAblut.Name = "btAblut";
            btAblut.Size = new Size(38, 24);
            btAblut.TabIndex = 5;
            btAblut.Click += btAblut_Click;
            // 
            // btSelectTag
            // 
            btSelectTag.Location = new Point(391, 18);
            btSelectTag.Margin = new Padding(3, 2, 3, 2);
            btSelectTag.Name = "btSelectTag";
            btSelectTag.Size = new Size(38, 24);
            btSelectTag.TabIndex = 4;
            btSelectTag.Text = "+";
            btSelectTag.Click += btSelectTag_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listView1);
            groupBox1.Controls.Add(btRemoveTag);
            groupBox1.Controls.Add(btAddTag);
            groupBox1.Location = new Point(12, 6);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(443, 116);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "中间参数列表";
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Location = new Point(6, 24);
            listView1.Margin = new Padding(3, 2, 3, 2);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new Size(377, 88);
            listView1.TabIndex = 2;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "中间变量";
            columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "地址";
            columnHeader2.Width = 160;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "数据类型";
            columnHeader3.Width = 100;
            // 
            // btRemoveTag
            // 
            btRemoveTag.Location = new Point(389, 54);
            btRemoveTag.Margin = new Padding(3, 2, 3, 2);
            btRemoveTag.Name = "btRemoveTag";
            btRemoveTag.Size = new Size(38, 24);
            btRemoveTag.TabIndex = 3;
            btRemoveTag.Text = "-";
            btRemoveTag.Click += btRemoveTag_Click;
            // 
            // btAddTag
            // 
            btAddTag.Location = new Point(389, 24);
            btAddTag.Margin = new Padding(3, 2, 3, 2);
            btAddTag.Name = "btAddTag";
            btAddTag.Size = new Size(38, 26);
            btAddTag.TabIndex = 1;
            btAddTag.Text = "+";
            btAddTag.Click += button2_Click;
            // 
            // btClose
            // 
            btClose.Location = new Point(472, 61);
            btClose.Margin = new Padding(3, 2, 3, 2);
            btClose.Name = "btClose";
            btClose.Size = new Size(75, 26);
            btClose.TabIndex = 4;
            btClose.Text = "关闭";
            btClose.Click += btClose_Click;
            // 
            // btSave
            // 
            btSave.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btSave.Appearance.ForeColor = Color.White;
            btSave.Appearance.Options.UseBackColor = true;
            btSave.Appearance.Options.UseForeColor = true;
            btSave.Location = new Point(472, 30);
            btSave.Margin = new Padding(3, 2, 3, 2);
            btSave.Name = "btSave";
            btSave.Size = new Size(75, 26);
            btSave.TabIndex = 0;
            btSave.Text = "保存";
            btSave.Click += btSave_Click;
            // 
            // diaExpress
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(562, 256);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            IconOptions.Icon = (Icon)resources.GetObject("diaExpress.IconOptions.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "diaExpress";
            StartPosition = FormStartPosition.CenterParent;
            Text = "表达式编辑";
            Load += diaExpress_Load;
            panel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Panel panel1;
        private GroupBox groupBox1;
        private ListView listView1;
        private DevExpress.XtraEditors.SimpleButton btRemoveTag;
        private DevExpress.XtraEditors.SimpleButton btAddTag;
        private DevExpress.XtraEditors.SimpleButton btClose;
        private DevExpress.XtraEditors.SimpleButton btSave;
        private GroupBox groupBox2;
        private DevExpress.XtraEditors.SimpleButton btSelectTag;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private DevExpress.XtraEditors.SimpleButton btCheck;
        private ColumnHeader columnHeader3;
        private DevExpress.XtraEditors.SimpleButton btAblut;
    }
}