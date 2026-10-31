// System controls correspond to TopoData.SubPage.ucSystemCfg.
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using auDASLib;
using auDASLib.Model;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class ucSystemCfg : BusinessPage
{
    private bool _isLoading = true;
    public ucSystemCfg()
    {
        InitializeComponent();
        cbDB.ItemsSource = new[] { "Microsoft SQL Server", "PostgreSQL", "MySql", "SqlLite" };
        cbDB.SelectedIndex = 0;
        UiText.Apply(this);
        _isLoading = false;
    }
    public ucSystemCfg(AppContextServices context) : this()
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
            var cfg = Context.Profile.Server;
            var sql = cfg.SqlConfigInfo;
            cbDB.SelectedItem = sql.DbType.Contains("lite", StringComparison.OrdinalIgnoreCase) ? "SqlLite"
                : sql.DbType.StartsWith("My", StringComparison.OrdinalIgnoreCase) ? "MySql" : sql.DbType;
            tbWdSQLName.Text = sql.ServerName;
            tbWdSQLDBName.Text = sql.DbName;
            tbWdSQLPort.Text = sql.Port.ToString();
            tbWdSQLUserName.Text = sql.User;
            tbWdSQLDPsw.Text = sql.Psw;
            tbMonth.Text = sql.StoreMonth.ToString();
            cbEableLog4.IsChecked = cfg.EnableLog4;
            tbRestURI.Text = cfg.RestUri1;
            cbActive.IsChecked = cfg.AutoStart;
            var mqtt = Context.Profile.Mqtt;
            tsEnableMqtt.IsChecked = mqtt.EnableMqtt;
            tsEnableMqttSecurity.IsChecked = mqtt.UseSecurity;
            tsEnableSingalTag.IsChecked = mqtt.PublishSingleTag;
            tbMqttHost.Text = mqtt.Server;
            tbMqttPort.Text = mqtt.Port.ToString();
            tbMqttUser.Text = mqtt.Username;
            tbMqttPsw.Text = mqtt.Password;
            tbMqttTopic.Text = mqtt.TopicPrefix;
            tbPublishIntervalMs.Text = mqtt.PublishIntervalMs.ToString();
            UpdateDatabaseControls();
            UpdateMqttControls();
        }
        finally { _isLoading = false; }
    }
    public override void Commit()
    {
        if (!HasRuntimeContext || Context.IsRunning) return;
        CommitDatabase();
        CommitMqtt();
        Context.Profile.Server.RestUri1 = tbRestURI.Text ?? "";
        Context.Profile.Server.AutoStart = cbActive.IsChecked == true;
    }
    private void CommitDatabase()
    {
        var type = cbDB.SelectedItem as string ?? "SqlLite";
        if (string.IsNullOrWhiteSpace(tbWdSQLDBName.Text)) throw new InvalidDataException("数据库不能为空。");
        var sqlite = type == "SqlLite";
        var port = sqlite && string.IsNullOrWhiteSpace(tbWdSQLPort.Text) ? 0
            : Ui.Integer(tbWdSQLPort, "数据库端口", sqlite ? 0 : 1, 65535);
        var months = Ui.Integer(tbMonth, "数据库保留时间", 0, 1200);
        var cfg = Context.Profile.Server.SqlConfigInfo;
        cfg.DbType = type;
        cfg.DbName = sqlite ? Path.GetFullPath(tbWdSQLDBName.Text!) : tbWdSQLDBName.Text!;
        cfg.ServerName = tbWdSQLName.Text ?? "";
        cfg.DataSource = cfg.ServerName;
        cfg.Port = port; cfg.User = tbWdSQLUserName.Text ?? ""; cfg.Psw = tbWdSQLDPsw.Text ?? "";
        cfg.StoreMonth = months;
        Context.Profile.Server.EnableLog4 = cbEableLog4.IsChecked == true;
    }
    private void CommitMqtt()
    {
        var port = Ui.Integer(tbMqttPort, "MQTT 端口", 1, 65535);
        var interval = Ui.Integer(tbPublishIntervalMs, "发布间隔", 100, 86400000);
        var cfg = Context.Profile.Mqtt;
        cfg.EnableMqtt = tsEnableMqtt.IsChecked == true;
        cfg.UseSecurity = tsEnableMqttSecurity.IsChecked == true;
        cfg.PublishSingleTag = tsEnableSingalTag.IsChecked == true;
        cfg.Server = tbMqttHost.Text ?? ""; cfg.Port = port;
        cfg.Username = tbMqttUser.Text ?? ""; cfg.Password = tbMqttPsw.Text ?? "";
        cfg.TopicPrefix = tbMqttTopic.Text ?? ""; cfg.PublishIntervalMs = interval;
    }
    private void cbDB_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (panelDb == null) return;
        UpdateDatabaseControls();
        if (_isLoading || !HasRuntimeContext) return;
        // 对应 InitDBCfg：SqlLite 清空并禁用服务器字段，其它类型从当前配置回填
        if (Equals(cbDB.SelectedItem, "SqlLite"))
        {
            tbDBPath.Text = "";
            tbWdSQLDPsw.Text = "";
            tbWdSQLUserName.Text = "";
            tbWdSQLName.Text = "";
            tbWdSQLPort.Text = "";
        }
        else
        {
            var sql = Context.Profile.Server.SqlConfigInfo;
            tbWdSQLName.Text = sql.ServerName;
            tbWdSQLDBName.Text = sql.DbName;
            tbWdSQLDPsw.Text = sql.Psw;
            tbWdSQLUserName.Text = sql.User;
            tbWdSQLPort.Text = sql.Port.ToString();
            tbMonth.Text = sql.StoreMonth.ToString();
            tbRestURI.Text = Context.Profile.Server.RestUri1;
        }
    }
    private void UpdateDatabaseControls()
    {
        var sqlite = Equals(cbDB.SelectedItem, "SqlLite");
        panelDb.IsVisible = !sqlite;
        panelRoot.IsVisible = Equals(cbDB.SelectedItem, "Microsoft SQL Server");
        btSelectSqLiteDbFile.IsVisible = sqlite;
        tbDBPath.IsEnabled = !sqlite;
        tbWdSQLDPsw.IsEnabled = !sqlite;
        tbWdSQLUserName.IsEnabled = !sqlite;
        tbWdSQLName.IsEnabled = !sqlite;
        tbWdSQLPort.IsEnabled = !sqlite;
    }
    private void tsEnableSingalTag_IsCheckedChanged(object? sender, RoutedEventArgs e) => UpdateMqttControls();
    private void UpdateMqttControls()
    {
        if (tbMqttTopic != null) tbMqttTopic.IsEnabled = tsEnableSingalTag.IsChecked != true;
    }
    private async void cbActive_IsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (_isLoading || !HasRuntimeContext) return;
        await RunActionAsync(sender, async () =>
        {
            Context.EnsureStopped();
            Context.Profile.Server.AutoStart = cbActive.IsChecked == true;
            Context.Save();
            // 原版 cbActive_Toggled 同时创建/删除桌面快捷方式。
            DesktopStartupService.SetDesktopShortcut(cbActive.IsChecked == true, Context.Store.DirectoryPath);
            await Task.CompletedTask;
        });
    }
    private async void btSaveSQL_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, () => { CommitDatabase(); Context.Save(); });
    private async void btSaveMqtt_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        if (!await ValidateMqttNumbersAsync()) return;
        CommitMqtt();
        Context.Save();
    });
    private async void btSaveRestURI_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, () =>
    {
        // 原版不做地址格式校验，直接写回配置
        Context.Profile.Server.RestUri1 = tbRestURI.Text ?? "";
        Context.Save();
    });
    /// <summary>Mirrors btTestSQL_Click: tests the typed values with SugarDao.IsConnected.</summary>
    private async void btTestSQL_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        var type = cbDB.SelectedItem as string ?? "SqlLite";
        var port = type == "SqlLite" ? 1433 : int.Parse(tbWdSQLPort.Text ?? "1433");
        var result = await SugarDao.IsConnected(SugarDao.GetDbCode(type), tbWdSQLUserName.Text ?? "",
            tbWdSQLDPsw.Text ?? "", tbWdSQLDBName.Text ?? "", tbWdSQLName.Text ?? "", port);
        await Ui.MessageAsync(this, "Communication Test",
            result.IsSucess ? "Database communication is good." : "Database communication failed");
    });
    private async void btTestMqtt_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        if (!await ValidateMqttNumbersAsync()) return;
        CommitMqtt();
        try
        {
            await MqttService.TestAsync(Context.Profile.Mqtt);
            await Ui.MessageAsync(this, "Communication Test", "MQTT server is good.");
        }
        catch (Exception)
        {
            await Ui.MessageAsync(this, "Communication Test", "MQTT server communication failed");
        }
    });

    /// <summary>Mirrors the original numeric checks and messages before saving or testing MQTT.</summary>
    private async Task<bool> ValidateMqttNumbersAsync()
    {
        if (!int.TryParse(tbMqttPort.Text, out _))
        {
            await Ui.MessageAsync(this, "Information", "Mqtt Port should be number.");
            return false;
        }
        if (!int.TryParse(tbPublishIntervalMs.Text, out _))
        {
            await Ui.MessageAsync(this, "Information", "Mqtt Publish Interval should be number.");
            return false;
        }
        return true;
    }
    /// <summary>Mirrors btCreateDB_Click: SQLite asks for a file, SQL Server needs a storage path;
    /// after creation the legacy application log and history tables are created as well.</summary>
    private async void btCreateDB_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, CreateDatabaseAsync);

    private async Task CreateDatabaseAsync()
    {
        var type = cbDB.SelectedItem as string ?? "SqlLite";
        var port = type == "SqlLite" ? 1433 : int.Parse(tbWdSQLPort.Text ?? "1433");
        string database;
        if (type == "SqlLite")
        {
            var path = await Ui.SaveAsync(this, "创建数据库", "topo.db", "db");
            if (path == null) return;
            tbWdSQLDBName.Text = path;
            database = path;
        }
        else
            database = tbWdSQLDBName.Text ?? "";

        var master = type switch { "PostgreSQL" => "postgres", "MySql" => "mysql", _ => "master" };
        var code = SugarDao.GetDbCode(type);
        var user = tbWdSQLUserName.Text ?? "";
        var password = tbWdSQLDPsw.Text ?? "";
        var host = tbWdSQLName.Text ?? "";
        var directory = tbDBPath.Text ?? "";
        SugarDao? masterDb = null;
        SugarDao? targetDb = null;
        try
        {
            masterDb = new SugarDao(code, user, password, master, host, port);
            var created = false;
            if (!masterDb.DatabaseExists(database))
            {
                if (type == "Microsoft SQL Server" && string.IsNullOrEmpty(directory))
                {
                    await Ui.MessageAsync(this, "Information", "Please fill Database storage path .");
                    return;
                }
                created = masterDb.CreateDatabase(database, type == "SqlLite" ? null : directory);
                if (created && type == "Microsoft SQL Server")
                {
                    // 原版创建后立即调整数据文件与日志文件的增长策略。
                    var client = masterDb.GetInstance();
                    client.Ado.ExecuteCommand($@"ALTER DATABASE [{database}] MODIFY FILE 
                        ( NAME = N'{database}', MAXSIZE = UNLIMITED, FILEGROWTH = 100MB );");
                    client.Ado.ExecuteCommand($@"ALTER DATABASE [{database}] MODIFY FILE 
                        ( NAME = N'{database}_log', MAXSIZE = UNLIMITED, FILEGROWTH = 100MB );");
                }
            }
            masterDb.Close();
            masterDb = null;

            targetDb = new SugarDao(code, user, password, database, host, port);
            if (targetDb.IsConnected())
            {
                var client = targetDb.GetInstance();
                client.CodeFirst.SetStringDefaultLength(200);
                client.CodeFirst.InitTables(typeof(AppLogMessage), typeof(pubHisValues));
                // 原版仅在本次真正新建数据库时提示成功。
                if (created) await Ui.MessageAsync(this, "Create Table", "Create Database Sucessful.");
            }
            else
                await Ui.MessageAsync(this, "Create Table", "Create table fail");
        }
        catch (Exception ex)
        {
            await Ui.MessageAsync(this, "Create Database fail", ex.Message);
        }
        finally
        {
            masterDb?.Close();
            targetDb?.Close();
        }
    }
    private async void btSelectSqLiteDbFile_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Please select db file", AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType("DB file") { Patterns = ["*.db", "*.db3", "*.sqlite", "*.sqlite3"] }]
        });
        var path = files.FirstOrDefault()?.TryGetLocalPath();
        if (path != null) tbWdSQLDBName.Text = path;
    });
    private IStorageProvider StorageProvider => TopLevel.GetTopLevel(this)!.StorageProvider;
    private async void btSelectDbPath_Click(object? sender, RoutedEventArgs e) => await RunActionAsync(sender, async () =>
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { Title = "请选择文件夹", AllowMultiple = false });
        var path = folders.FirstOrDefault()?.TryGetLocalPath();
        if (path != null) tbDBPath.Text = path;
    });
}
