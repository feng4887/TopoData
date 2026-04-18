using System.Drawing;
using System.Windows.Forms;

namespace TopoData.SubPage
{
    partial class diaWrite
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
            btWrite = new DevExpress.XtraEditors.SimpleButton();
            btCancel = new DevExpress.XtraEditors.SimpleButton();
            label1 = new DevExpress.XtraEditors.LabelControl();
            label2 = new DevExpress.XtraEditors.LabelControl();
            tbAddress = new DevExpress.XtraEditors.TextEdit();
            tbValue = new DevExpress.XtraEditors.TextEdit();
            tbName = new DevExpress.XtraEditors.TextEdit();
            label3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)tbAddress.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbValue.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbName.Properties).BeginInit();
            SuspendLayout();
            // 
            // btWrite
            // 
            btWrite.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btWrite.Appearance.Options.UseBackColor = true;
            btWrite.Location = new Point(73, 104);
            btWrite.Margin = new Padding(3, 2, 3, 2);
            btWrite.Name = "btWrite";
            btWrite.Size = new Size(64, 23);
            btWrite.TabIndex = 0;
            btWrite.Text = "写入";
            btWrite.Click += btWrite_Click;
            // 
            // btCancel
            // 
            btCancel.Location = new Point(159, 104);
            btCancel.Margin = new Padding(3, 2, 3, 2);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(61, 23);
            btCancel.TabIndex = 1;
            btCancel.Text = "关闭";
            btCancel.Click += btCancel_Click;
            // 
            // label1
            // 
            label1.Location = new Point(30, 45);
            label1.Name = "label1";
            label1.Size = new Size(24, 12);
            label1.TabIndex = 2;
            label1.Text = "单位";
            // 
            // label2
            // 
            label2.Location = new Point(30, 69);
            label2.Name = "label2";
            label2.Size = new Size(24, 12);
            label2.TabIndex = 3;
            label2.Text = "数值";
            // 
            // tbAddress
            // 
            tbAddress.Location = new Point(70, 43);
            tbAddress.Margin = new Padding(3, 2, 3, 2);
            tbAddress.Name = "tbAddress";
            tbAddress.Properties.ReadOnly = true;
            tbAddress.Size = new Size(181, 18);
            tbAddress.TabIndex = 4;
            // 
            // tbValue
            // 
            tbValue.Location = new Point(70, 67);
            tbValue.Margin = new Padding(3, 2, 3, 2);
            tbValue.Name = "tbValue";
            tbValue.Size = new Size(181, 18);
            tbValue.TabIndex = 5;
            // 
            // tbName
            // 
            tbName.Location = new Point(73, 20);
            tbName.Margin = new Padding(3, 2, 3, 2);
            tbName.Name = "tbName";
            tbName.Properties.ReadOnly = true;
            tbName.Size = new Size(181, 18);
            tbName.TabIndex = 7;
            // 
            // label3
            // 
            label3.Location = new Point(30, 24);
            label3.Name = "label3";
            label3.Size = new Size(24, 12);
            label3.TabIndex = 6;
            label3.Text = "变量";
            // 
            // diaWrite
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(296, 135);
            Controls.Add(tbName);
            Controls.Add(label3);
            Controls.Add(tbValue);
            Controls.Add(tbAddress);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btCancel);
            Controls.Add(btWrite);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            Name = "diaWrite";
            StartPosition = FormStartPosition.CenterParent;
            Text = "写操作";
            Load += frmWrite_Load;
            ((System.ComponentModel.ISupportInitialize)tbAddress.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbValue.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbName.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btWrite;
        private DevExpress.XtraEditors.SimpleButton btCancel;
        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.TextEdit tbAddress;
        private DevExpress.XtraEditors.TextEdit tbValue;
        private DevExpress.XtraEditors.TextEdit tbName;
        private DevExpress.XtraEditors.LabelControl label3;
    }
}