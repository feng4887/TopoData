// Recipe parameter editor corresponding to TopoData.SubPage.diaRecipeTagSelect.
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class diaRecipeTagSelect : Window
{
    // 原下拉框固定六项；DataTypeString 的其它取值在回显已有配方参数时也要可用
    private static readonly string[] DataTypeItems =
        ["bool", "int", "float", "string", "wstring", "datetime", "byte", "int16", "word", "double", "int64"];

    private readonly WorkspaceProfile? _profile;
    private readonly recipe_data? _original;
    private readonly bool _multiSelect;

    public string TagID { get => tbTagName.Text ?? ""; set => tbTagName.Text = value; }
    public string UOM { get => tbUOM.Text ?? ""; set => tbUOM.Text = value; }
    public string DataType { get => cbDataType.SelectedItem as string ?? ""; set => cbDataType.SelectedItem = value; }
    public string Atribute { get => tbAtribute.Text ?? ""; set => tbAtribute.Text = value; }
    public string DefaultValue { get; set; } = "";
    public List<DBTag> tags = new();
    public bool isInternalValue { get; set; }

    public diaRecipeTagSelect()
    {
        InitializeComponent();
        cbDataType.ItemsSource = DataTypeItems;
        UiText.Apply(this);
    }

    public diaRecipeTagSelect(WorkspaceProfile profile, recipe_data? value = null,
        bool multiSelect = false) : this()
    {
        _profile = profile;
        _original = value;
        _multiSelect = multiSelect;
        Atribute = value?.parameter_name ?? "";
        TagID = value?.tag_name ?? "";
        UOM = value?.uom ?? "";
        DataType = value?.data_type ?? "";
        tbDefaultValue.Text = value?.value ?? "";
        DefaultValue = value?.value ?? "";
    }

    /// <summary>Mirrors btSelect_Click: one tag fills the fields, many tags only join their names.</summary>
    private async void btSelect_Click(object? sender, RoutedEventArgs e)
    {
        if (_profile == null) return;
        var diaTagSelect = new diaTagSelect(_profile, _multiSelect);
        await diaTagSelect.ShowDialog((Window)TopLevel.GetTopLevel(this)!);
        tags = diaTagSelect.tags;
        if (diaTagSelect.tags.Count == 1)
        {
            if (!string.IsNullOrEmpty(diaTagSelect.TagName))
            {
                tbTagName.Text = diaTagSelect.TagName;
                tbUOM.Text = diaTagSelect.UOM;
                cbDataType.SelectedItem = diaTagSelect.DataType;
            }
            tbAtribute.IsEnabled = true;
        }
        else if (diaTagSelect.tags.Count > 1)
        {
            tbTagName.Text = string.Join(";", tags.Select(tag => tag.TagName));
            tbAtribute.Text = "";
            tbAtribute.IsEnabled = false;
            tbUOM.Text = "";
            cbDataType.SelectedItem = null;
        }
        else
        {
            tags.Clear();
            tbAtribute.IsEnabled = true;
        }
    }

    private void btCancel_Click(object? sender, RoutedEventArgs e) => Close();

    /// <summary>Mirrors btOK_Click: original validation order, messages and default-value parsing.</summary>
    private async void btOK_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tbAtribute.Text) && tags.Count == 1)
            {
                await Ui.MessageAsync(this, "提示", "数据表字段名不能为空");
                return;
            }
            if (Regex.IsMatch(tbAtribute.Text ?? "", @"^\d") && tags.Count == 1)
            {
                await Ui.MessageAsync(this, "提示", "字段不能以数字开头");
                return;
            }
            if (string.IsNullOrWhiteSpace(DataType) && tags.Count == 1)
            {
                await Ui.MessageAsync(this, "提示", "请选择一个数据类型");
                return;
            }

            DefaultValue = tbDefaultValue.Text ?? "";
            Atribute = tbAtribute.Text ?? "";
            UOM = tbUOM.Text ?? "";

            // 原版在此按数据类型解析默认值，解析失败弹出异常信息
            _ = DataType switch
            {
                "bool" => bool.Parse(DefaultValue),
                "string" or "wstring" => (object)DefaultValue,
                "byte" => byte.Parse(DefaultValue),
                "int" => int.Parse(DefaultValue),
                "float" => float.Parse(DefaultValue),
                "double" => double.Parse(DefaultValue),
                _ => float.Parse(DefaultValue)
            };

            Close(new recipe_data
            {
                parameter_name = Atribute,
                tag_name = TagID,
                uom = UOM,
                data_type = DataType,
                value = DefaultValue,
                description = _original?.description ?? ""
            });
        }
        catch (Exception ex)
        {
            await Ui.MessageAsync(this, "提示", ex.Message);
        }
    }
}
