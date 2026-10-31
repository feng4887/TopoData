// Historical queries use the original table-mode and combined date/time controls.
using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class ucHisQuery : BusinessPage
{
    // 原版 radioGroup1：宽表（普通任务表）与时序表（实时历史表）两项
    private const string WideTable = "宽表";
    private const string RealtimeTable = "时序表";
    private const string RealtimeTablename = "tb_au_pubhisvalues";

    private DataTable? _queryResult;

    public ucHisQuery()
    {
        InitializeComponent();
        radioGroup1.ItemsSource = new[] { WideTable, RealtimeTable };
        radioGroup1.SelectedIndex = 0;
        dtStime.Value = DateTime.Now.AddDays(-1);
        dtEtime.Value = DateTime.Now;
        UiText.Apply(this);
    }

    public ucHisQuery(AppContextServices context) : this() => InitializeRuntime(context);

    /// <summary>Mirrors btSearch_Click: resolve the table, validate the task columns, then query.</summary>
    private async Task QueryAsync()
    {
        var realtimeQuery = radioGroup1.SelectedIndex == 1;
        var sample = Context.Profile.Server.dataSamples.FirstOrDefault();
        if (sample == null) return;

        if (dtStime.Value is not { } from || dtEtime.Value is not { } to)
            throw new InvalidDataException("请选择开始时间和结束时间。");

        var table = RealtimeTablename;
        var tagNames = new List<string>();
        if (realtimeQuery)
        {
            // 原版按数据类型挑选时序表列：float/int/bool/string 直接入选，wstring 额外排除
            // DateTime 与 WO 列（原 Where 表达式的实际优先级即如此）。
            tagNames = sample.Samples
                .Where(s => s.DataType.Equals("float", StringComparison.OrdinalIgnoreCase)
                            || s.DataType.Equals("int", StringComparison.OrdinalIgnoreCase)
                            || s.DataType.Equals("bool", StringComparison.OrdinalIgnoreCase)
                            || s.DataType.Equals("string", StringComparison.OrdinalIgnoreCase)
                            || s.DataType.Equals("wstring", StringComparison.OrdinalIgnoreCase)
                               && s.Column != "DateTime" && s.Column != "WO")
                .Select(s => s.Column).ToList();
        }
        else
        {
            if (string.IsNullOrWhiteSpace(sample.TableName)) return;
            table = sample.TableName;
            if (sample.Samples.Count == 0)
            {
                await Ui.MessageAsync(this, "Information", "查询任务没有字段，查询失败。");
                return;
            }
        }

        var waiting = new waitForm();
        waiting.SetCaption("查询");
        waiting.Show((Window)TopLevel.GetTopLevel(this)!);
        try
        {
            _queryResult = await Context.Database.QueryAsync(table, from, to, "", 100000);
        }
        finally { waiting.Close(); }

        // 原版在 SQL 中用 TagN IN (...) 过滤；本工程查询接口只支持单条件，改为结果集过滤。
        if (realtimeQuery && tagNames.Count > 0 && _queryResult.Columns.Contains("TagN"))
        {
            var names = tagNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var row in _queryResult.Rows.Cast<DataRow>().ToList())
                if (!names.Contains(Convert.ToString(row["TagN"]) ?? "")) _queryResult.Rows.Remove(row);
        }

        BindSheet(_queryResult);
        Context.Notify($"查询完成，共 {_queryResult.Rows.Count} 行。");
    }

    /// <summary>Mirrors bindSheet: replace the grid source and keep the first column readable.</summary>
    private void BindSheet(DataTable table)
    {
        HistoryColumns.SetSource(gridControl1, table);
        if (gridControl1.Columns.Count > 0 && gridControl1.Columns[0].Width.Value < 155)
            gridControl1.Columns[0].Width = new DataGridLength(155);
    }

    /// <summary>Mirrors btExport_Click: warn when empty, otherwise write the queried table to xlsx.</summary>
    private async Task ExportAsync()
    {
        if (_queryResult == null || _queryResult.Rows.Count == 0)
        {
            await Ui.MessageAsync(this, "Information", "No data to export.");
            return;
        }

        var path = await Ui.SaveAsync(this, "导出历史数据",
            "Data_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", "xlsx");
        if (path == null) return;

        var waiting = new waitForm();
        waiting.SetCaption("导出");
        waiting.Show((Window)TopLevel.GetTopLevel(this)!);
        try { await Task.Run(() => TagExchangeService.WriteTable(path, _queryResult)); }
        finally { waiting.Close(); }
    }

    private async void btSearch_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, QueryAsync);
    private async void btExport_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, ExportAsync);
}
