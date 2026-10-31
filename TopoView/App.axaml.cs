// Owns the desktop window and its isolated application services.
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TopoView.Services;

namespace TopoView;

public sealed partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (!Design.IsDesignMode &&
            ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var context = new AppContextServices(Program.Arguments);
            desktop.MainWindow = new frmMain(context);
        }
        base.OnFrameworkInitializationCompleted();
    }
}
