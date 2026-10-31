// Fluent shell using TopoData frmMain page and action names.
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using TopoView.Services;
using TopoView.Views;

namespace TopoView;

public sealed partial class frmMain : Window
{
    private readonly Dictionary<string, BusinessPage> _pages = new();
    private readonly Dictionary<string, Control> _hosts = new();
    private readonly DispatcherTimer _statusTimer = new() { Interval = TimeSpan.FromSeconds(1) };
    private AppContextServices? _context;
    private string _activePage = "devices";
    private bool _isSwitchingPage;
    private bool _isSwitchingLanguage;
    private bool _canClose;
    private bool _isClosing;
    private AppContextServices Context => _context ?? throw new InvalidOperationException("主窗口尚未连接运行服务。");

    public frmMain()
    {
        InitializeComponent();
        // csproj 的 ApplicationIcon 只写进 Windows PE 资源；Linux ELF 无内嵌图标机制，
        // 窗口/任务栏图标必须在运行时从程序集资源加载（Windows 同样受益）。
        using var iconStream = Avalonia.Platform.AssetLoader.Open(new Uri("avares://TopoView/Assets/2.ico"));
        Icon = new WindowIcon(iconStream);
        UiText.Apply(this);
    }
    public frmMain(AppContextServices context) : this()
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
        _pages.Add("devices", ucCannelCfg2); _hosts.Add("devices", PageCannelCfg);
        _pages.Add("storage", ucDataTableCfg1); _hosts.Add("storage", PageDataTable);
        _pages.Add("recipes", ucRecipe); _hosts.Add("recipes", PageRecipe);
        _pages.Add("scripts", ucScripts1); _hosts.Add("scripts", PageScript);
        _pages.Add("diagnostics", ucDiagnose1); _hosts.Add("diagnostics", PageDiagnose);
        _pages.Add("history", ucHisQuery1); _hosts.Add("history", PageHisQuery);
        _pages.Add("settings", ucSystemCfg1); _hosts.Add("settings", PageSysCfg);
        _pages.Add("about", ucAbout); _hosts.Add("about", PageAbout);
        foreach (var page in _pages.Values) page.InitializeRuntime(context);
        Context.Message += OnMessage; Context.StateChanged += OnStateChanged;
        _statusTimer.Tick += (_, _) => toolStrip_ServiceStatus.Text = Context.LastMessage;
        _statusTimer.Start();
        ApplyProfileLanguage();
        UpdateWidth();
        ShowPage("devices");
    }
    public void ShowPage(string key)
    {
        if (_context == null || !_pages.TryGetValue(key, out var page)) return;
        _activePage = key;
        SelectNavigationItem(key);
        page.Refresh();
        foreach (var host in _hosts) host.Value.IsVisible = host.Key == key;
        OnStateChanged();
    }
    public void ReloadProfilePages()
    {
        if (_context == null) return;
        ApplyProfileLanguage();
        foreach (var page in _pages.Values) page.Refresh();
        ShowPage(_activePage);
    }
    private void SelectNavigationItem(string key)
    {
        _isSwitchingPage = true;
        accordionControl1.SelectedItem = accordionControl1.MenuItems.OfType<NavigationViewItem>()
            .First(item => Equals(item.Tag, key));
        _isSwitchingPage = false;
    }
    private void ApplyProfileLanguage()
    {
        _isSwitchingLanguage = true;
        UiText.IsEnglish = Context.Profile.Server.Localization.StartsWith("en", StringComparison.OrdinalIgnoreCase);
        cbLanguage.SelectedIndex = UiText.IsEnglish ? 1 : 0;
        UiText.Apply(this);
        _isSwitchingLanguage = false;
    }
    private void UpdateWidth()
    {
        if (accordionControl1 == null) return;
        var compact = Width < 1200;
        accordionControl1.PaneDisplayMode = compact ? NavigationViewPaneDisplayMode.LeftCompact : NavigationViewPaneDisplayMode.Left;
        accordionControl1.IsPaneOpen = !compact;
    }
    private void OnMessage(string message) => Dispatcher.UIThread.Post(() => toolStrip_ServiceStatus.Text = message);
    private void OnStateChanged() => Dispatcher.UIThread.Post(() =>
    {
        if (_context == null) return;
        barButtonStart.IsEnabled = !Context.IsRunning;
        barButtonEnd.IsEnabled = Context.IsRunning;
        barButtonImport.IsEnabled = barButtonExport.IsEnabled = cbLanguage.IsEnabled = !Context.IsRunning;
        foreach (var page in _pages) page.Value.IsEnabled = !Context.IsRunning || page.Key is "diagnostics" or "history" or "about";
        toolStrip_ServiceStatus.Text = Context.LastMessage;
    });
    private async Task RunAsync(Func<Task> operation)
    {
        if (_context == null || Design.IsDesignMode) return;
        barButtonStart.IsEnabled = barButtonEnd.IsEnabled = false;
        try { await operation(); }
        catch (Exception error) { Context.Log(error.Message); await Ui.MessageAsync(this, "操作未完成", error.Message); }
        finally { OnStateChanged(); }
    }
    private async void barButtonStart_ItemClick(object? sender, RoutedEventArgs e) => await RunAsync(async () =>
    { _pages[_activePage].Commit(); await Context.StartAsync(); });
    private async void barButtonEnd_ItemClick(object? sender, RoutedEventArgs e) => await RunAsync(() => Context.StopAsync());
    private async void barButtonImport_ItemClick(object? sender, RoutedEventArgs e) => await RunAsync(async () =>
    {
        var path = await Ui.OpenAsync(this, "导入 TopoData 配置压缩包", "zip");
        if (path == null || !await Ui.ConfirmAsync(this, "导入将替换当前配置，原配置保留为 .bak。是否继续？")) return;
        Context.Import(path); ReloadProfilePages();
        await Ui.MessageAsync(this, "导入结果", Context.LastMessage);
    });
    private async void barButtonExport_ItemClick(object? sender, RoutedEventArgs e) => await RunAsync(async () =>
    {
        _pages[_activePage].Commit();
        var path = await Ui.SaveAsync(this, "导出配置", "TopoView-config.zip", "zip");
        if (path == null) return;
        Context.Save(); Context.Store.ExportZip(Context.Profile, path); Context.Notify("配置已导出。");
    });
    private void accordionControl1_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (_context == null || _isSwitchingPage || e.SelectedItem is not NavigationViewItem item || item.Tag is not string key) return;
        try { _pages[_activePage].Commit(); ShowPage(key); }
        catch (Exception error) { SelectNavigationItem(_activePage); _ = Ui.MessageAsync(this, "请检查当前页面", error.Message); }
    }
    private async void cbLanguage_EditValueChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_context == null || _isSwitchingLanguage || _pages.Count == 0) return;
        try
        {
            _pages[_activePage].Commit();
            Context.Profile.Server.Localization = cbLanguage.SelectedIndex == 1 ? "en" : "zh";
            // 原版语言切换会立即把 hiTopoCfg.xml 写回磁盘。
            Context.Save();
            ReloadProfilePages();
        }
        catch (Exception error)
        {
            _isSwitchingLanguage = true; cbLanguage.SelectedIndex = UiText.IsEnglish ? 1 : 0; _isSwitchingLanguage = false;
            await Ui.MessageAsync(this, "请检查当前页面", error.Message);
        }
    }
    private void frmMain_SizeChanged(object? sender, SizeChangedEventArgs e) => UpdateWidth();
    private async void frmMain_Opened(object? sender, EventArgs e)
    {
        if (_context != null && !string.IsNullOrEmpty(Context.StartupError)) await Ui.MessageAsync(this, "配置读取失败", Context.StartupError);
    }
    /// <summary>Commits edits and waits for owned runtime workers before closing.</summary>
    private async void frmMain_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (_canClose || _context == null) return;
        e.Cancel = true;
        if (_isClosing) return;
        _isClosing = true;
        try
        {
            var canDiscard = false;
            try { _pages[_activePage].Commit(); }
            catch (Exception exception)
            {
                if (!await Ui.ConfirmAsync(this, "当前编辑尚未通过检查：" +
                    exception.Message + "\n放弃当前编辑并退出？")) return;
                canDiscard = true;
            }
            if (!canDiscard && !Context.IsRunning && Context.HasChanges)
            {
                var dialog = new MessageDialog
                {
                    Title = "保存更改", PrimaryButtonText = "保存",
                    SecondaryButtonText = "不保存", CloseButtonText = "取消"
                };
                dialog.SetMessage("配置有未保存的修改，是否保存后退出？");
                var result = await dialog.ShowAsync(this);
                if (result == ContentDialogResult.None) return;
                if (result == ContentDialogResult.Primary) Context.Save();
            }
            if (Context.IsRunning && !await Ui.ConfirmAsync(this,
                "退出将停止当前采集、存储与脚本服务，是否继续？")) return;
            await Context.DisposeAsync();
            _canClose = true;
            _statusTimer.Stop();
            Context.Message      -= OnMessage;
            Context.StateChanged -= OnStateChanged;
            Close();
        }
        catch (Exception exception)
        {
            await Ui.MessageAsync(this, "退出未完成", exception.Message);
        }
        finally { _isClosing = false; }
    }
}
