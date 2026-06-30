using auDASLib;
using DevExpress.XtraBars;
using DevExpress.XtraBars.FluentDesignSystem;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraScheduler.Reporting;
using Microsoft.IdentityModel.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.Page;
using TopoData.Properties;
namespace TopoData
{

    /// <summary>
    /// Main Form of HiTopo Data Acquisition System
    /// </summary>
    public partial class frmMain : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        #region [Form Constructor]
        public frmMain()
        {
            InitializeComponent();
            navigationFrame1.Dock = DockStyle.Fill;
            navigationFrame1.SelectedPage = PageCannelCfg; //设备画面起始画面
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ucCannelCfg2.Dock = DockStyle.Fill;
            ucSystemCfg1.Dock = DockStyle.Fill;
            ucDiagnose1.Dock = DockStyle.Fill;
            ucHisQuery1.Dock = DockStyle.Fill;
            ucDataTableCfg1.Dock = DockStyle.Fill;
            ucAbout.Dock = DockStyle.Fill;
            ucRecipe.Dock = DockStyle.Fill;
            barButtonEnd.Visibility = BarItemVisibility.Never;

            ApplyDefaultLanguage();

            //Auto Start 启动项
            //if (AutoStart.IsAutoStartViaShortcutEnabled("TopoData"))
            //{
            //    AutoStartOperation();
            //}
        }

        /// <summary>
        ///  Auto start data operation service
        /// 自动启动服务操作事件处理程序
        /// </summary>
        private async void AutoStartOperation()
        {
            try
            {
                barButtonStart.Visibility = BarItemVisibility.Never;
                barButtonEnd.Visibility = BarItemVisibility.Always;

                // 异步调用后台任务
                await Task.Run(() =>
                {
                    HiTopoServer.MainClass.Start();
                });

                StartMqttService();

                toolStrip_ServiceStatus.Caption = $" Service Status: Running ";
                toolStrip_ServiceStatus.ItemAppearance.Normal.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                toolStrip_ServiceStatus.Caption = $" Service Status: Stopped ";
                toolStrip_ServiceStatus.ItemAppearance.Normal.ForeColor = Color.Red;
                DevExpress.XtraEditors.XtraMessageBox.Show($"Start fail:{ex.Message}");
            }
        }

        MqttOperator? _mqttEngine;

        /// <summary>
        /// Pubish Mqqt items
        /// </summary>
        async void StartMqttService()
        {
            _mqttEngine = MqttOperator.Instance;
            _mqttEngine.OnStatusChanged += msg => Console.WriteLine($"[状态] {msg}");
            _mqttEngine.OnError += (msg, ex) => Console.WriteLine($"[错误] {msg}：{ex.Message}");

            _mqttEngine.Start(Program._mqtt_configPath);
        }

        async void StopMqttService()
        {
            if (_mqttEngine != null)
            {
                _mqttEngine.Stop();
                _mqttEngine = null;
            }
        }

        #endregion [Form Constructor]

        #region [button click event]
        /// <summary>
        /// 强制关闭窗口，不需要询问
        /// </summary>
        protected bool ForceClose = false;

        /// <summary>
        /// Import Project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void barButtonImport_ItemClick(object sender, ItemClickEventArgs e)
        {
            string fileName1 = "";
            System.Windows.Forms.OpenFileDialog dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            dlgOpenFile.Filter = "Project.cab|*.cab";

            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {

                if (File.Exists(pubDefine.TagDefine))
                    File.Delete(pubDefine.TagDefine);

                if (File.Exists(master_recipe.RecipePathDefine))
                    File.Delete(master_recipe.RecipePathDefine);

                ZipHelper.ImportZipArchive(dlgOpenFile.FileName, pubDefine.Folder);

                if (MessageBox.Show("Import action sucessfully，Software will be restarted .", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    PubFunction.StartApp("TopoData.exe", 0);
                    ForceClose = true;
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Export Project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<DBTag> list = new List<DBTag>();
            string localFilePath; //获得文件路径 
            string fileNameExt;  //获取文件名，不带路径

            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            //设置文件类型 
            sfd.Filter = "Project.cab|*.cab";

            //设置默认文件类型显示顺序 
            sfd.FilterIndex = 1;

            //保存对话框是否记忆上次打开的目录 
            sfd.RestoreDirectory = true;

            //点了保存按钮进入 
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                localFilePath = sfd.FileName.ToString(); //获得文件路径 
                fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1); //获取文件名，不带路径
            }
            else
            {
                localFilePath = ""; fileNameExt = "";
            }

            if (string.IsNullOrEmpty(localFilePath))
                return;

            try
            {
                if (!File.Exists(pubDefine.TagDefine))
                    return;

                // 创建一个测试文件夹
                string folderPath = pubDefine.Folder + @"Temp";
                Directory.CreateDirectory(folderPath);

                File.Copy(pubDefine.Configuration, folderPath + @"\hiTopoCfg.xml", overwrite: true);
    
                if (File.Exists(pubDefine.TagDefine))
                    File.Copy(pubDefine.TagDefine, folderPath + @"\hiTopoTagDef.xml", overwrite: true);

                if (File.Exists(master_recipe.RecipePathDefine))
                    File.Copy(master_recipe.RecipePathDefine, folderPath + @"\hiTopoRecipeDef.xml", overwrite: true);

                if (File.Exists(Program._mqtt_configPath))
                    File.Copy(Program._mqtt_configPath, folderPath + @"\mqtt_config.xml", overwrite: true);

                string interfile = pubDefine.Folder + fileNameExt;

                if (File.Exists(interfile))
                    File.Delete(interfile);
                ZipHelper.ExportZipArchive(folderPath, interfile);

                File.Copy(interfile, localFilePath, true);

                Directory.Delete(folderPath, true);
                File.Delete(interfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Project export failed：" + ex.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void accordionControlElement1_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = PageSysCfg;
        }

        /// <summary>
        /// 显示数采通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = PageCannelCfg;
        }

        private void accordionControlucDiagnose_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = PageDiagnose;
        }

        private void accordionControlHisQuery_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = PageHisQuery;
        }
        private void accordionControlDatatable_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = PageDataTable;
        }

        /// <summary>
        /// 编辑配方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlRecipe_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = PageRecipe;
        }

        private void accordionControlAbout_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = PageAbout;
        }

        /// <summary>
        /// Stop Service 服务停止事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void barButtonEnd_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                barButtonStart.Visibility = BarItemVisibility.Always;
                barButtonEnd.Visibility = BarItemVisibility.Never;

                await Task.Run(() =>
                {
                    HiTopoServer.MainClass.Stop();
                });

                StopMqttService();

                toolStrip_ServiceStatus.Caption = $" Service Status: Stopped ";
                toolStrip_ServiceStatus.ItemAppearance.Normal.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show($"End fail:{ex.Message}");
            }

        }

        /// <summary>
        /// Start Service 服务启动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void barButtonStart_ItemClick(object sender, ItemClickEventArgs e)
        {
            AutoStartOperation();
        }
        #endregion //[button click event]

        #region [Localization]
        /// <summary>
        /// Language Change 语言切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLanguage_EditValueChanged(object sender, EventArgs e)
        {
            if (cbLanguage.EditValue.ToString() == "English")
            {
                ServerCfg.Instance.Localization = "en";
            }
            else if (cbLanguage.EditValue.ToString() == "简体中文")
            {
                ServerCfg.Instance.Localization = "zh-CN";
            }

            CultureInfo cultureInfo;
            cultureInfo = new CultureInfo(ServerCfg.Instance.Localization);
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ApplyLanguage();

            if (File.Exists(pubDefine.Configuration))
            {
                try
                {
                    ServerCfg sc = XmlHelper.XmlDeserializeFromFile<ServerCfg>(pubDefine.Configuration, Encoding.UTF8);
                    sc.Localization = ServerCfg.Instance.Localization;
                    XmlHelper.XmlSerializeToFile(sc, pubDefine.Configuration, Encoding.UTF8);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void ApplyDefaultLanguage()
        {
            CultureInfo cultureInfo;
            //cultureInfo = new CultureInfo("en");
            cultureInfo = new CultureInfo(ServerCfg.Instance.Localization);
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            if (ServerCfg.Instance.Localization == "en")
            {
                cbLanguage.EditValue = "English";
            }
            else
            {
                cbLanguage.EditValue = "简体中文";
            }

            //Equipment Config
            accordionControlElement3.Text = Resources.MenChannel;

            //Data Table Config
            accordionControlDatatable.Text = Resources.MenDataTable;

            accordionControlRecipe.Text = Resources.MenFormula;

            //Diagnose
            accordionControlucDiagnose.Text = Resources.MenDiagnose;

            //History Query
            accordionControlHisQuery.Text = Resources.MenHistory;
            accordionControlSysCfg.Text   = Resources.MenSystem;
            accordionControlAbout.Text    = Resources.MenAbout;
            barButtonStart.Caption  = Resources.Start;
            barButtonEnd.Caption    = Resources.Close;
            barButtonImport.Caption = Resources.Import;
            barButtonExport.Caption = Resources.Export;

            ucCannelCfg2.ApplyLanguage();
            ucDiagnose1.ApplyLanguage();
            ucHisQuery1.ApplyLanguage();
            ucSystemCfg1.ApplyLanguage();
            ucDataTableCfg1.ApplyLanguage();
            ucRecipe.ApplyLanguage();
            ucAbout.ApplyLanguage();
        }
        #endregion //[Localization]
    }
}
