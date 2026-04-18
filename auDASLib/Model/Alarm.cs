using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auDASLib.Model
{
    /// <summary>
    /// 报警日志
    /// </summary>
    [SugarTable("tb_au_alarm")]
    public class Alarm
    {
        [SqlSugar.SugarColumn(ColumnName = "f_id", Length = 100, IsPrimaryKey = true, IsNullable = false)]
        public string f_id { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "tagid", Length = 100, IsNullable = true)]
        public string TagID { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "seq_name", Length = 100, IsNullable = true)]
        public string seq_name { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "uom", Length = 100, IsNullable = true)]
        public string UOM { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "priority", IsNullable = true, ColumnDescription = "优先级")]
        public int? Priority { get; set; }

        [SqlSugar.SugarColumn(ColumnName = "alm_group", Length = 100, IsNullable = true)]
        public string alm_group { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "section1", IsNullable = true)]
        public string Section1 { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "section2", IsNullable = true)]
        public string Section2 { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "area", IsNullable = true)]
        public string Area { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "alarmclass", Length = 100, IsNullable = true, ColumnDescription = "HiHi，Hi，Lo，LoLo，TRUE，FALSE")]
        public string AlarmClass { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "almtype", IsNullable = true, ColumnDescription = "ALM , EVENT")]
        public string AlmType { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "event_time", IsNullable = true, ColumnDescription = "报警产生时间")]
        public DateTime? event_time { get; set; }

        [SqlSugar.SugarColumn(ColumnName = "event_timeutc", IsNullable = true, ColumnDescription = "报警产生UTC时间")]
        public DateTime? event_timeUTC { get; set; }


        /// <summary> /// 1：产生2：离开 3：确认 4：推迟 /// </summary>
        /// 
        [SqlSugar.SugarColumn(ColumnName = "status", IsNullable = true, ColumnDescription = "1：产生2：离开 3：确认 4：推迟")] 
        public int? Status { get; set; }

        [SqlSugar.SugarColumn(ColumnName = "causeid", IsNullable = true)]
        public int? CauseId { get; set; } 
        [SqlSugar.SugarColumn(ColumnName = "limit", IsNullable = true)] 
        public float? Limit { get; set; } 
        [SqlSugar.SugarColumn(ColumnName = "limitstring", Length = 100, IsNullable = true)]
        public String LimitString { get; set; } = ""; 

        [SqlSugar.SugarColumn(ColumnName = "alarmvalue", IsNullable = true)]
        public float? AlarmValue { get; set; } 

        [SqlSugar.SugarColumn(ColumnName = "valuestring", Length = 100, IsNullable = true)]
        public String ValueString { get; set; } = ""; 

        [SqlSugar.SugarColumn( Length = 100, IsNullable = true, ColumnDescription = "报警消息号")] 
        public String msg_nr { get; set; } = ""; 

        [SqlSugar.SugarColumn(Length = 3000, IsNullable = true, ColumnDescription = "报警消息内容")]
        public String alarm_content { get; set; } = ""; 

        [SqlSugar.SugarColumn(ColumnName = "confirmed", IsNullable = true)]
        public bool Confirmed { get; set; } = false; 

        [SqlSugar.SugarColumn(ColumnName = "confirmuser", Length = 100, IsNullable = true)]
        public String ConfirmUser { get; set; } = ""; 

        [SqlSugar.SugarColumn(ColumnName = "confirmdescription", Length = 3000, IsNullable = true)] 
        public String ConfirmDescription { get; set; } = ""; 

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? f_last_modify_time { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "0使用，1删除")]
        public int? state { get; set; }


        [SqlSugar.SugarColumn(ColumnName = "confirmtime", IsNullable = true)]
        public DateTime? ConfirmTime { get; set; }

        [SqlSugar.SugarColumn(ColumnName = "confirmtimeutc",  IsNullable = true)]
        public DateTime? ConfirmTimeUTC { get; set; }

        [SqlSugar.SugarColumn(ColumnName = "returntime",  IsNullable = true)]
        public DateTime? ReturnTime { get; set; }

        [SqlSugar.SugarColumn(ColumnName = "returntimeutc",  IsNullable = true)]
        public DateTime? ReturnTimeUTC { get; set; }



        [SqlSugar.SugarColumn(IsNullable = true)] 
        public String spare1 { get; set; } = ""; 
        [SqlSugar.SugarColumn(IsNullable = true)]
        public String spare2 { get; set; } = ""; 
        [SqlSugar.SugarColumn(IsNullable = true)] 
        public String spare3 { get; set; } = "";
        [SqlSugar.SugarColumn(IsNullable = true, Length = 3000)]
        public String spare4 { get; set; } = "";
    }
}
