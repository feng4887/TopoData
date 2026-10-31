// Shared runtime boundary for AXAML pages; preview construction never creates services.
using Avalonia.Controls;
using TopoView.Services;

namespace TopoView.Views;

public class BusinessPage : UserControl
{
    private AppContextServices? _context;

    protected bool HasRuntimeContext => _context != null;
    protected AppContextServices Context => _context ??
        throw new InvalidOperationException("当前页面尚未连接运行服务。");

    /// <summary>Attaches the caller-owned services after the AXAML has loaded.</summary>
    public void InitializeRuntime(AppContextServices context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public virtual void Commit() { }
    public virtual void Refresh() { }

    /// <summary>Runs an explicit UI action and reports recoverable operation failures.</summary>
    protected async Task RunActionAsync(object? sender, Func<Task> operation)
    {
        if (!HasRuntimeContext || Design.IsDesignMode) return;
        var button = sender as Button;
        var wasEnabled = button?.IsEnabled ?? false;
        if (button != null) button.IsEnabled = false;
        try { await operation(); }
        catch (Exception exception)
        {
            Context.Log(exception.Message);
            await Ui.MessageAsync(this, "操作未完成", exception.Message);
        }
        finally { if (button != null) button.IsEnabled = wasEnabled; }
    }

    protected Task RunActionAsync(object? sender, Action operation) =>
        RunActionAsync(sender, () => { operation(); return Task.CompletedTask; });

    protected void SaveChanges()
    {
        Commit();
        Context.Save();
    }
}
