//=============================================================================
// 对齐 TopoData：auDASLib/DBOperate.cs（CreateOrUpdateTable）与
//               ucDataTableCfg / ucSystemCfg 的建表、删列、建库入口
// Description: 与旧版界面操作对应的表结构维护动作。
//=============================================================================
// Schema actions exposed by TopoData ucDataTableCfg and ucSystemCfg.
using System.Data.Common;

namespace TopoView.Services;

public sealed partial class DatabaseService
{
    /// <summary>Creates the legacy task table and adds missing configured columns. Returns the same
    /// codes as the legacy DBOperate.CreateOrUpdateTable: -1 created, 0 unchanged, N columns added.</summary>
    public async Task<int> SynchronizeTableAsync(auDASLib.DataSample task)
    {
        ValidateTask(task);
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var exists = true;
        try
        {
            await using var query = connection.CreateCommand();
            query.CommandText = $"SELECT * FROM {Quote(task.TableName)} WHERE 1 = 0";
            await using var reader = await query.ExecuteReaderAsync();
            for (var index = 0; index < reader.FieldCount; index++)
                existing.Add(reader.GetName(index));
        }
        catch (DbException) { exists = false; }

        if (!exists || task.bSave2ProcessTable)
        {
            await CreateTableAsync(task);
            return exists ? 0 : -1;
        }

        var added = 0;
        await using var transaction = await connection.BeginTransactionAsync();
        foreach (var column in task.Samples.Where(column => !existing.Contains(column.Column)))
        {
            await using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = $"ALTER TABLE {Quote(task.TableName)} ADD {Quote(column.Column)} " +
                SqlType(column.DataType, column.MaxLength ?? 255) + (column.IsNullable ? "" : " NOT NULL");
            await command.ExecuteNonQueryAsync();
            added++;
        }
        await transaction.CommitAsync();
        return added;
    }

    /// <summary>Deletes the explicitly selected ordinary-table column after UI confirmation.</summary>
    public async Task DeleteSampleColumnAsync(auDASLib.DataSample task, string column)
    {
        ValidateIdentifier(task.TableName);
        ValidateIdentifier(column);
        if (task.bSave2ProcessTable || !task.Samples.Any(sample =>
            string.Equals(sample.Column, column, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("只能删除普通任务表中已配置的采集字段。");
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = $"ALTER TABLE {Quote(task.TableName)} DROP COLUMN {Quote(column)}";
        await command.ExecuteNonQueryAsync();
    }

    /// <summary>Honors the SQL Server database-directory field from the legacy settings page.</summary>
    public async Task CreateDatabaseAtAsync(string? directory)
    {
        if (!IsSqlServer || string.IsNullOrWhiteSpace(directory))
        {
            await CreateDatabaseAsync();
            return;
        }
        var name = profile().Server.SqlConfigInfo.DbName;
        ValidateIdentifier(name);
        await using var connection = CreateConnection();
        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connection.ConnectionString)
            { InitialCatalog = "master" };
        connection.ConnectionString = builder.ConnectionString;
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = """
            DECLARE @statement nvarchar(max) =
                N'CREATE DATABASE ' + QUOTENAME(@name) +
                N' ON PRIMARY (NAME = ' + QUOTENAME(@name, N'''') +
                N', FILENAME = N''' + REPLACE(@file, N'''', N'''''') + N''')';
            EXEC(@statement);
            """;
        AddParameter(command, "@name", name);
        AddParameter(command, "@file", directory.TrimEnd('\\', '/') +
            (directory.Contains('\\') ? "\\" : "/") + name + ".mdf");
        await command.ExecuteNonQueryAsync();
    }
}
