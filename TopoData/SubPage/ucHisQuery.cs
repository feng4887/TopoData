using auDASLib;
using auDASLib.Model;
using DevExpress.Data;
using DevExpress.Data.Mask;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraSpreadsheet.TileLayout;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraWaitForm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.model;
using TopoData.Properties;
using TopoData.SubPage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TopoData.Page
{
    public partial class ucHisQuery : DevExpress.XtraEditors.XtraUserControl
    {
        protected pWO _pWO;
        DataTable _dtData = new DataTable();
        DataTable _dtWo = new DataTable();

        SugarDao db;
        SqlSugarClient _sugarClient;
        VirtualServerModeSource virtualServerModeSource = new VirtualServerModeSource();

        public ucHisQuery()
        {
            InitializeComponent();
        }
        private void ucHisQuery_Load(object sender, EventArgs e)
        {
            dtEtime.EditValue = DateTimeOffset.Now;
            dtStime.EditValue = DateTimeOffset.Now.AddDays(-1);


            //// Creates and initializes a virtual data source.
            //virtualServerModeSource.RowType = typeof(DataTable);
            ////virtualServerModeSource.RowType = typeof(Product);
            //virtualServerModeSource.ConfigurationChanged += virtualServerModeSource_ConfigurationChanged;
            //virtualServerModeSource.MoreRows += virtualServerModeSource_MoreRows;

            //gridControl1.DataSource = virtualServerModeSource;
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            btExport.Text = Resources.Export;
            btSearch.Text = Resources.Search;
            label3.Text = Resources.TimeStart;
            label4.Text = Resources.TimeEnd;
            //toolStripStatusLabel1.Text = Resources.msgRptQuerySucess;
            radioGroup1.Properties.Items[0].Description = Resources.DataTable;
            radioGroup1.Properties.Items[1].Description = Resources.TimeScaleTable;
        }

        int batchSize = 20;
        int maxRowCount = 500;

        private void virtualServerModeSource_ConfigurationChanged(object sender, DevExpress.Data.VirtualServerModeRowsEventArgs e)
        {
            //e.UserData = GetRows(0);
        }

        private void virtualServerModeSource_MoreRows(object sender, DevExpress.Data.VirtualServerModeRowsEventArgs e)
        {
            e.RowsTask = System.Threading.Tasks.Task.Factory.StartNew(() => {
                bool moreRows = e.CurrentRowCount < maxRowCount - batchSize;
                return new VirtualServerModeRowsTaskResult(GetRows(e.CurrentRowCount), moreRows, e.UserData);
            }, e.CancellationToken);
        }


        List<Product> GetRows(int startRowIndex)
        {
            List<Product> lst = new List<Product>();
            for (int i = startRowIndex; i < startRowIndex + batchSize; i++)
                lst.Add(new Product { ID = i, Name = $"Product{i}" });
            return lst;
        }

        #region [query]
        DataTable dt = null;
        private async void btSearch_Click(object sender, EventArgs e)
        {

            // 立即移除点击事件处理器
            // btSearch.Click -= btSearch_Click;
            bool bRealtimeQyery = (radioGroup1.SelectedIndex == 1); // 实时查询
            string taskName = "";// cbRealtimeTask.Text;
            string strSQL = "";
            string tableName = "";
            DataSample dataSample = null;

            if (ServerCfg.Instance == null || ServerCfg.Instance.dataSamples == null)
                return;

            if (ServerCfg.Instance.dataSamples.Count > 0)
            {
                taskName = ServerCfg.Instance.dataSamples[0].TaskName;
                tableName = ServerCfg.Instance.dataSamples[0].TableName;
                dataSample = ServerCfg.Instance.dataSamples[0];
            }
            else
                return;

            DBCode dc = SugarDao.GetDbCode(ServerCfg.Instance.SqlConfigInfo.DbType);
            string strtype = (ServerCfg.Instance.SqlConfigInfo.DbType == null ? auDASLib.DBStrName.SQLServer : ServerCfg.Instance.SqlConfigInfo.DbType);
            SqlSugar.DbType dbType = SugarDao.GetDbType(strtype);
            db = new SugarDao(
                   dc,
                   ServerCfg.Instance.SqlConfigInfo.User,
                   ServerCfg.Instance.SqlConfigInfo.Psw,
                   ServerCfg.Instance.SqlConfigInfo.DbName,
                   ServerCfg.Instance.SqlConfigInfo.ServerName,
                   ServerCfg.Instance.SqlConfigInfo.Port
                );
            _sugarClient = db.GetInstance();
            string stime = string.Format("{{{0:yyyy/M/d H:mm:ss}}}", dtStime.EditValue).Trim('{', '}');
            string etime = string.Format("{{{0:yyyy/M/d H:mm:ss}}}", dtEtime.EditValue).Trim('{', '}');

            using (WaitForm waiting = new WaitForm())
            {
                waiting.StartPosition = FormStartPosition.CenterScreen;

                //<如果选择了指定任务，获取任务 表列名明细>

                // 1.实时历史数据表
                if (bRealtimeQyery)
                {
                    tableName = "tb_au_pubHisValues";
                    List<string> tags = new List<string>();

                    if (dataSample != null && dataSample.Samples?.Count > 0)
                    {
                        tags = dataSample.Samples.Where(s => s.DataType.Equals("float", StringComparison.OrdinalIgnoreCase)
                                        || s.DataType.Equals("int", StringComparison.OrdinalIgnoreCase)
                                        || s.DataType.Equals("bool", StringComparison.OrdinalIgnoreCase)
                                        || s.DataType.Equals("string", StringComparison.OrdinalIgnoreCase)
                                        || s.DataType.Equals("wstring", StringComparison.OrdinalIgnoreCase)
                                        && s.Column != "DateTime" && s.Column != "WO"
                                        )
                            .Select(s => s.Column).ToList();
                    }

                    //</如果选择了指定任务，获取任务 表列名明细>

                    strSQL = Get_tb_au_pubHisValuesQuery(dbType, tableName, stime, etime, tags);
                }

                //2.普通任务表
                else
                {
                    if ("tb_au_pubHisValues" != tableName && !string.IsNullOrWhiteSpace(tableName))
                    {
                        //按普通任务表查询
                        strSQL = GetFormattedDateTimeQuery(dbType, tableName, stime, etime);
                        //strSQL = $"select FORMAT(Datetime, 'yyyy-MM-dd HH:mm:ss.ffff')  as Datetime, %_% from {tableName}   WITH(NOLOCK)  where datetime >= '{dtStime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' and datetime <= '{dtEtime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' order by datetime desc";     
                        string items = string.Join(",", dataSample.Samples.Select(it => it.Column).ToArray());
                        if (string.IsNullOrEmpty(items))
                        {
                            //diaMessageY mb = new diaMessageY(Resources.msgNoTagFail, Resources.PromptMessage);
                            //mb.ShowDialog();
                            //MessageBox.Show("查询任务没有字段，查询失败。");
                            DevExpress.XtraEditors.XtraMessageBox.Show("查询任务没有字段，查询失败。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        dynamic columns;

                        try
                        {
                            // <获取表字段信息>
                            columns = _sugarClient.DbMaintenance.GetColumnInfosByTableName(tableName).Select(it => it.DbColumnName).ToList();
                            if (columns.Count == 0)
                            {
                                //diaMessageY mb = new diaMessageY(Resources.msgNoTableQFail, Resources.PromptMessage);
                                //mb.ShowDialog();
                                //MessageBox.Show("数据库表不存在，或者查询数据库没有字段。查询失败。");
                                DevExpress.XtraEditors.XtraMessageBox.Show("数据库表不存在，或者查询数据库没有字段。查询失败。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }


                        List<string> v = new List<string>();
                        Dictionary<string, DSTbDefine> itsDict = dataSample.Samples.GroupBy(it => it.Column)
                            .ToDictionary(
                                g => g.Key,
                                g => g.First()  // 保留第一个出现的元素
                                , StringComparer.OrdinalIgnoreCase
                            );

                        foreach (var column in columns)
                        {
                            if (column != null && itsDict.ContainsKey(column))
                            {
                                //GetRoundedFloatField(SqlSugar.DbType dbType, string fValue = "fValue")
                                if (itsDict[column].DataType.Equals("float", StringComparison.OrdinalIgnoreCase)
                                    || itsDict[column].DataType.Equals("double", StringComparison.OrdinalIgnoreCase)
                                    || itsDict[column].DataType.Equals("reale", StringComparison.OrdinalIgnoreCase)
                                    )
                                {
                                    v.Add(GetRoundedFloatField(dbType, column));
                                }
                                else
                                {
                                    v.Add(column);
                                }
                            }
                        }

                        if (v.Count == 0)
                        {
                            //MessageBox.Show("查询任务字段和实际数据库字段不符。查询失败。");
                            //diaMessageY mb = new diaMessageY(Resources.msgNoMatchQFail, Resources.PromptMessage);
                            //mb.ShowDialog();
                            DevExpress.XtraEditors.XtraMessageBox.Show("查询任务字段和实际数据库字段不符。查询失败。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string sqlitems = "";
                        if (v.Count > 1)
                            sqlitems = string.Join(",", v);
                        else if (v.Count == 1)
                            sqlitems = v[0];
                        else
                            return;

                        strSQL = strSQL.Replace("%_%", sqlitems);
                    }
                }

                //3. 查询并显示数据
                try
                {
                    // 查询数据
                    if (!string.IsNullOrWhiteSpace(strSQL))
                        dt = await _sugarClient.Ado.GetDataTableAsync(strSQL);

                    // 切回 UI 线程
                    var tcs = new TaskCompletionSource<bool>();
                    this.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            bindSheet(dt);
                            tcs.SetResult(true);
                        }
                        catch (Exception ex)
                        {
                            tcs.SetException(ex);
                        }

                    }));

                    //await tcs.Task;
                    waiting.Close();

                }
                catch (Exception ex)
                {
                    //diaMessageY mb = new diaMessageY(ex.Message, Resources.msgAttributeError1);
                    //DialogResult dr = mb.ShowDialog();
                    DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "infor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 完成后重新添加事件处理器并启用按钮
                    btSearch.Enabled = true;
                    // btSearch.Click += btSearch_Click;
                }
            }
        }

        private void bindSheet(DataTable dt)
        {
            ClearAllSheet();
            _dtData.Clear();

            if (dt != null && dt.Rows.Count >= 0)
            {
                _dtData = dt.Copy();

                //virtualServerModeSource.RowType = typeof(DataTable);
                gridControl1.BeginUpdate();
                gridControl1.DataSource = _dtData;
                gridControl1.EndUpdate();
                // 判断是否有数据
                if (gridView1.RowCount > 0 && gridView1.Columns.Count > 0)
                {
                    var firstColumn = gridView1.Columns[0];

                    // 设置最小列宽
                    firstColumn.MinWidth = 155;   // 你需要的最小值

                    // 如果当前宽度比最小值小，可以顺便拉大
                    if (firstColumn.Width < firstColumn.MinWidth)
                        firstColumn.Width = firstColumn.MinWidth;
                }
            }
        }

        /// <summary>
        /// 清空表数据
        /// </summary>
        private void ClearAllSheet()
        {
            gridView1.Columns.Clear();
            DataTable dt = new DataTable();
            this.gridControl1.DataSource = dt;
            gridControl1.Refresh();
        }

        private string GetRoundedFloatField(SqlSugar.DbType dbType, string fValue = "fValue")
        {
            switch (dbType)
            {
                case SqlSugar.DbType.PostgreSQL:
                    return $"ROUND({fValue}::numeric, 3) AS {fValue}";

                default:
                    return $"ROUND({fValue}, 3) AS {fValue}";
            }
        }

        public string GetFormattedDateTimeQuery(SqlSugar.DbType dbType, string tableName, string startDate, string endDate)
        {
            string query = dbType switch
            {
                SqlSugar.DbType.SqlServer => $@"
                    SELECT FORMAT(Datetime, 'yyyy-MM-dd HH:mm:ss.fff') AS Datetime ,WO, %_% 
                    FROM {tableName} WITH(NOLOCK)
                    WHERE Datetime >= '{startDate}' AND Datetime <= '{endDate}'
                    ORDER BY Datetime DESC;",

                SqlSugar.DbType.MySql => $@"
                    SELECT DATE_FORMAT(Datetime, '%Y-%m-%d %H:%i:%s.%f') AS Datetime, WO, %_% 
                    FROM {tableName}
                    WHERE Datetime >= '{startDate}' AND Datetime <= '{endDate}'
                    ORDER BY Datetime DESC;",

                SqlSugar.DbType.PostgreSQL => $@"
                    SELECT TO_CHAR(Datetime, 'YYYY-MM-DD HH24:MI:SS.MS') AS Datetime, WO, %_% 
                    FROM {tableName}
                    WHERE Datetime >= '{startDate}' AND Datetime <= '{endDate}'
                    ORDER BY Datetime DESC;",

                SqlSugar.DbType.Sqlite => $@"
                    SELECT strftime('%Y-%m-%d %H:%M:%f', Datetime) AS Datetime, WO, %_% 
                    FROM {tableName}
                    WHERE Datetime >= '{startDate}' AND Datetime <= '{endDate}'
                    ORDER BY Datetime DESC;",

                SqlSugar.DbType.Oracle => $@"
                    SELECT TO_CHAR(Datetime, 'YYYY-MM-DD HH24:MI:SS.FF') AS Datetime, WO, %_% 
                    FROM {tableName}
                    WHERE Datetime >= TO_TIMESTAMP('{startDate}', 'YYYY-MM-DD HH24:MI:SS')
                      AND Datetime <= TO_TIMESTAMP('{endDate}', 'YYYY-MM-DD HH24:MI:SS')
                    ORDER BY Datetime DESC;",

                _ => throw new NotSupportedException($"不支持的数据库类型: {dbType}")
            };

            return query;
        }

        public string Get_tb_au_pubHisValuesQuery(SqlSugar.DbType dbType, string tableName, string startDate, string endDate, List<string> tags)
        {
            string timeField = dbType switch
            {
                SqlSugar.DbType.SqlServer => "FORMAT(dateTime, 'yyyy-MM-dd HH:mm:ss.fff') AS dateTime",
                SqlSugar.DbType.MySql => "DATE_FORMAT(dateTime, '%Y-%m-%d %H:%i:%s.%f') AS dateTime",
                SqlSugar.DbType.PostgreSQL => "TO_CHAR(dateTime, 'YYYY-MM-DD HH24:MI:SS.MS') AS dateTime",
                SqlSugar.DbType.Sqlite => "strftime('%Y-%m-%d %H:%M:%f', dateTime) AS dateTime",
                SqlSugar.DbType.Oracle => "TO_CHAR(dateTime, 'YYYY-MM-DD HH24:MI:SS.FF') AS dateTime",
                _ => throw new NotSupportedException($"不支持的数据库类型: {dbType}")
            };

            string dateCondition = dbType switch
            {
                SqlSugar.DbType.Oracle => $@"
                    dateTime >= TO_TIMESTAMP('{startDate}', 'YYYY-MM-DD HH24:MI:SS') 
                    AND dateTime <= TO_TIMESTAMP('{endDate}', 'YYYY-MM-DD HH24:MI:SS')",

                _ => $@"
                    dateTime >= '{startDate}' 
                    AND dateTime <= '{endDate}'"
            };

            // TagN 过滤：将 List<string> tags 生成 IN 子句
            //    - 去除空白、去重
            //    - 转义单引号（' -> ''）
            //    - 如果最终集合为空，不加条件
            string tagFilter = string.Empty;
            if (tags != null && tags.Count > 0)
            {
                var cleaned = tags
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase) // 去重（忽略大小写）
                    .Select(s => s.Replace("'", "''"))           // 转义单引号，避免注入/语法错误
                    .ToList();

                if (cleaned.Count > 0)
                {
                    string inList = string.Join(",", cleaned.Select(s => $"'{s}'"));
                    tagFilter = $" AND TagN IN ({inList})";
                }
            }

            //保留3位有效小数
            string fValueField = GetRoundedFloatField(dbType);

            //fValue
            string query = $@"
                SELECT {timeField},
                    TagID,
                    TagN,
                    {fValueField},
                    bValue,
                    strValue,
                    uom,
                    wo_no,
                    unit_id,
                    seq_name
                FROM {tableName} 
                WHERE {dateCondition}
                 {tagFilter} 
                ORDER BY dateTime DESC ,TagN;
            ";

            return query;
        }
        #endregion //[query]

        private void btExport_Click(object sender, EventArgs e)
        {
            if (_dtData == null || _dtData.Rows.Count == 0)
            {
                //new diaMessageY(Resources.NoDataReport, Resources.PromptMessage).ShowDialog();
                DevExpress.XtraEditors.XtraMessageBox.Show("No data to export.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 创建保存对话框
            SaveFileDialog saveDataSend = new SaveFileDialog();
            // Environment.SpecialFolder.MyDocuments 表示在我的文档中
            saveDataSend.RestoreDirectory = true;
            //saveDataSend.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);   // 获取文件路径
            saveDataSend.Filter = "*.xlsx|Excel file";   // 设置文件类型为文本文件
            saveDataSend.DefaultExt = ".xlsx";   // 默认文件的拓展名
            saveDataSend.FileName = $"Data_{System.DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";   // 文件默认名
            if (saveDataSend.ShowDialog() == DialogResult.OK)   // 显示文件框，并且选择文件
            {
                string fName = saveDataSend.FileName;   // 获取文件名
                                                        // 参数1：写入文件的文件名；参数2：写入文件的内
                this.BeginInvoke(new Action(() =>
                {
                    using (waitForm waiting = new waitForm())
                    {
                        waiting.StartPosition = FormStartPosition.CenterScreen;
                        waiting.Show();
                        waiting.Update();  // 这一步很关键

                        string strSQL = "";
                        string tableName = "";

                        try
                        {
                            ExcelHelper eh = new ExcelHelper();

                            eh.OutputRoot = fName;
                            eh.Ds.Tables.Add(dt.Copy());
                            eh.SaveDsWithHeader(true);
                            //MessageBox.Show("共导出 " + dtl.Rows.Count.ToString() + " 条数据。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // new diaMessageY(Resources.msgExport1 + dt.Rows.Count.ToString() + Resources.msgExport2, Resources.PromptMessage).ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            //new diaMessageY(Resources.msgExportExcelFail + "\r\n" + ex.Message + "\r\n" + ex.InnerException, Resources.PromptMessage).ShowDialog();
                        }
                        finally
                        {
                            //tcs.SetResult(true);
                        }
                    }
                }));

            }
        }

        private void virtualServerModeSource1_MoreRows(object sender, DevExpress.Data.VirtualServerModeRowsEventArgs e)
        {

        }

        private void virtualServerModeSource1_ConfigurationChanged(object sender, DevExpress.Data.VirtualServerModeRowsEventArgs e)
        {
            var x = e.ConfigurationInfo;
            //var enumerator = GetItems(e.ConfigurationInfo).GetEnumerator();
            //e.UserData = enumerator;
        }

        private void virtualServerModeSource1_GetUniqueValues(object sender, DevExpress.Data.VirtualServerModeGetUniqueValuesEventArgs e)
        {

        }
    }
}
