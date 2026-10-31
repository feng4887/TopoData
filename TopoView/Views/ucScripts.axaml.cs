// Python script editing corresponding to TopoData.SubPage.ucScripts.
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class ucScripts : BusinessPage
{
    private string _triggerExpression = "";
    private List<DBTag> _expressionTags = [];
    private PythonScriptItem? _currentScript;
    private bool _isLoading;

    /// <summary>Initializes declarative controls without opening runtime resources.</summary>
    public ucScripts()
    {
        InitializeComponent();
        UiText.Apply(this);
        UpdateExpressionControlState();
        ConfigurePythonEditor();
    }

    public ucScripts(AppContextServices context) : this()
    {
        InitializeRuntime(context);
        Refresh();
    }

    /// <summary>
    /// Mirrors TopoData ucScripts.ConfigurePythonEditor: Python lexer colors,
    /// 4-space indent (spaces instead of tabs) and line numbers like the Scintilla margins.
    /// </summary>
    private void ConfigurePythonEditor()
    {
        memoEditScriptText.SyntaxHighlighting = PythonHighlighting.Definition;
        memoEditScriptText.Options.IndentationSize = 4;
        memoEditScriptText.Options.ConvertTabsToSpaces = true;
    }

    public override void Refresh()
    {
        if (!HasRuntimeContext) return;
        _isLoading = true;
        try
        {
            var scripts = Context.Profile.Scripts.Scripts
                .Select(script => new ScriptEntry(script)).ToList();
            navBarControl1.ItemsSource = scripts;
            navBarControl1.SelectedItem = scripts.FirstOrDefault(
                entry => entry.Script == _currentScript) ?? scripts.FirstOrDefault();
            LoadScript((navBarControl1.SelectedItem as ScriptEntry)?.Script);
            UpdateExpressionControlState();
        }
        finally
        {
            _isLoading = false;
        }
    }

    /// <summary>Writes the panel back without the original's validation prompts.</summary>
    public override void Commit()
    {
        if (!HasRuntimeContext || _currentScript == null || Context.IsRunning) return;
        var (success, _, script) = TryBuildScript(false);
        if (!success) return;
        _currentScript.ScriptName = script.ScriptName;
        _currentScript.Description = script.Description;
        _currentScript.ScriptText = script.ScriptText;
        _currentScript.bActive = script.bActive;
        _currentScript.Trigger = script.Trigger;
        _currentScript.last_modify_time = script.last_modify_time;
    }

    /// <summary>Mirrors LoadScriptToPanel / ClearScriptPanel.</summary>
    private void LoadScript(PythonScriptItem? script)
    {
        _currentScript = script;
        tbTaskName.Text = script?.ScriptName ?? "";
        tbDescription.Text = script?.Description ?? "";
        memoEditScriptText.Text = script?.ScriptText ?? "";
        // 原版 ClearScriptPanel 在无脚本时把启用开关置为打开。
        cbActive.IsChecked = script?.bActive ?? true;
        tbTagName1.Text = script?.Trigger.TagName ?? "";
        cbExpress.IsChecked = script?.Trigger.bUseExpression == true;
        _triggerExpression = script?.Trigger.ExpressionText ?? "";
        _expressionTags = script?.Trigger.ExpressionTags ?? [];
    }

    /// <summary>Mirrors NavBarControl1_LinkClicked: switching reloads the panel without saving.</summary>
    private void NavBarControl1_LinkClicked(object? sender, SelectionChangedEventArgs e)
    {
        if (!HasRuntimeContext || _isLoading) return;
        LoadScript((navBarControl1.SelectedItem as ScriptEntry)?.Script);
        UpdateExpressionControlState();
    }

    private async void barButtonAdd_ItemClick(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, AddScriptAsync);

    private async void barButtonRemove_ItemClick(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, DeleteScriptAsync);

    private async void barButtonItemSave_ItemClick(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, SaveScriptAsync);

    private async void barButtonRun_ItemClick(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, RunScriptAsync);

    private async void btTagSelect1_Click(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, SelectTriggerTagAsync);

    private async void btExpress_Click(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, EditExpressionAsync);

    private async void btAddReadTag_Click(object? sender, RoutedEventArgs e)
        => await RunActionAsync(sender, InsertReadTagAsync);

    private void cbExpress_Click(object? sender, RoutedEventArgs e) => UpdateExpressionControlState();

    /// <summary>Mirrors UpdateExpressionControlState: expression mode disables the bool trigger inputs.</summary>
    private void UpdateExpressionControlState()
    {
        var useExpression = cbExpress.IsChecked == true;
        tbTagName1.IsEnabled = !useExpression;
        btTagSelect1.IsEnabled = !useExpression;
        btExpress.IsEnabled = useExpression;
    }

    /// <summary>Mirrors TryBuildScriptFromPanel, including every validation message and its order.</summary>
    private (bool Success, string Message, PythonScriptItem Script) TryBuildScript(
        bool validateTrigger = true)
    {
        var script = new PythonScriptItem();
        var scriptName = (tbTaskName.Text ?? "").Trim();
        // Python 缩进属于脚本语义，正文必须逐字符保存，不能 Trim。
        var scriptText = memoEditScriptText.Text ?? "";

        if (string.IsNullOrWhiteSpace(scriptName))
            return (false, "脚本名不能为空。", script);
        if (PubFunction.StartsWithSpecialCharOrDigit(scriptName))
            return (false, "脚本名不允许特殊字符或数字开头。", script);
        if (HasDuplicateName(scriptName))
            return (false, "脚本名不能重复。", script);
        if (string.IsNullOrWhiteSpace(scriptText))
            return (false, "Python 脚本内容不能为空。", script);

        if (validateTrigger)
        {
            var useExpression = cbExpress.IsChecked == true;
            if (!useExpression)
            {
                var tagName = (tbTagName1.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(tagName))
                    return (false, "bool 触发点不能为空。", script);
                var tag = FindTag(tagName);
                if (tag == null)
                    return (false, "选择的通讯点不存在, 请检查通讯配置。", script);
                if (tag.DataType != 0)
                    return (false, "脚本触发点必须是 bool 类型。", script);
            }
            else if (string.IsNullOrWhiteSpace(_triggerExpression))
            {
                return (false, "表达式不能为空。", script);
            }
        }

        script = new PythonScriptItem
        {
            ScriptName = scriptName,
            Description = tbDescription.Text ?? "",
            bActive = cbActive.IsChecked == true,
            ScriptText = scriptText,
            last_modify_time = DateTime.Now,
            Trigger = new PythonScriptTrigger
            {
                TagName = (tbTagName1.Text ?? "").Trim(),
                bUseExpression = cbExpress.IsChecked == true,
                ExpressionText = _triggerExpression,
                ExpressionTags = [.. _expressionTags]
            }
        };
        return (true, "", script);
    }

    /// <summary>Mirrors the original duplicate check, which ignores the currently edited script.</summary>
    private bool HasDuplicateName(string scriptName) =>
        Context.Profile.Scripts.Scripts.Any(script =>
            !string.Equals(script.ScriptName, _currentScript?.ScriptName, StringComparison.OrdinalIgnoreCase)
            && string.Equals(script.ScriptName, scriptName, StringComparison.OrdinalIgnoreCase));

    /// <summary>Mirrors FindTag: match by TagId, "Cannel.TagName" or TagName.</summary>
    private DBTag? FindTag(string tagName) =>
        Context.Profile.Tags.FirstOrDefault(tag =>
            string.Equals(tag.TagId, tagName, StringComparison.OrdinalIgnoreCase)
            || string.Equals($"{tag.CannelID}.{tag.TagName}", tagName, StringComparison.OrdinalIgnoreCase)
            || string.Equals(tag.TagName, tagName, StringComparison.OrdinalIgnoreCase));

    /// <summary>Mirrors barButtonAdd_ItemClick: validate the panel, reject duplicates and add.</summary>
    private async Task AddScriptAsync()
    {
        var (success, message, script) = TryBuildScript();
        if (!success)
        {
            await Ui.MessageAsync(this, "Information", message);
            return;
        }

        if (Context.Profile.Scripts.Scripts.Any(item =>
            string.Equals(item.ScriptName, script.ScriptName, StringComparison.OrdinalIgnoreCase)))
        {
            await Ui.MessageAsync(this, "Information", "脚本名不能重复。");
            return;
        }

        Context.Profile.Scripts.Scripts.Add(script);
        _currentScript = script;
        Context.Profile.Scripts.last_modify_time = DateTime.Now;
        Context.Save();
        Refresh();
        await Ui.MessageAsync(this, "Information", "脚本添加成功。");
    }

    /// <summary>Mirrors barButtonItemSave_ItemClick: replace the current script or append it.</summary>
    private async Task SaveScriptAsync()
    {
        var (success, message, script) = TryBuildScript();
        if (!success)
        {
            await Ui.MessageAsync(this, "Information", message);
            return;
        }

        var index = _currentScript == null ? -1 : Context.Profile.Scripts.Scripts.IndexOf(_currentScript);
        if (index < 0) Context.Profile.Scripts.Scripts.Add(script);
        else Context.Profile.Scripts.Scripts[index] = script;
        _currentScript = script;
        Context.Profile.Scripts.last_modify_time = DateTime.Now;
        Context.Save();
        Refresh();
        await Ui.MessageAsync(this, "Information", "脚本保存成功。");
    }

    /// <summary>Mirrors barButtonRemove_ItemClick: confirm by name, remove and save.</summary>
    private async Task DeleteScriptAsync()
    {
        if (_currentScript == null || string.IsNullOrWhiteSpace(_currentScript.ScriptName)) return;
        if (!await Ui.ConfirmAsync(this, $"是否删除 {_currentScript.ScriptName}")) return;

        Context.Profile.Scripts.Scripts.RemoveAll(script => string.Equals(
            script.ScriptName, _currentScript.ScriptName, StringComparison.OrdinalIgnoreCase));
        _currentScript = null;
        Context.Profile.Scripts.last_modify_time = DateTime.Now;
        Context.Save();
        Refresh();
    }

    /// <summary>Mirrors barButtonRun_ItemClick: execute the panel content without trigger validation.</summary>
    private async Task RunScriptAsync()
    {
        var (success, message, script) = TryBuildScript(false);
        if (!success)
        {
            await Ui.MessageAsync(this, "Information", message);
            return;
        }

        try
        {
            var output = await Context.Scripts.RunAsync(script);
            await Ui.MessageAsync(this, "Python", string.IsNullOrWhiteSpace(output) ?
                "脚本执行成功。" : output);
        }
        catch (Exception ex)
        {
            await Ui.MessageAsync(this, "Python", $"脚本执行失败: {ex.Message}");
        }
    }

    /// <summary>Mirrors btTagSelect1_Click: expression mode and bool type guards.</summary>
    private async Task SelectTriggerTagAsync()
    {
        if (cbExpress.IsChecked == true)
        {
            await Ui.MessageAsync(this, "Information", "使用表达式时不需要选择单个 bool 触发点。");
            return;
        }

        var tags = await TagDialogs.PickAsync(this, Context.Profile);
        if (tags.Count == 0 || string.IsNullOrEmpty(tags[0].TagId)) return;
        if (!string.Equals(DBTag.DataTypeString(tags[0].DataType), "bool", StringComparison.OrdinalIgnoreCase))
        {
            await Ui.MessageAsync(this, "Information", "脚本触发点必须是 bool 类型。");
            return;
        }
        tbTagName1.Text = tags[0].TagId;
    }

    /// <summary>Mirrors btExpress_Click: the expression editor requires the checkbox.</summary>
    private async Task EditExpressionAsync()
    {
        if (cbExpress.IsChecked != true)
        {
            await Ui.MessageAsync(this, "Information", "请先勾选使用表达式。");
            return;
        }

        var result = await new diaExpress(Context.Profile, _triggerExpression, _expressionTags)
            .ShowDialog<ExpressionResult>((Window)TopLevel.GetTopLevel(this)!);
        if (result == null) return;
        _triggerExpression = result.Expression;
        _expressionTags = result.Bindings;
    }

    /// <summary>Mirrors btAddReadTag_Click: insert Read[Cannel.TagName] at the caret.</summary>
    private async Task InsertReadTagAsync()
    {
        var tags = await TagDialogs.PickAsync(this, Context.Profile);
        if (tags.Count == 0 || string.IsNullOrEmpty(tags[0].TagId)) return;
        var tag = tags[0];
        if (string.IsNullOrEmpty(tag.CannelID))
        {
            await Ui.MessageAsync(this, "Information", "所选标签缺少通道信息，请检查通讯点配置。");
            return;
        }

        var prefix = tag.CannelID + ".";
        var tagName = tag.TagId.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? tag.TagId
            : prefix + tag.TagId;
        var text = $"Read[{tagName}]";
        var caret = memoEditScriptText.CaretOffset;
        memoEditScriptText.Document.Insert(caret, text);
        memoEditScriptText.CaretOffset = caret + text.Length;
    }

    private sealed record ScriptEntry(PythonScriptItem Script)
    {
        public override string ToString() => Script.ScriptName;
    }
}
