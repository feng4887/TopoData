// Retains the TopoData GridColumn/ColumnHeader identifier in editable AXAML.
using Avalonia.Controls;

namespace TopoView.Views;

public sealed class LegacyTextColumn : DataGridTextColumn
{
    public string Name { get; set; } = "";
}

public sealed class LegacyTemplateColumn : DataGridTemplateColumn
{
    public string Name { get; set; } = "";
}
