// Common English interface labels. Configuration values and user-authored content are never translated.
using System.Globalization;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.LogicalTree;
using FluentAvalonia.UI.Controls;

namespace TopoView.Views;

public static class UiText
{
    public static bool IsEnglish { get; set; }
    public static string Translate(string value) => IsEnglish && English.TryGetValue(value, out var text) ? text : value;

    private static readonly ConditionalWeakTable<object, Dictionary<string, string>> Captions = new();
    private static readonly ConditionalWeakTable<Control, object> InitializedRoots = new();

    private static string Caption(object owner, string property, string value)
    {
        var captions = Captions.GetOrCreateValue(owner);
        if (!captions.TryGetValue(property, out var original)) captions.Add(property, original = value);
        return Translate(original);
    }

    /// <summary>Reapplies only captured AXAML captions, preserving data and runtime column names.</summary>
    public static void Apply(Control root)
    {
        var existingRoot = InitializedRoots.TryGetValue(root, out _);
        foreach (var control in CaptionTargets(root))
        {
            if (string.IsNullOrEmpty(control.Name) ||
                (existingRoot && !Captions.TryGetValue(control, out _))) continue;
            Captions.GetOrCreateValue(control);
            if (control is TextBlock label && label.Text is { } text)
                label.Text = Caption(control, "Text", text);
            if (control is ContentControl content && content.Content is string caption)
                content.Content = Caption(control, "Content", caption);
            if (control is HeaderedContentControl header && header.Header is string title)
                header.Header = Caption(control, "Header", title);
            if (control is TextBox editor && editor.Watermark is { } hint)
                editor.Watermark = Caption(control, "Watermark", hint);
            if (control is DataGrid grid)
                foreach (var column in grid.Columns)
                    if (column.Header is string name &&
                        (!existingRoot || Captions.TryGetValue(column, out _)))
                        column.Header = Caption(column, "Header", name);
            if (control is ContentDialog dialog)
            {
                if (dialog.Title is string dialogTitle) dialog.Title = Caption(dialog, "Title", dialogTitle);
                dialog.PrimaryButtonText = Caption(dialog, "Primary", dialog.PrimaryButtonText ?? "");
                dialog.SecondaryButtonText = Caption(dialog, "Secondary", dialog.SecondaryButtonText ?? "");
                dialog.CloseButtonText = Caption(dialog, "Close", dialog.CloseButtonText ?? "");
            }
        }
        InitializedRoots.GetOrCreateValue(root);
    }

    /// <summary>Navigation items are hosted by the pane's repeater, so they are not logical children.</summary>
    private static IEnumerable<Control> CaptionTargets(Control root)
    {
        var controls = root.GetLogicalDescendants().OfType<Control>().Prepend(root).ToList();
        foreach (var nav in controls.OfType<NavigationView>().ToList())
            foreach (var item in nav.MenuItems.Concat(nav.FooterMenuItems).OfType<Control>())
                controls.AddRange(item.GetLogicalDescendants().OfType<Control>().Prepend(item));
        return controls;
    }
    private static readonly Dictionary<string, string> English = new()
    {
        ["关于"] = "About",
        ["报警文件名前缀"] = "Alarm File Prefix",
        ["Audit文件名前缀"] = "Audit File Prefix",
        ["批次号"] = "batch number",
        ["确认退出应用程序吗？"] = "Do you want Exit this Application?",
        ["是否退出？选 [否] ，最小化到托盘"] = "Do you want Exit this Application? Select [No] will change to Tral Mode.",
        ["基本配置"] = "Basic Configuration",
        ["通讯配置"] = "Comunication",
        ["配置"] = "Configuration",
        ["数据库存储位置"] = "DB Storage location",
        ["数据库保留时间"] = "DB Store Duration",
        ["数据库被创建，并且连接正常。"] = "Database created and connection good.",
        ["通讯诊断"] = "Comunication Diagnosis",
        ["硬盘剩余百分比"] = "Drive Free Percentage",
        ["硬盘剩余空间"] = "Drive Free Space",
        ["硬盘空间"] = "Drive Space",
        ["本地Log日志"] = "Enable Log4",
        ["导出"] = "Export",
        ["加载文件"] = "File Upload",
        ["报警"] = "Alarm",
        ["审计追踪"] = "AuditTrail",
        ["工单查询"] = "WorkorderSelect",
        ["数采通道"] = "CannelCfg",
        ["参数存储配置"] = "Parameter storage Cfg",
        ["隐藏"] = "Hide",
        ["导入"] = "Import",
        ["语言"] = "Language",
        ["数据采集系统"] = "Data Sample system",
        ["月"] = "Month",
        ["否"] = "No",
        ["没查到数据，不能导出报表。"] = "No data, and cannot export report.",
        ["序号"] = "Num",
        ["加载报警文件"] = "LoadAlarmFile",
        ["退出"] = "Quit",
        ["查询"] = "Search",
        ["服务已经启动... ..."] = "Service Started... ...",
        ["共享文件夹"] = "Share Folder",
        ["软件自动启动"] = "Application Automatic Start",
        ["软件托盘启动"] = "Software Tray Start",
        ["点数"] = "Tag Count",
        ["变量名"] = "TagName",
        ["测试"] = "Test",
        ["测试模式(采集周期1秒)"] = "Test Mode(Sample 1000ms)",
        ["时间"] = "Time",
        ["数值"] = "Value",
        ["工单号"] = "WO NO.",
        ["打开"] = "Open",
        ["启动"] = "Start",
        ["诊断"] = "Diagnose",
        ["步序存储配置"] = "Step storage Cfg",
        ["表达式变量"] = "ExpressTagCfg",
        ["通道名"] = "Cannel Name",
        ["通道类型"] = "Cannel Type",
        ["驱动类型"] = "Driver Type",
        ["通道配置"] = "Cannel Configuration",
        ["启用"] = "Enable",
        ["保存"] = "Save",
        ["清空"] = "Clear",
        ["停止时间"] = "End Time",
        ["按工单"] = "by WO",
        ["按时间"] = "by Time",
        ["任务"] = "Task",
        ["创建DB"] = "Create DB",
        ["测试DB"] = "Test DB",
        ["数据库配置"] = "DB Configuration",
        ["数据存储时间"] = "DB Store Time",
        ["用户"] = "User",
        ["数据库位置"] = "DB Location",
        ["数据库文件"] = "DB File",
        ["设备名"] = "Equipment",
        ["同步表结构"] = "Synchronize Table",
        ["添加"] = "Add",
        ["更新"] = "Update",
        ["删除"] = "Delete",
        ["向上"] = "Up",
        ["向下"] = "Down",
        ["时序存储表"] = "Time Scale Table",
        ["数据库表"] = "DB Table",
        ["表抬头/步序"] = "Title/Step",
        ["触发条件"] = "triggering condition",
        ["表达式"] = "Expression",
        ["通讯点"] = "Tag",
        ["Bool触发点"] = "Bool Trig Tag",
        ["连续采集"] = "Consecutive ",
        ["任务列表"] = "Task list",
        ["步序类型"] = "Step type",
        ["步序名称"] = "Step name",
        ["配方"] = "Recipe",
        ["产品"] = "Product",
        ["阶段"] = "Phase",
        ["获取Endpoint"] = "GetEndpoint",
        ["使用账号密码登录"] = "Use user logIn",
        ["PLC配置注意事项："] = "PLC Configuration Notes:",
        ["任何配置修改，需要重启软件"] = "Any changes require a restart.",
        ["S7-400 : 看配置"] = "S7-400 : Check HW configuration",
        ["地址"] = "Address",
        ["中间变量"] = "Internal tag",
        ["中间变量列表"] = "Internal tag list",
        ["表达式编辑"] = "Expression Edit",
        ["表达式检查"] = "Expression Check",
        ["检查"] = "Check",
        ["设备点"] = "Device tag",
        ["是否删除数据库列及相关数据？"] = "Do you want to delete the database column and the associated data?",
        ["删除失败。"] = "Delete action failed.",
        ["删除成功, 数据库删除列的数据也被删除。"] = "The deletion was successful. The data in the deleted column of the database was also deleted.",
        ["数据库表名或报表抬头不能为空。"] = "The name of the database table or the header of the report cannot be left blank.",
        ["创建数据库表失败, 数据库连接正常情况下，请重新保存创建数据采集表。"] = "The creation of the database table failed. ",
        ["请先选择一行！"] = "Please a line !",
        ["IP 地址解析错误。"] = "IP address error.",
        ["保持失败"] = "Save Error",
        ["Rack or Slot 解析错误"] = "Rack or Slot Error",
        ["请选择Endpoint。"] = "Please an Endpoint。",
        ["数据导出到excel失败："] = "Export data to Excel failed:",
        ["共导出 "] = "Export total ",
        [" 条数据。"] = " items.",
        ["导入变量点 "] = "Import total ",
        ["Excel 导入失败："] = "Import Excel fail:",
        ["系统诊断"] = "Diagnose",
        ["工单名称不能为空，查询失败。"] = "The name of the work order cannot be empty. The query failed.",
        ["请选择一个存储任务。"] = "Please Select a store task.",
        ["查询任务没有字段，查询失败。"] = "Query task has no field and the query failed.",
        ["报表导出失败。"] = "Report exported  Fail.",
        ["报表导出成功。"] = "Report exported successfully.",
        ["数据库表不存在，或者查询数据库没有字段。查询失败。"] = "The query failed.The database table does not exist, or there are no fields in the database being queried. ",
        ["查询任务字段和实际数据库字段不符。查询失败。"] = "The query failed.The query task fields do not match the actual database fields.",
        ["写入"] = "Write down",
        ["写操作"] = "Write  operation",
        ["选择"] = "Select",
        ["通讯测试"] = "Comunication Test",
        ["数据库通讯正常。"] = "DB comunication is good.",
        ["数据库通讯失败。"] = "DB comunication failed.",
        ["创建表失败。"] = "Create table fail.",
        ["创建数据库成功。"] = "Create database sucessfully.",
        ["创建数据库已经存在,同步数据库表成功。"] = "The database has been created and the synchronization of database tables has been successful.",
        ["请指定数据库存储路径，创建数据库失败。"] = "Please specify database storage path. Database creation failed.",
        ["数据库表名或任务名不能为空."] = "The name of the database table or the task cannot be empty.",
        ["通讯点名不能为空。"] = "Tag name cannot empty.",
        ["选择的通讯点不存在, 请检查通讯配置。"] = "Selected Tagdoes not exist. Please check  settings.",
        ["设定采样周期不能小于 400 ms"] = "Sampling period must not be less than 400 ms.",
        ["此属性不能重复添加"] = "This attribute cannot be added repeatedly.",
        ["表达式例子：(a = 100 OR b > 0) AND c <> 2"] = "Expression example: (a = 100 OR b > 0) AND c <> 2",
        ["a,b,c为参数名"] = "a,b,c  are parameter name",
        ["TopoData 集团"] = "TopoData Group",
        [" 服务连接异常 "] = " Service connection error ",
        ["通讯正常"] = "Comunication is good.",
        ["小时"] = "hour",
        ["试用授权，试用时间"] = "Trial authorization, trial period",
        ["试用"] = "Trial period is over for",
        ["小时结束，请重启 "] = "hours. Please restart.",
        ["总共："] = "Total:",
        ["是否删除所有数据？"] = "Do you want to delete all?",
        ["没有选择OPC UA安全策略。 请选择Endpoint."] = "OPC UA security configuration is null.  Try to select Endpoint.",
        ["下列 TagName 已存在："] = "Following TagName exists:",
        ["将添加其余 "] = "Will add other ",
        ["个不重复项，是否继续？"] = " not duplicated tags, do you want continue?",
        ["服务节点"] = "Server Node",
        ["属性"] = "Attribute",
        ["获取Tag"] = "Brows Tag",
        ["服务器地址"] = "Host Address",
        ["表名"] = "Table Name",
        ["是否删除选中数据？"] = "Do you want to delete selected items?",
        ["导出Excel"] = "Export Excel",
        ["导出趋势图"] = "ExportChart",
        ["趋势图导出成功。"] = "Chart exported successfully.",
        ["趋势图导出失败。"] = "Chart exported failed.",
        ["表格"] = "DataTable",
        ["趋势图"] = "Trend Chart",
        ["表字段"] = "Table Colum",
        ["删除数据"] = "Delete Data",
        ["创建数据库失败。"] = "Database creation failed.",
        ["正在查询数据，请稍候…"] = "Data is being retrieved. Please wait a moment...",
        ["时序存储表不能空。"] = "Time Scale Table cannot empty.",
        ["当前值"] = "current value",
        ["列名不能特殊字符或数字开头"] = "Column name Error: Cannot start with Specia lChar Or DigitError",
        ["Tag 数据类型应该是字符串类型。"] = "Tag Type should be string.",
        ["数据查询"] = "History Query",
        ["备注"] = "Description",
        ["设备列表"] = "Equipments",
        ["禁用"] = "Off",
        ["批次通讯点"] = "Batch Tag",
        ["WebAPI 配置"] = "WebAPI Configuration",
        ["点表"] = "Tag List",
        ["获取当前节点"] = "Get This Item",
        ["获取所有下一级子节点"] = "Get all subItems",
        ["服务"] = "Host",
        ["启用安全"] = "Enable Security",
        ["禁用安全"] = "Disable Security",
        ["组发布订阅"] = "Group Mqtt Topic",
        ["发布时间(sm)"] = "Publish Time（sm）",
        ["MQTT 配置"] = "MQTT Configuration",
        ["单点发布"] = "Singal Tag Publish",
        ["组发布"] = "Group Tags Publish",
        ["脚本配置"] = "Script Configuration",
        ["基本信息"] = "Basic Information",
        ["脚本名"] = "Script Name",
        ["脚本触发条件配置"] = "Script trig configuration",
        ["仿真模式"] = "Simulator Mode",
        ["设备管理"] = "Devices", ["数据存储"] = "Data storage", ["配方管理"] = "Recipes", ["Python 脚本"] = "Python scripts",
        ["参数诊断"] = "Diagnostics", ["历史查询"] = "History", ["系统配置"] = "Settings", ["关于系统"] = "About",
        ["工业数据采集与管理"] = "Industrial data acquisition", ["配置通信设备、驱动参数和采集点表"] = "Configure devices, driver parameters and acquisition tags",
        ["定义采样任务、触发条件与数据表映射"] = "Define sampling jobs, triggers and table mappings", ["维护设备参数配方并按需下载"] = "Maintain equipment parameter recipes",
        ["配置事件触发逻辑与自定义数据处理"] = "Configure triggered scripts and custom data processing", ["查看真实连接状态、实时质量与点位数值"] = "Inspect connection states, quality and realtime values",
        ["查询已存储的工艺数据并导出 Excel"] = "Query stored process values and export to Excel", ["管理数据库、WebAPI、MQTT 和运行环境"] = "Manage databases, WebAPI, MQTT and runtime settings",
        ["HiTopo 跨平台工业数据采集与管理"] = "HiTopo cross-platform industrial data platform",
        ["导入配置"] = "Import", ["导出配置"] = "Export", ["启动采集"] = "Start", ["停止采集"] = "Stop", ["服务已停止"] = "Stopped",
        ["采集运行中"] = "Running", ["仿真运行中"] = "Simulation", ["新增设备"] = "Add device", ["删除设备"] = "Delete device",
        ["保存配置"] = "Save", ["采集点表"] = "Acquisition tags", ["设备与通信"] = "Device and connection", ["添加点位"] = "Add tag",
        ["删除所选"] = "Delete selected", ["导入 Excel"] = "Import Excel", ["导出 Excel"] = "Export Excel", ["点位名称"] = "Tag name",
        ["设备地址"] = "Address", ["数据类型"] = "Data type", ["单位"] = "Unit", ["描述"] = "Description", ["读写"] = "Read/write", ["只读"] = "Read only", ["只写"] = "Write only", ["初始值"] = "Initial value",
        ["设备配置"] = "Device configuration", ["设备标识"] = "Device ID", ["设备描述"] = "Description", ["通信驱动"] = "Driver",
        ["采样周期 / ms"] = "Sampling interval / ms", ["单次采样数量"] = "Sample batch size", ["启用设备"] = "Enabled", ["仿真模式（不连接真实设备）"] = "Simulator (no hardware connection)", ["只写不读"] = "Write-only channel",
        ["OPC UA 连接"] = "OPC UA connection", ["服务端地址"] = "Endpoint URL", ["消息安全模式"] = "Security mode", ["安全策略"] = "Security policy",
        ["认证方式"] = "Authentication", ["用户名认证"] = "User authentication", ["用户名"] = "Username", ["密码"] = "Password", ["订阅模式"] = "Subscription mode",
        ["发现端点"] = "Discover endpoints", ["浏览变量"] = "Browse nodes", ["网络与协议参数"] = "Network and protocol", ["设备 IP / 主机名"] = "Device IP / hostname", ["端口"] = "Port",
        ["串口参数"] = "Serial settings", ["串口设备"] = "Serial device", ["波特率"] = "Baud rate", ["数据位"] = "Data bits", ["校验位"] = "Parity", ["停止位"] = "Stop bits",
        ["新增任务"] = "Add job", ["删除任务"] = "Delete job", ["创建数据表"] = "Create table", ["存储任务"] = "Storage job", ["任务名称"] = "Job name", ["数据库表名"] = "Table name",
        ["设备名称"] = "Equipment", ["报表描述"] = "Report description", ["启用存储任务"] = "Enabled", ["存入公共时序表"] = "Store in time-series table", ["采集条件"] = "Sampling condition",
        ["触发方式"] = "Trigger mode", ["周期 / ms"] = "Period / ms", ["触发布尔点位"] = "Boolean trigger tag", ["批号点位"] = "Batch tag", ["选择触发点"] = "Select trigger", ["选择批号点"] = "Select batch tag",
        ["使用表达式"] = "Use expression", ["编辑表达式"] = "Edit expression", ["采集数据列"] = "Sample columns", ["从点表添加"] = "Add from tags", ["添加固定值列"] = "Add constant column",
        ["数据库列名"] = "Column", ["绑定点位"] = "Bound tag", ["可空"] = "Nullable", ["固定值"] = "Constant", ["默认值"] = "Default value",
        ["上升沿采集"] = "Rising edge", ["周期采集"] = "Periodic", ["上升沿与下降沿采集"] = "Both edges",
        ["新增配方"] = "Add recipe", ["删除配方"] = "Delete recipe", ["配方信息"] = "Recipe information", ["配方名称"] = "Recipe name", ["配方描述"] = "Description",
        ["从点表添加参数"] = "Add parameters", ["删除所选参数"] = "Delete parameters", ["参数名称"] = "Parameter", ["设备点位"] = "Tag", ["类型"] = "Type", ["配方值"] = "Recipe value",
        ["新增脚本"] = "Add script", ["删除脚本"] = "Delete script", ["运行一次"] = "Run once", ["脚本与触发条件"] = "Script and trigger", ["脚本名称"] = "Script name", ["启用触发执行"] = "Enable trigger",
        ["Python 编辑器"] = "Python editor", ["插入 Read 点位"] = "Insert Read tag", ["导入 .py 文件"] = "Import .py", ["运行输出"] = "Execution output",
        ["写入所选点位"] = "Write tag", ["下载配方"] = "Download recipe", ["实时值"] = "Value", ["质量"] = "Quality", ["时间戳"] = "Timestamp", ["连接状态"] = "Connection",
        ["查询条件"] = "Query conditions", ["数据表"] = "Table", ["最大结果行数"] = "Row limit", ["开始日期"] = "Start date", ["开始时间"] = "Start time", ["结束日期"] = "End date", ["结束时间"] = "End time", ["点位筛选"] = "Tag filter", ["查询历史"] = "Query",
        ["保存全部配置"] = "Save settings", ["从文件夹导入旧配置"] = "Import legacy folder", ["数据库"] = "Database", ["数据库类型"] = "Database type", ["数据库名 / SQLite 文件"] = "Database / SQLite file",
        ["服务器"] = "Server", ["历史保留月数"] = "Retention / months", ["保存本地日志"] = "Local logging", ["信任 SQL Server 自签名证书"] = "Trust SQL Server certificate",
        ["选择 SQLite 文件"] = "Select SQLite file", ["测试连接"] = "Test connection", ["创建数据库"] = "Create database", ["清理过期历史"] = "Clean expired history", ["监听地址"] = "Listen URL", ["启用 WebAPI"] = "Enable WebAPI",
        ["启用 MQTT 发布"] = "Enable MQTT", ["按点位分别发布"] = "Publish per tag", ["保留消息"] = "Retain", ["客户端标识"] = "Client ID", ["组发布主题"] = "Group topic", ["发布周期 / ms"] = "Publish interval / ms", ["测试 MQTT"] = "Test MQTT",
        ["Python 与配置位置"] = "Python and profile", ["Python 可执行文件"] = "Python executable", ["登录后启动 TopoView"] = "Launch TopoView at login",
        ["登录启动只打开界面；采集仍需手动启动。"] = "Login startup opens the UI; acquisition must be started manually.",
        ["运行与配置"] = "Runtime and profile", ["运行日志"] = "Runtime log", ["刷新日志"] = "Refresh log", ["关闭"] = "Close", ["确定"] = "OK", ["确认"] = "Confirm", ["取消"] = "Cancel", ["确认操作"] = "Confirm action", ["操作未完成"] = "Operation failed"
    };
}

// Display conversion leaves the selected item and serialized values unchanged.
public sealed class UiTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter,
        CultureInfo culture) => UiText.Translate(value?.ToString() ?? "");

    public object ConvertBack(object? value, Type targetType, object? parameter,
        CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
