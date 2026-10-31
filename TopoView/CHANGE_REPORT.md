# TopoView 本次代码变更记录

本次为新增子项目，除解决方案增加项目引用外，原 TopoData、auDASLib 和 StdLog4 源码未修改。以下为逐文件实际行号；测试截图与第三方许可证原文作为文档资源随项目保存。

| 序号 | 修改或增加代码的目的 | 修改类型 | 旧代码行号 | 新代码行号 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|---|
| 1 | 将 TopoView 加入解决方案 | 增加 | —（新增，无旧行号） | 第 12–13、32–35 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\HiTopo.sln | 与 TopoData 并行开发和构建 | 2026-10-16 16:26:46 |
| 2 | 排除构建产物、测试数据与日志 | 增加 | —（新增，无旧行号） | 第 1–8 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\.gitignore | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 3 | 应用 Fluent 主题与蓝色强调色 | 增加 | —（新增，无旧行号） | 第 1–9 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\App.axaml | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 4 | 初始化桌面应用服务 | 增加 | —（新增，无旧行号） | 第 1–22 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\App.axaml.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 5 | 适配 FluentAvalonia 3 控件类型 | 增加 | —（新增，无旧行号） | 第 1–6 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\GlobalUsings.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 6 | 创建深蓝汉堡导航和浅灰工作区 | 增加 | —（新增，无旧行号） | 第 1–55 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\MainWindow.axaml | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 7 | 连接导航、语言及采集生命周期 | 增加 | —（新增，无旧行号） | 第 1–221 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\MainWindow.axaml.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 8 | 添加桌面入口或离线检查入口 | 增加 | —（新增，无旧行号） | 第 1–19 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Program.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 9 | 说明启动、迁移和功能对应 | 增加 | —（新增，无旧行号） | 第 1–99 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\README.md | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 10 | 隔离实时值池与日志入口 | 增加 | —（新增，无旧行号） | 第 1–28 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\DriverBridge.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 11 | 消除采集对旧日志数据库的依赖 | 增加 | —（新增，无旧行号） | 第 1–10 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\DriverLog.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 12 | 复用托管 Modbus 编解码与队列 | 增加 | —（新增，无旧行号） | 第 1–1213 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\Modbus\ModbusHelpercs.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 13 | 迁移 Modbus RTU 驱动并修正停止 | 增加 | —（新增，无旧行号） | 第 1–379 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\Modbus\ModbusRtuOperator.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 14 | 迁移 Modbus TCP 驱动并修正停止 | 增加 | —（新增，无旧行号） | 第 1–527 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\Modbus\ModbusTcpOperator.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 15 | 迁移 UA 辅助功能并改用目录证书 | 增加 | —（新增，无旧行号） | 第 1–2359 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\OPCUA\UAClientHelperAPI.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 16 | 实现可等待停止的 OPC UA 采集 | 增加 | —（新增，无旧行号） | 第 1–175 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\OPCUA\UAOperator.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 17 | 定义可等待的托管驱动接口 | 增加 | —（新增，无旧行号） | 第 1–261 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\OperatorHelper.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 18 | 复用 S7 基础字节转换 | 增加 | —（新增，无旧行号） | 第 1–49 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\Sharp7\S70.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 19 | 保留原 Sharp7 托管协议源码 | 增加 | —（新增，无旧行号） | 第 1–6221 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\Sharp7\Sharp7.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 20 | 复用 S7 地址和数据转换 | 增加 | —（新增，无旧行号） | 第 1–510 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\Sharp7\Sharp7Helper.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 21 | 迁移 S7 驱动并修正生命周期 | 增加 | —（新增，无旧行号） | 第 1–1173 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\Sharp7\Sharp7Operator.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 22 | 保留原免费版本数量限制 | 增加 | —（新增，无旧行号） | 第 1–45 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Drivers\VersionCtrl.cs | 复用原项目业务并去除 Windows 运行依赖 | 2026-10-16 16:26:46 |
| 23 | 兼容原 Read 点位表达式语法 | 增加 | —（新增，无旧行号） | 第 1–200 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Scripts\PythonScriptReadResolver.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 24 | 管理托管驱动与类型化写入 | 增加 | —（新增，无旧行号） | 第 1–160 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\AcquisitionService.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 25 | 隔离配置并管理启动、停止及回滚 | 增加 | —（新增，无旧行号） | 第 1–201 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\AppContextServices.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 26 | 迁移旧 XML 并排除 OPC DA | 增加 | —（新增，无旧行号） | 第 1–237 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\ConfigurationStore.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 27 | 实现数据库历史、存储与显式清理 | 增加 | —（新增，无旧行号） | 第 1–255 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\DatabaseService.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 28 | 提供无 COM 的用户登录启动 | 增加 | —（新增，无旧行号） | 第 1–36 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\DesktopStartupService.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 29 | 实现可取消的 MQTT 发布 | 增加 | —（新增，无旧行号） | 第 1–70 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\MqttService.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 30 | 实现触发存储和脚本调度 | 增加 | —（新增，无旧行号） | 第 1–136 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\RuntimeJobs.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 31 | 实现外部 Python 和表达式执行 | 增加 | —（新增，无旧行号） | 第 1–97 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\ScriptService.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 32 | 实现点表与历史 Excel 交换 | 增加 | —（新增，无旧行号） | 第 1–58 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\TagExchangeService.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 33 | 保持原 HTTP 读写和配方入口 | 增加 | —（新增，无旧行号） | 第 1–72 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\WebApiService.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 34 | 统一工业界面的圆角、字体和表格 | 增加 | —（新增，无旧行号） | 第 1–20 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Styles\Industrial.axaml | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 35 | 增加本机协议读写检查对端 | 增加 | —（新增，无旧行号） | 第 1–70 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\LoopbackModbusServer.cs | 验证隔离环境中的实际业务行为 | 2026-10-16 16:26:46 |
| 36 | 检查配置、采样、协议与八页面渲染 | 增加 | —（新增，无旧行号） | 第 1–265 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\Program.cs | 验证隔离环境中的实际业务行为 | 2026-10-16 16:26:46 |
| 37 | 配置原生渲染与离线集成检查 | 增加 | —（新增，无旧行号） | 第 1–12 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\TopoView.Tests.csproj | 验证隔离环境中的实际业务行为 | 2026-10-16 16:26:46 |
| 38 | 保留依赖来源和原作者声明 | 增加 | —（新增，无旧行号） | 第 1–18 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\THIRD_PARTY_NOTICES.md | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 39 | 配置 Avalonia 与 FluentAvalonia 跨平台依赖 | 增加 | —（新增，无旧行号） | 第 1–37 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\TopoView.csproj | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 40 | 记录已验证结果与现场验收边界 | 增加 | —（新增，无旧行号） | 第 1–16 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\VALIDATION.md | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 41 | 显示版本、运行范围及日志 | 增加 | —（新增，无旧行号） | 第 1–22 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\AboutPage.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 42 | 实现通信设备及采集点表编辑 | 增加 | —（新增，无旧行号） | 第 1–312 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DevicesPage.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 43 | 实现实时诊断与受控写入 | 增加 | —（新增，无旧行号） | 第 1–112 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DiagnosticsPage.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 44 | 实现历史查询和结果导出 | 增加 | —（新增，无旧行号） | 第 1–61 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\HistoryPage.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 45 | 实现配方与参数维护 | 增加 | —（新增，无旧行号） | 第 1–102 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\RecipesPage.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 46 | 实现 Python 编辑和手动执行 | 增加 | —（新增，无旧行号） | 第 1–132 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ScriptsPage.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 47 | 实现数据库、接口及环境设置 | 增加 | —（新增，无旧行号） | 第 1–142 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\SettingsPage.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 48 | 实现存储任务及触发编辑 | 增加 | —（新增，无旧行号） | 第 1–165 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\StoragePage.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 49 | 实现点位选择与表达式编辑 | 增加 | —（新增，无旧行号） | 第 1–90 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\TagDialogs.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 50 | 实现 OPC UA 按层浏览和变量导入 | 增加 | —（新增，无旧行号） | 第 1–139 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\UaBrowserWindow.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 51 | 复用原生布局、表格及对话框 | 增加 | —（新增，无旧行号） | 第 1–169 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\Ui.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |
| 52 | 提供中英文常用界面标签 | 增加 | —（新增，无旧行号） | 第 1–48 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\UiText.cs | 完成跨平台桌面复刻及可核验交付 | 2026-10-16 16:26:46 |


## AXAML 界面资源化（2026-10-16 21:49:36）

按本轮开始前快照核对真实行号；原 .cs 布局文件删除后迁入 .axaml / .axaml.cs。Services 内容与快照逐文件一致。

| 序号 | 修改或增加代码的目的 | 修改类型 | 旧代码行号 | 新代码行号 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|---|
| 1 | 防止设计模式初始化生产运行服务 | 修改 | 第 15 行 | 第 16–17 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\App.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 2 | 防止设计模式初始化生产运行服务 | 增加 | —（新增，无旧行号） | 第 3 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\App.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 3 | 资源化主窗口和具名汉堡导航 | 修改 | 第 1–4、20–23、27–28、30–34、38–39、41、45–46、48、51–52 行 | 第 1–12、28–36、40–45、47–59、63–108、110–113、117–126、128–130、133–140 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\MainWindow.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 4 | 分离设计构造及运行服务并使用具名事件 | 修改 | 第 1、3、16–21、23、26–85、87、89–92、141–144、148–151、153–166、172–176、178、181、183、185、187–192、196–200、202、205–212、216、218–219 行 | 第 1–2、4、16–23、25–29、39–40、42、44–45、58–65、69–70、72–75、81–82、84、87、89–104、106–107、109–120、125–131、133–136、139–150、155–156、158–290 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\MainWindow.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 5 | 分离设计构造及运行服务并使用具名事件 | 增加 | —（新增，无旧行号） | 第 31–37、50、53、122、152 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\MainWindow.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 6 | 分离设计构造及运行服务并使用具名事件 | 删除 | 第 14、95–132 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\MainWindow.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 7 | 说明 Visual Studio 编辑入口及工具边界 | 增加 | —（新增，无旧行号） | 第 8–36 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\README.md | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 8 | 集中界面标签、表格及组合框的资源样式 | 修改 | 第 1 行 | 第 1–2 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Styles\Industrial.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 9 | 集中界面标签、表格及组合框的资源样式 | 增加 | —（新增，无旧行号） | 第 6、11–12、18–22 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Styles\Industrial.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 10 | 检查设计构造、模板、具名事件和模态弹窗 | 修改 | 第 321、334、336 行 | 第 330、343、345 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\Program.cs | 验证资源迁移后的实际显示、交互及设计安全 | 2026-10-16 21:49:36 |
| 11 | 检查设计构造、模板、具名事件和模态弹窗 | 增加 | —（新增，无旧行号） | 第 3–4、16、18–19、306–308、325、357–594 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\Program.cs | 验证资源迁移后的实际显示、交互及设计安全 | 2026-10-16 21:49:36 |
| 12 | 设置 AXAML 代码后置的 VS 文件嵌套 | 增加 | —（新增，无旧行号） | 第 33–35 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\TopoView.csproj | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 13 | 记录本轮编译、交互与回归验证结果 | 修改 | 第 1 行 | 第 1–13 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\VALIDATION.md | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 14 | 声明系统信息布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–38 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\AboutPage.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 15 | 保留系统信息数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–34 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\AboutPage.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 16 | 移出系统信息的原 C# 控件拼装 | 删除 | 第 1–22 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\AboutPage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 17 | 提供无服务预览及统一事件执行边界 | 增加 | —（新增，无旧行号） | 第 1–49 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\BusinessPage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 18 | 声明设备及点表布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–127 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DevicesPage.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 19 | 保留设备及点表数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–357 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DevicesPage.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 20 | 移出设备及点表的原 C# 控件拼装 | 删除 | 第 1–313 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DevicesPage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 21 | 声明实时诊断布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–33 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DiagnosticsPage.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 22 | 保留实时诊断数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–150 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DiagnosticsPage.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 23 | 移出实时诊断的原 C# 控件拼装 | 删除 | 第 1–112 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DiagnosticsPage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 24 | 声明端点选择布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–25 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\EndpointSelectionDialog.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 25 | 保留端点选择数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–33 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\EndpointSelectionDialog.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 26 | 声明表达式编辑布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–41 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ExpressionWindow.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 27 | 保留表达式编辑数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–129 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ExpressionWindow.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 28 | 按查询元数据加载 AXAML 历史列模板 | 增加 | —（新增，无旧行号） | 第 1–83 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\HistoryColumns.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 29 | 声明历史查询布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–45 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\HistoryPage.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 30 | 保留历史查询数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–76 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\HistoryPage.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 31 | 移出历史查询的原 C# 控件拼装 | 删除 | 第 1–61 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\HistoryPage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 32 | 声明提示确认及输入布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–13 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\MessageDialog.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 33 | 保留提示确认及输入数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–26 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\MessageDialog.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 34 | 声明配方布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–64 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\RecipesPage.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 35 | 保留配方数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–163 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\RecipesPage.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 36 | 移出配方的原 C# 控件拼装 | 删除 | 第 1–102 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\RecipesPage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 37 | 声明Python 脚本布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–84 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ScriptsPage.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 38 | 保留Python 脚本数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–213 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ScriptsPage.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 39 | 移出Python 脚本的原 C# 控件拼装 | 删除 | 第 1–132 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ScriptsPage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 40 | 声明系统配置布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–148 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\SettingsPage.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 41 | 保留系统配置数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–212 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\SettingsPage.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 42 | 移出系统配置的原 C# 控件拼装 | 删除 | 第 1–143 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\SettingsPage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 43 | 声明存储任务布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–79 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\StoragePage.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 44 | 保留存储任务数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–247 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\StoragePage.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 45 | 移出存储任务的原 C# 控件拼装 | 删除 | 第 1–172 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\StoragePage.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 46 | 保留选点接口并调用声明式选择弹窗 | 修改 | 第 1–4、16、18–86 行 | 第 1、10–14、16–18 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\TagDialogs.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 47 | 保留选点接口并调用声明式选择弹窗 | 删除 | 第 6–8 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\TagDialogs.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 48 | 声明点位选择布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–32 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\TagSelectionDialog.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 49 | 保留点位选择数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–43 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\TagSelectionDialog.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 50 | 声明OPC UA 浏览布局与具名控件 | 增加 | —（新增，无旧行号） | 第 1–51 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\UaBrowserWindow.axaml | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 51 | 保留OPC UA 浏览数据装载与具名事件 | 增加 | —（新增，无旧行号） | 第 1–243 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\UaBrowserWindow.axaml.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 52 | 移出OPC UA 浏览的原 C# 控件拼装 | 删除 | 第 1–139 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\UaBrowserWindow.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 53 | 移除代码布局工厂并复用 AXAML 弹窗 | 修改 | 第 1–3、18–112、116、121–122、124、126–128、130、132–135、138、140–144 行 | 第 1、9–12、16–20、26–34、36–38、40–47、49–51、53–59、62–64、66–73 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\Ui.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 54 | 移除代码布局工厂并复用 AXAML 弹窗 | 增加 | —（新增，无旧行号） | 第 23 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\Ui.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 55 | 移除代码布局工厂并复用 AXAML 弹窗 | 删除 | 第 5–8、10–12、149–169 行 | —（已删除，无新行号） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\Ui.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 56 | 翻译声明式控件标题并保留绑定值 | 增加 | —（新增，无旧行号） | 第 2–9、16–41、83–92 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\UiText.cs | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |
| 57 | 排除本轮隔离界面检查日志 | 增加 | —（新增，无旧行号） | 第 9 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\.gitignore | 界面可由 AXAML 设计器编辑并保持当前业务行为 | 2026-10-16 21:49:36 |


## .NET 8 兼容调整（2026-10-16 22:20:29）

| 序号 | 修改或增加代码的目的 | 修改类型 | 旧代码行号 | 新代码行号 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|---|
| 1 | 设置 .NET 8 及兼容的 UI 包版本 | 修改 | 第 4、12–16 行 | 第 4、12–16 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\TopoView.csproj | FluentAvalonia 3.x 仅支持 .NET 10 | 2026-10-16 22:20:29 |
| 2 | 测试项目使用 .NET 8 和配套 Headless | 修改 | 第 4、10 行 | 第 4、10 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\TopoView.Tests.csproj | 在目标运行时验证原有检查 | 2026-10-16 22:20:29 |
| 3 | 更新框架、VS2022 与发布目录说明 | 修改 | 第 3、13、40、58–59、127 行 | 第 3、13、40、58–61、129 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\README.md | 提供与实际 .NET 8 输出一致的使用入口 | 2026-10-16 22:20:29 |
| 4 | 记录 .NET 8 构建与隔离验证结果 | 增加 | —（新增，无旧行号） | 第 1–14 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\VALIDATION.md | 区分实测范围与未验收项目 | 2026-10-16 22:20:29 |
| 5 | 关于页显示 .NET 8 | 修改 | 第 14 行 | 第 14 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\AboutPage.axaml | 框架信息与当前目标一致 | 2026-10-16 22:20:29 |
| 6 | 全局别名映射 FluentAvalonia 2 控件 | 修改 | 第 1–6 行 | 第 1–6 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\GlobalUsings.cs | 适配旧版本未带 FA 前缀的类型 | 2026-10-16 22:20:29 |
| 7 | 导航与图标使用 FluentAvalonia 2 名称 | 修改 | 第 63、67–109、114、132 行 | 第 63、67–109、114、132 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\MainWindow.axaml | 保持汉堡侧栏的现有布局及功能 | 2026-10-16 22:20:29 |
| 8 | 适配导航枚举和事件类型 | 修改 | 第 129、203 行 | 第 129、203 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\MainWindow.axaml.cs | 保留页面切换与提交逻辑 | 2026-10-16 22:20:29 |
| 9 | 适配 ContentDialog 资源声明 | 修改 | 第 1、13 行 | 第 1、13 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\MessageDialog.axaml | 使用 FluentAvalonia 2 对应控件 | 2026-10-16 22:20:29 |
| 10 | 适配 ContentDialog 基类与主题键 | 修改 | 第 6、8 行 | 第 6、8 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\MessageDialog.axaml.cs | 保留弹窗默认模板和交互结果 | 2026-10-16 22:20:29 |
| 11 | 适配 ContentDialog 资源声明及占位提示 | 修改 | 第 1、18、32 行 | 第 1、18、32 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\TagSelectionDialog.axaml | 使用 FluentAvalonia 2 对应控件 | 2026-10-16 22:20:29 |
| 12 | 适配 ContentDialog 基类与主题键 | 修改 | 第 9、11 行 | 第 9、11 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\TagSelectionDialog.axaml.cs | 保留弹窗默认模板和交互结果 | 2026-10-16 22:20:29 |
| 13 | 适配 ContentDialog 资源声明 | 修改 | 第 1、25 行 | 第 1、25 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\EndpointSelectionDialog.axaml | 使用 FluentAvalonia 2 对应控件 | 2026-10-16 22:20:29 |
| 14 | 适配 ContentDialog 基类与主题键 | 修改 | 第 7、9 行 | 第 7、9 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\EndpointSelectionDialog.axaml.cs | 保留弹窗默认模板和交互结果 | 2026-10-16 22:20:29 |
| 15 | 适配弹窗类型与 Watermark 翻译 | 修改 | 第 28–29、33 行 | 第 28–29、33 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\UiText.cs | 保留静态文字和占位提示的双语行为 | 2026-10-16 22:20:29 |
| 16 | 测试引用对应的 ContentDialog 类型 | 修改 | 第 537、551、566、583、588 行 | 第 537、551、566、583、588 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\Program.cs | 保留弹窗实际交互与取消回归检查 | 2026-10-16 22:20:29 |
| 17 | 筛选输入框使用 Watermark | 修改 | 第 11 行 | 第 11 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\DiagnosticsPage.axaml | 适配 Avalonia 11 占位提示属性 | 2026-10-16 22:20:29 |
| 18 | 历史筛选框使用 Watermark | 修改 | 第 24 行 | 第 24 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\HistoryPage.axaml | 适配 Avalonia 11 占位提示属性 | 2026-10-16 22:20:29 |


## 子页面与 TopoData 对齐（2026-10-22）

逐页对照 `TopoData\SubPage` 的 Designer 与业务代码，只修改 `TopoView\Views` 下的子页面及其测试；服务层、运行时和 TopoData 源码未改动。行号为本轮修改后的实际行数。

| 序号 | 修改或增加代码的目的 | 修改类型 | 旧代码行号 | 新代码行号 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|---|
| 1 | 恢复原写入对话框尺寸、字段与空值提示 | 修改 | 第 1–13 行 | 第 1–27 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\diaWrite.axaml | 控件文本与布局对齐 diaWrite.Designer.cs | 2026-10-22 10:20:00 |
| 2 | 恢复 TagName/UOM/strValue 属性与空值校验 | 修改 | 第 1–29 行 | 第 1–29 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\diaWrite.axaml.cs | 对齐 diaWrite.cs 的校验与返回值 | 2026-10-22 10:20:00 |
| 3 | 诊断页按原版列宽和过滤规则刷新 | 修改 | 第 1–22、1–143 行 | 第 1–22、1–207 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucDiagnose.axaml(.cs) | 对齐 ucDiagnose.cs 的定时刷新、过滤与写入流程 | 2026-10-22 10:25:00 |
| 4 | 字段配置对话框恢复原布局与校验顺序 | 修改 | 第 1–15、1–69 行 | 第 1–43、1–181 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\diaColumnSelect.axaml(.cs) | 对齐 diaColumnSelect.cs 的提示与默认值解析 | 2026-10-22 10:35:00 |
| 5 | 选点对话框恢复多选、过滤算法与列宽 | 修改 | 第 1–20、1–30 行 | 第 1–28、1–100 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\diaTagSelect.axaml(.cs) | 对齐 diaTagSelect.cs 的选择属性与搜索 | 2026-10-22 10:40:00 |
| 6 | 表达式编辑器恢复变量校验和中间变量添加 | 修改 | 第 1–25、1–128 行 | 第 1–44、1–126 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\diaExpress.axaml(.cs) | 对齐 diaExpress.cs 的 button2/保存/检查流程 | 2026-10-22 10:50:00 |
| 7 | 表达式检查恢复逐行校验与结果提示 | 修改 | 第 1–15、1–47 行 | 第 1–24、1–89 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\diaExpressCheck.axaml(.cs) | 对齐 diaExpressCheck.cs 的提示文本 | 2026-10-22 10:52:00 |
| 8 | 表达式说明恢复原尺寸与控件结构 | 修改 | 第 1–10 行 | 第 1–20 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\diaExpressAbout.axaml | 对齐 diaExpressAbout.Designer.cs | 2026-10-22 10:53:00 |
| 9 | 配方字段对话框恢复多选与校验消息 | 修改 | 第 1–15、1–64 行 | 第 1–38、1–135 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\diaRecipeTagSelect.axaml(.cs) | 对齐 diaRecipeTagSelect.cs | 2026-10-22 10:58:00 |
| 10 | 关于页文案恢复原 TopoData 说明 | 修改 | 第 11–15 行 | 第 10–26 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucAbout.axaml.cs | 对齐 ucAbout.cs 的 ucAbout_Load | 2026-10-22 11:00:00 |
| 11 | 历史查询恢复宽表/时序表选项与提示 | 修改 | 第 1–35、1–52 行 | 第 1–35、1–118 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucHisQuery.axaml(.cs) | 对齐 ucHisQuery.cs 的查询、导出与提示 | 2026-10-22 11:08:00 |
| 12 | 存储任务恢复原字段名、触发校验和列操作 | 修改 | 第 1–55、1–150 行 | 第 1–55、1–283 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucDataTableCfg.axaml(.cs) | 对齐 ucDataTableCfg.cs 的保存与字段增删 | 2026-10-22 11:25:00 |
| 13 | 配方页恢复自动命名、保存提示与删除确认 | 修改 | 第 1–165 行 | 第 1–232 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucRecipe.axaml.cs | 对齐 ucRecipe.cs 的按钮流程 | 2026-10-22 11:35:00 |
| 14 | 脚本页恢复面板校验、执行与 Read 插入 | 修改 | 第 1–34、1–195 行 | 第 1–34、1–326 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucScripts.axaml(.cs) | 对齐 ucScripts.cs 的校验消息与执行 | 2026-10-22 11:45:00 |
| 15 | 系统配置恢复数据库/MQTT 提示与字段状态 | 修改 | 第 1–163 行 | 第 1–242 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucSystemCfg.axaml.cs | 对齐 ucSystemCfg.cs 的测试与保存消息 | 2026-10-22 11:55:00 |
| 16 | 通讯配置恢复 IP 校验、点表导入导出提示 | 修改 | 第 1–225 行 | 第 1–297 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucCannelCfg.axaml.cs | 对齐 ucCannelCfg.cs 的保存与导入导出 | 2026-10-22 12:05:00 |
| 17 | 驱动面板补充 Host 只读属性 | 修改 | 第 19–24、19–26 行 | 第 22–25、22–24 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucDriverProfinet.axaml.cs、ucDriverModbusTCP.axaml.cs | 供保存时的 IP 校验使用 | 2026-10-22 12:05:00 |
| 18 | 选点接口传递多选参数 | 修改 | 第 1–22 行 | 第 1–23 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\TagDialogs.cs | 支持原版的批量选点 | 2026-10-22 10:45:00 |
| 19 | 测试适配新对话框接口并确认删除 | 修改 | 第 598–616、527–546、673–678、686–692 行 | 第 598–616、527–551、675–690、692–711 行 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\Program.cs | 保持 178 项检查全部通过 | 2026-10-22 12:15:00 |

## Public 文件夹整理（2026-10-25）

新增 TopoView\Public 文件夹（对标 TopoData 项目）后，命名空间整理、引用来源切换与重复清理；整理后 TopoView 与 Tests 均 0 错误，测试套件 181 项全部通过。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 删除与 Services/DesktopStartupService 重复的登录自启类 | 删除 | 原 Public\AutoStart.cs 依赖 IWshRuntimeLibrary COM 组件（Windows 专属，TopoView 未引用） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Public\AutoStart.cs | 功能已由跨平台 DesktopStartupService 实现 | 2026-10-25 11:07:54 |
| 2 | 删除与 Services/TagExchangeService 重复的 Excel 帮助类 | 删除 | 原 Public\ExcelHelper.cs 依赖 EPPlus（TopoView 统一用 ClosedXML）且含无效 WPF using | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Public\ExcelHelper.cs | 功能已由 TagExchangeService 实现 | 2026-10-25 11:07:54 |
| 3 | 删除与 Services/WebApiService 重复的 Web 服务类 | 删除 | 原 Public\WebServer.cs 依赖 Microsoft.Graph（TopoView 未引用） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Public\WebServer.cs | 功能已由 WebApiService 实现 | 2026-10-25 11:07:54 |
| 4 | 删除与 Services/ScriptService 重复的 Python 执行类 | 删除 | 原 Public\PythonScriptExecutor.cs、PythonScriptOperater.cs 依赖 Python.Runtime（TopoView 未引用） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Public\PythonScriptExecutor.cs、Public\PythonScriptOperater.cs | 功能已由 ScriptService/RuntimeJobs 实现 | 2026-10-25 11:07:54 |
| 5 | 删除与 diaExpressCheck/ScriptService 重复的表达式求值类 | 删除 | 原 Public\FleeOperator.cs 的 ConditionIsOK 已在 diaExpressCheck.Evaluate 与 ScriptService 内联实现 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Public\FleeOperator.cs | 消除功能重复 | 2026-10-25 11:07:54 |
| 6 | 删除无引用的数据类型 DTO 死代码 | 删除 | DataTypeItem/Product 在 TopoView 全工程无引用，且携带遗留 TopoData.Page 命名空间 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Public\DataTypeList.cs | 死代码清理 | 2026-10-25 11:07:54 |
| 7 | 删除与 Public 重复的 Read 解析器副本 | 删除 | Runtime\Scripts\PythonScriptReadResolver.cs 与 Public 副本逐字节相同，导致 CS0101/CS0111 重复定义错误 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Runtime\Scripts\PythonScriptReadResolver.cs | 保留 Public 作为唯一位置 | 2026-10-25 11:07:54 |
| 8 | 删除与 hiLib 重复的 DataImport 副本 | 删除 | hiServer 引擎 ItemPool.Start 从 hiLib 的 auDASLib.DataImport 静态字典构建 TagPool，TopoView 必须继续引用 hiLib 版本 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Public\DataImport.cs | 避免引擎点表池与 UI 静态状态分离（曾导致写入测试"点位不存在"） | 2026-10-25 11:07:54 |
| 9 | 统一保留文件的命名空间为 TopoView.Public | 修改 | 命名空间 auDASLib → TopoView.Public；PythonScriptReadResolver/UAOperator 补 using auDASLib（DBTag、RealTimeValue、UACfg） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Public\PubFunction.cs、UAClientHelperAPI.cs、UAOperator.cs、PythonScriptReadResolver.cs | 归属 TopoView 项目命名空间 | 2026-10-25 11:07:54 |
| 10 | 全局别名绑定到本地 Public 副本 | 修改 | 新增 4 行 global using 别名；UAOperator 未别名化——AcquisitionService 的通道连接判断必须匹配 hiServer 引擎的 auDAServer.UAOperator 驱动类 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\GlobalUsings.cs | PubFunction/UAClientHelperAPI/PythonScriptReadResolver 改为"从这里"引用，不再取自 hiLib/hiServer | 2026-10-25 11:07:54 |

## 信息对话框标题拖动（2026-10-25）

MessageBox 类对话框（Ui.MessageAsync / ConfirmAsync / PromptAsync 及 frmMain 的保存询问，均经 MessageDialog）的卡片支持按住标题区拖动：FluentAvalonia 的 ContentDialog 是窗口内居中的浮层卡片而非独立窗口，实现方式为按住标题时对模板卡片 Border#BackgroundElement 施加 TranslateTransform，并把光标显示为移动样式；消息正文与输入框区域的点击不会触发拖动。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 对话框卡片支持按标题拖动 | 修改 | 在 Loaded（标题区位于内容 ScrollViewer 内、物化较晚）及首次按下时定位模板部件 Title 与 BackgroundElement 并挂钩指针事件；拖动时按窗口范围收敛位置（四周各保留 60 dip 可见） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\MessageDialog.axaml.cs | FluentAvalonia ContentDialog 浮层本身不支持移动 | 2026-10-25 11:42:49 |
| 2 | 新增标题拖动自动验证 | 修改 | RunDialogChecks 中以无头鼠标按下/移动/抬起模拟拖动标题，断言卡片位移 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\Program.cs | 保持 182 项检查全部通过 | 2026-10-25 11:42:49 |

## 信息对话框改为真窗口（2026-10-25）

针对"暗背景遮罩"与"只能窗口内拖动"两点反馈，MessageDialog 由 FluentAvalonia ContentDialog 浮层改为真正的无边框模态窗口：不再有模态暗背景（遮罩层随之取消），标题栏通过 BeginMoveDrag 移动真实窗口，可拖到屏幕任意位置。卡片保持 Fluent 外观（圆角、阴影、顶部标题条、右下按钮组），确定/取消/关闭按钮语义与 ContentDialog 一致（Esc/Alt+F4 返回 None）；确认与输入对话框等公开 API（Title/PrimaryButtonText/SecondaryButtonText/CloseButtonText/SetMessage/SetInput/InputValue/ShowAsync）不变，Ui 与 frmMain 调用方仅 ShowAsync 实参从 TopLevel 收窄为 Window。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 对话框改为无边框模态真窗口 | 修改 | SystemDecorations=None + 透明窗口 + 圆角卡片；标题栏 PointerPressed 调 BeginMoveDrag；按钮 Click 调 Close(ContentDialogResult)；主按钮 IsDefault、关闭按钮 IsCancel，打开时焦点落在主按钮 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\MessageDialog.axaml(.cs) | 取消暗背景遮罩并支持全屏范围拖动 | 2026-10-25 12:05:00 |
| 2 | Ui 对话框调用适配真窗口 | 修改 | MessageAsync/ConfirmAsync/PromptAsync 的 ShowAsync 实参由 TopLevel 强转为 Window | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\Ui.cs | ShowDialog 需要窗口所有者 | 2026-10-25 12:05:00 |
| 3 | 测试适配真窗口对话框 | 修改 | 渲染检查与截图改针对对话框窗口自身；新增"无边框模态窗口"属性检查替换原卡片位移检查（拖动为系统级 BeginMoveDrag）；CancelDialog 参数放宽为 Control；ConfirmOpenDialog 经 OwnedWindows 查找已打开对话框 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\Program.cs | 保持 182 项检查全部通过 | 2026-10-25 12:05:00 |

## 配置目录按操作系统区分（2026-10-25）

默认配置目录改为按操作系统解析：Windows 沿用现有共享生产目录（auDASLib.pubDefine.Folder，即 C:\Users\Public\Documents\Config，与 TopoData 互通不变）；Linux 改为应用可执行文件同级目录下的 Cfg 文件夹（AppContext.BaseDirectory\Cfg，此前 Linux 沿用 pubDefine 指向的 应用目录\Config）。Linux 上引擎（hiServer）不自行读盘——TopoView 读取 XML 后经 ServerCfg.Instance 注入，故无路径不一致问题；PythonScripts 子目录、preferences.json、默认 SQLite 库路径均随配置目录自动跟随。TOPOVIEW_CONFIG_DIR 环境变量与 --config-dir 命令行参数的覆盖优先级保持不变。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 默认配置目录按操作系统区分 | 修改 | 构造函数目录解析改为 DefaultConfigDirectory()：Windows 返回 pubDefine.Folder，Linux 返回 AppContext.BaseDirectory\Cfg；TOPOVIEW_CONFIG_DIR 与 --config-dir 覆盖逻辑不变 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\AppContextServices.cs | Linux 按"应用文件相同目录\Cfg"读取配置 | 2026-10-25 12:19:30 |
| 2 | 注释同步目录约定 | 修改 | 头部注释说明默认目录按操作系统区分并指向 DefaultConfigDirectory | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Services\ConfigurationStore.cs | 保持文档一致 | 2026-10-25 12:19:30 |

## 五个配置 XML 的遗留路径定义对齐 Cfg（2026-10-25）

TopoView 自身的五个 XML（hiTopoCfg / hiTopoTagDef / hiTopoRecipeDef / hiTopoPythonScriptDef / mqtt_config）此前已随配置目录按操作系统读取，本次补齐遗留路径定义：auDASLib.pubDefine 的 Linux 分支由 应用目录\Config 改为 应用目录\Cfg（Folder、Configuration=hiTopoCfg.xml、TagDefine=hiTopoTagDef.xml），使 auDAServer / auDAServerLinux 服务经 pubDefine 读取的配置与 TopoView 的 Cfg 目录一致（Config→Cfg 也是 pubDefine 注释中保留的早期约定）。Windows 分支未动，TopoData 不受影响；TopoView 运行流程不触达 pubDefine 的这些路径（引擎经 ServerCfg.Instance 注入配置），Runtime\hiServer.dll 无需重建，服务侧重新发布后生效。pubDefine 中 Austart.xml 与 LogSoftwareReg.sql（服务授权/注册辅助文件，不属于五个配置 XML）的 Linux 路径仍为 Config。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | pubDefine Linux 分支对齐 Cfg | 修改 | Folder、Configuration、TagDefine 三个属性的 Linux 分支 ApplicationBase+"Config/…" 改为 ApplicationBase+"Cfg/…"；Windows 分支与 ReportPath 保持不变 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\auDASLib\Model\Define.cs | 服务端读取 hiTopoCfg/hiTopoTagDef 与 TopoView 的 Cfg 目录保持一致 | 2026-10-25 12:32:23 |

## 临时文件清理（2026-10-25）

按 .gitignore 声明的可丢弃范围清理构建与测试中间产物，共释放约 640 MB：bin\、obj\、Tests\bin\、Tests\obj\（构建输出与中间文件，下次构建自动重建）、artifacts\tests\（测试运行数据，跑测试自动重建）、artifacts\publish\（.NET 10 预览期旧发布包 176M + 清理前误覆盖的 net8 依赖框架包 176M，功能由 publish-net8 覆盖）。保留：artifacts\publish-net8\（README/VALIDATION 引用的现行交付包）、artifacts\*.json（交付清单）、Runtime\hiServer.user-bak.dll（与现行 hiServer.dll 内容不同的历史备份，待用户确认后再决定去留）。README 与 VALIDATION 中对已删除 artifacts\publish\ 的引用已同步更新。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 清理构建中间产物与过时发布包 | 删除 | bin\、obj\、Tests\bin\、Tests\obj\、artifacts\tests\、artifacts\publish\（含 .NET 10 旧包与重复的 net8 包） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\bin、obj、Tests\bin、Tests\obj、artifacts\tests、artifacts\publish | .gitignore 声明的可丢弃产物，均可用构建/测试重新生成 | 2026-10-25 12:52:22 |
| 2 | 文档同步清理结果 | 修改 | README 发布说明与 VALIDATION 第 43 行改为"旧 artifacts\publish\ 已在临时文件清理中移除" | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\README.md、VALIDATION.md | 文档与目录实际状态保持一致 | 2026-10-25 12:52:22 |

## 脚本页 Python 语法高亮（2026-10-25）

参考 TopoData.SubPage.ucScripts.ConfigurePythonEditor（Scintilla lexer "python"）为脚本页编辑器补齐 Python 语法高亮：引入 AvaloniaEdit（Avalonia 生态标准代码编辑控件，MIT），以 xshd 定义复刻原版配色与两组关键字表——注释绿 #008000、字符串红 #A31515（含三引号/转义）、关键字蓝加粗 #0000FF、内建函数棕 #795E26、数字青绿 #098658、装饰器紫 #800080；编辑行为对齐原版：显示行号、4 空格缩进（Tab 转空格）、不自动换行。AvaloniaEdit 的控件模板不随包自动应用，需在 App.axaml 显式引入其 Fluent 主题，否则 TextEditor 不渲染（曾导致编辑器空白）。UI 一致性测试的角色映射同步登记 Scintilla→TextEditor 对应关系；Tests/Program.cs 曾临时注入演示脚本用于截图验证高亮渲染，验证（五种配色像素均在渲染图中出现）后已还原。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 引入 AvaloniaEdit 并嵌入高亮定义 | 增加 | PackageReference Avalonia.AvaloniaEdit 11.4.1（第 14 行）；Assets\Python-Mode.xshd 以 EmbeddedResource 嵌入（第 39 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\TopoView.csproj | 提供带语法高亮的代码编辑控件 | 2026-10-25 14:38:09 |
| 2 | 新建 Python 高亮定义 | 增加 | 全新 147 行 xshd：配色与关键字表逐项对应 TopoData ConfigurePythonEditor 的 Scintilla 样式与 SetKeywords(0/1) | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Assets\Python-Mode.xshd | 与 WinForms 原版高亮观感一致 | 2026-10-25 14:38:09 |
| 3 | 新建高亮加载器 | 增加 | 全新 22 行：从程序集清单流加载 xshd 并缓存为 Definition | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\PythonHighlighting.cs | 供脚本页编辑器一次性获取定义 | 2026-10-25 14:38:09 |
| 4 | 脚本编辑器替换为 TextEditor | 修改 | memoEditScriptText 由 TextBox（AcceptsReturn/Tab）改为 avaloniaEdit:TextEditor（第 27-32 行）：白底黑字、等宽字体、行号、不换行、双向滚动 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucScripts.axaml | 实现语法高亮显示 | 2026-10-25 14:38:09 |
| 5 | 编辑器配置与光标 API 适配 | 修改 | 新增 ConfigurePythonEditor（第 32-40 行：挂高亮、缩进 4、Tab 转空格）；InsertReadTagAsync 的 CaretIndex/Text 改为 CaretOffset/Document.Insert（第 333-335 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucScripts.axaml.cs | 对齐原版编辑行为与 AvaloniaEdit API | 2026-10-25 14:38:09 |
| 6 | 显式引入 AvaloniaEdit Fluent 主题 | 修改 | 应用样式追加 avares://AvaloniaEdit/Themes/Fluent/AvaloniaEdit.xaml（第 7 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\App.axaml | 该控件模板不会自动应用，缺失时编辑器整体空白 | 2026-10-25 14:38:09 |
| 7 | UI 一致性测试登记新角色 | 修改 | Compatible 映射把 Scintilla 单列：允许 TextBox 或 TextEditor（第 79-80 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Tests\UiParityChecks.cs | 保持跨框架控件角色映射可核验 | 2026-10-25 14:38:09 |

## 内容区换装 Siemens iX 浅色主题（2026-10-25）

按 Siemens Industrial Experience classic-light 官方 token（siemens/ix 2.7.0 theme-classic-light.css）把内容区控件统一为 iX 设计语言：petrol 主色 #006E93（hover #196269 / active #16565C），iX 灰阶画布 #EFF0F1 与白色面板分层，#000A14 三档透明度文字色，#001428 系边框，全部控件 0 圆角方正造型、1px 边框、32px 紧凑高度，正文字号 14、Arial 优先字体链；表格灰色表头 + petrol 淡色选中行（#EBF7F8/#D1FBFF），列表项 petrol 选中态，按钮完整 default/hover/pressed/disabled 四态（同时覆盖 FluentAvalonia 的 Button/AccentButton 状态资源键）。左侧导航汉堡菜单的窗体本地资源（#13243D 海军蓝系）按约定未做任何改动。无头渲染 8 个页面像素验证：petrol 与灰阶画布到位、旧蓝色系（#2475E8/#1F62CA/#E8F0FF/#F2F5F9）全部 0 像素、导航区 #13243D 与选中态 #213C60 逐页保持。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 内容区控件统一切换 iX 浅色主题 | 修改 | 重写全部样式：iX 色板资源（主色/灰阶/文字/边框/状态色）、ControlCornerRadius=0、Button 四态+primary、TextBox/ComboBox/DataGrid/ListBox.entityList/legacyGroup 面板等（1-155 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Styles\Industrial.axaml | 对齐 Siemens iX classic-light 设计语言 | 2026-10-25 16:27:25 |
| 2 | 全局强调色改为 iX petrol | 修改 | FluentAvaloniaTheme CustomAccentColor #2475E8→#006E93（第 6 行），驱动焦点/选中/复选/单选等 Fluent 状态色 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\App.axaml | 与 iX 主色一致 | 2026-10-25 16:27:25 |
| 3 | 顶栏适配 iX（导航资源未动） | 修改 | 顶栏底边框 #E1E7EF→#33001428、logo 圆角 8→0 且底色改 petrol、HiTopo 文字 #172B49→#E6000A14（第 21-29 行）；Window.Resources 中导航本地配色保持原值 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\frmMain.axaml | 顶栏与内容区同语言，左侧汉堡菜单按约定不变 | 2026-10-25 16:27:25 |

## 主题收敛为单一“样式表”（token 单一来源，2026-10-25）

把 iX 主题整理成与 Web 单一 CSS 等价的结构：所有色值只出现在 Styles/Industrial.axaml 顶部的 token 块（等价 CSS :root 变量，Color + Brush 两层），文件内全部样式、FluentAvalonia 状态键覆盖（Button/AccentButton/DataGrid 系列）一律经 StaticResource 引用 token；页面里的散点色值改为 DynamicResource 引用——frmMain 顶栏（底色/边框/logo/文字，Window 底色改用 IxGray2Brush 并删除旧的 TopoCanvas 键）、ucScripts 的 Python 编辑器（白底/深字）、MessageDialog（圆角 8→0、阴影改 iX elevation-4 规格 0 4 12 #24000000）；诊断页质量红绿灯从代码硬编码 #389853/#F1973F 改为跟随统一样式表的 IxSuccessBrush/IxWarningBrush（页面挂树时 TryGetResource 注入 RealtimeRow.ApplyThemeBrushes，静态默认值同色兜底）。App.axaml 的 CustomAccentColor 是主题引擎属性、无法引用样式资源，保留字面值并加同步注释。左侧导航窗体本地资源仍按约定冻结不动。今后换主题/换色只改 Industrial.axaml 的 token 块（及 App.axaml 一个同步值）。验证：编译 0 错误、134 项 UI 检查通过、8 页像素比对与收敛前完全一致（导航 #13243D/#213C60 逐页不变、旧蓝 0 像素），诊断页质量灯出现 iX #2C8500/#B88D00 各 120 像素。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 主题 token 化（单一来源） | 修改 | 重构为 Color+Brush 两层 token 块，全部样式与 Fluent 状态键改 StaticResource 引用（第 8-127 行）；新增 IxScrimBrush 等语义键 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Styles\Industrial.axaml | 等价 CSS 变量：改色只动一处 | 2026-10-25 16:58:00 |
| 2 | 顶栏与窗口底色改资源引用 | 修改 | 顶栏 3 处内联色与 Window Background 改 DynamicResource（IxGray1/2、IxBorderSoft、IxPrimary、IxPrimaryContrast、IxText），删除已无人使用的 TopoCanvas 键 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\frmMain.axaml | 色值不再散落页面 | 2026-10-25 16:58:00 |
| 3 | 脚本编辑器配色接入 token | 修改 | TextEditor Background/Foreground 由 White/Black 改 DynamicResource IxGray1Brush/IxTextBrush（第 29 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucScripts.axaml | 编辑器底色跟随统一样式表 | 2026-10-25 16:58:00 |
| 4 | 对话框 iX 化并去内联色 | 修改 | 卡片圆角 8→0，阴影改 iX elevation-4 规格（第 13 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\MessageDialog.axaml | 与统一样式表同一设计语言 | 2026-10-25 16:58:00 |
| 5 | 质量红绿灯接入统一样式表 | 修改 | 硬编码 #389853/#F1973F 改为 iX 成功/警告色，挂树时经 TryGetResource 注入 RealtimeRow.ApplyThemeBrushes，静态默认同色兜底（第 198-212、30-48 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Views\ucDiagnose.axaml.cs | 语义状态色与主题单一来源 | 2026-10-25 16:58:00 |
| 6 | 强调色同步注释 | 修改 | CustomAccentColor 上方注明与 Industrial.axaml 的 IxPrimaryColor 保持同值 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\App.axaml | 主题引擎属性无法引用样式资源，注释防漂移 | 2026-10-25 16:58:00 |

## 左侧汉堡菜单去标题与白色底（2026-10-25）

去掉导航窗格顶部的 TOPOVIEW 标题（删除 NavigationView 的 PaneTitle 属性）。排查发现汉堡折叠按钮的白色药丸底是 Industrial.axaml 全局 Button 样式（iX 白底）命中了 NavigationView 模板内的 Button——样式优先级高于控件主题（PaneToggleButtonStyle 绑 NavigationViewItemBackground），资源覆盖无效，改用 /template/ Button#TogglePaneButton 精确选择器还原为透明幽灵按钮（悬停 #1C3450、按压 #0F2036，图标 #C5D2E5）；同时经 NavigationView 本地资源把导航项悬停/按压/选中悬停全部覆盖为海军蓝系（键名已对照 FluentAvaloniaUI 2.4.1 程序集逐一确认存在），内容区 iX 按钮样式不再外溢进导航。验证：编译 0 错误、134 项 UI 检查通过；渲染图导航区（顶栏以下）近白像素 0，海军蓝底 168080 像素保持。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 移除导航窗格标题 | 修改 | 删除 PaneTitle="TOPOVIEW"（原第 46 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\frmMain.axaml | 用户要求去掉 TOPVIEW 字样 | 2026-10-25 17:20:00 |
| 2 | 汉堡折叠按钮去白底 | 修改 | 新增 Window.Styles 三条 /template/ Button#TogglePaneButton 选择器（常态透明/悬停/按压，第 17-34 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\frmMain.axaml | 全局 Button 样式优先级高于控件主题导致白底 | 2026-10-25 17:20:00 |
| 3 | 导航项交互态屏蔽浅色 | 修改 | NavigationView.Resources 覆盖 ItemBackground/Foreground 的 PointerOver/Pressed/Selected 组合共 8 键为海军蓝系 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\frmMain.axaml | 运行时悬停不出现浅色底 | 2026-10-25 17:20:00 |

## 信息日志移至窗口底部状态栏（2026-10-25）

toolStrip_ServiceStatus（运行消息/信息日志，Context.LastMessage 与 OnMessage 均写入该控件）原先挤在顶栏中间，现移到窗口底部新增的状态栏，同时还原原版 TopoData 的布局语义（原版即为底部 Bar 的 BarStaticItem，右对齐）。主网格行定义 Auto,* 改为 Auto,*,Auto；底栏为无名 Border（不引入新命名控件，UI 一致性测试的名称集合不变），白底 + 上边框 1px（DynamicResource IxGray1Brush/IxBorderSoftBrush），文字 12px 柔和色、超长省略并带完整内容 ToolTip。顶栏保留 logo 与语言/操作按钮。验证：编译 0 错误、134 项 UI 检查通过，渲染图确认状态文字位于窗口底部。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 信息日志改到底部显示 | 修改 | 主网格 Auto,* → Auto,*,Auto；toolStrip_ServiceStatus 从顶栏移至新增底部状态栏（无名 Border，iX 资源配色，12px 柔和文字+省略+ToolTip） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\frmMain.axaml | 用户要求信息日志不显示在顶端，改在底部；亦还原原版底部 Bar 布局 | 2026-10-25 17:38:00 |

## 选中行颜色调浅一档（2026-10-25）

应用户要求把所有表格与列表的选中行底色调浅：统一样式表 token IxSelectedColor 由 #EBF7F8 调浅为 #F3FBFC（同 petrol 色相），选中悬停 IxSelectedHoverColor 由 #D1FBFF 调浅为 #E8F7F9；DataGrid（含选中悬停/按压）与 ListBox.entityList 选中态均经 token 生效，选中行文字仍为深青 #005E66 保证可读。因前期已完成 token 单一来源化，本次仅改 Industrial.axaml token 块两行。验证：编译 0 错误、134 项 UI 检查通过；渲染像素统计新色约 14.1 万像素、旧精确值 0。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 选中行底色整体调浅 | 修改 | IxSelectedColor #EBF7F8→#F3FBFC、IxSelectedHoverColor #D1FBFF→#E8F7F9（token 块第 19-21 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Styles\Industrial.axaml | 用户要求选中行颜色稍浅；表格与列表经统一 token 一处生效 | 2026-10-25 17:52:00 |

## 选中行再调浅并修正 DataGrid/内容区配色接线（2026-10-25）

应用户要求把选中行再调浅一档：IxSelectedColor #F3FBFC→#F7FCFD、IxSelectedHoverColor #E8F7F9→#EEF9FA。排查中发现两处接线问题并修正：（1）此前覆盖的 DataGrid 选中键是 stock Avalonia Fluent 的键名，本项目实际用 FluentAvalonia 主题，其 DataGrid 行选中走 DataGridRowSelected*Brush+Opacity 组合键——经反查 FluentAvaloniaUI 2.4.1 程序集与 DataGridRowStyles.axaml 源码后全部替换为 FA 真实键名（Selected/SelectedHovered/SelectedUnfocused/SelectedHoveredUnfocused 四组 Brush+Opacity，另补 DataGridRowBackgroundBrush 行底色、DataGridRowHoveredBackgroundColor 悬停、DataGridColumnHeader*Brush 表头族、DataGridBorder）。以进程内实证收尾：程序化选中设备表首行后读取行模板 BackgroundRectangle 实际 Fill=#F7FCFD、Opacity=1，token 真正到达渲染元素（临时验证代码已还原）。（2）像素排查发现内容区大片 #F7F7F8 为 FluentAvalonia NavigationView 内容区默认背景盖住了 iX 灰画布，经窗体资源补 NavigationViewContentBackground=#EFF0F1 修正，8 页渲染 #F7F7F8 清零、灰画布铺满，海军蓝导航与 petrol 不变。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 选中行再调浅一档 | 修改 | IxSelectedColor #F3FBFC→#F7FCFD、IxSelectedHoverColor #E8F7F9→#EEF9FA | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Styles\Industrial.axaml | 用户要求再浅一点 | 2026-10-25 18:20:00 |
| 2 | DataGrid 选中键换成 FA 真实键名 | 修改 | 删除无效的 stock Avalonia 键（DataGridRowBackgroundSelected 等 8 个），替换为 FluentAvalonia 实际使用的 DataGridRowSelected* 四组 Brush+Opacity、DataGridRowBackgroundBrush、DataGridRowHoveredBackgroundColor、DataGridColumnHeader*Brush、DataGridBorder（第 108-127 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Styles\Industrial.axaml | 原 stock 键在 FA 主题下不生效，选中行一直渲染 FA 默认 #F7F7F8 | 2026-10-25 18:20:00 |
| 3 | 内容区画布修正为 iX 灰 | 修改 | 窗体资源补 NavigationViewContentBackground=#EFF0F1（第 16-17 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\frmMain.axaml | FA 内容区默认 #F7F7F8 盖住灰画布 | 2026-10-25 18:20:00 |

## 重新发布 linux-x64 独立包（2026-10-25）

用户在 Ubuntu Desktop 22 双击 topoview.exe（Windows PE 产物）无法运行。按 README 既定命令重新发布自包含 linux-x64 包，纳入当日全部改动（Python 高亮、iX 主题、底部状态栏、导航与 DataGrid 修正）：dotnet publish -c Release -r linux-x64 --self-contained true -o artifacts/publish-net8/linux-x64。验证：输出 433 个文件、约 163MB，TopoView 为原生 ELF（魔数校验通过），libcoreclr/libhostfxr/libe_sqlite3/libHarfBuzzSharp 等 Linux 原生库齐全；另打包 artifacts/publish-net8/topoview-linux-x64.tar.gz（68.5MB）供传输（tar 保留可执行位，直接复制文件夹经 FAT/exFAT U 盘会丢失 +x）。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 重新发布 Linux 包并打包传输归档 | 增加 | linux-x64 目录 433 文件/163MB 重建（原包为 10 月 16 日旧版）；新增 topoview-linux-x64.tar.gz 传输归档 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\artifacts\publish-net8\linux-x64、topoview-linux-x64.tar.gz | Windows .exe 无法在 Ubuntu 运行，需 ELF 自包含包；旧包不含当日改动 | 2026-10-25 19:31:00 |

## Linux 版图标修复：窗口图标 + 桌面集成（2026-10-25）

Linux 无“可执行文件内嵌图标”机制（Windows .exe 的图标是 PE 资源，csproj 的 ApplicationIcon 只对 PE 生效），因此 Linux 包需要两件事：（1）运行时窗口/任务栏图标——frmMain 构造函数从程序集资源加载 2.ico 设置 Window.Icon（Windows 同样受益，此前窗口图标实际也未设置）；（2）文件管理器/应用菜单图标——遵循 Linux 桌面标准，随包分发 TopoView.desktop（模板含 @INSTALL_DIR@ 占位）与安装脚本（装入 desktop 条目 + hicolor 64x64 图标并替换 Exec 为实际路径，免 root、仅当前用户），图标 PNG 自 2.ico 最大帧 64×64 提取转换。csproj 以 Content+Link 平铺到 linux-x64 发布根目录（Condition RuntimeIdentifier=linux-x64，Windows 包不受影响）。已重新发布并重打 tar 包，三个集成文件验证在包内。

| 序号 | 修改或增加代码的目的 | 修改类型 | 说明 | 修改的文件 | 修改原因 | 修改时间 |
|---:|---|---|---|---|---|---|
| 1 | 运行时窗口图标 | 修改 | frmMain 构造函数经 AssetLoader 加载 avares://TopoView/Assets/2.ico 设置 Window.Icon（第 25-33 行） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\frmMain.axaml.cs | ApplicationIcon 仅写 PE 资源，Linux ELF 无内嵌机制，运行时需显式设置 | 2026-10-25 22:04:00 |
| 2 | Linux 桌面集成文件 | 增加 | linux/TopoView.desktop 模板（中英文名、StartupWMClass）与 linux/ 安装脚本（desktop 条目 + hicolor 图标，免 root） | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\linux\ | Linux 文件管理器/应用菜单图标依赖 .desktop 与图标主题目录 | 2026-10-25 22:04:00 |
| 3 | 图标 PNG 提取 | 增加 | 自 2.ico 最大帧（64×64，DIB）转出 | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\Assets\topoview.png | hicolor 图标需要 PNG/SVG | 2026-10-25 22:04:00 |
| 4 | 发布集成与重打包 | 修改 | csproj 增加 linux-x64 条件的 Content+Link（平铺到发布根）；重发 publish-net8/linux-x64 与 topoview-linux-x64.tar.gz | E:\10Work\17Code\018HiTopo\HiTopoGIt\TopoData\TopoView\TopoView.csproj、artifacts\publish-net8\ | 交付即含桌面集成 | 2026-10-25 22:04:00 |
