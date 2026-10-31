// Expression editor corresponding to TopoData.SubPage.diaExpress.
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class diaExpress : Window
{
    private readonly WorkspaceProfile? _profile;
    private readonly ObservableCollection<ExpressionBindingRow> _bindings = [];

    public string ExpressionText { get => richTextBox1.Text ?? ""; set => richTextBox1.Text = value; }
    public List<DBTag> ExpressionTags => BuildTags();

    /// <summary>Loads the editor without opening the production profile.</summary>
    public diaExpress()
    {
        InitializeComponent();
        listView1.ItemsSource = _bindings;
        UiText.Apply(this);
    }

    public diaExpress(WorkspaceProfile profile, string expression, List<DBTag> bindings) : this()
    {
        _profile = profile;
        richTextBox1.Text = expression;
        foreach (var binding in bindings)
        {
            // 旧版把点位全名存于 Address、别名存于 TagName；保留该拆分，避免回存后丢失地址。
            var reference = string.IsNullOrWhiteSpace(binding.Address)
                ? ConfigurationStore.ResolveTagId(binding.TagId, binding.TagName, binding.CannelID)
                : binding.Address;
            _bindings.Add(new ExpressionBindingRow
            {
                Alias = binding.TagName,
                DataType = DBTag.DataTypeString(binding.DataType),
                TagId = reference
            });
        }
    }

    /// <summary>Mirrors button2_Click: add an intermediate variable through the column dialog.</summary>
    private async void btAddTag_Click(object? sender, RoutedEventArgs e)
    {
        if (_profile == null) return;
        var diaColumnSelect = new diaColumnSelect(_profile);
        await diaColumnSelect.ShowDialog<DSTbDefine>((Window)TopLevel.GetTopLevel(this)!);
        var at = diaColumnSelect.Atribute;
        var tn = diaColumnSelect.TagID;
        var dt = diaColumnSelect.DataType;

        if (string.IsNullOrEmpty(tn))
            return;

        if (Regex.IsMatch(at, @"^\d"))
        {
            await Ui.MessageAsync(this, "提示", "字段不能以数字开头");
            return;
        }

        if (!Regex.IsMatch(at, @"^[a-zA-Z0-9_]+$"))
        {
            await Ui.MessageAsync(this, "提示", "变量名只允许由字母数值和_组成");
            return;
        }

        foreach (var binding in _bindings)
        {
            if (at == binding.Alias)
            {
                await Ui.MessageAsync(this, "提示", "此属性不能重复添加");
                return;
            }
        }

        _bindings.Add(new ExpressionBindingRow { Alias = at, TagId = tn, DataType = dt });
    }

    /// <summary>Mirrors btSave_Click: rebuild the tag list from the rows and return OK.</summary>
    private void btSave_Click(object? sender, RoutedEventArgs e) =>
        Close(new ExpressionResult(richTextBox1.Text ?? "", BuildTags()));

    private void btClose_Click(object? sender, RoutedEventArgs e) => Close();

    /// <summary>Mirrors btRemoveTag_Click: remove the selected intermediate variables.</summary>
    private void btRemoveTag_Click(object? sender, RoutedEventArgs e)
    {
        foreach (var binding in listView1.SelectedItems.Cast<ExpressionBindingRow>().ToList())
            _bindings.Remove(binding);
    }

    /// <summary>Mirrors btSelectTag_Click: insert the selected alias at the caret.</summary>
    private void btSelectTag_Click(object? sender, RoutedEventArgs e)
    {
        if (listView1.SelectedItem is not ExpressionBindingRow row) return;
        var caret = richTextBox1.CaretIndex;
        richTextBox1.Text = (richTextBox1.Text ?? "").Insert(caret, " " + row.Alias + " ");
        richTextBox1.CaretIndex = caret + row.Alias.Length + 2;
    }

    /// <summary>Mirrors btCheck_Click: hand the built expression and tags to the check dialog.</summary>
    private async void btCheck_Click(object? sender, RoutedEventArgs e) =>
        await new diaExpressCheck(new ExpressionResult(richTextBox1.Text ?? "", BuildTags()))
            .ShowDialog(this);

    private async void btAblut_Click(object? sender, RoutedEventArgs e) =>
        await new diaExpressAbout().ShowDialog(this);

    /// <summary>Mirrors the btSave/btCheck tag rebuild: alias, address and parsed data type.</summary>
    private List<DBTag> BuildTags() =>
        _bindings.Select(row => new DBTag
        {
            TagName = row.Alias,
            TagId = row.TagId,
            Address = row.TagId,
            DataType = DBTag.GetDataType(row.DataType)
        }).ToList();
}

public sealed class ExpressionBindingRow
{
    public string Alias { get; set; } = "";
    public string TagId { get; set; } = "";
    public string DataType { get; set; } = "";
}
