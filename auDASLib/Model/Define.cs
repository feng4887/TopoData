using Newtonsoft.Json.Linq;
using SqlSugar;
using System.Drawing;
using System.IO.Ports;
using System.Xml.Serialization;
/****************************************************************
 * 标题: 结构体定义
 * 描述: 
 * 作者: 杜金旺 
 * 日期:2024-7-4
 * *************************************************************/
namespace auDASLib
{
    /// <summary>
    /// 公有的定义
    /// </summary>
    public class pubDefine
    {
        public static string SysPlcTime = "@@PlcTime";
        //public static string ReportPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Report\";
        //public static string Folder = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Cfg";
        //public static string Configuration = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Cfg\auDAServerCfg.xml";
        //public static string TagDefine = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Cfg\auDAServerTagDef.xml";
        //public static string Austart = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Cfg\Austart.xml";
        public static string ReportPath
        {
            get
            {
                if (OSChecker.IsWindows())
                {
                    return @"C:\Users\Public\Documents\" + @"Report\"; ;
                }
                else { return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Report/"; }
            }
        }
        public static string Folder
        {
            get
            {
                if (OSChecker.IsWindows())
                {
                    return @"C:\Users\Public\Documents\" + @"Config\"; ;
                }
                else { return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Config//"; }
            }
        }

        public static string Configuration
        {
            get
            {
                if (OSChecker.IsWindows())
                {
                    return @"C:\Users\Public\Documents\" + @"Config\hiTopoCfg.xml"; ;
                }
                else
                {
                    //Console.WriteLine("------------------------ ", Color.Yellow);
                    //Console.WriteLine("1" + System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Cfg/auDAServerCfg.xml");
                    //Console.WriteLine("1" + System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Cfg/" + " auDAServerCfg.xml");
                    //string path = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Cfg/", "auDAServerCfg.xml");

                    return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config/hiTopoCfg.xml"; }
            }
        }
        public static string TagDefine
        {
            get
            {
                if (OSChecker.IsWindows())
                {
                    return @"C:\Users\Public\Documents\" + @"Config\hiTopoTagDef.xml"; ;
                }
                else
                {
                    //Console.WriteLine("------------------------ " , Color.Yellow);
                    //Console.WriteLine("1"+System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Cfg/auDAServerTagDef.xml");
                    //Console.WriteLine("1" + System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Cfg/" +" auDAServerTagDef.xml");
                    //string path = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Cfg/", "auDAServerTagDef.xml");
                    //Console.WriteLine("1" + path);
                    return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config/hiTopoTagDef.xml"; 
                }
            }
        }
        public static string Austart
        {
            get
            {
                if (OSChecker.IsWindows())
                {
                    return @"C:\Users\Public\Documents\" + @"Config\Austart.xml"; ;
                }
                else { return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config/Austart.xml"; }
            }
        }

        public static string LogFilePath
        {
            get
            {
                if (OSChecker.IsWindows())
                {
                    return @"C:\Users\Public\Documents\" + @"Config\LogSoftwareReg.xml"; ;
                }
                else { return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config/LogSoftwareReg.sql"; }
            }
        }
    }

    /// <summary>
    /// 系统状态
    /// </summary>
    public class tmStatus
    {
        /// <summary>
        /// 写操作锁，写的时候不允许读; true-正在写操作，不能读；false-读状态中
        /// </summary>
        public static bool WriteLock = false;

    }

    /// <summary>
    /// 用于Redis 写操作队列
    /// </summary>
    public class WriteValueQueue
    {
        public List<WriteValue> WriteValues = new List<WriteValue>();
        public DateTime Datetime = DateTime.Now;
        public string Cannel = string.Empty;
        public WriteValueQueue()
        {
        }
        public WriteValueQueue(List<WriteValue> wvs, string cannel = "")
        {
            Cannel = cannel;
            WriteValues = wvs;
            Datetime = DateTime.Now;
        }
    }

    /// <summary>
    /// 用于Redis 写操作队列
    /// </summary>
    public class WriteValue
    {
        public string CannelID { get; set; }
        public string ValueName { get; set; }
        public string Address { get; set; }
        public dynamic value { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        /// <summary>
        /// 0-bool;10-float;6-int32;5-word;1-string;2-WSTRING
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 重写次数
        /// </summary>
        public int RewriteCount { get; set; } = 0;
    }

    public class TagNames
    {
        public TagNames()
        {
            TagList = new List<string>();
        }
        public List<string> TagList { get; set; }
    }

    public class WriteResult
    {
        public int count { get; set; }
        public int lastError { get; set; }
        public string ErrorMessage { get; set; }
    }

    // [Serializable]
    public class RealTimeValue
    {
        public string Cannel { get; set; }
        /// <summary>
        /// ValueID
        /// </summary>
        public string ValueName { get; set; }
        public DateTime Timestamp { get; set; }
        public dynamic RealValue { get; set; }
        public int Quality { get; set; }
        public string uom { get; set; }

        public RealTimeValue()
        {
        }

        public RealTimeValue(string valuename, decimal value, int quality, string canne, string uoml= "")
        {
            this.ValueName = valuename;
            this.RealValue = value;
            this.uom = uoml;
            this.Quality = quality;
            this.Timestamp = System.DateTime.Now;
        }

        public void Set(string name, DateTime time, dynamic value, int quality,string uom)
        {
            ValueName = name;
            Timestamp = time;
            RealValue = value;
            Quality = quality;
            this.uom = uom;
        }
    }

    /// <summary>
    /// Server configuration
    /// </summary>
    public class ServerCfg
    {

        private static ServerCfg instance = null;
        static ServerCfg()
        {
            if (instance == null)
                instance = new ServerCfg();
        }
        public static ServerCfg Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public int tagStepCount = 1;

        /// <summary>
        /// 多语言,默认英文
        /// </summary>
        public string Localization = "en";

        /// <summary>
        /// 自动启动服务
        /// </summary>
        public bool AutoStart = false;

        /// <summary>
        /// 测试模式
        /// </summary>
        public bool TestMode = false;

        /// <summary>
        /// 托盘启动
        /// </summary>
        public bool TratStart = false;

        /// <summary>
        /// Sql server
        /// </summary>
        public SQLCfg SqlConfigInfo = new SQLCfg();

        /// <summary>
        /// Restful Server URL
        /// </summary>
        public string RestUri1 = "http://0.0.0.0:8883/";
        public string RestUri2 = "https://0.0.0.0:8884/";

        /// <summary>
        /// Disable Restful Server
        /// </summary>
        public bool RestEnable = true;

        /// <summary>
        /// 读数据保存成CSV log
        /// </summary>
        public bool SaveCsvLog_R = false;

        /// <summary>
        /// 写数据保存成CSV log
        /// </summary>
        public bool SaveCsvLog_W = false;

        /// <summary>
        /// 保存csv日志位置
        /// </summary>
        public string SaveCsvLogPath = @"C:\";

        /// <summary>
        /// 保存Log4日志
        /// </summary>
        public bool EnableLog4 = false;

        /// <summary>
        /// 用于Redis Writ eQue Key
        /// </summary>
        public string WriteQueKey = "WriteQueKey";

        public List<ConnctDevice> cDevices = new List<ConnctDevice>();

        public string RestfuServiceName = "AusDataSampleServer";

        public string ShareFolder = @"C:\";

        /// <summary>
        /// 备注
        /// </summary>
        public string Description = "";

        /// <summary>
        /// 数据采样逻辑
        /// </summary>
        public List<DataSample> dataSamples = new List<DataSample>();

        /// <summary>
        /// 步序采样逻辑
        /// </summary>
        public List<DataSample> stepSamples = new List<DataSample>();
    }

    public enum DeviceType
    {
        WinCC = 2,
        OPCUA = 1,
        OPCDA = 3,
        ProfinetS7 = 4,
        ModbusRTU = 5,
        ModbusASCCII = 6,
        ModbusTCP = 7
    }

    /// <summary>
    /// 读写类型 WR 读写，ReadOnly 只读，WriteOnly 只写
    /// </summary>
    public enum RwType
    {
        RW = 0,
        ReadOnly = 1,
        WriteOnly = 2
    }

    public enum UnitStatus
    {
        READY = 0,
        RUNNING = 10,
        DONE = 11,
        HELD = 21,
        ABORTED = 51,
        INTERLOCK = 81
    }

    public class ConnctDevice
    {
        /// <summary>
        /// 通信类型
        /// </summary>
        public DeviceType devType = DeviceType.OPCUA;

        public UACfg UAParamter = new UACfg();

        public IpCfg IpCfg = new IpCfg();

        public ComCfg ComCfg = new ComCfg();

        public string CannelID = "0";

        /// <summary>
        /// 地址或连接字符串
        /// </summary>
        public string Address = "";

        /// <summary>
        /// sample interval / ms
        /// </summary>
        public int SampleInterval = 600;

        /// <summary>
        /// sample quantity
        /// </summary>
        public int SampleQty = 6000;

        /// <summary>
        /// 只写不读
        /// </summary>
        public bool WriteOnly = false;

        /// <summary>
        /// 设备状态 1- 开机启动 ;0-disable;2;pending
        /// </summary>
        public int iActive = 1;

        /// <summary>
        /// 通讯/设备的备注
        /// </summary>
        public string Description = "";
    }

    public class UACfg
    {
        public string EndpointUrl { get; set; } = "opc.tcp://127.0.0.1:49320/";
        public string SecurityMode { get; set; }   = ""; //None
        public string securityPolicy { get; set; } = "";//#None
        public string User { get; set; } = "";
        public string Psw { get; set; } = "";
        public bool UseUserLogIn { get; set; } = false;

        /// <summary>
        /// Get from OPC UA server, 不是一个必须的参数
        /// </summary>
        public string ApplicationName { get; set; } = "";
        /// <summary>
        /// true-订阅模式（8000点以下）；false-注册tag模式（大点数，但某些PLC不支持）
        /// </summary>
        public bool Subscribe { get; set; } = true;
    }

    /// <summary>
    /// IP type device
    /// </summary>
    public class IpCfg
    {

        public string Host = "192.168.0.1";
        public int Port = 43;
        /// <summary>
        /// S7 use
        /// </summary>
        public int Rack = 0;
        /// <summary>
        /// S7 use, 300-2;1500、1200-1,1；400 see actule
        /// </summary>
        public int Slot = 2;

    }

    /// <summary>
    /// serial port communication Device
    /// </summary>
    /// <summary>
    /// 串口通信配置类
    /// </summary>
    public class ComCfg
    {
        // 必填属性
        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;

        public Parity Parity { get; set; } = Parity.None;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Handshake Handshake { get; set; } = Handshake.None;

        // 超时设置（重要！避免程序卡死）
        public int ReadTimeout { get; set; } = 500;
        public int WriteTimeout { get; set; } = 500;

        // 缓冲区大小
        public int ReadBufferSize { get; set; } = 4096;
        public int WriteBufferSize { get; set; } = 2048;

        // 其他实用属性
        public string Description { get; set; }
        public bool DtrEnable { get; set; }
        public bool RtsEnable { get; set; }

        /// <summary>
        /// 将配置应用到SerialPort对象
        /// </summary>
        public void ApplyToPort(SerialPort port)
        {
            if (port == null)
                throw new ArgumentNullException(nameof(port));

            port.PortName = PortName;
            port.BaudRate = BaudRate;
            port.Parity = Parity;
            port.DataBits = DataBits;
            port.StopBits = StopBits;
            port.Handshake = Handshake;
            port.ReadTimeout = ReadTimeout;
            port.WriteTimeout = WriteTimeout;
            port.ReadBufferSize = ReadBufferSize;
            port.WriteBufferSize = WriteBufferSize;
            port.DtrEnable = DtrEnable;
            port.RtsEnable = RtsEnable;
        }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public bool Validate(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(PortName))
            {
                errorMessage = "串口名称不能为空";
                return false;
            }

            if (BaudRate <= 0)
            {
                errorMessage = "波特率必须大于0";
                return false;
            }

            if (DataBits < 5 || DataBits > 8)
            {
                errorMessage = "数据位必须是5-8之间的值";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// 克隆配置
        /// </summary>
        public ComCfg Clone()
        {
            return (ComCfg)this.MemberwiseClone();
        }
    }

    public class SQLCfg
    {
        public string DataSource = @"localhost\WinCC";
        public string Table = "aaTagList";
        public string DbType = "Microsoft SQL Server";
        public int Port = 1433;
        public string ServerName = "Localhost";
        public string DbName = "master";
        public string TagDB = @"sa";
        public string User = "sa";
        public string Psw = "sa";
        public int scanWindow = 500; //sm
        /// <summary>
        /// 历史数据存储时长
        /// </summary>
        public int StoreMonth = 12;

    }

    /// <summary>
    /// 数据库点表配置
    /// </summary>
    [XmlRoot("TagLists")]
    public class DBTag
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = false, ColumnDataType = "nvarchar(100)")]
        public string TagId { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(100)")]
        public string TagName { get; set; }

        [SqlSugar.SugarColumn(IsIgnore =true)]// 不会映射到数据库
        public string RegID { get; set; }

        [SqlSugar.SugarColumn(IsNullable = false, ColumnDataType = "nvarchar(100)")]
        public string Address { get; set; }

        [SqlSugar.SugarColumn(IsNullable = false)]
        public string CannelID { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(20)")]
        public string UOM { get; set; }
        /// <summary>
        /// wr：0= 读写，1=只读，2=只写
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public int RW { get; set; }

        [SqlSugar.SugarColumn(IsNullable = true)]
        public int DataType { get; set; }

        /// <summary>
        /// 0-传统点（batch public，unit public, phase public, point ）, 1-内部点, 2-外部导入点
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public int Class { get; set; } = 0;

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDataType = "varchar(max)")]
        public string Description { get; set; }
        [SqlSugar.SugarColumn(IsNullable = true, ColumnDataType = "varchar(max)")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是一个内存点
        /// </summary>

        [SqlSugar.SugarColumn(IsNullable = true, ColumnDataType = "bit")]
        public bool isInternalValue { get; set; } = false;

        [SqlSugar.SugarColumn(IsIgnore = true)] // 不会映射到数据库
        public int ListviewIndex { get; set; }

        public static int GetDataType(string type)
        {
            if(string.IsNullOrEmpty(type))
                return -1;

            string ty = type.ToLower();
            switch (ty)
            {
                case ("bool"):
                case ("boolean"):
                    return 0;
                case ("bit"):
                    return 0;
                case "string":
                    return 1;
                case "wstring":
                    return 2;
                case "byte":
                    return 3;
                case "int16":
                    return 4;
                case "uint16":
                case "word":
                    return 5;
                case "int":
                case "int32":
                    return 6;
                case "float":
                    return 10;
                case "double":
                    return 11;
                default:
                    return -1;
            }
        }
        public static string DataTypeString(int type)
        {
            switch (type)
            {
                case (0):
                    return "bool";
                case 1:
                    return "string";
                case 2:
                    return "wstring";
                case 3:
                    return "byte";
                case 4:
                    return "int16";
                case 5:
                    return "word";
                case 6:
                    return "int";
                case 10:
                    return "float";
                case 11:
                    return "double";
                default:
                    return "";
            }
        }
    }

    public class PaeResult
    {
        public bool IsSucess = false;
        public string ErrorMessage = "";
    }
}
