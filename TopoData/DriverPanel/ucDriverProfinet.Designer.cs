using System.Drawing;
using System.Windows.Forms;

namespace TopoData
{
    partial class ucDriverProfinet
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
            tbStationSlotMain = new TextBox();
            label3 = new Label();
            tbStationIPMain = new TextBox();
            tbStationRackMain = new TextBox();
            label8 = new Label();
            label1 = new Label();
            label2 = new Label();
            label19 = new Label();
            label16 = new Label();
            label15 = new Label();
            label14 = new Label();
            label11 = new Label();
            SuspendLayout();
            // 
            // tbStationSlotMain
            // 
            tbStationSlotMain.Location = new Point(62, 69);
            tbStationSlotMain.Name = "tbStationSlotMain";
            tbStationSlotMain.Size = new Size(110, 23);
            tbStationSlotMain.TabIndex = 41;
            tbStationSlotMain.Text = "2";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 69);
            label3.Name = "label3";
            label3.Size = new Size(30, 17);
            label3.TabIndex = 40;
            label3.Text = "Slot";
            // 
            // tbStationIPMain
            // 
            tbStationIPMain.Location = new Point(62, 11);
            tbStationIPMain.Name = "tbStationIPMain";
            tbStationIPMain.Size = new Size(110, 23);
            tbStationIPMain.TabIndex = 37;
            tbStationIPMain.Text = "192.168.0.1";
            // 
            // tbStationRackMain
            // 
            tbStationRackMain.Location = new Point(62, 40);
            tbStationRackMain.Name = "tbStationRackMain";
            tbStationRackMain.Size = new Size(110, 23);
            tbStationRackMain.TabIndex = 38;
            tbStationRackMain.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(20, 11);
            label8.Name = "label8";
            label8.Size = new Size(19, 17);
            label8.TabIndex = 35;
            label8.Text = "IP";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 40);
            label1.Name = "label1";
            label1.Size = new Size(36, 17);
            label1.TabIndex = 36;
            label1.Text = "Rack";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(219, 86);
            label2.Name = "label2";
            label2.Size = new Size(164, 17);
            label2.TabIndex = 57;
            label2.Text = "任何配置修改，需要重启软件";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(219, 68);
            label19.Name = "label19";
            label19.Size = new Size(95, 17);
            label19.TabIndex = 56;
            label19.Text = "S7-400 : 看配置";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label16.Location = new Point(219, 0);
            label16.Name = "label16";
            label16.Size = new Size(115, 17);
            label16.TabIndex = 55;
            label16.Text = "PLC配置注意事项：";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(219, 51);
            label15.Name = "label15";
            label15.Size = new Size(156, 17);
            label15.TabIndex = 54;
            label15.Text = "S7-1500 : Rack 0;Slot 0/1;";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(219, 34);
            label14.Name = "label14";
            label14.Size = new Size(156, 17);
            label14.TabIndex = 53;
            label14.Text = "S7-1200 : Rack 0;Slot 0/1;";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(219, 17);
            label11.Name = "label11";
            label11.Size = new Size(137, 17);
            label11.TabIndex = 52;
            label11.Text = "S7-300 : Rack 0;Slot 2;";
            // 
            // ucDriverProfinet
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label2);
            Controls.Add(label19);
            Controls.Add(label16);
            Controls.Add(label15);
            Controls.Add(label14);
            Controls.Add(label11);
            Controls.Add(tbStationSlotMain);
            Controls.Add(label3);
            Controls.Add(tbStationIPMain);
            Controls.Add(tbStationRackMain);
            Controls.Add(label8);
            Controls.Add(label1);
            Name = "ucDriverProfinet";
            Size = new Size(404, 116);
            Load += ucDriverProfinet_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbStationSlotMain;
        private Label label3;
        private TextBox tbStationIPMain;
        private TextBox tbStationRackMain;
        private Label label8;
        private Label label1;
        private Label label2;
        private Label label19;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label11;
    }
}
