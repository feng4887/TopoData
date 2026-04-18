using System.Drawing;
using System.Windows.Forms;

namespace TopoData.SubPage
{
    partial class diaExpressAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(diaExpressAbout));
            label1 = new DevExpress.XtraEditors.LabelControl();
            label2 = new DevExpress.XtraEditors.LabelControl();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            btYes = new DevExpress.XtraEditors.SimpleButton();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(103, 21);
            label1.Name = "label1";
            label1.Size = new Size(252, 14);
            label1.TabIndex = 0;
            label1.Text = "表达式例子：(a = 100 OR b > 0) AND c <> 2";
            // 
            // label2
            // 
            label2.Location = new Point(103, 44);
            label2.Name = "label2";
            label2.Size = new Size(75, 14);
            label2.TabIndex = 1;
            label2.Text = "a,b,c为参数名";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(457, 80);
            panel1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 21);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(54, 37);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btYes
            // 
            btYes.Location = new Point(344, 85);
            btYes.Margin = new Padding(3, 2, 3, 2);
            btYes.Name = "btYes";
            btYes.Size = new Size(77, 24);
            btYes.TabIndex = 5;
            btYes.Text = "关闭";
            btYes.Click += btYes_Click;
            // 
            // diaExpressAbout
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(457, 118);
            Controls.Add(btYes);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            IconOptions.Icon = (Icon)resources.GetObject("diaExpressAbout.IconOptions.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "diaExpressAbout";
            StartPosition = FormStartPosition.CenterParent;
            Load += diaExpressAbout_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.LabelControl label2;
        private Panel panel1;
        private PictureBox pictureBox1;
        private DevExpress.XtraEditors.SimpleButton btYes;
    }
}