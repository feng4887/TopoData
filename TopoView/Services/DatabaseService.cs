//=============================================================================
// 对齐 TopoData：auDAServer/BLL（SqlDispatch、DbClear、SqlConsumHelper）与
//               ucDataTableCfg / ucSystemCfg 的建库建表、清理入口
// Description: 跨平台数据库访问：标识符白名单校验、值全部参数化。
//=============================================================================
// Cross-platform database access with validated identifiers and parameterized values.
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text.RegularExpressions;
using auDASLib;
using auDAServer;
using Microsoft.Data.Sqlite;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Npgsql;

namespace TopoView.Services;

/// <summary>
/// 跨平台数据库访问：建库建表、历史写入、查询导出与过期清理；
/// 对应旧版 SqlDispatch/DbClear 的存储与清理语义。
/// </summary>
public sealed partial class DatabaseService(Func<WorkspaceProfile> profile)
{
    /// <summary>Trusts the SQL Server certificate without chain validation, matching the legacy
    /// clients and the shared engine's connection string (TrustServerCertificate=true). The settings
    /// page can turn strict validation back on.</summary>
    public bool TrustServerCertificate { get; set; } = true;
    [GeneratedRegex("^[A-Za-z_][A-Za-z0-9_]{0,62}$", RegexOptions.CultureInvariant)]
    private static partial Regex IdentifierPattern();

    /// <summary>按系统配置创建 SQLite / SQL Server / PostgreSQL / MySQL 连接。</summary>
    public DbConnection CreateConnection()
    {
        var cfg = profile().Server.SqlConfigInfo;
        if (IsSqlite && !OperatingSystem.IsWindows() && Regex.IsMatch(cfg.DbName, @"^[A-Za-z]:[\\/]"))
            throw new InvalidDataException("SQLite 路径来自 Windows，请在系统配置中选择当前 Linux 机器上的数据库文件。");
        return cfg.DbType switch
        {
            "SQLite" or "SQLite数据库" or "Sqlite" or "SqlLite" or "SQLite Database" => new SqliteConnection(
                new SqliteConnectionStringBuilder { DataSource = cfg.DbName, Mode = SqliteOpenMode.ReadWriteCreate }.ConnectionString),
            "Microsoft SQL Server" => new SqlConnection(new SqlConnectionStringBuilder
            {
                DataSource = $"{cfg.ServerName},{cfg.Port}", InitialCatalog = cfg.DbName, UserID = cfg.User,
                Password = cfg.Psw, ConnectTimeout = 8, Encrypt = true, TrustServerCertificate = TrustServerCertificate
            }.ConnectionString),
            "PostgreSQL" => new NpgsqlConnection(new NpgsqlConnectionStringBuilder
            {
                Host = cfg.ServerName, Port = cfg.Port, Database = cfg.DbName,
                Username = cfg.User, Password = cfg.Psw, Timeout = 8
            }.ConnectionString),
            "MySQL" or "MySql" => new MySqlConnection(new MySqlConnectionStringBuilder
            {
                Server = cfg.ServerName, Port = checked((uint)cfg.Port), Database = cfg.DbName,
                UserID = cfg.User, Password = cfg.Psw, ConnectionTimeout = 8
            }.ConnectionString),
            _ => throw new NotSupportedException($"不支持的数据库：{cfg.DbType}")
        };
    }

    /// <summary>数据库连接测试（SELECT 1）。</summary>
    public async Task TestAsync(CancellationToken token = default)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync(token);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1";
        await command.ExecuteScalarAsync(token);
    }

    /// <summary>创建数据库：SQLite 直接建文件；服务器库连接 master/postgres 后执行 CREATE DATABASE。</summary>
    public async Task CreateDatabaseAsync()
    {
        var cfg = profile().Server.SqlConfigInfo;
        if (IsSqlite)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(cfg.DbName))!);
            await TestAsync();
            return;
        }
        ValidateIdentifier(cfg.DbName);
        await using var connection = CreateConnection();
        switch (connection)
        {
            case SqlConnection sql: var sb = new SqlConnectionStringBuilder(sql.ConnectionString) { InitialCatalog = "master" }; sql.ConnectionString = sb.ConnectionString; break;
            case NpgsqlConnection pg: var pb = new NpgsqlConnectionStringBuilder(pg.ConnectionString) { Database = "postgres" }; pg.ConnectionString = pb.ConnectionString; break;
            case MySqlConnection my: var mb = new MySqlConnectionStringBuilder(my.ConnectionString) { Database = "" }; my.ConnectionString = mb.ConnectionString; break;
        }
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE {Quote(cfg.DbName)}";
        await command.ExecuteNonQueryAsync();
    }

    /// <summary>为存储任务建表：普通任务表（Datetime、WO + 采集列）或公共时序表 tb_au_pubhisvalues。</summary>
    public async Task CreateTableAsync(DataSample task, CancellationToken token = default)
    {
        ValidateTask(task);
        var table = task.bSave2ProcessTable ? "tb_au_pubhisvalues" : task.TableName;
        var columns = task.bSave2ProcessTable
            ? new[] { $"{Quote("f_id")} {SqlType("string", 36)} PRIMARY KEY", $"{Quote("datetime")} {SqlType("datetime")}",
                $"{Quote("tagid")} {SqlType("string", 150)}", $"{Quote("tagn")} {SqlType("string", 150)}",
                $"{Quote("fvalue")} {SqlType("double")}", $"{Quote("bvalue")} {SqlType("bool")}",
                $"{Quote("strvalue")} {SqlType("string", 3000)}", $"{Quote("uom")} {SqlType("string", 50)}",
                $"{Quote("wo_no")} {SqlType("string", 150)}", $"{Quote("unit_id")} {SqlType("string", 100)}", $"{Quote("seq_name")} {SqlType("string", 150)}" }
            : new[] { $"{Quote("Datetime")} {SqlType("datetime")}", $"{Quote("WO")} {SqlType("string", 150)}" }
                .Concat(task.Samples.Select(c => $"{Quote(c.Column)} {SqlType(c.DataType, c.MaxLength ?? 255)}{(c.IsNullable ? "" : " NOT NULL")}")).ToArray();
        await CreateTableIfMissingAsync(table, columns, token);
    }

    private async Task CreateTableIfMissingAsync(string table, string[] columns, CancellationToken token)
    {
        var quoted = Quote(table);
        var statement = $"CREATE TABLE {(IsSqlServer ? "" : "IF NOT EXISTS ")}{quoted} ({string.Join(",", columns)})";
        if (IsSqlServer) statement = $"IF OBJECT_ID(N'{table}',N'U') IS NULL {statement}";
        await using var connection = CreateConnection();
        await connection.OpenAsync(token);
        await using var command = connection.CreateCommand();
        command.CommandText = statement;
        await command.ExecuteNonQueryAsync(token);
    }

    /// <summary>Creates the legacy work-order table every SqlDispatch job wrote into.</summary>
    public async Task EnsureWorkOrderTableAsync(CancellationToken token = default)
    {
        var columns = new[]
        {
            $"{Quote("wo_no")} {SqlType("string", 100)} PRIMARY KEY", $"{Quote("lot_no")} {SqlType("string", 100)}",
            $"{Quote("sublot_no")} {SqlType("string", 100)}", $"{Quote("recipe_id")} {SqlType("string", 100)}",
            $"{Quote("item")} {SqlType("string", 100)}", $"{Quote("item_qty")} {SqlType("double")}",
            $"{Quote("uom")} {SqlType("string", 100)}", $"{Quote("mode")} {SqlType("int")}",
            $"{Quote("processtype")} {SqlType("int")}", $"{Quote("unit_id")} {SqlType("string", 100)}",
            $"{Quote("table_name")} {SqlType("string", 100)}", $"{Quote("state")} {SqlType("int")}",
            $"{Quote("start_time")} {SqlType("datetime")}", $"{Quote("end_time")} {SqlType("datetime")}",
            $"{Quote("f_last_modify_time")} {SqlType("datetime")}", $"{Quote("f_last_modify_user_id")} {SqlType("string", 100)}",
            $"{Quote("spare1")} {SqlType("string", 100)}", $"{Quote("spare2")} {SqlType("string", 100)}",
            $"{Quote("spare3")} {SqlType("string", 100)}", $"{Quote("spare4")} {SqlType("string", 3000)}"
        };
        await CreateTableIfMissingAsync("tb_au_workorder", columns, token);
    }

    /// <summary>
    /// 存储一行采样：坏质量或缺失点写 NULL 并保留行（旧版历史行语义），
    /// 公共时序表按运行值类型分别写 fvalue/bvalue/strvalue；配置工单点位时同步维护 tb_au_workorder。
    /// </summary>
    public async Task StoreSampleAsync(DataSample task, CancellationToken token)
    {
        var values = task.Samples.Select(column =>
        {
            var tagId = ConfigurationStore.ResolveTagId(column.TagID, column.TagName, column.CannelID);
            if (column.isInternalValue) return (Column: column, TagId: tagId, Value: (object?)column.DefaultValue, Unit: column.UOM ?? "");
            // Legacy historian rows kept their position: a missing or bad-quality point stored NULL.
            var good = ItemPool.ValuePool.TryGetValue(tagId, out var value) && value?.Quality == 192;
            return (Column: column, TagId: tagId, Value: good ? value!.RealValue : null, Unit: good ? value!.uom ?? "" : column.UOM ?? "");
        }).ToList();
        object batch = !string.IsNullOrWhiteSpace(task.wo) && ItemPool.ValuePool.TryGetValue(task.wo, out var batchValue)
            && batchValue?.Quality == 192 ? (object?)Convert.ToString(batchValue.RealValue, CultureInfo.InvariantCulture) ?? "" : "";
        await using var connection = CreateConnection();
        await connection.OpenAsync(token);
        await using var transaction = await connection.BeginTransactionAsync(token);
        if (task.bSave2ProcessTable)
        {
            var now = DateTime.Now;
            foreach (var value in values)
            {
                // Legacy GetBulkInsertHisTable split each value by runtime type and skipped the rest.
                float? f = 0; bool? b = false; string? s = null;
                switch (value.Value)
                {
                    case int number: f = number; b = number > 0; break;
                    case float number: f = (float)Math.Round(number, 4); b = f > 0; break;
                    case double number: f = (float)Math.Round(number, 4); b = f > 0; break;
                    case bool boolean: b = boolean; f = boolean ? 1 : 0; break;
                    case string text: s = text; break;
                    default: continue;
                }
                await InsertAsync(connection, transaction, "tb_au_pubhisvalues",
                    ["f_id", "datetime", "tagid", "tagn", "fvalue", "bvalue", "strvalue", "uom", "wo_no", "unit_id", "seq_name"],
                    [Guid.NewGuid().ToString().Trim('-'), now, value.TagId, value.Column.Column, f, b, s,
                        value.Unit, batch, task.EquipmentName, task.ReportName], token);
            }
        }
        else
        {
            // Legacy OnTrue jobs skipped the row only when every sampled column was bad.
            if (task.SampleTrig.Type != 0 || values.Any(v => v.Value != null))
                await InsertAsync(connection, transaction, task.TableName,
                    new[] { "Datetime", "WO" }.Concat(values.Select(v => v.Column.Column)).ToArray(),
                    new object?[] { DateTime.Now, batch }.Concat(values.Select(v => Round(v.Value))).ToArray(), token);
        }
        await transaction.CommitAsync(token);
        // Legacy ManageWorkOrder ran after every stored row; a missing or bad WO point was skipped silently.
        if (!string.IsNullOrWhiteSpace(task.wo) && batch is string workOrder && !string.IsNullOrWhiteSpace(workOrder))
            await TryUpsertWorkOrderAsync(connection, task, workOrder, token);
    }

    private async Task TryUpsertWorkOrderAsync(DbConnection connection, DataSample task, string workOrder, CancellationToken token)
    {
        try
        {
            var unit = string.IsNullOrWhiteSpace(task.EquipmentName) ? "-" : task.EquipmentName;
            await using (var update = connection.CreateCommand())
            {
                update.CommandText = $"UPDATE {Quote("tb_au_workorder")} SET {Quote("end_time")} = @now, {Quote("item_qty")} = @qty WHERE {Quote("wo_no")} = @wo AND {Quote("unit_id")} = @unit";
                AddParameter(update, "@now", DateTime.Now);
                AddParameter(update, "@qty", 0);
                AddParameter(update, "@wo", workOrder);
                AddParameter(update, "@unit", unit);
                if (await update.ExecuteNonQueryAsync(token) > 0) return;
            }
            await using (var insert = connection.CreateCommand())
            {
                insert.CommandText = $"INSERT INTO {Quote("tb_au_workorder")} ({Quote("wo_no")}, {Quote("item")}, {Quote("lot_no")}, {Quote("start_time")}, {Quote("table_name")}, {Quote("unit_id")}) VALUES (@wo, @item, @lot, @start, @table, @unit)";
                AddParameter(insert, "@wo", workOrder);
                AddParameter(insert, "@item", "");
                AddParameter(insert, "@lot", workOrder);
                AddParameter(insert, "@start", DateTime.Now);
                AddParameter(insert, "@table", task.TableName);
                AddParameter(insert, "@unit", unit);
                await insert.ExecuteNonQueryAsync(token);
            }
        }
        catch (DbException) { /* Legacy enqueued work orders asynchronously; storage must not fail. */ }
    }

    /// <summary>Legacy GetInsertSql rounded floating values to four decimal places.</summary>
    private static object? Round(object? value) => value switch
    {
        float number => Math.Round(number, 4),
        double number => Math.Round(number, 4),
        decimal number => Math.Round(number, 4),
        _ => value
    };

    /// <summary>历史查询：时间范围 + 点位筛选（仅公共时序表），按时间倒序并限制行数，参数化查询。</summary>
    public async Task<DataTable> QueryAsync(string table, DateTime from, DateTime to, string tagFilter, int limit = 10000)
    {
        ValidateIdentifier(table);
        if (from > to) throw new ArgumentException("开始时间不能晚于结束时间。");
        limit = Math.Clamp(limit, 1, 100000);
        var time = table == "tb_au_pubhisvalues" ? "datetime" : "Datetime";
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        var condition = $"{Quote(time)} >= @from AND {Quote(time)} <= @to";
        AddParameter(command, "@from", from);
        AddParameter(command, "@to", to);
        if (table == "tb_au_pubhisvalues" && !string.IsNullOrWhiteSpace(tagFilter))
        {
            condition += $" AND {Quote("tagid")} LIKE @filter";
            AddParameter(command, "@filter", "%" + tagFilter + "%");
        }
        command.CommandText = $"SELECT {(IsSqlServer ? "TOP " + limit : "")} * FROM {Quote(table)} WHERE {condition} ORDER BY {Quote(time)} DESC{(IsSqlServer ? "" : " LIMIT " + limit)}";
        await using var reader = await command.ExecuteReaderAsync();
        var result = new DataTable();
        result.Load(reader);
        return result;
    }

    /// <summary>校验存储任务：任务名、表名与数据列标识符合法且不重复。</summary>
    public static void ValidateTask(DataSample task)
    {
        if (string.IsNullOrWhiteSpace(task.TaskName)) throw new InvalidDataException("存储任务名称不能为空。");
        ValidateIdentifier(task.TableName);
        if (task.Samples.Count == 0) throw new InvalidDataException("请为存储任务添加至少一个数据列。");
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Datetime", "WO" };
        foreach (var column in task.Samples)
        {
            ValidateIdentifier(column.Column);
            if (!names.Add(column.Column)) throw new InvalidDataException($"数据列重复或使用保留名称：{column.Column}");
        }
    }

    /// <summary>按历史保留月数预览各表过期行数（“清理过期历史”第一步）。</summary>
    public async Task<List<RetentionPreview>> PreviewRetentionAsync()
    {
        var months = profile().Server.SqlConfigInfo.StoreMonth;
        if (months <= 0) throw new InvalidOperationException("历史保留月数为 0，未启用过期清理。");
        var cutoff = DateTime.Now.AddMonths(-months);
        var previews = new List<RetentionPreview>();
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        foreach (var table in profile().Server.dataSamples.Select(t => t.bSave2ProcessTable ? "tb_au_pubhisvalues" : t.TableName).Distinct())
        {
            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM {Quote(table)} WHERE {Quote(table == "tb_au_pubhisvalues" ? "datetime" : "Datetime")} < @cutoff";
            AddParameter(command, "@cutoff", cutoff);
            previews.Add(new(table, cutoff, Convert.ToInt64(await command.ExecuteScalarAsync())));
        }
        return previews;
    }

    /// <summary>在事务中删除预览确认的过期行；清理范围变化时拒绝执行。</summary>
    public async Task<int> CleanExpiredAsync(IReadOnlyList<RetentionPreview> previews)
    {
        var allowed = profile().Server.dataSamples.Select(t => t.bSave2ProcessTable ? "tb_au_pubhisvalues" : t.TableName).ToHashSet();
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        var count = 0;
        foreach (var preview in previews)
        {
            if (!allowed.Contains(preview.Table)) throw new InvalidOperationException("清理范围已变化，请重新预览。");
            await using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = $"DELETE FROM {Quote(preview.Table)} WHERE {Quote(preview.Table == "tb_au_pubhisvalues" ? "datetime" : "Datetime")} < @cutoff";
            AddParameter(command, "@cutoff", preview.Cutoff);
            count += await command.ExecuteNonQueryAsync();
        }
        await transaction.CommitAsync();
        return count;
    }

    /// <summary>Background retention worker restoring the legacy DbClear loop: every hour it deletes
    /// rows older than StoreMonth from the public history, app log and task tables.</summary>
    public async Task RunRetentionLoopAsync(Action<string> log, CancellationToken token)
    {
        var lastError = "";
        while (!token.IsCancellationRequested)
        {
            try
            {
                await CleanExpiredHourlyAsync(log);
                lastError = "";
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                if (lastError != ex.Message) log("历史清理：" + ex.Message);
                lastError = ex.Message;
            }
            await Task.Delay(TimeSpan.FromHours(1), token);
        }
    }

    private async Task CleanExpiredHourlyAsync(Action<string> log)
    {
        // DbClear clamped every configured value at or below 1 to a single retained month.
        var months = Math.Max(1, profile().Server.SqlConfigInfo.StoreMonth);
        var cutoff = DateTime.Now.AddMonths(-months);
        var tables = new (string Table, string Time)[]
            { ("tb_au_pubhisvalues", "datetime"), ("tb_au_his_applogmessage", "f_last_modify_time") }
            .Concat(profile().Server.dataSamples.Select(t => (t.bSave2ProcessTable ? "tb_au_pubhisvalues" : t.TableName, "Datetime")))
            .Distinct().ToList();
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        foreach (var (table, time) in tables)
        {
            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM {Quote(table)} WHERE {Quote(time)} < @cutoff";
                AddParameter(command, "@cutoff", cutoff);
                await command.ExecuteNonQueryAsync();
            }
            catch (DbException ex)
            {
                // Legacy DbClear silently skipped tables without a matching column (e.g. never-created
                // app log tables); other database errors are surfaced once per distinct message.
                if (!IsMissingObjectError(ex)) log($"历史清理 {table}：{ex.Message}");
            }
        }
    }

    private static bool IsMissingObjectError(DbException ex) =>
        ex.Message.Contains("no such table", StringComparison.OrdinalIgnoreCase) ||
        ex.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase) ||
        ex.Message.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase) ||
        ex.Message.Contains("doesn't exist", StringComparison.OrdinalIgnoreCase);

    /// <summary>SQL 标识符白名单校验：英文字母、数字、下划线，且不以数字开头。</summary>
    public static void ValidateIdentifier(string name)
    {
        if (string.IsNullOrEmpty(name) || !IdentifierPattern().IsMatch(name))
            throw new InvalidDataException($"数据库名称只能包含英文字母、数字和下划线，且不能以数字开头：{name}");
    }

    private bool IsSqlite => profile().Server.SqlConfigInfo.DbType.Contains("lite", StringComparison.OrdinalIgnoreCase);
    private bool IsSqlServer => profile().Server.SqlConfigInfo.DbType == "Microsoft SQL Server";
    private string Quote(string name)
    {
        ValidateIdentifier(name);
        if (IsSqlServer) return $"[{name}]";
        if (profile().Server.SqlConfigInfo.DbType.StartsWith("My", StringComparison.OrdinalIgnoreCase)) return $"`{name}`";
        // The legacy runtime concatenated unquoted SQL, so PostgreSQL folded every identifier to
        // lower case; keep emitting unquoted names to stay compatible with existing databases.
        if (profile().Server.SqlConfigInfo.DbType.StartsWith("Postgre", StringComparison.OrdinalIgnoreCase)) return name;
        return $"\"{name}\"";
    }

    private string SqlType(string type, int length = 255) => type.ToLowerInvariant() switch
    {
        "datetime" => IsSqlServer ? "DATETIME2" : IsSqlite ? "TEXT" : "TIMESTAMP",
        "bool" or "bit" or "boolean" => IsSqlServer ? "BIT" : "BOOLEAN",
        "float" or "real" or "double" or "decimal" => IsSqlServer ? "FLOAT" : "DOUBLE PRECISION",
        "byte" or "int" or "int16" or "int32" or "int64" or "long" or "word" => "BIGINT",
        "string" or "wstring" or "nvarchar" or "varchar" => IsSqlServer ? $"NVARCHAR({Math.Clamp(length, 1, 4000)})" : IsSqlite ? "TEXT" : $"VARCHAR({Math.Clamp(length, 1, 4000)})",
        _ => throw new InvalidDataException($"不支持的列类型：{type}")
    };

    private async Task InsertAsync(DbConnection connection, DbTransaction transaction, string table, string[] columns, object?[] values, CancellationToken token)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = $"INSERT INTO {Quote(table)} ({string.Join(",", columns.Select(Quote))}) VALUES ({string.Join(",", values.Select((_, i) => "@p" + i))})";
        for (var i = 0; i < values.Length; i++) AddParameter(command, "@p" + i, values[i]);
        await command.ExecuteNonQueryAsync(token);
    }

    private static void AddParameter(DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }
}

/// <summary>一张表的过期清理预览：表名、截止时间与过期行数。</summary>
public sealed record RetentionPreview(string Table, DateTime Cutoff, long Count);
