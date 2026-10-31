// TopoView desktop entry point. Services start only after an explicit user action.
using Avalonia;

namespace TopoView;

internal static class Program
{
    public static string[] Arguments { get; private set; } = [];

    [STAThread]
    public static void Main(string[] args)
    {
        Arguments = args;
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UsePlatformDetect().WithInterFont().LogToTrace();
}
