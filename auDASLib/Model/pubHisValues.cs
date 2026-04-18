using Microsoft.Data.SqlClient;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auDASLib.Model
{
    /// <summary>
    /// 公用工艺数据,历史值
    /// </summary>
    [SugarTable("tb_au_pubhisvalues")]
    [SugarIndex("index_datetime", nameof(pubHisValues.datetime), OrderByType.Asc)]
    public class pubHisValues
    {
        [SqlSugar.SugarColumn(Length = 100, IsPrimaryKey = true, IsNullable = false)]
        public String f_id { get; set; } = "";

        [SqlSugar.SugarColumn( IsNullable = true)]
        public DateTime? datetime { get; set; } = System.DateTime.Now;

        /// <summary>
        /// 标签地址
        /// </summary>
        [SqlSugar.SugarColumn(ColumnName = "tagid", Length = 150, IsNullable = true)]
        public String TagID { get; set; } = "";

        /// <summary>
        /// 标签名称 & ColumnDescription
        /// </summary>

        [SqlSugar.SugarColumn(ColumnName = "tagn", Length = 150, IsNullable = true)]
        public String TagN { get; set; } = "";

        [SqlSugar.SugarColumn(ColumnName = "fvalue", IsNullable = true)]
        public float? fValue { get; set; } = 0;

        [SqlSugar.SugarColumn(ColumnName = "bvalue", IsNullable = true)]
        public bool? bValue { get; set; } = false;

        [SqlSugar.SugarColumn(ColumnName = "strvalue", Length =3000, IsNullable = true)]
        public String strValue { get; set; } = "";
        [SqlSugar.SugarColumn(Length = 50, IsNullable = true)]
        public String uom { get; set; } = "";

        [SqlSugar.SugarColumn(Length = 150, IsNullable = true)]
        public String wo_no { get; set; } = "";        
        [SqlSugar.SugarColumn(Length = 100, IsNullable = true)]
        public String unit_id { get; set; } = "";

        /// <summary>
        /// 步序名称
        /// </summary>
        [SqlSugar.SugarColumn(Length = 150, IsNullable = true, ColumnDescription = "步序名称")]
        public String seq_name { get; set; } = "";

    }
}
