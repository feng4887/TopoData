using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SqlSugar;
/**************************************************************
** 标题: SQL Sugar 操作辅助类
** 描述: 添加nuget-SqlSugarCore
** 作者: 杜金旺 
** 日期: 2024-12-26
**************************************************************/
namespace auDASLib
{
    public enum DBCode
    {
        SqlLite    = 0,
        SQLServer  = 1,
        MySql      = 2,
        PostgreSQL = 3,
        Oracle     = 4,
        firebird   = 5,
        db2        = 6,
    }

    public class DBStrName
    {
        public static string SqlLite { get; }    =  "SqlLite" ;
        public static string SQLServer { get; }  = "Microsoft SQL Server" ;
        public static string MySql { get; }      =   "MySql" ;
        public static string PostgreSQL { get; } = "PostgreSQL" ;
        public static string Oracle { get; }     = "Oracle" ;
        public static string firebird { get; }   = "firebird" ;
        public static string db2 { get; }        = "db2" ;
    }

    /// <summary>
    /// SqlSugarc操作类
    /// </summary 
    [Description ("SqlSugar操作类")]
    public class SugarDao
    {
        public SugarDao()
        {
            DBCode dc = SugarDao.GetDbCode(ServerCfg.Instance.SqlConfigInfo.DbType);
            _dbtype = GetDbType(dc);
            _ConnectionString = GetConnectionString
            (
                dc,
                ServerCfg.Instance.SqlConfigInfo.DbName,
                ServerCfg.Instance.SqlConfigInfo.ServerName, 
                ServerCfg.Instance.SqlConfigInfo.Port, 
                ServerCfg.Instance.SqlConfigInfo.User,
                ServerCfg.Instance.SqlConfigInfo.Psw
            );
            sqlSugarClient = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _ConnectionString,
                DbType = _dbtype,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    PgSqlIsAutoToLower = false,  // 禁止强制小写     
                }
            });
        }
        public SugarDao(SQLCfg sQLCfg)
        {
            DBCode dc = SugarDao.GetDbCode(sQLCfg.DbType);
            _dbtype = GetDbType(dc);
            _ConnectionString = GetConnectionString(
                dc,
                sQLCfg.DbName,
                sQLCfg.ServerName,
                sQLCfg.Port,
                sQLCfg.User,
                sQLCfg.Psw);

            sqlSugarClient = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _ConnectionString,
                DbType = _dbtype,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    PgSqlIsAutoToLower = false,  // 禁止强制小写
                }
            });


        }

        public SugarDao(DBCode dbType,string userId, string password, string dbName, string ip, int? port = null)
        {
            // 如果没有指定端口，根据数据库类型使用默认端口
            int actualPort = port ?? GetDefaultPort(dbType);

            _dbtype = GetDbType(dbType);

            if (dbType == DBCode.SqlLite)
            {
                _ConnectionString = $"DataSource={dbName};";
            }
            else
            {
                _ConnectionString = GetConnectionString(dbType, dbName, ip, actualPort, userId, password);
            }

            sqlSugarClient = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _ConnectionString,
                DbType = _dbtype,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    PgSqlIsAutoToLower = false,  // 禁止强制小写
                }
            });

            //// AOP 方便调试
            //sqlSugarClient.Aop.OnLogExecuting = (sql, param) =>
            //{
            //    Console.WriteLine(sql);
            //};

            //// 错误处理
            //sqlSugarClient.Aop.OnError = exp =>
            //{
            //    Console.Error.WriteLine($"SQL Error: {exp.Message}");
            //};
        }

        public SqlSugar.DbType _dbtype = SqlSugar.DbType.Sqlite;
        public SqlSugar.DbType dbType
        {
            get { return _dbtype; }
            set { _dbtype = value; }
        }

        SqlSugarClient sqlSugarClient { get; set; }

        private string _ConnectionString
        {
            get;set;
        }

        /// <summary>
        /// 默认ACCESS数据库
        /// </summary>
        /// <returns></returns>
        public SqlSugarClient GetInstance()
        {
            return sqlSugarClient;
        }

        public void Close()
        {
            if (sqlSugarClient != null)
            { 
                sqlSugarClient.Close();
                sqlSugarClient.Dispose();
                sqlSugarClient= null;
            }
        }

        private int GetDefaultPort(DBCode dbType)
        {
            return dbType switch
            {
                DBCode.SQLServer  => 1433,
                DBCode.PostgreSQL => 5432,
                DBCode.MySql      => 3306,
                DBCode.Oracle     => 1521,
                DBCode.SqlLite    => 0, // SQLite 不需要端口
                _ => 1433            // 默认 SQL Server
            };
        }

        /// <summary>
        /// 测试连接特性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        static public bool IsConnected(DBCode type, string ConnectionString)
        {
            SqlSugar.DbType _type = SqlSugar.DbType.Sqlite;
            if (DBCode.SqlLite == type)
                _type = SqlSugar.DbType.Sqlite;
            else if (DBCode.SQLServer == type)
                _type = SqlSugar.DbType.SqlServer;
            else if (DBCode.MySql == type)
                _type = SqlSugar.DbType.MySql;
            else if (DBCode.PostgreSQL == type)
                _type = SqlSugar.DbType.PostgreSQL;
            else if (DBCode.Oracle == type)
                _type = SqlSugar.DbType.Oracle;

            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConnectionString,
                DbType = _type,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            }
                );
            bool ret = db.Ado.IsValidConnectionNoClose();
            db.Close();
            db.Dispose();
            return ret;
        }

        /// <summary>
        /// 测试连接特性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static async Task<PaeResult> IsConnected(DBCode type, string userId, string password, string dbName, string ip, int port = 1433)
        {
            var x = await Task.Run(() =>
            {
                PaeResult pr = new PaeResult();
                try
                {
                    SqlSugar.DbType _type = SqlSugar.DbType.Sqlite;

                    if (type == DBCode.SqlLite)
                        _type = SqlSugar.DbType.Sqlite;
                    else if (type == DBCode.SQLServer)
                        _type = SqlSugar.DbType.SqlServer;
                    else if (type == DBCode.MySql)
                        _type = SqlSugar.DbType.MySql;
                    else if (type == DBCode.PostgreSQL)
                        _type = SqlSugar.DbType.PostgreSQL;
                    else if (type == DBCode.Oracle)
                        _type = SqlSugar.DbType.Oracle;

                    string ConnectionString = GetConnectionString( type,dbName,ip,port,userId,password);
                    var db = new SqlSugarClient(new ConnectionConfig()
                    {
                        ConnectionString = ConnectionString,
                        DbType = _type,
                        IsAutoCloseConnection = true,
                        InitKeyType = InitKeyType.Attribute
                    }
                        );

                    bool ret = db.Ado.IsValidConnectionNoClose();

                    db.Close();
                    db.Dispose();

                    if (ret)
                        pr.IsSucess = true;
                    else
                        pr.IsSucess = false;

                    return pr;
                }
                catch (SqlSugar.SqlSugarException x)
                {
                    pr.ErrorMessage = x.Message;
                    return pr;
                }
            });
            return x;
        }

        /// <summary>
        /// 测试连接特性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool IsConnected()
        {
            bool ret = sqlSugarClient.Ado.IsValidConnectionNoClose();
            return ret;
        }

        public bool CreateDatabase(string databaseName, string databaseDirectory = null)
        {
            if(string.IsNullOrEmpty(databaseName))
                return false;
            if(databaseDirectory == null)
                return sqlSugarClient.DbMaintenance.CreateDatabase(databaseName);
            else
                return sqlSugarClient.DbMaintenance.CreateDatabase(databaseName, databaseDirectory);

        }

        public void CreateTable<T>()
        {
            sqlSugarClient.CodeFirst.InitTables<T>();
        }

        public static DBCode GetDbCode(string strDBType)
        {
            return strDBType switch
            {
                "MySql"      => DBCode.MySql,
                "Microsoft SQL Server" => DBCode.SQLServer,
                "PostgreSQL" => DBCode.PostgreSQL,
                "SqlLite"    => DBCode.SqlLite,
                "Oracle"     => DBCode.Oracle,
                "db2"        => DBCode.db2,
                "firebird"   => DBCode.firebird,
                _ => throw new ArgumentException("Unsupported database type")
            };
        }
        
        public static SqlSugar.DbType GetDbType(DBCode type)
        {
            SqlSugar.DbType _type = SqlSugar.DbType.Sqlite;

            if (DBCode.SqlLite == type)
                _type = SqlSugar.DbType.Sqlite;
            else if (DBCode.SQLServer == type)
                _type = SqlSugar.DbType.SqlServer;
            else if (DBCode.MySql == type)
                _type = SqlSugar.DbType.MySql;
            else if (DBCode.PostgreSQL == type)
                _type = SqlSugar.DbType.PostgreSQL;
            else if (DBCode.Oracle == type)
                _type = SqlSugar.DbType.Oracle;

            //else if (type == DBType.db2)
            //    _type = SqlSugar.DbType.db;
            //_type = SqlSugar.DbType.Oracle;

            return _type;
        }

        public static SqlSugar.DbType GetDbType(string type)
        {
            DBCode dc = GetDbCode(type);
            SqlSugar.DbType _type = SqlSugar.DbType.Sqlite;

            if (DBCode.SqlLite == dc)
                _type = SqlSugar.DbType.Sqlite;
            else if (DBCode.SQLServer == dc)
                _type = SqlSugar.DbType.SqlServer;
            else if (DBCode.MySql == dc)
                _type = SqlSugar.DbType.MySql;
            else if (DBCode.PostgreSQL == dc)
                _type = SqlSugar.DbType.PostgreSQL;
            else if (DBCode.Oracle == dc)
                _type = SqlSugar.DbType.Oracle;

            //else if (DBCode.db2 == dc)
            //    _type = SqlSugar.DbType.db2;

            return _type;
        }
        public static string GetConnectionString(DBCode dbType, string dbName, string ip,int port, string userId, string password)
        {
            return dbType switch
            {
                //DBCode.MySql => $"server={ip};Port={port};Database={dbName};Uid={userId};Pwd={password};",

                DBCode.MySql => $"Server={ip};Port={port};Database={dbName};Uid={userId};Pwd={password};Charset=utf8mb4;SslMode=Preferred;Pooling=true;",

                DBCode.SQLServer => string.Format(@"Server={0},{1};Database={2};User Id={3};Password={4};TrustServerCertificate=true", ip, port, dbName, userId, password),
                //DBCode.PostgreSQL => $"Host={ip};Port={port};Database={dbName};User Id={userId};Password={password};",

                DBCode.PostgreSQL =>
                    $"Host={ip};Port={port};Database={dbName};Username={userId};Password={password};" +
                    "SSL Mode=Prefer;Trust Server Certificate=true;Pooling=true;",


                DBCode.SqlLite => $"DataSource={dbName};",
                //(int)DBCode.Oracle => $"User Id={userId};Password={password};Data Source={ip}/{dbName};",
                DBCode.Oracle => $" Data Source= (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {ip})(PORT = {port})))(CONNECT_DATA = (SERVICE_NAME = {dbName})));User Id= {userId};Password= {password};Pooling= 'true';Max Pool Size = 150",
                DBCode.db2 => $"Server={ip}:50000;Database={dbName};UID={userId};PWD={password};",
                DBCode.firebird => $"User={userId};Password={password};Database={dbName};DataSource={ip};",
                _ => throw new ArgumentException("Unsupported database type")
            };
        }

        /// <summary>
        /// 获取执行语句。从工具导入的sql server执行脚本，脚本中带go语句
        /// </summary>
        /// <param name="content">脚本字符串</param>
        /// <returns>执行脚本集合</returns>
        public static string[] GetSqlStatementsfromScript(string content)
        {

            Regex reg = new Regex(@"\s*GO\s*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] sqlStatements = reg.Split(content);
            return sqlStatements;
        }

        public bool DatabaseExists(string databaseName)
        {
            if (sqlSugarClient == null) return false;

            try
            {
                switch (_dbtype)
                {
                    case SqlSugar.DbType.SqlServer:
                        return CheckSqlServer(databaseName);

                    case SqlSugar.DbType.MySql:
                        return CheckMySql(databaseName);

                    case SqlSugar.DbType.PostgreSQL:
                        return CheckPostgreSQL(databaseName);

                    case SqlSugar.DbType.Oracle:
                        return CheckOracle(databaseName);

                    case SqlSugar.DbType.Sqlite:
                        return CheckSqlite(databaseName);

                    default:
                        throw new NotSupportedException($"不支持的数据库类型: {sqlSugarClient.CurrentConnectionConfig.DbType}");
                }
            }
            catch (Exception ex)
            {
                // 记录日志
                Console.WriteLine($"检查数据库存在性失败: {ex.Message}");
                return false;
            }
        }

        private bool CheckSqlServer(string dbName)
        {
            string sql = "SELECT COUNT(*) FROM sys.databases WHERE name = @name";
            return sqlSugarClient.Ado.GetInt(sql, new { name = dbName }) > 0;
        }

        private bool CheckMySql(string dbName)
        {
            string sql = "SELECT COUNT(*) FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = @name";
            return sqlSugarClient.Ado.GetInt(sql, new { name = dbName }) > 0;
        }

        private bool CheckPostgreSQL(string dbName)
        {
            const string sql = "SELECT 1 FROM pg_database WHERE datname = @name LIMIT 1;";
            var res = sqlSugarClient.Ado.GetInt(sql, new { name = dbName });
            return res == 1;
        }

        private bool CheckOracle(string dbName)
        {
            try
            {
                // Oracle 需要权限，可能使用更简单的方法
                string sql = "SELECT COUNT(*) FROM all_users WHERE username = UPPER(@name)";
                return sqlSugarClient.Ado.GetInt(sql, new { name = dbName }) > 0;
            }
            catch
            {
                // 如果没有权限，尝试其他方法
                return false;
            }
        }

        private bool CheckSqlite(string dbName)
        {
            // SQLite 通常是文件数据库
            // 这里假设 connection string 中包含 Data Source
            string connString = sqlSugarClient.CurrentConnectionConfig.ConnectionString;

            // 从连接字符串中提取路径
            // 简单示例，实际需要更复杂的解析
            if (connString.Contains("Data Source="))
            {
                string path = connString.Split('=')[1].Trim();
                return File.Exists(path);
            }

            return false;
        }
    }

}
