

using System.Drawing;
using System.Windows.Forms;

namespace TopoData
{
    partial class ucDriverOPCUA
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
            label1 = new DevExpress.XtraEditors.LabelControl();
            tbEndPoint = new DevExpress.XtraEditors.TextEdit();
            btGetEndpoint = new DevExpress.XtraEditors.SimpleButton();
            tbPsw = new DevExpress.XtraEditors.TextEdit();
            label4 = new DevExpress.XtraEditors.LabelControl();
            tbUserName = new DevExpress.XtraEditors.TextEdit();
            label3 = new DevExpress.XtraEditors.LabelControl();
            cbUseUserLogIn = new DevExpress.XtraEditors.CheckEdit();
            label2 = new DevExpress.XtraEditors.LabelControl();
            btGetTag = new DevExpress.XtraEditors.SimpleButton();
            cbEndpointItems = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)tbEndPoint.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbPsw.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbUserName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbUseUserLogIn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbEndpointItems.Properties).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(4, 45);
            label1.Name = "label1";
            label1.Size = new Size(48, 12);
            label1.TabIndex = 0;
            label1.Text = "Endpoint";
            // 
            // tbEndPoint
            // 
            tbEndPoint.EditValue = "opc.tcp://127.0.0.1:4840";
            tbEndPoint.Location = new Point(63, 9);
            tbEndPoint.Margin = new Padding(3, 2, 3, 2);
            tbEndPoint.Name = "tbEndPoint";
            tbEndPoint.Size = new Size(217, 18);
            tbEndPoint.TabIndex = 1;
            // 
            // btGetEndpoint
            // 
            btGetEndpoint.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btGetEndpoint.Appearance.ForeColor = Color.White;
            btGetEndpoint.Appearance.Options.UseBackColor = true;
            btGetEndpoint.Appearance.Options.UseForeColor = true;
            btGetEndpoint.Location = new Point(63, 64);
            btGetEndpoint.Margin = new Padding(3, 2, 3, 2);
            btGetEndpoint.Name = "btGetEndpoint";
            btGetEndpoint.Size = new Size(100, 22);
            btGetEndpoint.TabIndex = 2;
            btGetEndpoint.Text = "获取EndPoint";
            btGetEndpoint.Click += btGetEndpoint_Click;
            // 
            // tbPsw
            // 
            tbPsw.Enabled = false;
            tbPsw.Location = new Point(355, 57);
            tbPsw.Margin = new Padding(3, 2, 3, 2);
            tbPsw.Name = "tbPsw";
            tbPsw.Properties.PasswordChar = '*';
            tbPsw.Size = new Size(107, 18);
            tbPsw.TabIndex = 10;
            // 
            // label4
            // 
            label4.Location = new Point(302, 60);
            label4.Name = "label4";
            label4.Size = new Size(24, 12);
            label4.TabIndex = 9;
            label4.Text = "密码";
            // 
            // tbUserName
            // 
            tbUserName.Enabled = false;
            tbUserName.Location = new Point(355, 27);
            tbUserName.Margin = new Padding(3, 2, 3, 2);
            tbUserName.Name = "tbUserName";
            tbUserName.Size = new Size(107, 18);
            tbUserName.TabIndex = 8;
            // 
            // label3
            // 
            label3.Location = new Point(303, 30);
            label3.Name = "label3";
            label3.Size = new Size(36, 12);
            label3.TabIndex = 7;
            label3.Text = "用户名";
            // 
            // cbUseUserLogIn
            // 
            cbUseUserLogIn.Location = new Point(342, 6);
            cbUseUserLogIn.Margin = new Padding(3, 2, 3, 2);
            cbUseUserLogIn.Name = "cbUseUserLogIn";
            cbUseUserLogIn.Properties.Caption = "使用账号密码登录";
            cbUseUserLogIn.Size = new Size(120, 20);
            cbUseUserLogIn.TabIndex = 6;
            cbUseUserLogIn.CheckStateChanged += cbUseUserLogIn_CheckStateChanged;
            // 
            // label2
            // 
            label2.Location = new Point(34, 12);
            label2.Name = "label2";
            label2.Size = new Size(18, 12);
            label2.TabIndex = 5;
            label2.Text = "URI";
            // 
            // btGetTag
            // 
            btGetTag.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btGetTag.Appearance.ForeColor = Color.White;
            btGetTag.Appearance.Options.UseBackColor = true;
            btGetTag.Appearance.Options.UseForeColor = true;
            btGetTag.Location = new Point(168, 64);
            btGetTag.Margin = new Padding(3, 2, 3, 2);
            btGetTag.Name = "btGetTag";
            btGetTag.Size = new Size(100, 22);
            btGetTag.TabIndex = 11;
            btGetTag.Text = "获取Tag";
            btGetTag.Click += btGetTag_Click;
            // 
            // cbEndpointItems
            // 
            cbEndpointItems.Location = new Point(63, 39);
            cbEndpointItems.Name = "cbEndpointItems";
            cbEndpointItems.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbEndpointItems.Properties.Items.AddRange(new object[] { "OPC UA", "Siemens Profinet" });
            cbEndpointItems.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbEndpointItems.Size = new Size(217, 18);
            cbEndpointItems.TabIndex = 12;
            cbEndpointItems.SelectedIndexChanged += cbEndpointItems_SelectedIndexChanged_1;
            // 
            // ucDriverOPCUA
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cbEndpointItems);
            Controls.Add(btGetTag);
            Controls.Add(tbPsw);
            Controls.Add(label4);
            Controls.Add(tbUserName);
            Controls.Add(label1);
            Controls.Add(label3);
            Controls.Add(tbEndPoint);
            Controls.Add(cbUseUserLogIn);
            Controls.Add(btGetEndpoint);
            Controls.Add(label2);
            Margin = new Padding(0);
            Name = "ucDriverOPCUA";
            Size = new Size(474, 102);
            Load += ucDriverOPCUA_Load;
            ((System.ComponentModel.ISupportInitialize)tbEndPoint.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbPsw.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbUserName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbUseUserLogIn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbEndpointItems.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.TextEdit tbEndPoint;
        private DevExpress.XtraEditors.SimpleButton btGetEndpoint;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.TextEdit tbPsw;
        private DevExpress.XtraEditors.LabelControl label4;
        private DevExpress.XtraEditors.TextEdit tbUserName;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.CheckEdit cbUseUserLogIn;
        private DevExpress.XtraEditors.SimpleButton btGetTag;
        private DevExpress.XtraEditors.ComboBoxEdit cbEndpointItems;
    }
}
