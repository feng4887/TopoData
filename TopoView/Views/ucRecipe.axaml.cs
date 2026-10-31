// Recipe maintenance corresponding to TopoData.SubPage.ucRecipe.
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class ucRecipe : BusinessPage
{
    private readonly ObservableCollection<recipe_data> _parameters = [];
    private equipment_recipe? _currentRecipe;
    private bool _isLoading;

    /// <summary>Initializes declarative controls without accessing runtime services.</summary>
    public ucRecipe()
    {
        InitializeComponent();
        gridControl1.ItemsSource = _parameters;
        UiText.Apply(this);
    }

    public ucRecipe(AppContextServices context) : this()
    {
        InitializeRuntime(context);
        Refresh();
    }

    public override void Refresh()
    {
        if (!HasRuntimeContext) return;
        _isLoading = true;
        try
        {
            var recipes = Context.Profile.Recipes.equipment_recipes
                .Select(recipe => new RecipeEntry(recipe)).ToList();
            navBarControl1.ItemsSource = recipes;
            navBarControl1.SelectedItem = recipes.FirstOrDefault(
                entry => entry.Recipe == _currentRecipe) ?? recipes.FirstOrDefault();
            LoadRecipe((navBarControl1.SelectedItem as RecipeEntry)?.Recipe);
        }
        finally
        {
            _isLoading = false;
        }
    }

    public override void Commit()
    {
        if (!HasRuntimeContext) return;
        gridControl1.CommitEdit();
        if (_currentRecipe == null || Context.IsRunning) return;
        // 原版保存时不做名称或数值校验，只把面板内容写回配方对象
        _currentRecipe.equipment_name = tbTaskName.Text?.Trim() ?? "";
        _currentRecipe.equipment_id = _currentRecipe.equipment_name;
        _currentRecipe.description = tbDescription.Text ?? "";
        _currentRecipe.recipe_status = cbActive.IsChecked == true ? RecipeStatus.InUse : RecipeStatus.Unknown;
        _currentRecipe.recipe_data = _parameters.ToList();
        Context.Profile.Recipes.last_modify_time = DateTime.Now;
    }

    /// <summary>Mirrors NavBarControl1_LinkClicked: fill the panel from the clicked recipe.</summary>
    private void LoadRecipe(equipment_recipe? recipe)
    {
        _currentRecipe = recipe;
        tbTaskName.Text = recipe?.equipment_name ?? "";
        tbDescription.Text = recipe?.description ?? "";
        cbActive.IsChecked = recipe?.recipe_status == RecipeStatus.InUse;
        _parameters.Clear();
        foreach (var parameter in recipe?.recipe_data ?? []) _parameters.Add(parameter);
    }

    private void NavBarControl1_LinkClicked(object? sender, SelectionChangedEventArgs e)
    {
        if (!HasRuntimeContext || _isLoading) return;
        LoadRecipe((navBarControl1.SelectedItem as RecipeEntry)?.Recipe);
    }

    private async void barButtonAdd_ItemClick(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, AddRecipeAsync);

    private async void barButtonRemove_ItemClick(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, DeleteRecipeAsync);

    private async void barButtonItemSave_ItemClick(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, SaveRecipeAsync);

    /// <summary>Mirrors barButtonAdd_ItemClick: next MyRecipeN number, no name prompt.</summary>
    private async Task AddRecipeAsync()
    {
        if (!HasRuntimeContext) return;
        var next = NextRecipeNumber();
        var name = $"MyRecipe{next}";
        _currentRecipe = new equipment_recipe
        {
            equipment_id = name,
            equipment_name = name,
            description = "",
            recipe_data = []
        };
        Context.Profile.Recipes.equipment_recipes.Add(_currentRecipe);
        SaveChanges();
        Refresh();
        tbTaskName.Text = name;
    }

    /// <summary>Mirrors GetNextRecipeNumber: highest numeric suffix of MyRecipeN plus one.</summary>
    private int NextRecipeNumber()
    {
        var max = 0;
        foreach (var recipe in Context.Profile.Recipes.equipment_recipes)
        {
            var match = Regex.Match(recipe.equipment_id ?? "", @"MyRecipe(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var number))
                max = Math.Max(max, number);
        }
        return max + 1;
    }

    /// <summary>Mirrors barButtonRemove_ItemClick: confirm by item name, remove, then save at once.</summary>
    private async Task DeleteRecipeAsync()
    {
        if (_currentRecipe == null) return;
        var itemName = _currentRecipe.equipment_id;
        if (!await Ui.ConfirmAsync(this, $"是否删除{itemName}")) return;
        Context.Profile.Recipes.equipment_recipes.Remove(_currentRecipe);
        _currentRecipe = null;
        Context.Save();
        Refresh();
        tbTaskName.Text = "";
        tbDescription.Text = "";
        _parameters.Clear();
    }

    /// <summary>Mirrors barButtonItemSave_ItemClick: save, rename the entry and report success.</summary>
    private async Task SaveRecipeAsync()
    {
        Commit();
        if (!HasRuntimeContext) return;
        Context.Save();
        Refresh();
        await Ui.MessageAsync(this, "Information", "配方保存成功!");
    }

    private async void btColumnAdd_Click(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, AddParametersAsync);

    private async void btColumnUpdate_Click(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, UpdateParameterAsync);

    private async void btColumnDelete_Click(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, DeleteSelectedParametersAsync);

    /// <summary>Mirrors btColumnAdd_Click: one or many tags with duplicate and name checks.</summary>
    private async Task AddParametersAsync()
    {
        if (_currentRecipe == null) throw new InvalidOperationException("请先新增配方。");
        var dialog = new diaRecipeTagSelect(Context.Profile, null, true);
        var accepted = await dialog.ShowDialog<recipe_data>((Window)TopLevel.GetTopLevel(this)!);
        if (accepted == null) return;

        if (dialog.tags.Count <= 1)
        {
            AddParameter(dialog.Atribute, dialog.TagID, dialog.UOM, dialog.DataType, dialog.DefaultValue);
            return;
        }

        foreach (var tag in dialog.tags)
        {
            var parameter = tag.TagName.StartsWith(tag.CannelID + ".")
                ? tag.TagName.Substring((tag.CannelID + ".").Length)
                : "";
            if (string.IsNullOrEmpty(tag.TagName)) return;
            if (_parameters.Any(item => string.Equals(item.parameter_name, parameter, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException($"此属性 {parameter} 不能重复添加");
            AddParameter(parameter, tag.TagName, tag.UOM, DBTag.DataTypeString(tag.DataType), tag.DefaultValue);
        }
    }

    /// <summary>Adds one parameter after the original duplicate and leading-character checks.</summary>
    private void AddParameter(string parameter, string tagName, string uom, string dataType, string value)
    {
        if (_parameters.Any(item => string.Equals(item.parameter_name, parameter, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidDataException($"此属性 {parameter} 不能重复添加");
        if (PubFunction.StartsWithSpecialCharOrDigit(parameter))
            throw new InvalidDataException($" {parameter} - 不允许特殊字符或数字开头");

        _parameters.Add(new recipe_data
        {
            parameter_name = parameter,
            tag_name = tagName,
            uom = uom,
            data_type = dataType,
            value = value
        });
    }

    /// <summary>Mirrors btColumnUpdate_Click: keep the dialog values on the same row index.</summary>
    private async Task UpdateParameterAsync()
    {
        if (_currentRecipe == null) throw new InvalidOperationException("请先新增配方。");
        if (gridControl1.SelectedItem is not recipe_data selected)
            throw new InvalidOperationException("请选择要更新的参数。");
        var result = await new diaRecipeTagSelect(Context.Profile, selected)
            .ShowDialog<recipe_data>((Window)TopLevel.GetTopLevel(this)!);
        if (result == null || string.IsNullOrEmpty(result.tag_name)) return;
        _parameters[_parameters.IndexOf(selected)] = result;
    }

    /// <summary>Mirrors btColumnDelete_Click: the first selected row's name is confirmed once and
    /// every parameter with that name is removed.</summary>
    private async Task DeleteSelectedParametersAsync()
    {
        if (gridControl1.SelectedItem is not recipe_data selected) return;
        if (!await Ui.ConfirmAsync(this, $"是否删除{selected.parameter_name}")) return;
        foreach (var parameter in _parameters.Where(item => item.parameter_name == selected.parameter_name).ToList())
            _parameters.Remove(parameter);
    }

    private sealed record RecipeEntry(equipment_recipe Recipe)
    {
        public override string ToString() => Recipe.equipment_id;
    }
}
