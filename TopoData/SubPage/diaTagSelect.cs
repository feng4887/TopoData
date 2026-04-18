//using auDAManager.Properties;
using auDASLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using TopoData.model;

namespace TopoData.SubPage
{
    public partial class diaTagSelect : DevExpress.XtraEditors.XtraForm
    {
        public string TagName { get; set; }
        public string Cannel { get; set; }
        public string UOM { get; set; }
        public string DataType { get; set; }

        public List<DBTag> tags = new List<DBTag>();

        private bool _MultiSelect = false;
        public diaTagSelect(bool MultiSelect = false)
        {
            InitializeComponent();
            _MultiSelect = MultiSelect;
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            tags.Clear();
            foreach (int selectedIndex in listView1.SelectedIndices)
            {
                var selectedTag = _currentTagList[selectedIndex];
                tags.Add(new DBTag()
                {
                    CannelID = selectedTag.CannelID,
                    TagName = selectedTag.TagId,
                    DataType = selectedTag.DataType,
                    UOM = selectedTag.UOM
                });
            }
            this.Close();
        }

        private void diaTagSelect_Load(object sender, EventArgs e)
        {
            listView1.MultiSelect = _MultiSelect;

            // 设置列表视图的样式和虚拟模式
            listView1.View = View.Details;
            listView1.VirtualMode = true;
            listView1.RetrieveVirtualItem += listView1_RetrieveVirtualItem;
            UpdateListView();
            ApplyLanguage();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// 应用多语言
        /// </summary>
        public void ApplyLanguage()
        {
            //this.Text     = Resources.Select;
            //label1.Text   = Resources.TagName;
            //btSelect.Text = Resources.Select;
            //listView1.Columns[0].Text = Resources.Num;
            //listView1.Columns[1].Text = Resources.CannelName;
            //listView1.Columns[2].Text = Resources.TagName;
            //listView1.Columns[3].Text = Resources.DataType;
            //listView1.Columns[4].Text = Resources.UOM;
        }


        // 在类中定义字段来存储数据
        private List<DBTag> _currentTagList;

        private void UpdateListView()
        {
            if (File.Exists(pubDefine.TagDefine))
            {

                // ①获取当前通道信息，第一个通道
                if (ServerCfg.Instance == null || ServerCfg.Instance.cDevices == null)
                    return;

                string _CannelID = string.Empty;
                if (ServerCfg.Instance.cDevices.Count > 0)
                {
                    _CannelID = ServerCfg.Instance.cDevices[0].CannelID;
                }
                else
                    return;

                if (string.IsNullOrEmpty(_CannelID))
                    return;

                List<DBTag> yy = XmlHelper.XmlDeserializeFromFile<List<DBTag>>(pubDefine.TagDefine, Encoding.UTF8);
                yy = yy.Where(it => it.CannelID == _CannelID).ToList();
                if (yy != null && yy.Count > 0)
                {
                    _currentTagList = yy.OrderBy(it => it.TagId).ToList();
                    listView1.VirtualListSize = _currentTagList.Count;
                    listView1.Invalidate();
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.FullRowSelect = true;

            if (this.listView1.SelectedIndices.Count > 0)
            {
                int index         = listView1.SelectedIndices[0];
                ListViewItem item = listView1.Items[index];
                string a = item.Text;
                Cannel   = item.SubItems[1].Text.ToString();
                TagName  = item.SubItems[2].Text.ToString();
                DataType = item.SubItems[3].Text.ToString();
                UOM      = item.SubItems[4].Text.ToString();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (this.listView1.SelectedIndices.Count > 0)
            //{
            //    int index = listView1.SelectedIndices[0];
            //    ListViewItem item = listView1.Items[index];
            //    string a = item.Text;
            //    Cannel = item.SubItems[1].Text.ToString();
            //    TagName = item.SubItems[2].Text.ToString();
            //    DataType = item.SubItems[3].Text.ToString();
            //    UOM = item.SubItems[4].Text.ToString();
            //    this.Close();
            //}

            tags.Clear();
            foreach (int selectedIndex in listView1.SelectedIndices)
            {
                var selectedTag = _currentTagList[selectedIndex];
                tags.Add(new DBTag()
                {
                    CannelID = selectedTag.CannelID,
                    TagName = selectedTag.TagId,
                    DataType = selectedTag.DataType,
                    UOM = selectedTag.UOM
                });
            }
            this.Close();
        }

        /// <summary>
        ///  点过滤按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTnFilter_Click(object sender, EventArgs e)
        {
            string strFilter = tbTnFilter.Text.Trim();
            if (!File.Exists(pubDefine.TagDefine))
                return;

            // 1. 反序列化所有 Tag
            List<DBTag> allTags = XmlHelper.XmlDeserializeFromFile<List<DBTag>>(pubDefine.TagDefine, Encoding.UTF8);

            if (allTags == null || allTags.Count == 0)
                return;

            List<DBTag> filtered;

            // 2. 如果输入为空 → 返回所有，按 TagId 排序
            if (string.IsNullOrEmpty(strFilter))
            {
                filtered = allTags.OrderBy(t => t.TagId).ToList();
            }
            else
            {
                // 3. 多关键词 AND 过滤（支持空格分词，忽略大小写）
                string[] terms = strFilter
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim())
                    .Where(t => t.Length > 0)
                    .ToArray();

                filtered = allTags
                    .Where(tag =>
                        tag.TagName != null &&
                        terms.All(term =>
                            tag.TagName.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            tag.TagId.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0
                        )
                    )
                    .OrderBy(t => t.TagId)
                    .ToList();
            }

            // 4. 更新当前数据源
            _currentTagList = filtered;

            // 5. 刷新 ListView 虚拟模式
            listView1.VirtualListSize = filtered.Count;

            // 6. 通知 UI 刷新
            listView1.Invalidate();
        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs args)
        {
            var tag = _currentTagList[args.ItemIndex];
            var item = new ListViewItem(args.ItemIndex.ToString());
            item.SubItems.Add(tag.CannelID);
            item.SubItems.Add(tag.TagId);
            item.SubItems.Add(DBTag.DataTypeString(tag.DataType));
            item.SubItems.Add(tag.UOM);
            args.Item = item;
        }
    }
}
