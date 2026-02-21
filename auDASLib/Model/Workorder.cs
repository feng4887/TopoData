using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auDASLib.Model
{
    /// <summary>
    /// 工单类
    /// </summary>
    [SugarTable("tb_au_workorder")]
    public class pWO
    {
        /// <summary>
        /// 工单号
        /// </summary>

        [SugarColumn(Length = 100,IsPrimaryKey = true, IsNullable = false)] // 默认类型
        public string wo_no { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型

        public string lot_no { get; set; }


        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string sublot_no { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string recipe_id { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string item { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public float? item_qty { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string uom { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "模式(0自动1手动)")]

        public int? mode { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "根据项目特点自定义")]
        public int? processtype { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "1.设备编码2.适用于一个工单一个设备情况，可以记录设备编码")] // 默认类型
        public string unit_id { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnDescription = "表名")] // 默认类型
        public string table_name { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDescription = "批次状态(0新建;1运行;2暂停;3完成;4废弃;)")]
        public int? state { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? start_time { get; set; }


        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? end_time { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public DateTime? f_last_modify_time { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string f_last_modify_user_id { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string spare1 { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string spare2 { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)] // 默认类型
        public string spare3 { get; set; }
        [SqlSugar.SugarColumn(Length = -1, IsNullable = true)]
        public string spare4 { get; set; }
    }

}
