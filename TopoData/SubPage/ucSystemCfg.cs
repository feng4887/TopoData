using auDASLib;
using auDASLib.Model;
using DevExpress.XtraEditors;
using IWshRuntimeLibrary;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.model;
using TopoData.Properties;

namespace TopoData
{
    public partial class ucSystemCfg : DevExpress.XtraEditors.XtraUserControl
    {
        public ucSystemCfg()
        {
            InitializeComponent();
        }

        private void ucSystemCfg_Load(object sender, EventArgs e)
        {
            InitCfg();
            cbDB.Text = string.IsNullOrEmpty(ServerCfg.Instance.SqlConfigInfo.DbType) ? "Microsoft SQL Server" : ServerCfg.Instance.SqlConfigInfo.DbType;
            panelRoot.Visible = true;
            btSelectDbPath.Visible = true;

            if (AutoStart.IsAutoStartViaShortcutEnabled("TopoData"))
            { 
                cbActive.IsOn = true;
            }
            else
            {
                cbActive.IsOn = false;
            }

            ApplyLanguage();
        }

        public void ApplyLanguage()
        {
            btSaveRestURI.Text = Resources.Save;
            btSaveSQL.Text = Resources.Save;
            btCreateDB.Text = Resources.DBCreate;
            btTestSQL.Text = Resources.DBTest;
            groupControl1.Text = Resources.DBCfg;
            groupControl2.Text = Resources.WebAPICfg;
            groupControl3.Text = Resources.SftwareAutoStart;
            cbActive.Properties.OffText = Resources.Off;
            cbActive.Properties.OnText = Resources.On;
            label4.Text = Resources.Month;
            label9.Text = Resources.DBType;
            label10.Text = Resources.DBStoreTime;
            label3.Text = Resources.DB;
            label8.Text = Resources.DBLocation;
            label5.Text = Resources.User_user;
            label6.Text = Resources.User_psd;
            label7.Text = Resources.port;
            cbEableLog4.Text = Resources.EnableLog4;

        }

        public void InitCfg()
        {
            ServerCfg sc = XmlHelper.XmlDeserializeFromFile<ServerCfg>(pubDefine.Configuration, Encoding.UTF8);
            if (sc != null)
            {
                tbWdSQLName.Text = ServerCfg.Instance.SqlConfigInfo.ServerName;
                tbWdSQLDBName.Text = ServerCfg.Instance.SqlConfigInfo.DbName;
                tbWdSQLDPsw.Text = ServerCfg.Instance.SqlConfigInfo.Psw;
                tbWdSQLUserName.Text = ServerCfg.Instance.SqlConfigInfo.User;
                tbWdSQLPort.Text = ServerCfg.Instance.SqlConfigInfo.Port.ToString();
                tbMonth.Text = ServerCfg.Instance.SqlConfigInfo.StoreMonth.ToString();
                tbRestURI.Text = ServerCfg.Instance.RestUri1;
                cbDB.Text = string.IsNullOrEmpty(ServerCfg.Instance.SqlConfigInfo.DbType) ? "Microsoft SQL Server" : ServerCfg.Instance.SqlConfigInfo.DbType;
                cbEableLog4.Checked = ServerCfg.Instance.EnableLog4;
                btSelectDbPath.Visible = false;
                panelRoot.Visible = false;
                btSelectDbPath.Visible = false;
            }
        }

        private void btSaveRestURI_Click(object sender, EventArgs e)
        {
            try
            {
                ServerCfg.Instance.RestUri1 = tbRestURI.Text;
            }
            catch (Exception ex)
            {
                //new EBR_Collection.diaMessageY(ex.Message, "Information").ShowDialog();
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            XmlHelper.XmlSerializeToFile(ServerCfg.Instance, pubDefine.Configuration, Encoding.UTF8);
        }

        private void btSaveSQL_Click(object sender, EventArgs e)
        {
            try
            {
                int months = 12;
                ServerCfg.Instance.SqlConfigInfo.DbName = tbWdSQLDBName.Text;
                ServerCfg.Instance.SqlConfigInfo.DbType = cbDB.Text;
                if (cbDB.Text != "SqlLite")
                {
                    months = int.Parse(tbMonth.Text);
                    ServerCfg.Instance.SqlConfigInfo.StoreMonth = months;
                    ServerCfg.Instance.SqlConfigInfo.ServerName = tbWdSQLName.Text;
                    ServerCfg.Instance.SqlConfigInfo.Psw = tbWdSQLDPsw.Text;
                    ServerCfg.Instance.SqlConfigInfo.User = tbWdSQLUserName.Text;
                    ServerCfg.Instance.SqlConfigInfo.Port = int.Parse(tbWdSQLPort.Text);
                }
            }
            catch (Exception ex)
            {
                //new EBR_Collection.diaMessageY(ex.Message, Resources.PromptMessage).ShowDialog();
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            XmlHelper.XmlSerializeToFile(ServerCfg.Instance, pubDefine.Configuration, Encoding.UTF8);
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCreateDB_Click(object sender, EventArgs e)
        {
            int port = 1433;
            if (cbDB.Text != "SqlLite")
                port = int.Parse(tbWdSQLPort.Text);
            else
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "DB file (*.db;*.db3;*.sqlite;*.sqlite3)|*.db;*.db3;*.sqlite;*.sqlite3|所有文件 (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    tbWdSQLDBName.Text = filePath;
                }
                else
                    return;
            }

            string DbName = tbWdSQLDBName.Text;// 数据库名
            string MasterDbName = "master";
            string DbUserName = tbWdSQLUserName.Text;
            string psw = tbWdSQLDPsw.Text;
            string strdb = cbDB.Text;
            string host = tbWdSQLName.Text;
            string dbpath = tbDBPath.Text;

            switch (strdb)
            {
                case "PostgreSQL":
                    MasterDbName = "postgres";
                    break;
                case "MySql":
                    MasterDbName = "mysql";
                    break;
            }

            DBCode dc = SugarDao.GetDbCode(strdb);
            SugarDao db = new SugarDao(
                    dc,
                    DbUserName,
                    psw,
                    MasterDbName,
                    host,
                    port
                );

            SqlSugarClient sc = db.GetInstance();

            // 使用 Invoke 回到 UI 线程
            this.Invoke(new Action(() =>
            {
                Form owner = this.FindForm();
                if (sc != null)
                {
                    bool IsAnyDatabase = false;
                    //<判断数据库是否存在，不存在则创建数据库>
                    try
                    {
                        // 查询 sys.databases 判断是否存在
                        IsAnyDatabase = db.DatabaseExists(DbName);
                        bool retCreate = false;
                        if (!IsAnyDatabase)
                        {
                            if (strdb == "SqlLite")
                            {
                                retCreate = db.CreateDatabase(DbName);
                            }
                            else
                            {
                                if (strdb == "Microsoft SQL Server" && string.IsNullOrEmpty(dbpath))
                                {
                                    //new EBR_Collection.diaMessageY(Resources.msgDBCreateFail1, "Information").ShowDialog(owner);
                                    DevExpress.XtraEditors.XtraMessageBox.Show("Please fill Database storage path .", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //MessageBox.Show("请指定数据库存储路径，创建数据库失败。");
                                    return;
                                }

                                retCreate = db.CreateDatabase(DbName, dbpath);
                                if (db != null)
                                {
                                    if (strdb == "Microsoft SQL Server")
                                    {   // 2. 设置为 Simple 模式
                                        //sc.Ado.ExecuteCommand($@"
                                        //ALTER DATABASE [{DbName}] SET RECOVERY SIMPLE;
                                        //");
                                        // 3. 设置数据文件和日志文件属性（增长策略 + 限制）
                                        sc.Ado.ExecuteCommand($@"
                                            ALTER DATABASE [{DbName}] MODIFY FILE 
                                            (
                                                NAME = N'{DbName}', 
                                                MAXSIZE = UNLIMITED, 
                                                FILEGROWTH = 100MB
                                            );");
                                        sc.Ado.ExecuteCommand($@"
                                            ALTER DATABASE [{DbName}] MODIFY FILE
                                            (
                                                NAME = N'{DbName}_log',
                                                MAXSIZE = UNLIMITED,
                                                FILEGROWTH = 100MB
                                            );");
                                    }
                                    db.Close();
                                    db = null;
                                }
                            }
                        }
                        //</判断数据库是否存在，不存在则创建数据库>

                        // 检查目标数据库是否存在
                        SugarDao db2 = new SugarDao(
                           dc,
                           DbUserName,
                           psw,
                           DbName,
                           host,
                           port
                        );

                        if (db2.IsConnected())
                        {
                            SqlSugarClient sc2 = db2.GetInstance();
                            sc2.CodeFirst.SetStringDefaultLength(200);
                            sc2.CodeFirst.InitTables(typeof(AppLogMessage), typeof(pubHisValues));

                            if (retCreate == true)
                            {
                                //diaMessageY i = new diaMessageY(Resources.msgDBCreateOK, Resources.PromptComTest);
                                //i.StartPosition = FormStartPosition.CenterParent;
                                //i.ShowDialog(owner);
                                DevExpress.XtraEditors.XtraMessageBox.Show("Create Database Sucessful.", "Create Table", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //MessageBox.Show("创建数据库成功");                         
                            }
                        }
                        else
                        {
                            //diaMessageY i = new diaMessageY(Resources.msgDBCreateFail2, Resources.PromptComTest);
                            //i.StartPosition = FormStartPosition.CenterParent;
                            //i.ShowDialog(owner);
                            //MessageBox.Show("创建表失败", "创建表");
                            DevExpress.XtraEditors.XtraMessageBox.Show("Create table fail", "Create Table", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        db2.Close();
                        db2 = null;
                    }
                    catch (Exception ex)
                    {
                        //diaMessageY i = new diaMessageY(Resources.msgDBcFail, Resources.PromptComTest);
                        //i.StartPosition = FormStartPosition.CenterParent;
                        //i.ShowDialog(owner);

                        //MessageBox.Show(ex.Message, "创建数据库失败"
                        //    , MessageBoxButtons.OK, MessageBoxIcon.Error);

                        DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "Create Database fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }));

            if (db != null)
            {
                db.Close();
                db = null;
            }
        }

        private async void btTestSQL_Click(object sender, EventArgs e)
        {
            try
            {
                int port = 1433;
                if (cbDB.Text != "SqlLite")
                    port = int.Parse(tbWdSQLPort.Text);

                DBCode dc = SugarDao.GetDbCode(ServerCfg.Instance.SqlConfigInfo.DbType);
                PaeResult ret = await SugarDao.IsConnected(
                       dc,
                       tbWdSQLUserName.Text,
                       tbWdSQLDPsw.Text,
                       tbWdSQLDBName.Text,
                       tbWdSQLName.Text,
                       port);

                // 获取当前窗口作为父窗口
                Form owner = this.FindForm() ?? Application.OpenForms[0];

                if (ret.IsSucess)
                {
                    //MessageBox.Show("数据库通讯正常", "通讯测试", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //using (diaMessageY i = new diaMessageY(Resources.msgDBcOK, Resources.PromptComTest))
                    //{
                    //    i.StartPosition = FormStartPosition.CenterParent;
                    //    i.ShowDialog(owner);  // 明确指定 Owner
                    //}
                    DialogResult dialogResult = DevExpress.XtraEditors.XtraMessageBox.Show("Database communication is normal", "Communication Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //using (diaMessageY i = new diaMessageY(Resources.msgDBcFail, Resources.PromptComTest))
                    //{
                    //    i.StartPosition = FormStartPosition.CenterParent;
                    //    i.ShowDialog(owner);  // 明确指定 Owner
                    //}
                    //MessageBox.Show("数据库通讯失败", "通讯测试", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    DialogResult dialogResult = DevExpress.XtraEditors.XtraMessageBox.Show("Database communication failed", "Communication Test", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "\r\n" + ex.InnerException + "\r\n" + ex.InnerException);

                Form owner = this.FindForm() ?? Application.OpenForms[0];
                //using (diaMessageY i = new diaMessageY(
                //    ex.Message + "\r\n" + ex.InnerException?.Message,
                //    Resources.PromptComTest))
                //{
                //    i.StartPosition = FormStartPosition.CenterParent;
                //    i.ShowDialog(owner);
                //}
                DialogResult dialogResult = DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message + "\r\n" + ex.InnerException, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btSelectSqLiteDbFile_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建OpenFileDialog实例
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

                // 设置对话框的标题
                openFileDialog.Title = "Please select db file";

                // 设置对话框的初始目录（可选，可以根据需要设置）
                // openFileDialog.InitialDirectory = "C:\\";

                // 设置对话框中显示的文件类型过滤器
                openFileDialog.Filter = "DB file (*.db;*.db3;*.sqlite;*.sqlite3)|*.db;*.db3;*.sqlite;*.sqlite3|所有文件 (*.*)|*.*";

                // 显示对话框并检查用户是否选择了文件
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取用户选择的文件路径
                    string filePath = openFileDialog.FileName;
                    tbWdSQLDBName.Text = filePath;

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// 数据库类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDBCfg();
        }

        private void InitDBCfg()
        {
            if (cbDB.Text == "SqlLite")
            {
                panelDb.Visible = false;
                btSelectSqLiteDbFile.Visible = true;
                tbDBPath.Text = "";
                tbWdSQLDPsw.Text = "";
                tbWdSQLUserName.Text = "";
                tbWdSQLName.Text = "";
                tbWdSQLPort.Text = "";

                tbDBPath.Enabled = false;
                tbWdSQLDPsw.Enabled = false;
                tbWdSQLUserName.Enabled = false;
                tbWdSQLName.Enabled = false;
                tbWdSQLPort.Enabled = false;
                //label1.Text = Resources.DBFile;
            }
            else
            {
                panelDb.Visible = true;
                if (cbDB.Text == "PostgreSQL" || cbDB.Text == "MySql")
                {
                    panelRoot.Visible = false;
                    btSelectDbPath.Visible = false;
                }
                else if (cbDB.Text == "Microsoft SQL Server")
                {
                    panelRoot.Visible = true;
                    //tbWdSQLPort.Text = "3306";
                    btSelectDbPath.Visible = true;
                }

                btSelectSqLiteDbFile.Visible = false;
                //label1.Text = Resources.DB;
                tbWdSQLName.Text = ServerCfg.Instance.SqlConfigInfo.ServerName;
                tbWdSQLDBName.Text = ServerCfg.Instance.SqlConfigInfo.DbName;
                tbWdSQLDPsw.Text = ServerCfg.Instance.SqlConfigInfo.Psw;
                tbWdSQLUserName.Text = ServerCfg.Instance.SqlConfigInfo.User;
                tbWdSQLPort.Text = ServerCfg.Instance.SqlConfigInfo.Port.ToString();
                tbMonth.Text = ServerCfg.Instance.SqlConfigInfo.StoreMonth.ToString();
                tbRestURI.Text = ServerCfg.Instance.RestUri1;
                //cbDB.Text            = string.IsNullOrEmpty(ServerCfg.Instance.SqlConfigInfo.DbType) ? "Microsoft SQL Server" : ServerCfg.Instance.SqlConfigInfo.DbType;

                tbDBPath.Enabled = true;

                tbWdSQLDPsw.Enabled = true;
                tbWdSQLUserName.Enabled = true;
                tbWdSQLName.Enabled = true;
                tbWdSQLPort.Enabled = true;
            }
        }

        private void btSelectDbPath_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "请选择文件夹";
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        tbDBPath.Text = folderBrowserDialog.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message); 
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Aotomatic start switch event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbActive_Toggled(object sender, EventArgs e)
        {
            if (cbActive.IsOn)
            {
                //快捷方式的描述、名称的默认值是当前的进程名，自启动默认为正常窗口，一般情况下不需要手动设置
                //设置快捷方式的描述，
                AutoStart.Instance.QuickDescribe = "TopoData";
                //设置快捷方式的名称
                AutoStart.Instance.QuickName = "TopoData";
                //设置自启动的窗口类型，后台服务类的软件可以设置为最小窗口
                AutoStart.Instance.WindowStyle = WshWindowStyle.WshMinimizedFocus;

                //快捷方式设置true时，有就忽略、没有就创建，自启动快捷方式只能存在一个
                //设置开机自启动，true 自启动，false 不自启动
                AutoStart.Instance.SetAutoStart(true);
                //设置桌面快捷方式，true 创建桌面快捷方式（有就跳过，没有就创建），false 删除桌面快捷方式
                AutoStart.Instance.SetDesktopQuick(true);
            }
            else
            {
                //设置快捷方式的描述，
                AutoStart.Instance.QuickDescribe = "TopoData";
                //设置快捷方式的名称
                AutoStart.Instance.QuickName = "TopoData";
                //设置自启动的窗口类型，后台服务类的软件可以设置为最小窗口
                AutoStart.Instance.WindowStyle = WshWindowStyle.WshMinimizedFocus;

                //快捷方式设置true时，有就忽略、没有就创建，自启动快捷方式只能存在一个
                //设置开机自启动，true 自启动，false 不自启动
                AutoStart.Instance.SetAutoStart(false);
                //设置桌面快捷方式，true 创建桌面快捷方式（有就跳过，没有就创建），false 删除桌面快捷方式
                //AutoStart2.Instance.SetDesktopQuick(true);
            }
        }
    }
}
