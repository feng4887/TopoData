using System.Drawing;
using System.Windows.Forms;

namespace TopoData.SubPage
{
    partial class diaExpressCheck
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            btClose = new DevExpress.XtraEditors.SimpleButton();
            btCheck = new DevExpress.XtraEditors.SimpleButton();
            dataGridView1 = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            DataType = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btClose
            // 
            btClose.Location = new Point(314, 58);
            btClose.Margin = new Padding(3, 2, 3, 2);
            btClose.Name = "btClose";
            btClose.Size = new Size(75, 26);
            btClose.TabIndex = 6;
            btClose.Text = "关闭";
            btClose.Click += btClose_Click;
            // 
            // btCheck
            // 
            btCheck.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btCheck.Appearance.ForeColor = Color.White;
            btCheck.Appearance.Options.UseBackColor = true;
            btCheck.Appearance.Options.UseForeColor = true;
            btCheck.Location = new Point(314, 27);
            btCheck.Margin = new Padding(3, 2, 3, 2);
            btCheck.Name = "btCheck";
            btCheck.Size = new Size(75, 26);
            btCheck.TabIndex = 5;
            btCheck.Text = "检查";
            btCheck.Click += btCheck_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, DataType });
            dataGridView1.GridColor = SystemColors.ScrollBar;
            dataGridView1.Location = new Point(12, 10);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(283, 146);
            dataGridView1.TabIndex = 7;
            // 
            // Column1
            // 
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            Column1.DefaultCellStyle = dataGridViewCellStyle1;
            Column1.HeaderText = "中间变量";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Width = 180;
            // 
            // Column2
            // 
            Column2.HeaderText = "数值设定";
            Column2.Name = "Column2";
            // 
            // DataType
            // 
            DataType.HeaderText = "DataType";
            DataType.Name = "DataType";
            DataType.Visible = false;
            // 
            // diaExpressCheck
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(409, 166);
            Controls.Add(dataGridView1);
            Controls.Add(btClose);
            Controls.Add(btCheck);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "diaExpressCheck";
            StartPosition = FormStartPosition.CenterParent;
            Text = "表达式检查";
            Load += diaExpressCheck_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btClose;
        private DevExpress.XtraEditors.SimpleButton btCheck;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn DataType;
    }
}