using auDASLib;
using DevExpress.Data;
using DevExpress.XtraEditors;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.model;
using TopoData.Properties;
using TopoData.SubPage;
using static OfficeOpenXml.ExcelErrorValue;


namespace TopoData.Page
{
    public partial class ucDiagnose : DevExpress.XtraEditors.XtraUserControl
    {
        public ucDiagnose()
        {
            InitializeComponent();
        }

        private void ucDiagnose_Load(object sender, EventArgs e)
        {
            gridControl1.Dock = DockStyle.Fill;
            timer1.Start();
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            this.Text = Resources.diaTitle;
            //listView1.Columns[0].Text = Resources.Num;
            //listView1.Columns[1].Text = Resources.TagName;
            //listView1.Columns[2].Text = Resources.Value;
            //listView1.Columns[3].Text = Resources.UOM;
            //listView1.Columns[4].Text = Resources.Quality;
            //listView1.Columns[5].Text = Resources.Time;
            label1.Text = Resources.TagName;
            label2.Text = Resources.Recipe;
        }

        ObservableCollection<RealTimeValue> _RealtimeValues;
        DevExpress.Data.RealTimeSource _myRealTimeSource = new DevExpress.Data.RealTimeSource();

        private async void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                List<string> cannels = ServerCfg.Instance.cDevices.Where(it => it.iActive == 1).Select(x => x.CannelID).ToList();
                if (cannels == null || cannels.Count == 0)
                    return;

                List<string> TagIDs = new List<string>();

                foreach (var v in cannels)
                {
                    if (DataImport.dicCannelTags.ContainsKey(v))
                        TagIDs.AddRange(DataImport.dicCannelTags[v].Select(it => it.TagId).ToList());
                }

                if (TagIDs == null || TagIDs.Count == 0)
                    return;

                TagIDs = FilterTagIDs(TagIDs);
                if (TagIDs == null || TagIDs.Count == 0)
                    return;

                //------------------------------------------

                List<RealTimeValue> rtvs = new List<RealTimeValue>();
                if (TagIDs != null && TagIDs.Count > 0)
                {
                    foreach (var item in TagIDs)
                    {
                        RealTimeValue realTimeValue = new RealTimeValue();
                        var vl = auDAServer.ItemPool.ValuePool.TryGetValue(item, out realTimeValue);
                        if (vl && realTimeValue != null)
                            rtvs.Add(realTimeValue);
                    }

                }

                // 首次初始化（仅一次）
                if (_RealtimeValues == null)
                {
                    _RealtimeValues = new ObservableCollection<RealTimeValue>();
                    foreach (var item in rtvs)
                    {
                        _RealtimeValues.Add(item);
                    }
                    _myRealTimeSource.DataSource = _RealtimeValues;
                    gridControl1.DataSource = _myRealTimeSource.DataSource;
                }
                else
                {
                    // 保存当前滚动位置
                    int topRowHandle = gridView1.TopRowIndex;

                    // 增量更新数据（不清空，保持现有数据结构）
                    UpdateRealtimeValues(rtvs);

                    // 恢复滚动位置
                    if (topRowHandle >= 0 && topRowHandle < gridView1.RowCount)
                    {
                        gridView1.TopRowIndex = topRowHandle;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 增量更新实时值（避免 Clear 导致滚动重置）
        /// </summary>
        private void UpdateRealtimeValues(List<RealTimeValue> newValues)
        {
            if (_RealtimeValues == null || newValues == null)
                return;

            // 构建新值的查找字典（按 ValueName 作为 key）
            var newDict = newValues.ToDictionary(v => v.ValueName, v => v);

            // 1：仅更新现有项的值，不改变顺序
            for (int i = 0; i < _RealtimeValues.Count; i++)
            {
                if (newDict.TryGetValue(_RealtimeValues[i].ValueName, out var newVal))
                {
                    // 替换整个对象或逐字段更新
                    _RealtimeValues[i] = newVal;
                    newDict.Remove(_RealtimeValues[i].ValueName);
                }
            }

            // 2：追加新增的项到末尾
            foreach (var newVal in newDict.Values)
            {
                _RealtimeValues.Add(newVal);
            }
        }

        /// <summary>
        /// 根据 strFilter 对 TagIDs 做模糊过滤（多关键词，AND 关系，忽略大小写）
        /// </summary>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        private List<string> FilterTagIDs(List<string> tagIds)
        {
            if (tagIds == null || tagIds.Count == 0)
                return tagIds ?? new List<string>();

            var filter = strFilter?.Trim();
            if (string.IsNullOrEmpty(filter))
                return tagIds;

            // 用空格分隔多个关键词，所有关键词都要匹配（可改为 Any 实现 OR）
            var terms = filter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .Where(t => t.Length > 0)
                              .ToArray();

            if (terms.Length == 0)
                return tagIds;

            return tagIds
                .Where(id => terms.All(term => id?.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();
        }

        private string strFilter = "";

        /// <summary>
        /// 过滤按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTnFilter_Click(object sender, EventArgs e)
        {
            strFilter = tbTnFilter.Text.Trim();
        }
        #region [write]
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int[] rowHandles = gridView1.GetSelectedRows();
            if (rowHandles.Length == 0) return;

            //foreach (var rowHandle in rowHandles)
            //{
            //    
            //    if (row != null)
            //    {
            //        XtraMessageBox.Show(
            //            $"TagID: {row.ValueName}\n" +
            //            $"RealValue: {row.RealValue}\n" +
            //            $"Quality: {row.Quality}\n" +
            //            $"Timestamp: {row.Timestamp}",
            //            "详细信息",
            //            MessageBoxButtons.OK,
            //            MessageBoxIcon.Information);
            //    }
            //}

            var row = gridView1.GetRow(rowHandles[0]) as RealTimeValue;
            string value = "";
            diaWrite fw = new diaWrite();
            fw.TagName = row.ValueName;
            fw.UOM = row.uom != null ? row.uom : "";

            if (fw.ShowDialog() == DialogResult.OK)
            {
                List<WriteValue> values = new List<WriteValue>();
                value = fw.strValue;

                values.Add(new WriteValue()
                {
                    CannelID = DataImport.dicTagPool[fw.TagName].CannelID,
                    Address = DataImport.dicTagPool[fw.TagName].Address,
                    ValueName = fw.TagName,
                    value = value,
                    DataType = 0
                });////
                _Write(values);
            }
        }

        public async void _Write(List<WriteValue> values)
        {
            try
            {
                if (values == null || values.Count == 0)
                    return;

                string url = ServerCfg.Instance.RestUri1;
                if (url.Contains(@"0.0.0.0:"))
                    url = url.Replace(@"0.0.0.0:", @"127.0.0.1:");

                bool m = PubFunction.IsURI(url);

                if (!m)
                    return;

                var client = new RestSharp.RestClient(url);
                var requestGet = new RestRequest("api/Data/WriteRealtimeValues", Method.Post);
                requestGet.AddHeader("Content-Type", "application/json");
                //string x = JsonHelper.Serialize<TagNames>(tn);
                //requestGet.AddParameter("TagNames", x);

                //<如果cannel 不激活则不写操作>
                List<string> cannels = ServerCfg.Instance.cDevices.Where(it => it.iActive == 1).Select(x => x.CannelID).ToList();
                if (cannels == null || cannels.Count == 0)
                    return;
                List<WriteValue> RVs = new List<WriteValue>();
                foreach (var v in cannels)
                {
                    if (DataImport.dicCannelTags.ContainsKey(v))
                    {
                        List<WriteValue> writeValues = values.Where(it => it.CannelID == v).ToList();
                        RVs.AddRange(writeValues);
                    }
                }

                if (RVs.Count == 0)
                    return;
                //</如果cannel 不激活则不写操作>

                requestGet.AddJsonBody(RVs);
                //requestGet.AddBody(TagIDs);
                //IRestResponse response = client.Execute(requestGet);

                var response = await client.ExecuteAsync(requestGet);
                if (response != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        XtraMessageBox.Show(this, "Parameters downloaded.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {

            }
        }
        #endregion //[write]

        /// <summary>
        /// 下载配方按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btDownloadRecipe_Click(object sender, EventArgs e)
        {
            string selectedRecipe = cbRecipes.Text;
            if (string.IsNullOrEmpty(selectedRecipe))
            {
                XtraMessageBox.Show(this, "请选择一个配方进行下载。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

           var recipe = master_recipe.Instance?.equipment_recipes?
            .FirstOrDefault(r => string.Equals(r.equipment_id, selectedRecipe, StringComparison.OrdinalIgnoreCase)
                      || string.Equals(r.equipment_name, selectedRecipe, StringComparison.OrdinalIgnoreCase));

            if (recipe == null)
                XtraMessageBox.Show(this, $"Recipe '{selectedRecipe}' not found.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (recipe.recipe_status != RecipeStatus.InUse)
                XtraMessageBox.Show($"Recipe '{selectedRecipe}' Status is not Active.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            try
            {

                string url = ServerCfg.Instance.RestUri1;
                if (url.Contains(@"0.0.0.0:"))
                    url = url.Replace(@"0.0.0.0:", @"127.0.0.1:");

                bool m = PubFunction.IsURI(url);

                if (!m)
                    return;

                var client = new RestSharp.RestClient(url);
                var requestGet = new RestRequest("/api/Recipe/DownloadRecipe", Method.Post);
                requestGet.AddHeader("Content-Type", "application/json");
                string jsonRecipeName = $"\"{selectedRecipe}\""; // 双引号包裹
                requestGet.AddJsonBody(jsonRecipeName);
                // requestGet.AddBody(selectedRecipe);

                var response = await client.ExecuteAsync(requestGet);
                if (response != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        XtraMessageBox.Show(this, "Recipe parameters downloaded.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show(this, $"下载配方失败: {response.Content}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show(this, $"服务器没反应", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {

            }

        }

        private void cbRecipes_QueryPopUp(object sender, CancelEventArgs e)
        {
            // 查询最新的数据源，例如从数据库获取
            List<string> updatedItems = new List<string>();
            foreach (var v in master_recipe.Instance.equipment_recipes)
            {
                if(v.recipe_status == RecipeStatus.InUse)
                    updatedItems.Add(v.equipment_name);
            }

            cbRecipes.Properties.Items.Clear();
            cbRecipes.Properties.Items.AddRange(updatedItems);
        }
    }
}
