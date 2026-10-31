// Field editor corresponding to TopoData.SubPage.diaColumnSelect.
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class diaColumnSelect : Window
{
    // 原下拉框固定六项；DataTypeString 的其余取值在选中点位后也要能回显，故并入列表
    private static readonly string[] DataTypeItems =
        ["bool", "int", "float", "string", "wstring", "datetime", "byte", "int16", "word", "double", "int64"];

    private readonly WorkspaceProfile? _profile;
    private readonly bool _multiSelect;
    private readonly DSTbDefine? _original;

    public string TagID { get => tbTagName.Text ?? ""; set => tbTagName.Text = value; }
    public string UOM { get => tbUOM.Text ?? ""; set => tbUOM.Text = value; }
    public string DataType { get => cbDataType.SelectedItem as string ?? ""; set => cbDataType.SelectedItem = value; }
    public string Atribute { get => tbAtribute.Text ?? ""; set => tbAtribute.Text = value; }
    public string DefaultValue { get; set; } = "";
    public List<DBTag> tags = new();
    public bool isInternalValue { get; set; }

    public diaColumnSelect()
    {
        InitializeComponent();
        cbDataType.ItemsSource = DataTypeItems;
        UiText.Apply(this);
        // 对应 diaColumnSelect_Load：默认值输入框初始禁用，单选按钮切换启用状态
        tbDefaultValue.IsEnabled = false;
    }

    public diaColumnSelect(WorkspaceProfile profile, DSTbDefine? value = null, bool multiSelect = false) : this()
    {
        _profile = profile;
        _multiSelect = multiSelect;
        _original = value;
        Atribute = value?.Column ?? "";
        TagID = value == null ? "" : ConfigurationStore.ResolveTagId(value.TagID, value.TagName, value.CannelID);
        UOM = value?.UOM ?? "";
        DataType = value?.DataType ?? "";
        tbDefaultValue.Text = value?.DefaultValue ?? "";
        isInternalValue = value?.isInternalValue == true;
        if (isInternalValue)
        {
            rbDevice.IsChecked = false;
            rbInternal.IsChecked = true;
        }
        DefaultValue = value?.DefaultValue ?? "";
    }

    /// <summary>Mirrors btSelect_Click: pick one or many tags and mirror the field states.</summary>
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

    /// <summary>Mirrors btOK_Click: the original validation order, messages and default-value parsing.</summary>
    private async void btOK_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tbAtribute.Text) && tags.Count == 1)
            {
                await Ui.MessageAsync(this, "提示", "数据表字段名不能为空");
                return;
            }
            var isNumeric = Regex.IsMatch(tbAtribute.Text ?? "", @"^\d");
            if (isNumeric && tags.Count == 1)
            {
                await Ui.MessageAsync(this, "提示", "字段不能以数字开头");
                return;
            }
            if (string.IsNullOrWhiteSpace(DataType) && tags.Count == 1)
            {
                await Ui.MessageAsync(this, "提示", "请选择一个数据类型");
                return;
            }
            if (rbInternal.IsChecked != true && string.IsNullOrEmpty(tbTagName.Text))
            {
                await Ui.MessageAsync(this, "提示", "通讯点位不能为空");
                return;
            }
            if (rbInternal.IsChecked == true && string.IsNullOrEmpty(tbDefaultValue.Text))
            {
                await Ui.MessageAsync(this, "提示", "内存默认值不能为空");
                return;
            }

            DefaultValue = tbDefaultValue.Text ?? "";
            isInternalValue = rbInternal.IsChecked == true;
            UOM = tbUOM.Text ?? "";
            Atribute = tbAtribute.Text ?? "";

            if (rbInternal.IsChecked == true)
            {
                // 原版按数据类型解析默认值，解析失败即提示异常并停留在对话框
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
            }

            Close(new DSTbDefine
            {
                Column = Atribute,
                TagID = isInternalValue ? "" : TagID,
                TagName = isInternalValue ? "" : _profile?.Tags
                    .FirstOrDefault(tag => tag.TagId == TagID)?.TagName ?? "",
                CannelID = isInternalValue ? "" : _profile?.Tags
                    .FirstOrDefault(tag => tag.TagId == TagID)?.CannelID ?? "",
                DataType = DataType,
                UOM = UOM,
                isInternalValue = isInternalValue,
                DefaultValue = DefaultValue,
                IsNullable = _original?.IsNullable ?? true,
                MaxLength = _original?.MaxLength
            });
        }
        catch (Exception ex)
        {
            await Ui.MessageAsync(this, "提示", ex.Message);
        }
    }

    /// <summary>Mirrors rbDevice_CheckedChanged: swap the tag picker and default value inputs.</summary>
    private void rbDevice_CheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (btSelect == null) return;
        if (rbDevice.IsChecked == true)
        {
            btSelect.IsEnabled = true;
            tbTagName.IsEnabled = true;
            tbDefaultValue.IsEnabled = false;
        }
        else
        {
            btSelect.IsEnabled = false;
            tbTagName.IsEnabled = false;
            tbDefaultValue.IsEnabled = true;
        }
    }
}
