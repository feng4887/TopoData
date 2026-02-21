using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace auDASLib
{
    /// <summary>
    /// 数据采样逻辑
    /// </summary>
    public class DataSample
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; } = "";

        /// <summary>
        /// 数据库表名称, Step步序名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 报表名称 或 步序描述
        /// </summary>
        public string ReportName { get; set; } = "";

        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; } = "";

        /// <summary>
        /// 设备名
        /// </summary>
        public string EquipmentName { get; set; } = "";
        /// <summary>
        /// 工单
        /// </summary>
        public string wo { get; set; }

        /// <summary>
        /// 阶段，Step上一级名称
        /// </summary>
        public string phase { get; set; }

        /// <summary>
        /// 配方
        /// </summary>
        public string recipe { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string lot { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 数据采样点集合
        /// </summary>
        public List<DSTbDefine> Samples    = new List<DSTbDefine>();
        public DataSampleTrig   SampleTrig = new DataSampleTrig();

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool bActive { get; set; } = true;

        /// <summary>
        /// 是否存储到时序表
        /// </summary>
        public bool bSave2ProcessTable { get; set; } = false;
    }

    public class DataSampleTrig
    {
        /// <summary>
        /// 采集类型 Type = 0 - DataSampleTrig_OnTrue；
        /// 采集类型 Type = 1 - DataSampleTrig_TimeWindow；
        /// 采集类型 Type = 2 - DataSampleTrig_OnTrue and OnFalse；
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 脚本或表达式
        /// </summary>
        public string ExpressionText { get; set; }

        /// <summary>
        /// 脚本内部变量
        /// </summary>
        public List<DBTag> ExpressionTags = new List<DBTag>();

        /// <summary>
        /// 使用表达式
        /// </summary>
        public bool bUseExpression { get; set; } = false;

        public DataSampleTrig_OnTrue dataSampleTrig_OnTrue = new DataSampleTrig_OnTrue();
        public DataSampleTrig_TimeWindow dataSampleTrig_TimeWindow = new DataSampleTrig_TimeWindow();
        public DataSampleTrig_OnTrueOnFalse dataSampleTrig_OnTrueOnFalse = new DataSampleTrig_OnTrueOnFalse();
    }

    /// <summary>
    /// On true 采集逻辑
    /// </summary>
    public class DataSampleTrig_OnTrue
    {
        public string TagName
        {
            get; set;
        }
        public string CannelID
        {
            get; set;
        }

        public string DataType
        {
            get; set;
        }

    }

    /// <summary>
    /// OnTrue OnFalse 脉冲采集
    /// </summary>
    public class DataSampleTrig_OnTrueOnFalse
    {
        public string TagName
        {
            get; set;
        }
        public string CannelID
        {
            get; set;
        }

        public string DataType
        {
            get; set;
        }

    }

    /// <summary>
    /// 按时间窗采集逻辑
    /// </summary>
    public class DataSampleTrig_TimeWindow
    {
        /// <summary>
        /// 采样时间间隔 ms
        /// </summary>
        public int TimeWindow { get; set; } = 1000;

        public string TagName
        {
            get; set;
        }

        public string CannelID
        {
            get; set;
        }

        public string DataType
        {
            get; set;
        }

    }

    /// <summary>
    /// 用于本地数据保存 表格逻辑
    /// 自动生成一个数据表的一个列
    /// </summary>
    public class DSTbDefine
    {

        public string Column { get; set; }             // 列名
        public string DataType { get; set; }           // 数据类型
        public int? MaxLength { get; set; } = null;    // 最大长度（可选，适用于字符串类型）
        public bool IsNullable { get; set; } = true;   // 是否允许为空
        public bool IsIndexed { get; set; } = false;   // 是否需要索引

        public string TagID
        {
            get; set;
        }
        public string TagName
        {
            get; set;
        }
        public string CannelID
        {
            get; set;
        }

        public string UOM
        {
            get; set;
        }
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是一个内存点
        /// </summary>
        public bool isInternalValue { get; set; } = false;
    }



}
