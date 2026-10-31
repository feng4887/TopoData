// Expression evaluation corresponding to TopoData.SubPage.diaExpressCheck.
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Flee.PublicTypes;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class diaExpressCheck : Window
{
    private string _expression = "";
    private readonly ObservableCollection<ExpressionValueRow> _rows = [];

    public string ExpressionText { get => _expression; set => _expression = value; }
    public List<DBTag> ExpressionTags { get; set; } = [];

    public diaExpressCheck() { InitializeComponent(); dataGridView1.ItemsSource = _rows; UiText.Apply(this); }

    public diaExpressCheck(ExpressionResult result) : this()
    {
        _expression = result.Expression;
        ExpressionTags = result.Bindings;
        // 对应 diaExpressCheck_Load：行初始值取点位 DefaultValue（通常为空，等待用户输入）
        foreach (var tag in result.Bindings)
            _rows.Add(new ExpressionValueRow
            {
                Alias = tag.TagName,
                Value = tag.DefaultValue ?? "",
                DataType = DBTag.DataTypeString(tag.DataType)
            });
    }

    /// <summary>Compiles and evaluates the expression with the entered values (FleeOperator.ConditionIsOK).</summary>
    public bool Evaluate()
    {
        dataGridView1.CommitEdit();
        var context = new ExpressionContext();
        foreach (var row in _rows)
            context.Variables.Add(row.Alias, ParseRow(row));
        return context.CompileGeneric<bool>(_expression).Evaluate();
    }

    private static object ParseRow(ExpressionValueRow row) => row.DataType switch
    {
        "bool" => bool.Parse(row.Value),
        "string" or "wstring" => row.Value,
        "byte" => byte.Parse(row.Value),
        "int" => int.Parse(row.Value),
        "float" => float.Parse(row.Value),
        "double" => double.Parse(row.Value),
        _ => float.Parse(row.Value)
    };

    /// <summary>Mirrors btCheck_Click: per-row value check then the original result messages.</summary>
    private async void btCheck_Click(object? sender, RoutedEventArgs e)
    {
        dataGridView1.CommitEdit();
        try
        {
            foreach (var row in _rows)
            {
                if (string.IsNullOrEmpty(row.Value))
                {
                    await Ui.MessageAsync(this, "Warning", $"[{row.Alias}] needs a input value.");
                    return;
                }
                ParseRow(row);
            }

            var result = Evaluate();
            await Ui.MessageAsync(this, "Information", $"Expression Good! Result is [{result}]");
        }
        catch (Exception ex)
        {
            await Ui.MessageAsync(this, "Error", "Expression Bad :( \r\n" + ex.Message);
        }
    }

    private void btClose_Click(object? sender, RoutedEventArgs e) => Close();
}

public sealed class ExpressionValueRow
{
    public string Alias { get; set; } = "";
    public string Value { get; set; } = "";
    public string DataType { get; set; } = "";
}
