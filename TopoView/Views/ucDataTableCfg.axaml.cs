// Sampling controls and actions correspond to TopoData.SubPage.ucDataTableCfg.
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using auDAServer;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class ucDataTableCfg : BusinessPage
{
    private readonly ObservableCollection<DSTbDefine> _sampleColumns = [];
    private DataSample? _currentTask;
    private string _expression = "";
    private List<DBTag> _expressionBindings = [];
    private int _loadedType;
    private int _loadedChoice;

    public ucDataTableCfg()
    {
        InitializeComponent();
        gridControl1.ItemsSource = _sampleColumns;
        radioGroup1.ItemsSource = new[] { "上升沿采集", "连续采集" };
        radioGroup1.SelectedIndex = 0;
        UiText.Apply(this);
    }

    public ucDataTableCfg(AppContextServices context) : this()
    {
        InitializeRuntime(context);
        Refresh();
    }

    public override void Refresh()
    {
        if (!HasRuntimeContext) return;
        _currentTask = Context.Profile.Server.dataSamples.FirstOrDefault();
        var task = _currentTask;
        tbTaskName.Text = task?.TaskName ?? "";
        Task1.Content = task?.TaskName ?? UiText.Translate("我的存储");
        tbTableName.Text = task?.TableName ?? "ProcessData";
        tbEquipment.Text = task?.EquipmentName ?? "";
        cbActive.IsChecked = task?.bActive == true;
        cbRealTimeTable.IsChecked = task?.bSave2ProcessTable == true;
        var trigger = task?.SampleTrig ?? new DataSampleTrig();
        _loadedType = trigger.Type;
        _loadedChoice = trigger.Type == 0 ? 0 : 1;
        radioGroup1.SelectedIndex = _loadedChoice;
        tbTagName1.Text = trigger.Type == 2 ? trigger.dataSampleTrig_OnTrueOnFalse.TagName : trigger.dataSampleTrig_OnTrue.TagName;
        tbTagName2.Text = trigger.dataSampleTrig_TimeWindow.TagName;
        tbTimeWindow.Text = trigger.dataSampleTrig_TimeWindow.TimeWindow.ToString();
        tbWoTagName.Text = task?.wo ?? "";
        cbExpress.IsChecked = trigger.bUseExpression;
        _expression = trigger.ExpressionText ?? "";
        _expressionBindings = trigger.ExpressionTags ?? [];
        _sampleColumns.Clear();
        foreach (var column in task?.Samples ?? [])
        {
            if (!column.isInternalValue && string.IsNullOrWhiteSpace(column.TagID) && !string.IsNullOrWhiteSpace(column.TagName))
                column.TagID = ConfigurationStore.ResolveTagId(null, column.TagName, column.CannelID);
            _sampleColumns.Add(column);
        }
        UpdateTrigger();
        UpdateExpressionState();
    }

    public override void Commit()
    {
        if (!HasRuntimeContext || Context.IsRunning) return;
        var name = tbTaskName.Text?.Trim() ?? "";
        if (_currentTask == null && name.Length == 0) return;
        // 原版 btSave_Click：表名与任务名任一为空都提示同一条消息
        if (name.Length == 0 || string.IsNullOrWhiteSpace(tbTableName.Text))
            throw new InvalidDataException("表名或者任务名不能为空。");
        DatabaseService.ValidateIdentifier(tbTableName.Text ?? "");
        // 原版直接 int.Parse 并只判断 <= 39（提示按 400ms 描述）
        if (!int.TryParse(tbTimeWindow.Text, out var period))
            throw new InvalidDataException("设定采样周期不能小于 400 ms");
        if (period <= 39) throw new InvalidDataException("设定采样周期不能小于 400 ms");
        var task = _currentTask ?? new DataSample();
        task.TaskName = name;
        task.TableName = tbTableName.Text!;
        task.EquipmentName = tbEquipment.Text ?? "";
        task.bActive = cbActive.IsChecked == true;
        task.bSave2ProcessTable = cbRealTimeTable.IsChecked == true;
        task.wo = tbWoTagName.Text ?? "";
        task.Samples = _sampleColumns.ToList();
        task.SampleTrig.Type = radioGroup1.SelectedIndex == _loadedChoice ? _loadedType : radioGroup1.SelectedIndex;
        task.SampleTrig.bUseExpression = cbExpress.IsChecked == true;
        task.SampleTrig.ExpressionText = _expression;
        task.SampleTrig.ExpressionTags = _expressionBindings;
        task.SampleTrig.dataSampleTrig_TimeWindow.TimeWindow = period;
        task.SampleTrig.dataSampleTrig_OnTrue.TagName = tbTagName1.Text ?? "";
        task.SampleTrig.dataSampleTrig_TimeWindow.TagName = tbTagName2.Text ?? "";
        if (task.SampleTrig.Type == 2) task.SampleTrig.dataSampleTrig_OnTrueOnFalse.TagName = tbTagName1.Text ?? "";
        ValidateTrigger(task);
        if (task.bActive) DatabaseService.ValidateTask(task);
        if (_currentTask == null) Context.Profile.Server.dataSamples.Add(task);
        _currentTask = task;
    }

    /// <summary>Mirrors SaveConfiguration trigger checks and messages, including the tag existence test.</summary>
    private void ValidateTrigger(DataSample task)
    {
        if (task.SampleTrig.bUseExpression) return;
        var tagNames = Context.Profile.Tags
            .Select(tag => tag.CannelID + "." + tag.TagName).ToList();
        var trigger = task.SampleTrig.Type == 0
            ? task.SampleTrig.dataSampleTrig_OnTrue.TagName
            : task.SampleTrig.dataSampleTrig_TimeWindow.TagName;
        if (string.IsNullOrEmpty(trigger))
        {
            throw new InvalidDataException(task.SampleTrig.Type == 0
                ? "表条件通讯点名不能为空。"
                : "条件通讯点名不能为空。");
        }
        if (!tagNames.Contains(trigger))
            throw new InvalidDataException("选择的通讯点不存在, 请检查通讯配置。");
    }

    private void radioGroup1_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (panel2 != null) UpdateTrigger();
    }

    /// <summary>Mirrors radioGroup1_SelectedIndexChanged: only the matching trigger panel is enabled.</summary>
    private void UpdateTrigger()
    {
        panel2.IsVisible = radioGroup1.SelectedIndex == 0;
        panel3.IsVisible = radioGroup1.SelectedIndex == 1;
    }

    /// <summary>Mirrors cbExpress_Properties_EditValueChanged: expression disables the trigger tag input.</summary>
    private void UpdateExpressionState()
    {
        var useExpression = cbExpress.IsChecked == true;
        tbTagName1.IsEnabled = !(useExpression && radioGroup1.SelectedIndex == 0);
        tbTagName2.IsEnabled = !(useExpression && radioGroup1.SelectedIndex == 1);
        btExpress.IsEnabled = useExpression;
    }

    private void cbExpress_Click(object? sender, RoutedEventArgs e) => UpdateExpressionState();

    private async void btSave_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, SaveChanges);

    /// <summary>Mirrors btTableCreate_Click: save first, then create or extend the table with the
    /// legacy success messages.</summary>
    private async void btTableCreate_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        Commit();
        if (_currentTask == null) throw new InvalidOperationException("请先配置存储任务。");
        Context.Save();
        var added = await Context.Database.SynchronizeTableAsync(_currentTask);
        if (added == -1) await Ui.MessageAsync(this, "Information", "创建表成功。");
        else if (added > 0) await Ui.MessageAsync(this, "Information", $" 创建了 {added} 个新列。");
    });

    private async void btTagSelect1_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender,
        () => SelectTagAsync(tbTagName1));
    private async void btTagSelect2_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender,
        () => SelectTagAsync(tbTagName2));

    /// <summary>Mirrors btTagSelect1/2_Click: no data type restriction on trigger tags.</summary>
    private async Task SelectTagAsync(TextBox target)
    {
        var tags = await TagDialogs.PickAsync(this, Context.Profile);
        if (tags.Count > 0 && !string.IsNullOrEmpty(tags[0].TagId)) target.Text = tags[0].TagId;
    }

    /// <summary>Mirrors btTagSelectWo_Click: the work-order tag must be a string type.</summary>
    private async void btTagSelectWo_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        var tags = await TagDialogs.PickAsync(this, Context.Profile);
        if (tags.Count == 0 || string.IsNullOrEmpty(tags[0].TagId)) return;
        var type = DBTag.DataTypeString(tags[0].DataType).ToLowerInvariant();
        if (type != "string" && type != "wstring")
            throw new InvalidDataException("工单标签类型错误, 只能是字符串类型。");
        tbWoTagName.Text = tags[0].TagId;
    });

    private async void btExpress_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        var result = await new diaExpress(Context.Profile, _expression, _expressionBindings)
            .ShowDialog<ExpressionResult>((Window)TopLevel.GetTopLevel(this)!);
        if (result == null) return;
        _expression = result.Expression; _expressionBindings = result.Bindings;
    });

    private async void btColumnAdd_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, AddColumnAsync);
    private async void btColumnUpdate_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, UpdateColumnAsync);

    /// <summary>Mirrors btColumnAdd_Click: one or many tags, duplicate and name checks, free column limit.</summary>
    private async Task AddColumnAsync()
    {
        // 与原版一致：读取对话框自身的属性（Atribute/TagID/tags），而不是返回值
        var dialog = new diaColumnSelect(Context.Profile, null, true);
        var accepted = await dialog.ShowDialog<DSTbDefine>((Window)TopLevel.GetTopLevel(this)!);

        if (_sampleColumns.Count > DACtrl.free_DataSampleColumns - 1)
            throw new InvalidDataException($"免费版最多支持添加 {DACtrl.free_DataSampleColumns} 列。");

        if (accepted == null) return;
        if (dialog.tags.Count <= 1)
        {
            AddColumn(dialog.Atribute, dialog.TagID, dialog.UOM, dialog.DataType,
                dialog.isInternalValue, dialog.DefaultValue, true);
            return;
        }

        foreach (var tag in dialog.tags)
        {
            var column = tag.TagName.StartsWith(tag.CannelID + ".")
                ? tag.TagName.Substring((tag.CannelID + ".").Length)
                : "";
            if (string.IsNullOrEmpty(tag.TagName)) return;
            if (_sampleColumns.Any(item => string.Equals(item.Column, column, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException($"此属性 {column} 不能重复添加");
            AddColumn(column, tag.TagName, tag.UOM, DBTag.DataTypeString(tag.DataType),
                dialog.isInternalValue, tag.DefaultValue, false);
        }
    }

    /// <summary>Adds one configured column after the original duplicate and name-prefix checks.</summary>
    private void AddColumn(string column, string tagId, string uom, string dataType,
        bool internalValue, string defaultValue, bool validateName)
    {
        if (validateName)
        {
            if (_sampleColumns.Any(item => string.Equals(item.Column, column, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException($"此属性 {column} 不能重复添加");
            if (PubFunction.StartsWithSpecialCharOrDigit(column))
                throw new InvalidDataException($" {column} - 不允许特殊字符或数字开头");
        }

        _sampleColumns.Add(new DSTbDefine
        {
            Column = column,
            TagID = tagId,
            UOM = uom,
            DataType = dataType,
            isInternalValue = internalValue,
            DefaultValue = defaultValue
        });
    }

    /// <summary>Mirrors btColumnUpdate_Click: keep the dialog values on the matching column row.</summary>
    private async Task UpdateColumnAsync()
    {
        if (gridControl1.SelectedItem is not DSTbDefine selected) return;
        var result = await new diaColumnSelect(Context.Profile, selected)
            .ShowDialog<DSTbDefine>((Window)TopLevel.GetTopLevel(this)!);
        if (result == null) return;

        var index = _sampleColumns.IndexOf(selected);
        _sampleColumns[index] = result;
    }

    /// <summary>Mirrors btColumnDelete_Click: confirm each selected column before removal.</summary>
    private async void btColumnDelete_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        foreach (var column in gridControl1.SelectedItems.Cast<DSTbDefine>().ToList())
        {
            if (!await Ui.ConfirmAsync(this, $"是否删除{column.Column}")) continue;
            _sampleColumns.Remove(column);
        }
    });

    /// <summary>Mirrors btRemoeDBDate_Click: confirm, drop the database column, then remove the row.</summary>
    private async void btRemoeDBDate_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        if (gridControl1.SelectedItem is not DSTbDefine column) return;
        Commit();
        if (_currentTask == null) return;
        if (!await Ui.ConfirmAsync(this, $"是否删除{column.Column} 及数据库表对应字段")) return;
        try
        {
            await Context.Database.DeleteSampleColumnAsync(_currentTask, column.Column);
            _sampleColumns.Remove(column);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("删除错误" + ex.Message, ex);
        }
    });
}
