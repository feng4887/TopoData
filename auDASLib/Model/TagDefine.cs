using SqlSugar;

namespace auDASLib
{
    public class ItemDefine
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public string TagName { get; set; }

        /// <summary>
        /// 用于OPC UA 注册地址
        /// </summary>
        public string RegID { get; set; }
        public string TagAddress { get; set; }
        public string CannelID { get; set; }

        /// <summary>
        /// 0-public,1-singal,2-import
        /// </summary>
        public int Class { get; set; }
        public int RW { get; set; }
        [SugarColumn(IsIgnore = true)]
        public Type type { get; set; }

        public int DataType { get; set; }
        public string UOM { get; set; }
    }

}
