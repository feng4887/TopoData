using System.Drawing;
using System.Windows.Forms;

namespace auDAManager.DriverPanel
{
    partial class ucDriverModbusTCP
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            tbIP = new DevExpress.XtraEditors.TextEdit();
            tbPort = new DevExpress.XtraEditors.TextEdit();
            label8 = new DevExpress.XtraEditors.LabelControl();
            label1 = new DevExpress.XtraEditors.LabelControl();
            cbParseData = new ComboBox();
            label2 = new DevExpress.XtraEditors.LabelControl();
            tbDeviceAddress = new DevExpress.XtraEditors.TextEdit();
            label3 = new DevExpress.XtraEditors.LabelControl();
            SuspendLayout();
            // 
            // tbIP
            // 
            tbIP.Location = new Point(86, 7);
            tbIP.Name = "tbIP";
            tbIP.Size = new Size(110, 23);
            tbIP.TabIndex = 44;
            tbIP.Text = "192.168.0.1";
            // 
            // tbPort
            // 
            tbPort.Location = new Point(86, 35);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(110, 23);
            tbPort.TabIndex = 45;
            tbPort.Text = "502";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(9, 10);
            label8.Name = "label8";
            label8.Size = new Size(19, 17);
            label8.TabIndex = 42;
            label8.Text = "IP";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 39);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 43;
            label1.Text = "Port";
            // 
            // cbParseData
            // 
            cbParseData.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParseData.FormattingEnabled = true;
            cbParseData.Items.AddRange(new object[] { "CDAB", "ABCD" });
            cbParseData.Location = new Point(86, 91);
            cbParseData.Name = "cbParseData";
            cbParseData.Size = new Size(110, 25);
            cbParseData.TabIndex = 46;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 94);
            label2.Name = "label2";
            label2.Size = new Size(40, 17);
            label2.TabIndex = 47;
            label2.Text = "Parse";
            // 
            // tbDeviceAddress
            // 
            tbDeviceAddress.Location = new Point(86, 63);
            tbDeviceAddress.Name = "tbDeviceAddress";
            tbDeviceAddress.Size = new Size(110, 23);
            tbDeviceAddress.TabIndex = 49;
            tbDeviceAddress.Text = "1";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 68);
            label3.Name = "label3";
            label3.Size = new Size(63, 17);
            label3.TabIndex = 48;
            label3.Text = "Device ID";
            // 
            // ucDriverModbusTCP
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tbDeviceAddress);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cbParseData);
            Controls.Add(tbIP);
            Controls.Add(tbPort);
            Controls.Add(label8);
            Controls.Add(label1);
            Name = "ucDriverModbusTCP";
            Size = new Size(210, 124);
            Load += ucDriverModbusTCP_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DevExpress.XtraEditors.TextEdit tbIP;
        private DevExpress.XtraEditors.TextEdit tbPort;
        private DevExpress.XtraEditors.LabelControl label8;
        private DevExpress.XtraEditors.LabelControl label1;
        private ComboBox cbParseData;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.TextEdit tbDeviceAddress;
        private DevExpress.XtraEditors.LabelControl label3;
    }
}
