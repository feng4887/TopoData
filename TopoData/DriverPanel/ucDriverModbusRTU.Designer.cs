using System.Drawing;
using System.Windows.Forms;

namespace auDAManager.DriverPanel
{
    partial class ucDriverModbusRTU
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
            cbComPort = new DevExpress.XtraEditors.ComboBoxEdit();
            cbBaudRate = new DevExpress.XtraEditors.ComboBoxEdit();
            cbParity = new DevExpress.XtraEditors.ComboBoxEdit();
            cbDataBits = new DevExpress.XtraEditors.ComboBoxEdit();
            cbStopBits = new DevExpress.XtraEditors.ComboBoxEdit();
            cbParseData = new DevExpress.XtraEditors.ComboBoxEdit();
            tbDeviceAddress = new DevExpress.XtraEditors.TextEdit();
            label1 = new DevExpress.XtraEditors.LabelControl();
            label2 = new DevExpress.XtraEditors.LabelControl();
            label3 = new DevExpress.XtraEditors.LabelControl();
            label4 = new DevExpress.XtraEditors.LabelControl();
            label5 = new DevExpress.XtraEditors.LabelControl();
            label6 = new DevExpress.XtraEditors.LabelControl();
            label7 = new DevExpress.XtraEditors.LabelControl();
            SuspendLayout();
            //
            // cbComPort
            //
            cbComPort.Location = new Point(86, 3);
            cbComPort.Name = "cbComPort";
            cbComPort.Properties.Items.AddRange(new object[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11" });
            cbComPort.Size = new Size(110, 20);
            cbComPort.TabIndex = 0;
            //
            // cbBaudRate
            //
            cbBaudRate.Location = new Point(86, 30);
            cbBaudRate.Name = "cbBaudRate";
            cbBaudRate.Properties.Items.AddRange(new object[] { "1200", "2400", "4800", "9600", "19200", "38400", "57600" });
            cbBaudRate.Size = new Size(110, 20);
            cbBaudRate.TabIndex = 1;
            //
            // cbParity
            //
            cbParity.Location = new Point(86, 57);
            cbParity.Name = "cbParity";
            cbParity.Properties.Items.AddRange(new object[] { "None", "Odd", "Even" });
            cbParity.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbParity.Size = new Size(110, 20);
            cbParity.TabIndex = 2;
            //
            // cbDataBits
            //
            cbDataBits.Location = new Point(86, 84);
            cbDataBits.Name = "cbDataBits";
            cbDataBits.Properties.Items.AddRange(new object[] { "8", "7" });
            cbDataBits.Size = new Size(110, 20);
            cbDataBits.TabIndex = 3;
            //
            // cbStopBits
            //
            cbStopBits.Location = new Point(86, 111);
            cbStopBits.Name = "cbStopBits";
            cbStopBits.Properties.Items.AddRange(new object[] { "One", "Two" });
            cbStopBits.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbStopBits.Size = new Size(110, 20);
            cbStopBits.TabIndex = 4;
            //
            // cbParseData
            //
            cbParseData.Location = new Point(86, 138);
            cbParseData.Name = "cbParseData";
            cbParseData.Properties.Items.AddRange(new object[] { "CDAB", "ABCD", "BADC", "DCBA" });
            cbParseData.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbParseData.Size = new Size(110, 20);
            cbParseData.TabIndex = 5;
            //
            // tbDeviceAddress
            //
            tbDeviceAddress.Location = new Point(86, 165);
            tbDeviceAddress.Name = "tbDeviceAddress";
            tbDeviceAddress.Size = new Size(110, 20);
            tbDeviceAddress.TabIndex = 6;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(5, 6);
            label1.Name = "label1";
            label1.Size = new Size(66, 14);
            label1.TabIndex = 7;
            label1.Text = "COM Port";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(5, 33);
            label2.Name = "label2";
            label2.Size = new Size(58, 14);
            label2.TabIndex = 8;
            label2.Text = "Baud Rate";
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new Point(5, 60);
            label3.Name = "label3";
            label3.Size = new Size(30, 14);
            label3.TabIndex = 9;
            label3.Text = "Parity";
            //
            // label4
            //
            label4.AutoSize = true;
            label4.Location = new Point(5, 87);
            label4.Name = "label4";
            label4.Size = new Size(51, 14);
            label4.TabIndex = 10;
            label4.Text = "Data Bits";
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Location = new Point(5, 114);
            label5.Name = "label5";
            label5.Size = new Size(48, 14);
            label5.TabIndex = 11;
            label5.Text = "Stop Bits";
            //
            // label6
            //
            label6.AutoSize = true;
            label6.Location = new Point(5, 141);
            label6.Name = "label6";
            label6.Size = new Size(57, 14);
            label6.TabIndex = 12;
            label6.Text = "Parse Data";
            //
            // label7
            //
            label7.AutoSize = true;
            label7.Location = new Point(5, 168);
            label7.Name = "label7";
            label7.Size = new Size(64, 14);
            label7.TabIndex = 13;
            label7.Text = "Device Addr";
            //
            // ucDriverModbusRTU
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tbDeviceAddress);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cbParseData);
            Controls.Add(cbStopBits);
            Controls.Add(cbDataBits);
            Controls.Add(cbParity);
            Controls.Add(cbBaudRate);
            Controls.Add(cbComPort);
            Name = "ucDriverModbusRTU";
            Size = new Size(210, 192);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cbComPort;
        private DevExpress.XtraEditors.ComboBoxEdit cbBaudRate;
        private DevExpress.XtraEditors.ComboBoxEdit cbParity;
        private DevExpress.XtraEditors.ComboBoxEdit cbDataBits;
        private DevExpress.XtraEditors.ComboBoxEdit cbStopBits;
        private DevExpress.XtraEditors.ComboBoxEdit cbParseData;
        private DevExpress.XtraEditors.TextEdit tbDeviceAddress;
        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.LabelControl label4;
        private DevExpress.XtraEditors.LabelControl label5;
        private DevExpress.XtraEditors.LabelControl label6;
        private DevExpress.XtraEditors.LabelControl label7;
    }
}
