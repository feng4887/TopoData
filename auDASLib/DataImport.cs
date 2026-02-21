using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
namespace auDASLib
{

    /// <summary>
    /// 提供一个功能强大的数据导入管理类，用于处理和组织来自不同通道的数据标签。
    /// Key："Cannel.TagName";Value-DBTag。
    /// </summary>
    /// <remarks></remarks>
    public class DataImport
    {
        #region 单例者
        static public DataImport Instance
        {
            get
            {

                if (_Instance == null)
                    return new DataImport();
                else
                    return _Instance;
            }
        }

        static private DataImport _Instance = new DataImport();
        #endregion //单例者

        /// <summary>
        /// 每个小时检查一次
        /// </summary>
        public int CheckWindow { get; set; } = 1000 * 60 * 60;

        public void Start()
        {

        }
        public void Stop()
        {
            try
            {
                _work.Cancel();
            }
            catch
            { }
        }

        CancellationTokenSource _work = new CancellationTokenSource();
        Task _tskAsyQueryFile = null;

        public static Dictionary<string, List<DBTag>> dicCannelTags = new Dictionary<string, List<DBTag>>();
        public static Dictionary<string, DBTag> dicTagPool = new Dictionary<string, DBTag>();
        public static Dictionary<string, DBTag> GetDicTags() 
        {
            Dictionary<string, DBTag> dic = new Dictionary<string, DBTag>();
            foreach (var cannel in dicCannelTags)
            {
                foreach (var tg in cannel.Value)
                {
                    string key = $"{tg.CannelID}.{tg.TagName}";
                    if (!dic.ContainsKey(key))
                        dic.Add(key,tg);
                }
            }
            return dic;
        }

        /// <summary>
        /// Add public master driver Comunication points
        /// </summary>
        /// <param name="tags"></param>
        public static void ImportCannelTags(List<DBTag> tags)
        {
            //<Add public master driver Comunication points>
            if (tags != null && tags.Count > 0) //开始工作
            {

                try 
                { 
                    for (int i = 0; i < tags.Count; i++)
                    {
                        if (dicCannelTags.ContainsKey(tags[i].CannelID))
                        {
                            dicCannelTags[tags[i].CannelID].Add(tags[i]);
                        }
                        else
                        { 
                            List<DBTag> ltb = new List<DBTag>();
                            if (tags[i] != null)
                            { 
                                ltb.Add(tags[i] == null? new DBTag() {} : tags[i]);
                                dicCannelTags.Add(tags[i].CannelID, ltb);                      
                            }
                   
                        }

                    }
                    dicTagPool = GetDicTags();
                }

                catch(Exception ex) {
                    //MessageBox.Show(ex.Message); 
                }

                //// add master PLC system date time tag
                //ItemPool.TagPool.TryAdd($"master.{pubDefine.SysPlcTime}", new ItemDefine()
                //{
                //    Address = $"master.{pubDefine.SysPlcTime}",
                //    TagId = $"master.{pubDefine.SysPlcTime}",
                //    TagName = $"master.{pubDefine.SysPlcTime}",
                //    DataType = 1,
                //    CannelID = $"master"
                //});

            }
        }
    }
}
