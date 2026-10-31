// Shared tag selection and expression editor results.
using Avalonia.Controls;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public static class TagDialogs
{
    /// <summary>Shows the tag selector without changing the source configuration.</summary>
    public static async Task<List<DBTag>> PickAsync(
        Control owner,
        WorkspaceProfile profile,
        Func<DBTag, bool>? filter = null,
        bool multiSelect = false)
    {
        var dialog = new diaTagSelect(profile, multiSelect, filter);
        var result = await dialog.ShowDialog<List<DBTag>>((Window)TopLevel.GetTopLevel(owner)!);
        return result ?? [];
    }
}

public sealed record ExpressionResult(string Expression, List<DBTag> Bindings);
