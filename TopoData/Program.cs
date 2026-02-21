using auDASLib;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using TopoData.model;

namespace TopoData
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitStartWork();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        static void InitStartWork()
        {
            string _filerout = pubDefine.Configuration;
            string folderPath = pubDefine.Folder;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //-----------------------------------------------------------------------
            //1. Read Server Config file
            if (!File.Exists(_filerout))
            {
                ServerCfg sc = new ServerCfg();
                sc.cDevices = new List<ConnctDevice>();
                sc.cDevices.Add(new ConnctDevice()
                {
                    CannelID = "MyDevice",
                    iActive = 1,
                    devType = DeviceType.OPCUA,
                    UAParamter = new UACfg()
                    {
                        EndpointUrl = "opc.tcp://localhost:49320",
                        securityPolicy = "None",
                        SecurityMode = "None",
                        UseUserLogIn = false,
                        User = "",
                        Psw = "",
                        ApplicationName = "TopoData OPC UA Client"
                    },
                });
                sc.dataSamples = new List<DataSample>();
                sc.dataSamples.Add(new DataSample()
                {
                    bActive = true,
                    EquipmentName = "MyDevice",
                    TableName   = "ProcessData",
                    TaskName    = "SampleTask",
                    ReportName  = "Report1",
                    ProductName = "Product1",
                    SampleTrig = new DataSampleTrig()
                    {
                        Type = 2,
                        bUseExpression = false,
                        dataSampleTrig_TimeWindow = new DataSampleTrig_TimeWindow(),
                     
                    }
                 
                });
                sc.SqlConfigInfo.DbType = "Microsoft SQL Server";
                sc.SqlConfigInfo.DbName = "master";
                sc.SqlConfigInfo.DataSource = "";
                sc.SqlConfigInfo.StoreMonth = 12;
                XmlHelper.XmlSerializeToFile(sc, _filerout, Encoding.UTF8);
                return;
            }
            else
            {
                try
                {
                    ServerCfg.Instance = XmlHelper.XmlDeserializeFromFile<ServerCfg>(_filerout, Encoding.UTF8);
                }
                catch { }
            }

            //-----------------------------------------------------------------------
            //2. Read Tag Define file
            if (File.Exists(pubDefine.TagDefine))
            {
                //添加通讯点表
                List<DBTag> tags = XmlHelper.XmlDeserializeFromFile<List<DBTag>>(pubDefine.TagDefine, Encoding.UTF8);

                if (tags == null) return;

                if (tags.Count > auDAServer.DACtrl.free_CannelTags)
                {
                    for (int i = tags.Count - 1; i >= auDAServer.DACtrl.free_CannelTags; i--)
                    {
                        tags.RemoveAt(i);
                    }
                    //MessageBox.Show($"免费版本导入变量点数量不能超过 {DACtrl.free_CannelTags} 个。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Debug.WriteLine($"免费版本导入变量点数量不能超过 {auDAServer.DACtrl.free_CannelTags} 个。[/]");
                }

                DataImport.ImportCannelTags(tags);

                //----------------------------------------------------------------------
                //3. Read Recipe
                if (!File.Exists(master_recipe.RecipePathDefine))
                {
                    master_recipe sc = new master_recipe();
                    XmlHelper.XmlSerializeToFile(sc, master_recipe.RecipePathDefine, Encoding.UTF8);
                    return;
                }
                else
                {
                    try
                    {
                        master_recipe.Instance = XmlHelper.XmlDeserializeFromFile<master_recipe>(master_recipe.RecipePathDefine, Encoding.UTF8);
                    }
                    catch { }
                }

                //-----------------------------------------------------------------------
                //4. Initiall WebAPI Server
                try
                {
                    //Start WebAPI Server
                    CancellationTokenSource cancellationToken = new CancellationTokenSource();
                    WebAPIBuilder.StartService(cancellationToken);

                }
                catch { }
            }
        }

     }
}