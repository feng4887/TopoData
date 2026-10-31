//=============================================================================
// 对齐 TopoData：auDAServer/BLL/Scheduler.cs 的采样调度与
//               Public/PythonScriptOperater.cs 的脚本监视任务
// Description: 采集运行期调度：存储任务触发采样与 Python 脚本触发执行。
//=============================================================================
// Cancellable sampling and script monitors. Script invocations never overlap for one trigger.
using auDASLib;
using auDAServer;

namespace TopoView.Services;

/// <summary>
/// 采集运行期任务调度：存储任务按触发类型采样入库，脚本按触发条件执行。
/// </summary>
public sealed class RuntimeJobs(DatabaseService database, ScriptService scripts, Action<string> log)
{
    /// <summary>
    /// 启动前校验：免费版任务数/列数上限、触发点与表达式有效性；确保任务表与工单表存在。
    /// </summary>
    public async Task InitializeAsync(WorkspaceProfile profile, CancellationToken token)
    {
        var tasks = profile.Server.dataSamples.Where(t => t.bActive).ToList();
        if (DACtrl.free_Version && tasks.Count > DACtrl.free_DataSamples)
            throw new InvalidOperationException($"当前版本最多启用 {DACtrl.free_DataSamples} 个存储任务。");
        void RequireTag(string id, bool boolean = false)
        {
            var tag = profile.Tags.FirstOrDefault(t => t.TagId == id);
            if (tag == null || boolean && tag.DataType != 0 || !profile.Server.cDevices.Any(d => d.CannelID == tag.CannelID && d.iActive == 1))
                throw new InvalidDataException($"触发或存储点位不存在、未启用或类型不符：{id}");
        }
        foreach (var task in tasks)
        {
            DatabaseService.ValidateTask(task);
            if (DACtrl.free_Version && task.Samples.Count > DACtrl.free_DataSampleColumns)
                throw new InvalidOperationException($"任务 {task.TaskName} 超过 {DACtrl.free_DataSampleColumns} 列限制。");
            if (task.SampleTrig.Type is < 0 or > 2) throw new InvalidDataException("采集触发类型无效。");
            foreach (var column in task.Samples.Where(c => !c.isInternalValue))
                RequireTag(ConfigurationStore.ResolveTagId(column.TagID, column.TagName, column.CannelID));
            // The legacy runtime tolerated a missing work-order point (rows were stored with an empty
            // WO) and inert Type-2 tasks whose both-edge tag was never configured.
            var trigger = task.SampleTrig;
            if (trigger.bUseExpression) ScriptService.ValidateExpression(trigger.ExpressionText, trigger.ExpressionTags, profile.Tags);
            else
            {
                var id = trigger.Type == 0 ? trigger.dataSampleTrig_OnTrue.TagName : trigger.Type == 2 ? trigger.dataSampleTrig_OnTrueOnFalse.TagName : trigger.dataSampleTrig_TimeWindow.TagName;
                if (trigger.Type == 0 || !string.IsNullOrWhiteSpace(id)) RequireTag(id, true);
            }
            // Legacy ucDataTableCfg only rejected periods at or below 39 ms; SqlDispatch clamped to 50.
            if (trigger.Type == 1 && trigger.dataSampleTrig_TimeWindow.TimeWindow <= 39)
                throw new InvalidDataException($"任务 {task.TaskName} 周期至少为 400 ms。");
        }
        foreach (var script in profile.Scripts.Scripts.Where(s => s.bActive))
        {
            if (string.IsNullOrWhiteSpace(script.ScriptText)) throw new InvalidDataException($"脚本 {script.ScriptName} 内容为空。");
            if (script.Trigger.bUseExpression) ScriptService.ValidateExpression(script.Trigger.ExpressionText, script.Trigger.ExpressionTags, profile.Tags);
            else if (!string.IsNullOrWhiteSpace(script.Trigger.TagName)) RequireTag(script.Trigger.TagName, true);
        }
        await database.EnsureWorkOrderTableAsync(token);
        foreach (var task in tasks) await database.CreateTableAsync(task, token);
    }

    /// <summary>
    /// 250 ms 主循环：按旧版语义驱动存储任务（上升沿/周期/双边沿/表达式）与脚本触发；
    /// 同一脚本不重叠执行，错误按消息去重后记日志。
    /// </summary>
    public async Task RunAsync(WorkspaceProfile profile, CancellationToken token)
    {
        var tasks = profile.Server.dataSamples.Where(t => t.bActive).ToList();
        var samples = tasks.Select(t => new SampleState(t)).ToList();
        var scriptStates = profile.Scripts.Scripts.Where(s => s.bActive).Select(s => new ScriptState(s)).ToList();
        var errors = new Dictionary<string, string>();
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(250));
        try
        {
            while (await timer.WaitForNextTickAsync(token))
            {
                foreach (var state in samples)
                {
                    try
                    {
                        var trigger = state.Task.SampleTrig;
                        // The legacy engine only ever ran two paths: Type 0 sampled on the OnTrue tag,
                        // everything else ran the time-window loop gated by the TimeWindow tag. A Type 2
                        // task without a both-edge tag therefore falls back to that periodic behaviour.
                        var bothEdge = trigger.dataSampleTrig_OnTrueOnFalse.TagName;
                        var periodic = trigger.Type == 1 || trigger.Type == 2 && string.IsNullOrWhiteSpace(bothEdge);
                        var tagId = trigger.Type == 0 ? trigger.dataSampleTrig_OnTrue.TagName
                            : periodic ? trigger.dataSampleTrig_TimeWindow.TagName : bothEdge;
                        var condition = trigger.bUseExpression ? ScriptService.Evaluate(trigger.ExpressionText, trigger.ExpressionTags)
                            : periodic && string.IsNullOrWhiteSpace(tagId) || ScriptService.ReadCondition(tagId);
                        var due = periodic
                            ? condition && DateTime.UtcNow >= state.Next
                            : trigger.Type == 0 ? condition && !state.Previous : condition != state.Previous;
                        state.Previous = condition;
                        if (due)
                        {
                            await database.StoreSampleAsync(state.Task, token);
                            state.Next = DateTime.UtcNow.AddMilliseconds(Math.Max(50, trigger.dataSampleTrig_TimeWindow.TimeWindow));
                            errors.Remove(state.Task.TaskName);
                        }
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        if (!errors.TryGetValue(state.Task.TaskName, out var prior) || prior != ex.Message)
                            log($"存储任务 {state.Task.TaskName}：{ex.Message}");
                        errors[state.Task.TaskName] = ex.Message;
                    }
                }
                foreach (var state in scriptStates)
                {
                    try
                    {
                        var trigger = state.Script.Trigger;
                        ScriptService.TriggerContext? context = null;
                        var condition = false;
                        string? valueText = null;
                        if (trigger.bUseExpression)
                            condition = ScriptService.Evaluate(trigger.ExpressionText, trigger.ExpressionTags);
                        else
                            condition = ScriptService.ReadCondition(trigger.TagName, out valueText);
                        if (condition)
                            context = new ScriptService.TriggerContext(
                                trigger.bUseExpression ? "Expression" : trigger.TagName,
                                trigger.bUseExpression ? "true" : valueText ?? "", DateTime.Now);
                        if (condition && !state.Previous && (state.Execution == null || state.Execution.IsCompleted))
                            state.Execution = ExecuteScriptAsync(state.Script, context, token);
                        state.Previous = condition;
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        var key = "script:" + state.Script.ScriptName;
                        if (!errors.TryGetValue(key, out var prior) || prior != ex.Message) log($"脚本 {state.Script.ScriptName}：{ex.Message}");
                        errors[key] = ex.Message;
                    }
                }
            }
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { }
        finally { await Task.WhenAll(scriptStates.Where(s => s.Execution != null).Select(s => s.Execution!)); }
    }

    /// <summary>执行单个脚本并记录输出；取消与失败只记日志，不中断调度循环。</summary>
    private async Task ExecuteScriptAsync(PythonScriptItem script, ScriptService.TriggerContext? trigger, CancellationToken token)
    {
        try { log($"脚本 {script.ScriptName}：{await scripts.RunAsync(script, token, trigger)}"); }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { }
        catch (Exception ex) { log($"脚本 {script.ScriptName} 执行失败：{ex.Message}"); }
    }

    private sealed class SampleState(DataSample task)
    {
        public DataSample Task { get; } = task;
        public bool Previous { get; set; }
        public DateTime Next { get; set; } = DateTime.MinValue;
    }
    private sealed class ScriptState(PythonScriptItem script)
    {
        public PythonScriptItem Script { get; } = script;
        public bool Previous { get; set; }
        public Task? Execution { get; set; }
    }
}
