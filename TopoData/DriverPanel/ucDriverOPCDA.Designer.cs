using System.Drawing;
using System.Windows.Forms;

namespace TopoData
{
    partial class ucDriverOPCDA
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
            tbHostName = new TextBox();
            label1 = new Label();
            cbEndPoint = new ComboBox();
            label16 = new Label();
            btGetServer = new Button();
            btGetTag = new Button();
            SuspendLayout();
            //
            // tbHostName
            //
            tbHostName.Location = new Point(113, 20);
            tbHostName.Margin = new Padding(4);
            tbHostName.Name = "tbHostName";
            tbHostName.Size = new Size(230, 23);
            tbHostName.TabIndex = 19;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(24, 26);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 18;
            label1.Text = "服务器：";
            //
            // cbEndPoint
            //
            cbEndPoint.FormattingEnabled = true;
            cbEndPoint.Location = new Point(113, 51);
            cbEndPoint.Margin = new Padding(4);
            cbEndPoint.Name = "cbEndPoint";
            cbEndPoint.Size = new Size(230, 25);
            cbEndPoint.TabIndex = 17;
            //
            // label16
            //
            label16.AutoSize = true;
            label16.Location = new Point(24, 57);
            label16.Margin = new Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new Size(86, 17);
            label16.TabIndex = 16;
            label16.Text = "OPC Server：";
            //
            // btGetServer
            //
            btGetServer.BackColor = Color.Teal;
            btGetServer.ForeColor = Color.White;
            btGetServer.Location = new Point(93, 81);
            btGetServer.Name = "btGetServer";
            btGetServer.Size = new Size(117, 31);
            btGetServer.TabIndex = 20;
            btGetServer.Text = "枚举服务";
            btGetServer.UseVisualStyleBackColor = false;
            btGetServer.Click += btGetServer_Click;
            //
            // btGetTag
            //
            btGetTag.BackColor = Color.Teal;
            btGetTag.ForeColor = Color.White;
            btGetTag.Location = new Point(216, 81);
            btGetTag.Name = "btGetTag";
            btGetTag.Size = new Size(117, 31);
            btGetTag.TabIndex = 21;
            btGetTag.Text = "获取Tag";
            btGetTag.UseVisualStyleBackColor = false;
            btGetTag.Click += btGetTag_Click;
            //
            // ucDriverOPCDA
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btGetTag);
            Controls.Add(btGetServer);
            Controls.Add(tbHostName);
            Controls.Add(label1);
            Controls.Add(cbEndPoint);
            Controls.Add(label16);
            Name = "ucDriverOPCDA";
            Size = new Size(368, 127);
            Load += ucDriverOPCDA_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbHostName;
        private Label label1;
        private ComboBox cbEndPoint;
        private Label label16;
        private Button btGetServer;
        private Button btGetTag;
    }
}
