namespace TopoData
{
    partial class ucSystemCfg
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbRestURI = new DevExpress.XtraEditors.TextEdit();
            label2 = new DevExpress.XtraEditors.LabelControl();
            btSaveRestURI = new DevExpress.XtraEditors.SimpleButton();
            cbDB = new DevExpress.XtraEditors.ComboBoxEdit();
            tbDBPath = new DevExpress.XtraEditors.TextEdit();
            btSelectDbPath = new DevExpress.XtraEditors.SimpleButton();
            cbEableLog4 = new DevExpress.XtraEditors.CheckEdit();
            tbWdSQLDBName = new DevExpress.XtraEditors.TextEdit();
            panelDb = new System.Windows.Forms.Panel();
            label5 = new DevExpress.XtraEditors.LabelControl();
            label6 = new DevExpress.XtraEditors.LabelControl();
            panelRoot = new DevExpress.XtraEditors.PanelControl();
            label8 = new DevExpress.XtraEditors.LabelControl();
            label7 = new DevExpress.XtraEditors.LabelControl();
            tbWdSQLUserName = new DevExpress.XtraEditors.TextEdit();
            tbWdSQLDPsw = new DevExpress.XtraEditors.TextEdit();
            tbWdSQLPort = new DevExpress.XtraEditors.TextEdit();
            btSelectSqLiteDbFile = new DevExpress.XtraEditors.SimpleButton();
            label3 = new DevExpress.XtraEditors.LabelControl();
            label9 = new DevExpress.XtraEditors.LabelControl();
            label4 = new DevExpress.XtraEditors.LabelControl();
            btCreateDB = new DevExpress.XtraEditors.SimpleButton();
            label10 = new DevExpress.XtraEditors.LabelControl();
            tbMonth = new DevExpress.XtraEditors.TextEdit();
            tbWdSQLName = new DevExpress.XtraEditors.TextEdit();
            label14 = new DevExpress.XtraEditors.LabelControl();
            btTestSQL = new DevExpress.XtraEditors.SimpleButton();
            btSaveSQL = new DevExpress.XtraEditors.SimpleButton();
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            groupControl2 = new DevExpress.XtraEditors.GroupControl();
            groupControl3 = new DevExpress.XtraEditors.GroupControl();
            cbActive = new DevExpress.XtraEditors.ToggleSwitch();
            ((System.ComponentModel.ISupportInitialize)tbRestURI.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbDB.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbDBPath.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbEableLog4.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLDBName.Properties).BeginInit();
            panelDb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelRoot).BeginInit();
            panelRoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLUserName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLDPsw.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLPort.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbMonth.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl2).BeginInit();
            groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl3).BeginInit();
            groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cbActive.Properties).BeginInit();
            SuspendLayout();
            // 
            // tbRestURI
            // 
            tbRestURI.EditValue = "http://0.0.0.0:8083/";
            tbRestURI.Location = new System.Drawing.Point(105, 32);
            tbRestURI.Name = "tbRestURI";
            tbRestURI.Size = new System.Drawing.Size(152, 18);
            tbRestURI.TabIndex = 27;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(29, 34);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(66, 12);
            label2.TabIndex = 26;
            label2.Text = "Restful URI";
            // 
            // btSaveRestURI
            // 
            btSaveRestURI.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btSaveRestURI.Appearance.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btSaveRestURI.Appearance.ForeColor = System.Drawing.Color.White;
            btSaveRestURI.Appearance.Options.UseBackColor = true;
            btSaveRestURI.Appearance.Options.UseFont = true;
            btSaveRestURI.Appearance.Options.UseForeColor = true;
            btSaveRestURI.Location = new System.Drawing.Point(314, 23);
            btSaveRestURI.Name = "btSaveRestURI";
            btSaveRestURI.Size = new System.Drawing.Size(64, 36);
            btSaveRestURI.TabIndex = 25;
            btSaveRestURI.Text = "保存配置";
            btSaveRestURI.Click += btSaveRestURI_Click;
            // 
            // cbDB
            // 
            cbDB.Location = new System.Drawing.Point(110, 27);
            cbDB.Name = "cbDB";
            cbDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbDB.Properties.Items.AddRange(new object[] { "Microsoft SQL Server", "PostgreSQL", "MySql" });
            cbDB.Size = new System.Drawing.Size(149, 18);
            cbDB.TabIndex = 33;
            cbDB.SelectedIndexChanged += cbDB_SelectedIndexChanged;
            // 
            // tbDBPath
            // 
            tbDBPath.Location = new System.Drawing.Point(91, 8);
            tbDBPath.Name = "tbDBPath";
            tbDBPath.Properties.ReadOnly = true;
            tbDBPath.Size = new System.Drawing.Size(149, 18);
            tbDBPath.TabIndex = 26;
            // 
            // btSelectDbPath
            // 
            btSelectDbPath.Appearance.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btSelectDbPath.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            btSelectDbPath.Appearance.Options.UseFont = true;
            btSelectDbPath.Appearance.Options.UseForeColor = true;
            btSelectDbPath.Location = new System.Drawing.Point(246, 6);
            btSelectDbPath.Name = "btSelectDbPath";
            btSelectDbPath.Size = new System.Drawing.Size(22, 19);
            btSelectDbPath.TabIndex = 27;
            btSelectDbPath.Text = "...";
            btSelectDbPath.Click += btSelectDbPath_Click;
            // 
            // cbEableLog4
            // 
            cbEableLog4.Location = new System.Drawing.Point(20, 245);
            cbEableLog4.Name = "cbEableLog4";
            cbEableLog4.Properties.Caption = "本地Log日志";
            cbEableLog4.Size = new System.Drawing.Size(140, 20);
            cbEableLog4.TabIndex = 32;
            // 
            // tbWdSQLDBName
            // 
            tbWdSQLDBName.Location = new System.Drawing.Point(110, 107);
            tbWdSQLDBName.Name = "tbWdSQLDBName";
            tbWdSQLDBName.Size = new System.Drawing.Size(152, 18);
            tbWdSQLDBName.TabIndex = 21;
            // 
            // panelDb
            // 
            panelDb.Controls.Add(label5);
            panelDb.Controls.Add(label6);
            panelDb.Controls.Add(panelRoot);
            panelDb.Controls.Add(label7);
            panelDb.Controls.Add(tbWdSQLUserName);
            panelDb.Controls.Add(tbWdSQLDPsw);
            panelDb.Controls.Add(tbWdSQLPort);
            panelDb.Location = new System.Drawing.Point(12, 132);
            panelDb.Name = "panelDb";
            panelDb.Size = new System.Drawing.Size(291, 108);
            panelDb.TabIndex = 30;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(6, 5);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(36, 12);
            label5.TabIndex = 17;
            label5.Text = "用户名";
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(6, 31);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(24, 12);
            label6.TabIndex = 18;
            label6.Text = "密码";
            // 
            // panelRoot
            // 
            panelRoot.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            panelRoot.Controls.Add(btSelectDbPath);
            panelRoot.Controls.Add(tbDBPath);
            panelRoot.Controls.Add(label8);
            panelRoot.Location = new System.Drawing.Point(5, 74);
            panelRoot.Name = "panelRoot";
            panelRoot.Size = new System.Drawing.Size(285, 32);
            panelRoot.TabIndex = 25;
            // 
            // label8
            // 
            label8.Location = new System.Drawing.Point(9, 9);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(60, 12);
            label8.TabIndex = 32;
            label8.Text = "数据库位置";
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(6, 57);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(24, 12);
            label7.TabIndex = 19;
            label7.Text = "端口";
            // 
            // tbWdSQLUserName
            // 
            tbWdSQLUserName.EditValue = "sa";
            tbWdSQLUserName.Location = new System.Drawing.Point(98, 3);
            tbWdSQLUserName.Name = "tbWdSQLUserName";
            tbWdSQLUserName.Size = new System.Drawing.Size(152, 18);
            tbWdSQLUserName.TabIndex = 22;
            // 
            // tbWdSQLDPsw
            // 
            tbWdSQLDPsw.Location = new System.Drawing.Point(98, 28);
            tbWdSQLDPsw.Name = "tbWdSQLDPsw";
            tbWdSQLDPsw.Properties.PasswordChar = '*';
            tbWdSQLDPsw.Size = new System.Drawing.Size(152, 18);
            tbWdSQLDPsw.TabIndex = 23;
            // 
            // tbWdSQLPort
            // 
            tbWdSQLPort.EditValue = "1433";
            tbWdSQLPort.Location = new System.Drawing.Point(98, 54);
            tbWdSQLPort.Name = "tbWdSQLPort";
            tbWdSQLPort.Size = new System.Drawing.Size(50, 18);
            tbWdSQLPort.TabIndex = 24;
            // 
            // btSelectSqLiteDbFile
            // 
            btSelectSqLiteDbFile.Appearance.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btSelectSqLiteDbFile.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            btSelectSqLiteDbFile.Appearance.Options.UseFont = true;
            btSelectSqLiteDbFile.Appearance.Options.UseForeColor = true;
            btSelectSqLiteDbFile.Location = new System.Drawing.Point(263, 107);
            btSelectSqLiteDbFile.Name = "btSelectSqLiteDbFile";
            btSelectSqLiteDbFile.Size = new System.Drawing.Size(22, 19);
            btSelectSqLiteDbFile.TabIndex = 31;
            btSelectSqLiteDbFile.Text = "...";
            btSelectSqLiteDbFile.Visible = false;
            btSelectSqLiteDbFile.Click += btSelectSqLiteDbFile_Click;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(20, 110);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(36, 12);
            label3.TabIndex = 16;
            label3.Text = "数据库";
            // 
            // label9
            // 
            label9.Location = new System.Drawing.Point(20, 33);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(60, 12);
            label9.TabIndex = 28;
            label9.Text = "数据库类型";
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(165, 59);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(12, 12);
            label4.TabIndex = 7;
            label4.Text = "月";
            // 
            // btCreateDB
            // 
            btCreateDB.Appearance.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btCreateDB.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            btCreateDB.Appearance.Options.UseFont = true;
            btCreateDB.Appearance.Options.UseForeColor = true;
            btCreateDB.Location = new System.Drawing.Point(318, 74);
            btCreateDB.Name = "btCreateDB";
            btCreateDB.Size = new System.Drawing.Size(78, 36);
            btCreateDB.TabIndex = 24;
            btCreateDB.Text = "DB创建";
            btCreateDB.Click += btCreateDB_Click;
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(20, 59);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(84, 12);
            label10.TabIndex = 5;
            label10.Text = "数据库保留时间";
            // 
            // tbMonth
            // 
            tbMonth.EditValue = "12";
            tbMonth.Location = new System.Drawing.Point(110, 57);
            tbMonth.Name = "tbMonth";
            tbMonth.Size = new System.Drawing.Size(50, 18);
            tbMonth.TabIndex = 6;
            // 
            // tbWdSQLName
            // 
            tbWdSQLName.EditValue = "127.0.0.1";
            tbWdSQLName.Location = new System.Drawing.Point(110, 82);
            tbWdSQLName.Name = "tbWdSQLName";
            tbWdSQLName.Size = new System.Drawing.Size(152, 18);
            tbWdSQLName.TabIndex = 20;
            // 
            // label14
            // 
            label14.Location = new System.Drawing.Point(20, 85);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(48, 12);
            label14.TabIndex = 15;
            label14.Text = "SQL Host";
            // 
            // btTestSQL
            // 
            btTestSQL.Appearance.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btTestSQL.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            btTestSQL.Appearance.Options.UseFont = true;
            btTestSQL.Appearance.Options.UseForeColor = true;
            btTestSQL.Location = new System.Drawing.Point(318, 115);
            btTestSQL.Name = "btTestSQL";
            btTestSQL.Size = new System.Drawing.Size(78, 36);
            btTestSQL.TabIndex = 13;
            btTestSQL.Text = "DB测试";
            btTestSQL.Click += btTestSQL_Click;
            // 
            // btSaveSQL
            // 
            btSaveSQL.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            btSaveSQL.Appearance.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            btSaveSQL.Appearance.ForeColor = System.Drawing.Color.White;
            btSaveSQL.Appearance.Options.UseBackColor = true;
            btSaveSQL.Appearance.Options.UseFont = true;
            btSaveSQL.Appearance.Options.UseForeColor = true;
            btSaveSQL.Location = new System.Drawing.Point(318, 33);
            btSaveSQL.Name = "btSaveSQL";
            btSaveSQL.Size = new System.Drawing.Size(78, 36);
            btSaveSQL.TabIndex = 14;
            btSaveSQL.Text = "保存配置";
            btSaveSQL.Click += btSaveSQL_Click;
            // 
            // groupControl1
            // 
            groupControl1.Controls.Add(cbDB);
            groupControl1.Controls.Add(label9);
            groupControl1.Controls.Add(btSaveSQL);
            groupControl1.Controls.Add(cbEableLog4);
            groupControl1.Controls.Add(btTestSQL);
            groupControl1.Controls.Add(tbWdSQLDBName);
            groupControl1.Controls.Add(label14);
            groupControl1.Controls.Add(tbWdSQLName);
            groupControl1.Controls.Add(btSelectSqLiteDbFile);
            groupControl1.Controls.Add(tbMonth);
            groupControl1.Controls.Add(label3);
            groupControl1.Controls.Add(label10);
            groupControl1.Controls.Add(btCreateDB);
            groupControl1.Controls.Add(label4);
            groupControl1.Controls.Add(panelDb);
            groupControl1.Location = new System.Drawing.Point(43, 106);
            groupControl1.Name = "groupControl1";
            groupControl1.Size = new System.Drawing.Size(415, 268);
            groupControl1.TabIndex = 28;
            groupControl1.Text = "数据库配置";
            // 
            // groupControl2
            // 
            groupControl2.Controls.Add(tbRestURI);
            groupControl2.Controls.Add(btSaveRestURI);
            groupControl2.Controls.Add(label2);
            groupControl2.Location = new System.Drawing.Point(43, 31);
            groupControl2.Name = "groupControl2";
            groupControl2.Size = new System.Drawing.Size(412, 62);
            groupControl2.TabIndex = 29;
            groupControl2.Text = "WebAPI接口";
            // 
            // groupControl3
            // 
            groupControl3.Controls.Add(cbActive);
            groupControl3.Location = new System.Drawing.Point(43, 390);
            groupControl3.Name = "groupControl3";
            groupControl3.Size = new System.Drawing.Size(412, 62);
            groupControl3.TabIndex = 30;
            groupControl3.Text = "自动启动";
            // 
            // cbActive
            // 
            cbActive.EditValue = null;
            cbActive.Location = new System.Drawing.Point(29, 33);
            cbActive.Name = "cbActive";
            cbActive.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            cbActive.Properties.OffText = "禁用";
            cbActive.Properties.OnText = "启用";
            cbActive.Size = new System.Drawing.Size(100, 18);
            cbActive.TabIndex = 36;
            cbActive.Toggled += cbActive_Toggled;
            // 
            // ucSystemCfg
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupControl3);
            Controls.Add(groupControl2);
            Controls.Add(groupControl1);
            Name = "ucSystemCfg";
            Size = new System.Drawing.Size(633, 505);
            Load += ucSystemCfg_Load;
            ((System.ComponentModel.ISupportInitialize)tbRestURI.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbDB.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbDBPath.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbEableLog4.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLDBName.Properties).EndInit();
            panelDb.ResumeLayout(false);
            panelDb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)panelRoot).EndInit();
            panelRoot.ResumeLayout(false);
            panelRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLUserName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLDPsw.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLPort.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbMonth.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbWdSQLName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl2).EndInit();
            groupControl2.ResumeLayout(false);
            groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)groupControl3).EndInit();
            groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cbActive.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DevExpress.XtraEditors.TextEdit tbRestURI;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.SimpleButton btSaveRestURI;
        private DevExpress.XtraEditors.TextEdit tbDBPath;
        private DevExpress.XtraEditors.SimpleButton btSelectDbPath;
        private DevExpress.XtraEditors.CheckEdit cbEableLog4;
        private DevExpress.XtraEditors.TextEdit tbWdSQLDBName;
        private System.Windows.Forms.Panel panelDb;
        private DevExpress.XtraEditors.LabelControl label5;
        private DevExpress.XtraEditors.LabelControl label6;
        private DevExpress.XtraEditors.LabelControl label7;
        private DevExpress.XtraEditors.TextEdit tbWdSQLUserName;
        private DevExpress.XtraEditors.TextEdit tbWdSQLDPsw;
        private DevExpress.XtraEditors.TextEdit tbWdSQLPort;
        private DevExpress.XtraEditors.SimpleButton btSelectSqLiteDbFile;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.LabelControl label9;
        private DevExpress.XtraEditors.LabelControl label4;
        private DevExpress.XtraEditors.SimpleButton btCreateDB;
        private DevExpress.XtraEditors.LabelControl label10;
        private DevExpress.XtraEditors.TextEdit tbMonth;
        private DevExpress.XtraEditors.TextEdit tbWdSQLName;
        private DevExpress.XtraEditors.LabelControl label14;
        private DevExpress.XtraEditors.SimpleButton btTestSQL;
        private DevExpress.XtraEditors.SimpleButton btSaveSQL;
        private DevExpress.XtraEditors.ComboBoxEdit cbDB;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.PanelControl panelRoot;
        private DevExpress.XtraEditors.LabelControl label8;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.ToggleSwitch cbActive;
    }
}
