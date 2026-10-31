// Realtime diagnostics view; AXAML owns the controls and runtime owns the data.
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using auDASLib;
using auDAServer;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class ucDiagnose : BusinessPage
{
    private readonly ObservableCollection<RealtimeRow> _realtimeRows = [];
    private readonly DispatcherTimer timer1 = new()
        { Interval = TimeSpan.FromMilliseconds(1000) };

    private string _strFilter = "";

    public ucDiagnose()
    {
        InitializeComponent();
        gridControl1.ItemsSource = _realtimeRows;
        UiText.Apply(this);
        timer1.Tick += (_, _) => UpdateRows();
        AttachedToVisualTree += (_, _) =>
        {
            SyncQualityBrushesWithTheme();
            if (!HasRuntimeContext) return;
            timer1.Start();
            Refresh();
        };
        DetachedFromVisualTree += (_, _) => timer1.Stop();
    }

    /// <summary>质量红绿灯颜色跟随 Industrial.axaml 的 iX 状态色 token（单一来源）。</summary>
    private void SyncQualityBrushesWithTheme()
    {
        if (TryGetResource("IxSuccessBrush", ActualThemeVariant, out var good) && good is IBrush goodBrush &&
            TryGetResource("IxWarningBrush", ActualThemeVariant, out var poor) && poor is IBrush poorBrush)
            RealtimeRow.ApplyThemeBrushes(goodBrush, poorBrush);
    }

    public ucDiagnose(AppContextServices context) : this()
    {
        InitializeRuntime(context);
    }

    /// <summary>Mirrors cbRecipes_QueryPopUp: the dropdown lists only InUse recipes.</summary>
    public override void Refresh()
    {
        if (!HasRuntimeContext) return;
        var selectedRecipe = (cbRecipes.SelectedItem as RecipeEntry)?.Recipe;
        var recipes = Context.Profile.Recipes.equipment_recipes
            .Where(recipe => recipe.recipe_status == RecipeStatus.InUse)
            .Select(recipe => new RecipeEntry(recipe)).ToList();
        cbRecipes.ItemsSource = recipes;
        cbRecipes.SelectedItem = recipes.FirstOrDefault(entry => entry.Recipe == selectedRecipe);
        UpdateRows();
    }

    /// <summary>Mirrors timer1_Tick: enabled channels, filtered tag list, live value pool rows.</summary>
    private void UpdateRows()
    {
        if (!HasRuntimeContext) return;
        try
        {
            var cannels = Context.Profile.Server.cDevices
                .Where(it => it.iActive == 1).Select(x => x.CannelID).ToList();
            if (cannels.Count == 0) return;

            var tagIds = new List<string>();
            foreach (var cannel in cannels)
                if (DataImport.dicCannelTags.TryGetValue(cannel, out var tags))
                    tagIds.AddRange(tags.Select(it => it.TagId));

            if (tagIds.Count == 0) return;

            tagIds = FilterTagIDs(tagIds, _strFilter);
            if (tagIds.Count == 0) return;

            var rtvs = new List<RealTimeValue>();
            foreach (var item in tagIds)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if (ItemPool.ValuePool.TryGetValue(item, out var realTimeValue) &&
                    realTimeValue != null)
                    rtvs.Add(realTimeValue);
            }

            UpdateRealtimeValues(rtvs);
        }
        catch (Exception ex)
        {
            Context.Log(ex.Message);
        }
    }

    /// <summary>Mirrors UpdateRealtimeValues: refresh, remove and append rows without clearing.</summary>
    private void UpdateRealtimeValues(List<RealTimeValue> newValues)
    {
        var newDict = newValues.ToDictionary(v => v.ValueName, v => v);

        for (var i = _realtimeRows.Count - 1; i >= 0; i--)
        {
            if (newDict.TryGetValue(_realtimeRows[i].Name, out var newVal))
            {
                _realtimeRows[i].Update(newVal);
                newDict.Remove(_realtimeRows[i].Name);
            }
            else
            {
                _realtimeRows.RemoveAt(i);
            }
        }

        foreach (var newVal in newDict.Values)
            _realtimeRows.Add(new RealtimeRow(newVal));
    }

    /// <summary>Mirrors gridView1_DoubleClick: dialog write, then submit through the acquisition service.</summary>
    private async Task WriteAsync()
    {
        if (gridControl1.SelectedItem is not RealtimeRow row) return;

        var fw = new diaWrite { TagName = row.Name, UOM = row.Unit };
        if (!await fw.ShowDialog<bool>((Window)TopLevel.GetTopLevel(this)!)) return;

        var count = await Context.Acquisition.WriteAsync([(row.Name, fw.strValue)]);
        await Ui.MessageAsync(this, "Information", "Parameters downloaded.");
    }

    /// <summary>Mirrors btDownloadRecipe_Click: recipe lookup, InUse notice and parameter download.</summary>
    private async Task DownloadAsync()
    {
        var selectedRecipe = (cbRecipes.SelectedItem as RecipeEntry)?.Recipe.equipment_name;
        if (string.IsNullOrEmpty(selectedRecipe))
        {
            await Ui.MessageAsync(this, "提示", "请选择一个配方进行下载。");
            return;
        }

        var recipe = Context.Profile.Recipes.equipment_recipes?.FirstOrDefault(r =>
            string.Equals(r.equipment_id, selectedRecipe, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(r.equipment_name, selectedRecipe, StringComparison.OrdinalIgnoreCase));

        if (recipe == null)
        {
            await Ui.MessageAsync(this, "提示", $"Recipe '{selectedRecipe}' not found.");
            return;
        }

        // TopoData 的服务端（RecipeController.DownloadRecipe）会拒绝非 InUse 配方，这里保持一致不写入。
        if (recipe.recipe_status != RecipeStatus.InUse)
        {
            await Ui.MessageAsync(this, "提示",
                $"Recipe '{selectedRecipe}' Status is not Active.");
            return;
        }

        // 原版通过 POST /api/Recipe/DownloadRecipe 交服务端下发；本工程把参数直接提交设备写入。
        // 服务端会跳过空 tag_name 的参数项，这里保持一致。
        await Context.Acquisition.WriteAsync(recipe.recipe_data
            .Where(parameter => !string.IsNullOrWhiteSpace(parameter.tag_name))
            .Select(parameter => (parameter.tag_name, parameter.value)));
        await Ui.MessageAsync(this, "Information", "Recipe parameters downloaded.");
    }

    /// <summary>Mirrors btTnFilter_Click: the filter text applies on the next timer tick.</summary>
    private void btTnFilter_Click(object? sender, RoutedEventArgs e) => _strFilter = tbTnFilter.Text ?? "";

    /// <summary>Mirrors TopoData PubFunction.FilterTagIDs: multi-keyword AND contains, ignore case.</summary>
    private static List<string> FilterTagIDs(List<string> tagIds, string strFilter)
    {
        var filter = strFilter?.Trim();
        if (string.IsNullOrEmpty(filter)) return tagIds;

        var terms = filter.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim()).Where(t => t.Length > 0).ToArray();
        if (terms.Length == 0) return tagIds;

        return tagIds
            .Where(id => terms.All(term => id?.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
            .ToList();
    }

    private async void btDownloadRecipe_Click(object? sender, RoutedEventArgs e) =>
        await RunActionAsync(sender, DownloadAsync);

    private async void gridView1_DoubleClick(object? sender, TappedEventArgs e) =>
        await RunActionAsync(sender, WriteAsync);

    private sealed record RecipeEntry(equipment_recipe Recipe)
    {
        public override string ToString() => Recipe.equipment_name;
    }
}

public sealed class RealtimeRow : INotifyPropertyChanged
{
    // TopoData ucDiagnose.Designer.cs 的 FormatConditionRuleIconSet（TrafficLights4）：
    // 实测质量 >= 75 渲染绿灯，其余橙灯。默认值与 Industrial.axaml 的
    // IxSuccessBrush / IxWarningBrush 同色；页面挂树后按统一样式表的实际资源刷新。
    internal static IBrush GoodQualityBrush { get; private set; } = new SolidColorBrush(Color.FromRgb(0x2C, 0x85, 0x00));
    internal static IBrush PoorQualityBrush { get; private set; } = new SolidColorBrush(Color.FromRgb(0xB8, 0x8D, 0x00));

    /// <summary>把统一样式表（Industrial.axaml）里的 iX 状态色注入质量红绿灯。</summary>
    internal static void ApplyThemeBrushes(IBrush good, IBrush poor)
    {
        GoodQualityBrush = good;
        PoorQualityBrush = poor;
    }

    public string Name { get; private set; } = "";
    public string Value { get; private set; } = "";
    public string Quality { get; private set; } = "";
    /// <summary>Quality 列模板排序用的数值（TopoData 按整型 Quality 排序）。</summary>
    public int QualityValue { get; private set; }
    public IBrush QualityBrush { get; private set; } = PoorQualityBrush;
    public string Timestamp { get; private set; } = "";
    public string Unit { get; private set; } = "";
    public event PropertyChangedEventHandler? PropertyChanged;

    public RealtimeRow(RealTimeValue value) => Update(value);

    public void Update(RealTimeValue value)
    {
        Name = value.ValueName ?? "";
        Value = Convert.ToString(value.RealValue, CultureInfo.InvariantCulture) ?? "";
        Quality = value.Quality.ToString();
        QualityValue = value.Quality;
        QualityBrush = value.Quality >= 75 ? GoodQualityBrush : PoorQualityBrush;
        Timestamp = value.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        Unit = value.uom ?? "";
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}
