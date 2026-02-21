using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace auDASLib
{
    public class ColumnInfo
    {
        public string ColumnName { get; set; }    // 列名
        public string DataType { get; set; }     // 数据类型
        public int? MaxLength { get; set; } = null;    // 最大长度（可选，适用于字符串类型）
        public bool IsNullable { get; set; } = true;   // 是否允许为空
        public bool IsIndexed { get; set; } = false;   // 是否需要索引
    }

    public class DBOperate
    {
        private SqlSugarClient _db;

        public DBOperate()
        {
            SugarDao sd =  new SugarDao();
            _db = sd.GetInstance();
        }

        public void Close()
        {
            if (_db != null)
            { 
                 _db.Close();  
                 _db.Dispose();
            }
        }

        public int CreateOrUpdateTable(string tableName, DataSample ds)
        {
            try
            {
                if (!IsTableExit(tableName))
                {
                  int r = CreateTable(tableName, ds);
                    if (r == 0)
                        return -2;
                    else return -1;
                }

                else
                {
                    int r = UpdateTable(tableName, ds);
                    return r;
                }
            }
            catch (Exception ex) 
            {
                throw (ex); 
            }
        }

        public string GetCreateTableSql(string tableName, List<DSTbDefine> columns, DbType dbType)
        {
            string createTableSql = $"CREATE TABLE {tableName} (";
            List<string> indexes = new List<string>();
            columns.Insert(0, new DSTbDefine() { Column = "WO", DataType = "string" ,MaxLength = 100});
            columns.Insert(0, new DSTbDefine() { Column = "DateTime", DataType = "datetime", IsNullable = false,IsIndexed = true });

            foreach (var column in columns)
            {
                string dataType = GetDataType(column.DataType, dbType,column.MaxLength);
                string nullable = column.IsNullable ? "NULL" : "NOT NULL";
                createTableSql += $"{column.Column} {dataType} {nullable}, ";

                // 如果是 datetime 类型并且需要索引
                if (column.IsIndexed)
                {
                    string indexSql = dbType switch
                    {
                        DbType.SqlServer  => $"CREATE NONCLUSTERED INDEX idx_{tableName} ON {tableName}({column.Column});",
                        DbType.PostgreSQL => $"CREATE INDEX idx_{tableName} ON {tableName}({column.Column});",
                        DbType.Sqlite     => $"CREATE INDEX idx_{tableName} ON {tableName}({column.Column});",
                        DbType.MySql      => $"CREATE INDEX idx_{tableName} ON {tableName}({column.Column});",
                        _ => throw new NotSupportedException($"Do not supported database type: {dbType}")
                    };
                    indexes.Add(indexSql);
                }
            }

            // 移除最后一个逗号和空格
            createTableSql = createTableSql.TrimEnd(',', ' ') + ");";

            // 添加索引语句
            foreach (var index in indexes)
            {
                createTableSql += " " + index;
            }
            columns.RemoveAt(0);
            columns.RemoveAt(0);
            return createTableSql;
        }

        private string GetDataType(string dataType, DbType dbType, int? maxLength = null)
        {
            string type = dbType switch
            {
                DbType.SqlServer => dataType switch
                {
                    "word" => "int",
                    "int16" => "int",
                    "int" => "int",
                    "float" => "float",
                    "double" => "float",
                    "string" => maxLength.HasValue ? $"nvarchar({maxLength.Value})" : "nvarchar(max)",
                    "wstring" => maxLength.HasValue ? $"nvarchar({maxLength.Value})" : "nvarchar(max)",
                    "bool" => "bit",
                    "datetime" => "datetime",
                    _ => "nvarchar(max)"
                },
                DbType.PostgreSQL => dataType switch
                {
                    "word" => "integer",
                    "int16" => "integer",
                    "int" => "integer",
                    "float" => "real",
                    "double" => "double precision", // PostgreSQL 的双精度浮点数
                    "string" => maxLength.HasValue ? $"varchar({maxLength.Value})" : "text",
                    "wstring" => maxLength.HasValue ? $"varchar({maxLength.Value})" : "text",
                    "bool" => "boolean",
                    "datetime" => "timestamp",
                    "timestamp" => "timestamp",
                    _ => "text"
                },
                DbType.Sqlite => dataType switch
                {
                    "word" => "integer",
                    "int16" => "integer",
                    "int" => "integer",
                    "float" => "real",
                    "double" => "real", // SQLite 的 real 就是双精度浮点数
                    "string" => maxLength.HasValue ? $"varchar({maxLength.Value})" : "text",
                    "wstring" => maxLength.HasValue ? $"varchar({maxLength.Value})" : "text",
                    "bool" => "integer", // SQLite 没有 boolean 类型，使用 integer
                    "datetime" => "datetime",
                    _ => "text"
                },
                DbType.MySql => dataType switch
                {
                    "word" => "int",
                    "int16" => "int",
                    "int" => "int",
                    "float" => "float",
                    "double" => "double", // MySQL 的 double 是双精度
                    "string" => maxLength.HasValue ? $"varchar({maxLength.Value})" : "varchar(255)",
                    "wstring" => maxLength.HasValue ? $"varchar({maxLength.Value})" : "varchar(255)",
                    "bool" => "tinyint(1)", // MySQL 使用 tinyint(1) 表示 boolean
                    "datetime" => "datetime",
                    _ => "varchar(2000)"
                },
                DbType.Oracle => dataType switch
                {
                    "word" => "number",
                    "int16" => "number",
                    "int" => "number",
                    "float" => "float",
                    "double" => "double precision", // Oracle 更明确的双精度类型
                    "string" => maxLength.HasValue ? $"varchar2({maxLength.Value})" : "varchar2(255)",
                    "wstring" => maxLength.HasValue ? $"varchar2({maxLength.Value})" : "varchar2(255)",
                    "bool" => "number(1)", // Oracle 使用 number(1) 表示 boolean
                    "datetime" => "date",
                    _ => "varchar2(2000)"
                },
                _ => throw new NotSupportedException($"不支持的数据库类型: {dbType}")
            };

            return type;
        }

        public int CreateTable(string tableName, DataSample ds) 
        {
            int Created = 0;

            if (ds == null)
            {
                return Created;
            }

            if (_db != null)
            {
                DBCode dc = SugarDao.GetDbCode(ServerCfg.Instance.SqlConfigInfo.DbType);
                var sugartp = SugarDao.GetDbType(dc);
                string newsql = GetCreateTableSql(tableName,ds.Samples, sugartp);
                if(sugartp == DbType.PostgreSQL)
                {
                    //newsql = newsql.Replace("nvarchar", "varchar");
                    //newsql = "\"" + newsql + "\"";
                }

                if (_db != null)
                    try 
                    { 
                        Created = _db.Ado.ExecuteCommand(newsql);
                        if (dc == DBCode.MySql && Created == 0)
                            Created = -1;
                    }
                    catch
                    {
                        Created = 0;
                    }

            }
            return Created;
        }

        public int UpdateTable(string tableName, DataSample ds)
        {
            if (!IsTableExit(tableName))
            {
                throw new Exception($"表 {tableName} 不存在");
            }

            List<ColumnInfo> columns = new List<ColumnInfo>();
            DBCode dc = SugarDao.GetDbCode(ServerCfg.Instance.SqlConfigInfo.DbType);
            DbType dbType = SugarDao.GetDbType(dc);

            foreach (var v in ds.Samples)
            {
                string dataType = GetDataType(v.DataType, dbType, v.MaxLength);
                columns.Add(new ColumnInfo()
                {
                    ColumnName = v.Column,
                    DataType = dataType,
                    MaxLength = v.MaxLength,
                    IsNullable = v.IsNullable,
                    IsIndexed = v.IsIndexed
                });
            }

            int addedCount = AddMissingColumns(_db, tableName, columns, dbType);
            return addedCount;
        }

        protected int AddMissingColumns(SqlSugarClient db, string tableName, List<ColumnInfo> columns, DbType dbType)
        {
            int addedCount = 0;

            foreach (var column in columns)
            {
                try
                { 
                     // 检查列是否存在
                    string checkColumnSql = dbType switch
                    {
                        DbType.SqlServer => @"
                            SELECT COUNT(*) 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName",
                        DbType.MySql => @"
                            SELECT COUNT(*) 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName",
                        DbType.PostgreSQL => @"
                            SELECT COUNT(*) 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName AND TABLE_SCHEMA = 'public'",
                        DbType.Sqlite => @"
                            SELECT COUNT(*) 
                            FROM pragma_table_info(@TableName) 
                            WHERE name = @ColumnName",
                        DbType.Oracle => @"
                            SELECT COUNT(*) 
                            FROM ALL_TAB_COLUMNS 
                            WHERE TABLE_NAME = UPPER(@TableName) AND COLUMN_NAME = UPPER(@ColumnName)",
                        _ => throw new NotSupportedException($"不支持的数据库类型: {dbType}")
                    };

                    int columnExists = db.Ado.GetInt(checkColumnSql, new { TableName = tableName, ColumnName = column.ColumnName });

                    if (columnExists == 0)
                    {
                        //string datatype = GetDataType(column.DataType, dbType, column.MaxLength);
                        // 动态生成添加列的 SQL
                        string alterTableSql = $@"
                            ALTER TABLE {tableName} ADD {column.ColumnName} {column.DataType}";

                        if (column.MaxLength.HasValue && column.DataType.ToUpper().Contains("CHAR"))
                        {
                            alterTableSql += $"({column.MaxLength.Value})";
                        }

                        alterTableSql += column.IsNullable ? " NULL" : " NOT NULL";

                        int ret = db.Ado.ExecuteCommand(alterTableSql);

                        if (dbType == DbType.MySql)
                        { 
                            if (ret == 0)
                                addedCount++;
                        }
                        else if (ret == -1)
                            addedCount++;
                    }               
                }
                catch (Exception ex)
                {

                }


            }

            return addedCount;
        }

        public int DeleteTable(string tableName)
        {
            DBCode dc = SugarDao.GetDbCode(ServerCfg.Instance.SqlConfigInfo.DbType);
            DbType dbType = SugarDao.GetDbType(dc);
            string sql = dbType switch
            {
                DbType.SqlServer  => $"DROP TABLE IF EXISTS {tableName};",
                DbType.MySql      => $"DROP TABLE IF EXISTS {tableName};",
                DbType.PostgreSQL => $"DROP TABLE IF EXISTS {tableName};",
                DbType.Sqlite => $"DROP TABLE IF EXISTS {tableName};",
                DbType.Oracle => $"DROP TABLE {tableName} PURGE",
                _ => throw new NotSupportedException($"Not Supported database type Exception: {dbType}")
            };

            int result = _db.Ado.ExecuteCommand(sql);
            return result;
        }

        public int DeleteColumn(string tableName, string columnName)
        {
            DBCode dc = SugarDao.GetDbCode(ServerCfg.Instance.SqlConfigInfo.DbType);
            DbType dbType = SugarDao.GetDbType(dc);
            string sql = dbType switch
            {
                DbType.SqlServer => $"ALTER TABLE {tableName} DROP COLUMN {columnName};",
                DbType.MySql     => $"ALTER TABLE {tableName} DROP COLUMN {columnName};",
                DbType.PostgreSQL => $"ALTER TABLE {tableName} DROP COLUMN {columnName};",
                DbType.Sqlite => $"ALTER TABLE {tableName} DROP COLUMN {columnName};",
                DbType.Oracle => $"ALTER TABLE {tableName} DROP COLUMN {columnName}",
                _ => throw new NotSupportedException($"Not Supported database type Exception: {dbType}")
            };

            int result = _db.Ado.ExecuteCommand(sql);

            if(dc == DBCode.MySql && result == 0) // MySQL 删除列成功时返回0; orcale sqlserver pg 返回-1
                result = -1;

            return result;
        }


        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public bool IsTableExit(string tbName)
        {
            DBCode dc = SugarDao.GetDbCode(ServerCfg.Instance.SqlConfigInfo.DbType);
            var sugartp = SugarDao.GetDbType(dc);

            string sql = sugartp switch
            {
                DbType.SqlServer => @"
                        SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = @TableName",
                DbType.MySql => @"
                        SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @TableName",
                DbType.PostgreSQL => @"
                        SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = @TableName AND TABLE_SCHEMA = 'public'",
                 DbType.Sqlite => @"
                        SELECT COUNT(*) 
                        FROM sqlite_master 
                        WHERE type = 'table' AND name = @TableName",
                DbType.Oracle => @"
                        SELECT COUNT(*) 
                        FROM ALL_TABLES 
                        WHERE TABLE_NAME = UPPER(@TableName)",

                _ => throw new NotSupportedException($"Not Supported database type Exception: {sugartp}")
            };

            int count = 0;
            if(sugartp == DbType.PostgreSQL)
                count = _db.Ado.GetInt(sql, new { TableName = tbName.ToLower() });
            else
                count = _db.Ado.GetInt(sql, new { TableName = tbName });

            return count > 0;
        }

        public static string GetCurrentDateTimeFunction(SqlSugar.DbType dbType, DateTime now)
        {
            string dateTimeStr = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return dbType switch
            {
                SqlSugar.DbType.SqlServer => $"'{dateTimeStr}'",
                SqlSugar.DbType.MySql => $"'{dateTimeStr}'",
                SqlSugar.DbType.PostgreSQL => $"'{dateTimeStr}'",
                SqlSugar.DbType.Sqlite => $"'{dateTimeStr}'",
                SqlSugar.DbType.Oracle => $"TO_DATE('{dateTimeStr}', 'YYYY-MM-DD HH24:MI:SS')",
                _ => throw new NotSupportedException($"不支持的数据库类型: {dbType}")
            };
        }

        public static string FormatBoolValue(DbType dbType, bool value)
        {
            switch (dbType)
            {
                case DbType.SqlServer:
                case DbType.MySql:
                case DbType.Sqlite:
                    return value ? "1" : "0";
                case DbType.PostgreSQL:
                    return value ? "TRUE" : "FALSE";
                default:
                    throw new NotSupportedException($"Unsupported database type: {dbType}");
            }
        }

    }
}
