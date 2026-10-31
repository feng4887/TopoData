# 第三方组件与原作者声明

TopoView 未改变旧 HiTopo / TopoData 自有代码的权属与免费版本限制。下面的清单不表示整个项目采用同一种许可证。

| 组件 | 使用方式与声明 |
|---|---|
| Avalonia / Avalonia.Desktop / Inter 字体包 | UI 框架使用 MIT；随包字体保留其自身字体许可 |
| FluentAvaloniaUI | MIT；现代 Fluent 主题与导航控件 |
| Avalonia.Controls.DataGrid | MIT；原生点表与结果列表 |
| Sharp7（随 `Runtime/hiServer.dll` 分发） | OPC UA / Profinet S7 / Modbus 驱动现由 `Runtime/hiServer.dll` 提供；其中 Sharp7 保留 Davide Nardella 2016 年版权及 **LGPL-3.0-or-later** 原文头部，源码在 `auDAServer/Driver/Sharp7/` |
| OPC UA helper（随 `hiServer.dll` 分发） | 来自现有服务端源码，保留 Siemens AG 2022 版权头；正式对外分发时应沿用原来源的授权文件 |
| `hiServer.dll` / `hiLib` / `Log4` | 由本仓库 `auDAServer` / `auDASLib` / `StdLog4` 源码构建的本地程序集（AnyCPU，已移除 OPC DA）；Python Read 解析器为 `Runtime/Scripts` 内的副本 |
| OPC Foundation .NET Standard Client、NModbus、MQTTnet、Flee、SqlSugar、ClosedXML、数据库客户端 | 通过 NuGet 引用；许可证及依赖以锁定版本包内元数据为准 |
| .NET、ASP.NET Core、Skia、SQLite 等运行时 | 自包含发布同时携带其运行时第三方声明，不应从发布目录移除 |

Sharp7 的许可证文本：[GNU LGPL v3](https://www.gnu.org/licenses/lgpl-3.0.html) 和 [GNU GPL v3](https://www.gnu.org/licenses/gpl-3.0.html)。商业分发应履行所使用组件的对应许可证要求；保留作者头部不代替其余分发义务。

直接 NuGet 版本固定于 `TopoView.csproj`；已解析的全部传递依赖见构建时 `obj/project.assets.json` 及发布后的 `TopoView.deps.json`。
