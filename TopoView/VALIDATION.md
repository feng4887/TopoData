# Views 事件与后台执行对齐（2026-10-23）

- 逐文件对照 `TopoData\SubPage`、`DriverPanel` 与 `frmMain.cs`，核对 25 个 AXAML 页面的控件事件、后台任务与保存流程；除 Avalonia 控件差异外，行为按旧实现修正，未引入新的交互。
- 表达式变量：恢复旧版 `Address` 优先的点位解析（旧 `diaExpress` 把别名存 `TagName`、点位全名存 `Address`），并让 `diaExpress` 回显/回存时保留该字段；新增两条离线回归检查。
- 设备页：保存时按旧版写入 `SampleInterval=20`、`SampleQty=10` 与 `Address`（OPC UA 为 "2"，其余为 "1"）；点表导入由“按名称追加”改回旧版“整表替换并立即保存”；配置校验的采样周期下限由 100 ms 放宽到旧版写入的 20 ms，避免既有 TopoData 配置被拒绝启动。
- 配方页：删除配方后立即保存；参数删除按旧版“确认一次并删除同名全部行”；保存时不再新增点位类型校验（旧版不校验）。
- 系统配置页：`创建数据库`按旧版区分 SQLite 选文件与 SQL Server 存储路径，创建后同时建 `AppLogMessage`、`pubHisValues` 表，SQL Server 追加数据/日志文件增长策略，仅在本次新建时提示成功；通讯测试改用 `SugarDao.IsConnected` 且不再改写当前配置；开启自启时同时创建桌面快捷方式（旧版 `SetDesktopQuick`）。
- 存储任务页：`同步表结构`按旧版 `CreateOrUpdateTable` 返回值分别提示“创建表成功”“创建了 N 个新列”，无变化时不提示；执行前先保存配置。
- 主窗口：切换语言立即写回配置（旧版行为）。
- 未改动的差异（框架/平台所致，或旧版为死代码）：`Write[...]` 插入按钮在旧 Designer 中未挂接，未移植；Scintilla 语法高亮、OPC DA 面板、pythonnet 进程内执行不移植；诊断页写值与配方下载改用进程内服务调用，不再经本地 HTTP（旧版经 `RestUri1`，REST 关闭时操作静默失效）；主窗口新增“未保存修改/退出确认”，导入后重载页面而非重启程序。
- `dotnet build TopoView.csproj` 与 `dotnet build Tests/TopoView.Tests.csproj` 均 0 错误；离线检查 **185 项全部通过**（新增旧格式表达式绑定解析与校验两项）。
- 未连接现场设备、生产数据库或 MQTT Broker。

# 以 TopoData 为准的运行时行为对齐（2026-10-23）

- 逐项对照 `TopoData\Public`、`TopoData\Controllers`、`frmMain.cs` 与 `auDAServer\BLL`（Scheduler、SqlDispatch、DbClear、SqlConsumHelper），把 Services 中与旧实现不一致的行为改回旧版语义；未修改 TopoData、auDASLib、auDAServer、StdLog4 与 Runtime。
- MQTT：恢复 camelCase JSON、`yyyy-MM-dd HH:mm:ss` 批量时间戳、QoS 1、500 ms 发布下限、无数据不发报文、只发布已启用设备的点位，首次生成的配置默认值同旧版 `MqttOperator.LoadConfig`（`127.0.0.1`/`admin`/`RealTimePublisher`/2000 ms）。
- Python：脚本先写入配置目录 `PythonScripts\` 下的 `.py` 文件再执行，注入 `HITOPO_SCRIPT_NAME`、`HITOPO_TRIGGER_NAME`、`HITOPO_TRIGGER_VALUE`、`HITOPO_TRIGGER_TIME`；删除默认 60 秒超时（改为可选 `ScriptService.Timeout`）；解释器按“系统设置 → `HITOPO_PYTHON_EXE` → 程序目录 `python\python.exe` → PATH”解析；缺失或坏质量触发点视为不触发，与旧版一致。
- 存储：缺失/坏质量列写入 NULL 并保留整行（上升沿任务所有列无效时才跳过），浮点四舍五入 4 位；公共时序表按值类型分流 `fvalue`/`bvalue`/`strvalue`，`seq_name` 改用 `ReportName`；恢复旧版工单表 `tb_au_workorder` 的插入/更新；恢复旧版 `DbClear` 每小时自动清理（保留月数最小 1 个月）；PostgreSQL 标识符不加引号，兼容旧库折叠为小写后的列名。
- 调度：`Type 2` 任务与原引擎一致，在没有双边沿点位时回退为 `TimeWindow` 周期采样；周期下限按旧 UI 的 40 ms 判定、运行期钳到 50 ms；工单点位缺失不再阻止启动（写入空 WO）。
- WebAPI：与旧 `Program.cs` 一致，应用打开即监听且停止采集不影响监听（关闭 REST 立即释放端口，启用需重启）；`WriteRealtimeValues` 恢复空数组返回与逐项容错；`DownloadRecipe` 恢复 404/未启用 400/大小写不敏感匹配；错误响应改回纯文本；补回旧 `CommandController` 的空操作路由。
- `dotnet build TopoView.csproj` 与 `dotnet build Tests/TopoView.Tests.csproj` 均 0 错误；离线检查 **183 项全部通过**（含真实 Python 解释器、脚本文件与 `HITOPO_*` 环境变量、超时终止、上升沿去重、坏质量不当下降沿、工单表维护、HTTP 读写/配方/状态、Modbus 回读、八个页面渲染与 24 项控件比对）。
- 有意保留的差异：默认 REST 仍关闭且只绑定本机示例地址（旧版首次运行即监听 `0.0.0.0:8883` 且无认证，保留更安全的默认值）；`mqtt_config.xml` 仍与 `hiTopoCfg.xml` 共用一套配置对象；OPC DA、WinForms/DevExpress、Scintilla 高亮、pythonnet 嵌入式执行仍不移植。

本轮未连接现场设备、生产数据库、MQTT Broker，未在 Linux 实机运行。

# 子页面对齐验证（2026-10-22）

- 逐页对照 `TopoData\SubPage` 的 Designer 与业务代码，修改 TopoView 的 12 个子页面及其弹窗；未修改 TopoData、auDASLib、auDAServer、StdLog4、Services 与 Runtime。
- `dotnet build TopoView.csproj` 与 `dotnet build Tests/TopoView.Tests.csproj` 均 0 错误。
- 离线检查 **178 项全部通过**，其中包含 24 个页面的控件名称/类型/事件处理器逐一比对（`UiParityChecks`）、八个业务页无参预览渲染、具名按钮提交、历史列绑定、标签/字段/配方/写入/表达式弹窗交互。
- 本轮对齐了原版校验顺序与提示文本：写值空值提示、字段名与数据类型校验、表达式变量名规则、配方/脚本/存储任务/系统配置/通讯配置的保存校验、批量删除确认、点表导入导出提示。
- 保留的平台差异：OPC DA 面板未移植（依赖 COM，Linux 不可用，已在 README 说明）；历史查询的 TagN 过滤由 SQL `IN` 改为结果集过滤。两者均有注释说明。
- 未连接现场设备、生产数据库或 MQTT Broker；未在 Linux 实机运行。

# .NET 8 兼容验证（2026-10-16）

- 主项目与测试项目已改为 `net8.0`。UI 使用 Avalonia 11.3.22、DataGrid 11.3.13、FluentAvalonia 2.4.1、Headless 11.3.22；正式 NuGet 包元数据确认 FluentAvalonia 3.x 和 2.5.x 仅提供 .NET 10 程序集。
- 已适配 FluentAvalonia 2 的导航、图标、弹窗及事件类型名称，以及 Avalonia 11 的 TextBox `Watermark` 属性；保留三个自定义弹窗的 `StyleKeyOverride`。
- .NET SDK CLI 构建成功，0 错误；本机 Visual Studio 2022 17.14.34 的 `MSBuild.exe` 实际构建同样成功。未安装或升级 IDE、SDK 或设计器扩展。
- 在本机 .NET 8 运行时完成 **100 项隔离检查**，全部通过；覆盖原有业务检查、八页渲染、无参预览、具名按钮、历史查询列绑定、确认/输入/选点/端点弹窗及取消行为。设备页面和弹窗截图已人工查看。
- `Runtime/hiServer.dll` 及其嵌入模型程序集原本即为 .NET 8，项目引用的 `hiLib` 为 .NET 8、`Log4` 为 .NET Standard 2.0。本轮未改动引擎 DLL、Services、共享项目源码、配置格式或业务接口。
- Windows x64 依赖框架发布与 Linux x64 自包含发布均成功，输出在 `artifacts/publish-net8/`；已核对两端 `runtimeconfig.json` 均为 `net8.0`，Linux 包包含 .NET / ASP.NET Core 8.0.27。旧 `artifacts/publish/`（.NET 10 预览期发布包）已在临时文件清理中移除。
- CodeGraph 已同步并确认索引最新。仍存在共享项目的旧 nullable 等编译警告及 `log4net 2.0.12` 的既有 NU1902 告警，未在本次框架调整中处理。

本轮未连接生产配置、现场设备或远程数据库，未在 Linux 实机运行。VS2022 构建成功不等于设计器拖拽验收；降为 .NET 8 不会增加 WinForms 式画布拖拽能力。

以下为此前界面资源化与业务验证记录，框架版本以本节和当前项目文件为准。

# 界面资源化验证（2026-10-16）

- 当前用户代码基线已通过 CodeGraph 同步和调用/依赖检查；全量重建因既有数据库占用未执行，使用 `sync` 更新，未停止现有服务。
- 主窗口、八个业务页和业务/通用弹窗已迁至独立 AXAML。固定控件、导航、表格列及模板声明在资源中；代码后置保留具名事件、数据装载与业务调用。
- TopoView 编译成功，0 错误；完整隔离检查 **100 项通过**。新增检查覆盖八页无参预览、无运行服务/配置写入副作用、具名按钮、类型提交、配方编辑、历史列特殊名称和实际值绑定、确认/输入/取消及选点/端点弹窗的实际渲染与返回值。
- 已修正自定义 `FAContentDialog` 子类的 `StyleKeyOverride`，实际显示和确认按钮返回值检查通过。
- 本轮 `Services` 全目录与任务开始快照逐文件 SHA256 一致；保留用户的共享生产配置目录、预编译 AnyCPU 引擎引用、`ResolveTagId` 与发现端点的 UI 线程修复。
- AXAML 通过编译器及 Headless + Skia 实际渲染；未在 Visual Studio 内进行真实拖放操作。本机是 VS2022，当前 `.NET 10` 目标需要 VS2026 / 18.0+ 及相应 Avalonia 工具。官方 Plus 的 Toolbox 支持拖入 AXAML，订阅与画布编辑边界见 README。
- 构建仍提示既有 `log4net 2.0.12` 的 NU1902 告警，未在界面重构中升级后端依赖；测试截图保存 API 也有既有过时提示。

本轮仅使用独立测试目录、内存设备、localhost 协议服务及 SQLite，未连接现场设备或生产数据库。以下为重构前的验证记录，保留供追溯。

# 重构前验证记录

日期：2026-10-16。开发环境：Windows 11 x64、.NET SDK 10.0.300。

- TopoView 与离线检查程序编译成功，0 错误。
- **58 项离线集成检查通过**：旧 XML / Excel / ZIP 往返、OPC DA 排除及来源保护、重复 ZIP 拒绝、损坏配置防覆盖、共享引擎（hiServer.dll）的仿真启停与默认值、类型写入与只读保护、Flee 表达式、SQLite 周期/上升沿/时序采样、Excel 历史导出、参数化标识符保护、HTTP 读写/配方/状态、Python Read 和超时终止、坏质量触发保护、启动失败回滚及重启、SQL Server 连接默认信任服务器证书且显式偏好可覆盖、共享引擎 Modbus TCP 真实本机协议读写与回读、OPC UA 未受信任服务器证书自动接受、引擎类型来源断言、发现端点的工作线程行为、八页面原生渲染、语言切换及窄窗口。
- **真实本机 OPC UA 服务器（KEPServerEX，127.0.0.1:49320）联调通过**：安全策略 None/None 端点匹配成功、会话连接、订阅取值质量 192、OPC DA 风格条目名（`模拟器示例.函数.DO`）自动映射为 `ns=2;s=…` 并收到真实数据；期间修复引擎三处问题（端点策略名 `#None`/`None` 双向兼容、订阅逐点容错不再因单个无效地址中断整批、失败路径日志同步到运行日志）。
- Windows x64 Release 发布成功（依赖框架）。
- Linux x64 Release 独立发布成功；输出含 ELF 启动程序、.NET / ASP.NET Core 运行时、Linux Skia 和 SQLite 原生库。
- 发布依赖清单未发现 DevExpress、WinForms、OPC DA / OpcRcw 或 IWshRuntimeLibrary。采集引擎为 `Runtime/hiServer.dll`（已验证 ILONLY、无 32 位标记、无 OpcDaNetCore 依赖）。纯数据模型保留旧 OPC DA 字段用于导入识别。
- `HiTopo.sln` 未改动；旧 TopoData 项目未修改；`auDASLib` 源码未修改；`StdLog4` 仅新增可选 `Sink` 钩子（默认 null，旧程序行为不变）。`auDAServer` 修复 OPC UA 路径三处问题后以 **AnyCPU** 重编译为 `Runtime/hiServer.dll`（原用户构建版本备份为 `Runtime/hiServer.user-bak.dll`）。TopoView 新增 `hiServer` 程序集引用与 `hiLib` / `Log4` 项目引用。

页面由 Avalonia Headless + Skia 实际渲染，未在现场 Windows 桌面进行完整鼠标操作验收。Linux 发布是 Windows 上的交叉发布，**未在 Linux 实机运行**。真实 OPC UA 服务器、安全证书握手、S7 PLC、Modbus RTU 串口、远程数据库、MQTT Broker 和桌面登录启动均未现场联调。

被引用的 `auDASLib` / `StdLog4` / `auDAServer` 项目仍会产生既有编译警告（旧 nullable 标注、废弃 API、异常重抛和未使用字段），未通过全局屏蔽隐藏；TopoView 自身编译已不再包含这些旧源码。主窗口使用构造注入，XAML 的无参数运行时加载警告不影响已验证的显式创建路径。

已验证范围足以供开发审阅及离线演示；现场验收应使用目标 Linux 发行版、设备和独立测试数据库逐项验证后再切换使用。
