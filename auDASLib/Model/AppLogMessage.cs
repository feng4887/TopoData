using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace auDASLib.Model
{
    public enum AppLogPriority
    {
        Infor = 0,
        Error = 1,
        Warning = 2,
        FatalError = 3
    }

    /// <summary>
    /// 用于记录软件日志
    /// 注册软件列表
    /// </summary>
    public class AppLogAppDefine
    {
        AppLogAppDefine()
        {
            SoftwareList.Add("dasManager");
            SoftwareList.Add("dasServer");
        }

        public List<string> SoftwareList { get; set; } = new List<string>();
        public static string dasManager { get; set; } = "dasManager";
        public static string dasServer { get; set; } = "dasServer";
    }

    /// <summary>
    /// 用于记录应用程序日志
    /// 数据库表名：tb_au_his_AppLogMessage
    /// </summary>
    [SugarTable("tb_au_his_applogmessage")]
    public class AppLogMessage
    {
        [SqlSugar.SugarColumn(Length = 100, IsPrimaryKey = true, IsNullable = false)]
        public string f_id { get; set; } = Guid.NewGuid().ToString("N");

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? f_last_modify_time { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string app_name { get; set; }
        //[SqlSugar.SugarColumn(ColumnDataType = "varchar(max)", IsNullable = true)]

        [SqlSugar.SugarColumn(Length = 300, IsNullable = true)]

        public string description { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string location { get; set; }


        [SqlSugar.SugarColumn( IsNullable = true,ColumnDescription = "0-Infor;1-Error;2-Warning;3-Fatal Error")]
        public int? priority { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true)]
        public int? state { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)] // 默认类型
        public string rest_resp { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string log_flag { get; set; }

        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="sQLCfg"></param>
        /// <param name="app"></param>
        /// <param name="msg"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static int InsertLog(SugarDao sd, string app, string msg, int priority,string log_flag = "")
        {
            try 
            {
                //SugarDao sd = new SugarDao(ServerCfg.Instance.SqlConfigInfo);

                if (sd == null)
                    return -1000;

                if (!sd.IsConnected())
                    return -1001;   

                SqlSugarClient sc = sd.GetInstance();
            
                // 删除超过20天的旧日志
                DateTime thresholdDate = DateTime.Now.AddDays(-20);
                sc.Deleteable<AppLogMessage>()
                  .Where(it => it.f_last_modify_time < thresholdDate)
                  .ExecuteCommand();

                //string guidString = Guid.NewGuid().ToString("N");
                var id = SnowFlakeSingle.Instance.NextId();//也可以在程序中直接获取ID

                int ret = sc.Insertable<AppLogMessage>(new AppLogMessage() { 
                    f_id = $"{app}-{id}",
                    app_name = app,
                    description = msg,
                    f_last_modify_time = DateTime.Now,
                    location = Environment.MachineName,
                    priority = priority,
                    state = 0,
                    rest_resp = "",
                    log_flag = log_flag
                }).ExecuteCommand();

                //sc.Close();
                return ret;           
            }
            catch { return -1001; }
        }

    }
}
