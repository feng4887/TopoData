//=============================================================================
// 对齐 TopoData：Public/AutoStart.cs（登录自启与桌面快捷方式）
// Description: 当前用户登录自启管理；不依赖 Windows COM（旧版为 IWshRuntimeLibrary
//              快捷方式），Windows 写启动文件夹 .cmd，Linux 写 autostart .desktop 项。
//=============================================================================
// Per-user desktop login startup without Windows COM or system-wide service changes.
using System.Reflection;
using System.Text;

namespace TopoView.Services;

/// <summary>
/// 登录自启与桌面快捷方式管理（对应旧版 AutoStart.Instance）。
/// </summary>
public static class DesktopStartupService
{
    private const string Marker = "Managed by TopoView";

    /// <summary>设置或取消登录自动启动（对应 AutoStart.SetAutoStart 的自启开关）。</summary>
    public static void SetEnabled(bool enabled, string configurationDirectory) =>
        SetEntry(StartupFolder(), enabled, configurationDirectory);

    /// <summary>Mirrors the legacy SetDesktopQuick call made when auto start is switched on.</summary>
    public static void SetDesktopShortcut(bool enabled, string configurationDirectory) =>
        SetEntry(DesktopFolder(), enabled, configurationDirectory);

    private static string StartupFolder()
    {
        var folder = OperatingSystem.IsWindows()
            ? Environment.GetFolderPath(Environment.SpecialFolder.Startup)
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "autostart");
        if (string.IsNullOrWhiteSpace(folder)) throw new IOException("无法确定当前用户的桌面自动启动目录。");
        return folder;
    }

    private static string DesktopFolder()
    {
        var folder = OperatingSystem.IsWindows()
            ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Desktop");
        if (string.IsNullOrWhiteSpace(folder)) throw new IOException("无法确定当前用户的桌面目录。");
        return folder;
    }

    private static void SetEntry(string folder, bool enabled, string configurationDirectory)
    {
        var entry = Path.Combine(folder, OperatingSystem.IsWindows() ? "TopoView.cmd" : "topoview.desktop");
        if (File.Exists(entry) && !File.ReadAllText(entry).Contains(Marker, StringComparison.Ordinal))
            throw new IOException("已存在同名启动项，未覆盖或删除，请先检查：" + entry);
        if (!enabled) { if (File.Exists(entry)) File.Delete(entry); return; }
        var process = Environment.ProcessPath ?? throw new IOException("无法确定程序启动路径。");
        var arguments = new List<string> { process };
        if (Path.GetFileNameWithoutExtension(process).Equals("dotnet", StringComparison.OrdinalIgnoreCase))
            arguments.Add(Assembly.GetExecutingAssembly().Location);
        arguments.AddRange(["--config-dir", Path.GetFullPath(configurationDirectory)]);
        Directory.CreateDirectory(folder);
        var text = OperatingSystem.IsWindows()
            ? "@echo off\r\nrem " + Marker + "\r\nstart \"\" " + string.Join(" ", arguments.Select(a => "\"" + a.Replace("%", "%%") + "\"")) + "\r\n"
            : "[Desktop Entry]\n# " + Marker + "\nType=Application\nName=TopoView\nTerminal=false\nExec=" +
                string.Join(" ", arguments.Select(DesktopQuote)) + "\nX-GNOME-Autostart-enabled=true\n";
        File.WriteAllText(entry, text, new UTF8Encoding(false));
    }

    private static string DesktopQuote(string text) => "\"" + text.Replace("\\", "\\\\").Replace("\"", "\\\"")
        .Replace("$", "\\$").Replace("`", "\\`").Replace("%", "%%") + "\"";
}
