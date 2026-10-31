// Historical column appearance comes from AXAML; this adapter applies the queried table schema.
using System.Data;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace TopoView.Views;

public sealed class HistoryColumnTemplate : ITemplate
{
    // DataGridColumn is not a Control, so the deferred result type must be explicit.
    [Content]
    [TemplateContent(TemplateResultType = typeof(DataGridColumn))]
    public object? Content { get; set; }

    public DataGridTextColumn Build()
    {
        var column = TemplateContent.Load<DataGridColumn>(Content)?.Result;
        return column as DataGridTextColumn
            ?? throw new InvalidOperationException("历史结果列模板必须声明 DataGridTextColumn。");
    }

    object ITemplate.Build() => Build();
}

public sealed class HistoryColumns : AvaloniaObject
{
    public static readonly AttachedProperty<DataTable?> SourceProperty =
        AvaloniaProperty.RegisterAttached<HistoryColumns, DataGrid, DataTable?>("Source");

    public static readonly AttachedProperty<HistoryColumnTemplate?> ColumnTemplateProperty =
        AvaloniaProperty.RegisterAttached<HistoryColumns, DataGrid, HistoryColumnTemplate?>("ColumnTemplate");

    static HistoryColumns()
    {
        SourceProperty.Changed.AddClassHandler<DataGrid>((grid, _) => ApplyTable(grid));
        ColumnTemplateProperty.Changed.AddClassHandler<DataGrid>((grid, _) => ApplyTable(grid));
    }

    public static DataTable? GetSource(DataGrid grid) => grid.GetValue(SourceProperty);

    public static void SetSource(DataGrid grid, DataTable? source) => grid.SetValue(SourceProperty, source);

    public static HistoryColumnTemplate? GetColumnTemplate(DataGrid grid) => grid.GetValue(ColumnTemplateProperty);

    public static void SetColumnTemplate(DataGrid grid, HistoryColumnTemplate? template)
        => grid.SetValue(ColumnTemplateProperty, template);

    private static void ApplyTable(DataGrid grid)
    {
        var table = GetSource(grid);
        if (table == null)
        {
            grid.ItemsSource = null;
            grid.Columns.Clear();
            return;
        }

        var template = GetColumnTemplate(grid)
            ?? throw new InvalidOperationException("请在 AXAML 中声明历史结果列模板。");
        var resultColumns = new List<DataGridTextColumn>();
        foreach (DataColumn schemaColumn in table.Columns)
        {
            var column = template.Build();
            column.Header = schemaColumn.ColumnName;
            // ItemArray avoids DataRowView's ambiguous int/string indexers and permits any column name.
            column.Binding = new Binding("Row.ItemArray[" + schemaColumn.Ordinal.ToString(CultureInfo.InvariantCulture) + "]")
            {
                Mode = BindingMode.OneWay
            };
            resultColumns.Add(column);
        }

        grid.ItemsSource = null;
        grid.Columns.Clear();
        foreach (var column in resultColumns) grid.Columns.Add(column);
        grid.ItemsSource = table.DefaultView;
    }
}
