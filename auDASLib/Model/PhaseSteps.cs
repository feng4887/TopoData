using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auDASLib.Model
{
    /// <summary>
    /// 历史数据存储-步序定义
    /// </summary>
    [SugarTable("tb_au_seq_operate")]
    public class PhaseSteps
    {
        [SqlSugar.SugarColumn(ColumnDescription = "开始时间", DefaultValue = "CURRENT_TIMESTAMP")]
        public DateTime start_time { get; set; } = DateTime.Now;

        [SqlSugar.SugarColumn(IsNullable = true,ColumnDescription = "结束时间")]
        public DateTime? end_time { get; set; }

        /// <summary>
        /// 步序类型
        /// </summary>
        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public string seq_type { get; set; }


        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public String unit_id { get; set; } = "";

        /// <summary>
        /// 步序名称
        /// </summary>
        [SqlSugar.SugarColumn(Length = 150, IsNullable = true,ColumnName = "seq_name", ColumnDescription = "步序名称")]
        public string StepDescription { get; set; }

        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public string PhaseName { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public string ProductName { get; set; } = "";

        /// <summary>
        /// 工单
        /// </summary>
        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public string wo_no { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public string lot_no { get; set; }

        /// <summary>
        /// 配方
        /// </summary>
        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public string recipe { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public string user { get; set; }
        [SqlSugar.SugarColumn( IsNullable = true,ColumnDescription = "状态(1运行;2暂停;3完成)")]
        public int state { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? f_last_modify_time { get; set; }

        [SqlSugar.SugarColumn(Length = 50, IsNullable = true)]
        public string f_last_modify_user_id { get; set; }

        [SqlSugar.SugarColumn(Length = 200, IsNullable = true)]
        public string spare1 { get; set; }
        [SqlSugar.SugarColumn(Length = 200, IsNullable = true)]
        public string spare2 { get; set; }
        [SqlSugar.SugarColumn(Length = 200, IsNullable = true)]
        public string spare3 { get; set; }
        [SqlSugar.SugarColumn(Length = -1, IsNullable = true)]
        public string spare4 { get; set; }
    }


    /// <summary>
    /// 报警日志
    /// </summary>
    [SugarTable("tb_au_alarmx")]
    public class Alarmx
    {
        [SqlSugar.SugarColumn(ColumnDataType = "varchar(100)", IsPrimaryKey = true)]
        public String f_id { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnDataType = "varchar(100)")]
        public String TagID { get; set; } = "";


        [SqlSugar.SugarColumn(ColumnDataType = "varchar(100)", IsNullable = true)]
        public String seq_name { get; set; } = "";


        [SqlSugar.SugarColumn(ColumnDataType = "varchar(50)", IsNullable = true)]
        public String UOM { get; set; } = "";

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "优先级")]
        public int Priority { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public String Group { get; set; } = "";

        /// <summary>
        /// The highest level in the section hierarchy where alarm (event)items are defined
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "The highest level in the section hierarchy where alarm (event)items are defined")]
        public String Section1 { get; set; } = "";
        [SqlSugar.SugarColumn(IsNullable = true)]
        public String Section2 { get; set; } = "";
        /// <summary>
        /// Alarm(event) alarm type
        ///NORMAL: Normal
        ///ALARM: Alarm
        ///ACKNOW: Acknowledgement
        ///DELAY: Delayed alarm
        ///DIRECT: Direct alarm
        ///FIRSTUP: First-up alarming and so on
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public String Area { get; set; } = "";


        /// <summary>
        /// HiHi，Hi，Lo，LoLo，TRUE，FALSE
        /// </summary>
        [SqlSugar.SugarColumn(ColumnDataType = "varchar(20)", IsNullable = true, ColumnDescription = "HiHi，Hi，Lo，LoLo，TRUE，FALSE")]
        public String AlarmClass { get; set; } = "";

        /// <summary>
        /// ALM , EVENT
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "ALM , EVENT")]
        public String AlmType { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnDescription = "报警产生时间")]
        public DateTime event_time { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? event_timeUTC { get; set; } = null;


        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "报警返回时间")]
        public DateTime? ReturnTime { get; set; } = null;

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? ReturnTimeUTC { get; set; } = null;

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "报警确认时间")]
        public DateTime? ConfirmTime { get; set; } = null;

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? ConfirmTimeUTC { get; set; } = null;

        /// <summary>
        /// 1：产生2：离开 3：确认 4：推迟
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "1：产生2：离开 3：确认 4：推迟")]
        public int Status { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public int CauseId { get; set; }

        public float Limit { get; set; }
        [SqlSugar.SugarColumn(ColumnDataType = "varchar(100)", IsNullable = true)]
        public String LimitString { get; set; } = "";

        [SqlSugar.SugarColumn(IsNullable = true)]
        public float AlarmValue { get; set; }

        [SqlSugar.SugarColumn(ColumnDataType = "varchar(100)", IsNullable = true)]
        public String ValueString { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnDataType = "varchar(100)", IsNullable = true, ColumnDescription = "报警消息号")]
        public String msg_nr { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnDataType = "varchar(2048)", IsNullable = true, ColumnDescription = "报警消息内容")]
        public String alarm_content { get; set; } = "";

        [SqlSugar.SugarColumn(IsNullable = true)]
        public bool Confirmed { get; set; } = false;

        [SqlSugar.SugarColumn(ColumnDataType = "varchar(50)", IsNullable = true)]
        public String ConfirmUser { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnDataType = "varchar(2048)", IsNullable = true)]
        public String ConfirmDescription { get; set; } = "";


        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime f_last_modify_time { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "0使用，1删除")]
        public int state { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public String spare1 { get; set; } = "";

        [SqlSugar.SugarColumn(IsNullable = true)]
        public String spare2 { get; set; } = "";

        [SqlSugar.SugarColumn(IsNullable = true)]
        public String spare3 { get; set; } = "";

    }
}
