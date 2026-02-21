using auDASLib;
using DevExpress.Utils.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.model;
using TopoData.Properties;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace TopoData.SubPage
{
    public partial class ucRecipe : DevExpress.XtraEditors.XtraUserControl
    {
        public ucRecipe()
        {
            InitializeComponent();
        }
        private void ucRecipe_Load(object sender, EventArgs e)
        {
            navBarControl1.LargeImages = imageList1;
            navBarControl1.SmallImages = imageList1;
            // 将 NavBarControl 设置为导航模式
            //navBarControl1.PaintStyleName = "NavigationPane";
            navBarControl1.LinkSelectionMode = LinkSelectionModeType.OneInGroup;

            loadRecipeItems();
        }
        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            navBarControl1.ActiveGroup.Caption = Resources.Recipe;
            cbActive.Properties.OffText = Resources.Off;
            cbActive.Properties.OnText = Resources.On;
            label1.Text = Resources.Recipe;
            label2.Text = Resources.Description;
            btColumnAdd.Text    = Resources.Add;
            btColumnUpdate.Text = Resources.Update;
            btColumnDelete.Text = Resources.Delete;

            //cbRealTimeTable.Text = Resources.TimeScaleTable;
            //cbExpress.Text = Resources.Expression;
            ////cbUpdate.Text = Resources.Update;
            //radioGroup1.Properties.Items[0].Description = Resources.CollectionRisingEdge;
            //radioGroup1.Properties.Items[1].Description = Resources.CollectionConsecutive;
            //btSave.Text = Resources.Save;
            //btTableCreate.Text = Resources.SynchronizeTable;

            ////btUp.Text = Resources.Up;
            ////btDown.Text = Resources.Down;
            //btRemoeDBDate.Text = Resources.DeleteDBDate;
            //btExpress.Text = Resources.Expression;

            //label8.Text = Resources.Task;
            //label1.Text = Resources.DBTable;
            //label2.Text = Resources.TagWO;
            ////label6.Text = Resources.TitleStep;
            //label9.Text = Resources.EquipmentName;
            //label3.Text = Resources.BoolTrigTag;
            //label4.Text = Resources.BoolTrigTag;
        }
        /// <summary>
        /// 加载配方项目
        /// </summary>
        private void loadRecipeItems()
        {
            navBarControl1.Items.Clear();
            if (File.Exists(master_recipe.RecipePathDefine))
            {
                try
                {
                    master_recipe.Instance = XmlHelper.XmlDeserializeFromFile<master_recipe>(master_recipe.RecipePathDefine, Encoding.UTF8);

                    if (master_recipe.Instance != null && master_recipe.Instance.equipment_recipes != null)
                    {
                        foreach (var recipe in master_recipe.Instance.equipment_recipes)
                        {
                            NavBarItem item1 = new NavBarItem();
                            item1.Caption = recipe.equipment_id;
                            item1.Name = recipe.equipment_id;
                            item1.LargeImageIndex = 0;
                            item1.SmallImageIndex = 0;
                            item1.ImageOptions.LargeImageSize = new Size(32, 32);
                            item1.AllowAutoSelect = true;
                            navBarControl1.Items.AddRange(new NavBarItem[] { item1 });
                            // 将项目链接到组
                            DataTableList.ItemLinks.Add(item1);


                        }
                        // 设置事件处理
                        navBarControl1.LinkClicked += NavBarControl1_LinkClicked;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 清空面板
        /// </summary>
        private void clearRecipePanel()
        {
            tbTaskName.Text = "";
            tbDescription.Text = "";
            gridControl1.DataSource = null;
        }

        private int GetNextRecipeNumber()
        {
            int maxNum = 0;
            foreach (NavBarItem item in navBarControl1.Items)
            {
                // 使用正则表达式提取名称中的数字
                Match match = Regex.Match(item.Name, @"MyRecipe(\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int num))
                {
                    maxNum = Math.Max(maxNum, num);
                }
            }
            return maxNum + 1;
        }


        private void barButtonAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int nextNum = GetNextRecipeNumber();

            NavBarItem item1 = new NavBarItem();
            item1.Caption = $"MyRecipe{nextNum}";
            item1.Name = $"MyRecipe{nextNum}";
            item1.LargeImageIndex = 0;
            item1.SmallImageIndex = 0;
            item1.ImageOptions.LargeImageSize = new Size(32, 32);
            item1.AllowAutoSelect = true;

            navBarControl1.Items.AddRange(new NavBarItem[] { item1 });

            // 将项目链接到组
            DataTableList.ItemLinks.Add(item1);

            // 设置事件处理
            navBarControl1.LinkClicked += NavBarControl1_LinkClicked;

            if (master_recipe.Instance != null)
            {
                if (master_recipe.Instance != null && master_recipe.Instance.equipment_recipes != null)
                {
                    master_recipe.Instance.equipment_recipes.Add(new equipment_recipe()
                    {
                        equipment_id = $"MyRecipe{nextNum}",
                        equipment_name = $"MyRecipe{nextNum}",
                        description = "",
                        recipe_data = new List<recipe_data>()
                    });
                    XmlHelper.XmlSerializeToFile(master_recipe.Instance, master_recipe.RecipePathDefine, Encoding.UTF8);
                }
            }

            tbTaskName.Text = $"MyRecipe{nextNum}";
        }

        /// <summary>
        /// 点选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavBarControl1_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            string itemName = e.Link.Item.Name;

            var recipe = master_recipe.Instance.equipment_recipes
                .FirstOrDefault(r => r.equipment_id == itemName);

            if (recipe != null)
            {
                tbTaskName.Text = recipe.equipment_name;
                tbDescription.Text = recipe.description;
                gridControl1.DataSource = recipe.recipe_data;
                cbActive.IsOn = recipe.recipe_status == RecipeStatus.InUse;
                _cd = recipe;
            }
            else
            {
                clearRecipePanel();
            }
        }

        private void barButtonRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 获取名为 DataTableList 的组
            NavBarGroup activeGroup = navBarControl1.Groups.FirstOrDefault(g => g.Name == "DataTableList");

            if (activeGroup?.SelectedLink != null)
            {
                NavBarItem itemToRemove = activeGroup.SelectedLink.Item;
                string itemName = itemToRemove.Name;
                if (DialogResult.Yes == DevExpress.XtraEditors.XtraMessageBox.Show($"是否删除{itemName}", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) // 对row进行操作
                {



                    // 从组中移除链接
                    activeGroup.ItemLinks.Remove(activeGroup.SelectedLink);

                    // 从控件中移除项目
                    navBarControl1.Items.Remove(itemToRemove);

                    clearRecipePanel();

                    // 从 master_recipe.Instance.equipment_recipe 中删除
                    if (master_recipe.Instance?.equipment_recipes != null)
                    {
                        var recipeToRemove = master_recipe.Instance.equipment_recipes
                            .FirstOrDefault(r => r.equipment_id == itemName);

                        if (recipeToRemove != null)
                        {
                            master_recipe.Instance.equipment_recipes.Remove(recipeToRemove);

                            // 保存到文件
                            try
                            {
                                XmlHelper.XmlSerializeToFile(master_recipe.Instance, master_recipe.RecipePathDefine, Encoding.UTF8);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"保存配方失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }


                }


            }
        }

        private void barButtonItemSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (master_recipe.Instance != null)
            {
                // 获取名为 DataTableList 的组
                NavBarGroup activeGroup = navBarControl1.Groups.FirstOrDefault(g => g.Name == "DataTableList");
                if (activeGroup?.SelectedLink != null)
                {
                    string itemName = activeGroup.SelectedLink.Item.Name;
                    var recipe = master_recipe.Instance.equipment_recipes
                        .FirstOrDefault(r => r.equipment_id == itemName);
                    if (recipe != null)
                    {
                        recipe.equipment_name = tbTaskName.Text;
                        recipe.equipment_id = tbTaskName.Text;
                        recipe.description = tbDescription.Text;
                        recipe.recipe_data = gridControl1.DataSource as List<recipe_data> ?? new List<recipe_data>();
                        recipe.recipe_status = cbActive.IsOn ? RecipeStatus.InUse : RecipeStatus.Unknown;
                        try
                        {

                            if (activeGroup?.SelectedLink != null)
                            {
                                NavBarItem itemToRemove = activeGroup.SelectedLink.Item;
                                itemToRemove.Name    = recipe.equipment_id;
                                itemToRemove.Caption = recipe.equipment_id;
                            }

                            XmlHelper.XmlSerializeToFile(master_recipe.Instance, master_recipe.RecipePathDefine, Encoding.UTF8);
                            DevExpress.XtraEditors.XtraMessageBox.Show("配方保存成功!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show($"保存配方失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        equipment_recipe _cd = new equipment_recipe();
        private void btColumnAdd_Click(object sender, EventArgs e)
        {
            diaRecipeTagSelect diaColumnSelect = new diaRecipeTagSelect(true);
            diaColumnSelect.ShowDialog();

            if (diaColumnSelect.DialogResult == DialogResult.Yes) //对话框确认添加
            {
                if (diaColumnSelect.tags.Count <= 1)
                {
                    string at = diaColumnSelect.Atribute;
                    string tn = diaColumnSelect.TagID;
                    string uom = diaColumnSelect.UOM;
                    string dt = diaColumnSelect.DataType;
                    string it = diaColumnSelect.isInternalValue == true ? "Yes" : "No";
                    string df = diaColumnSelect.DefaultValue;

                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        string u = Convert.ToString(gridView1.GetRowCellValue(i, "Column"));
                        string tagName = Convert.ToString(gridView1.GetRowCellValue(i, "TagName"));
                        //string dataType = Convert.ToString(gridView1.GetRowCellValue(i, "DataType"));
                        //string id = Convert.ToString(gridView1.GetRowCellValue(i, "Id"));

                        if (string.Equals(at, u, StringComparison.OrdinalIgnoreCase))
                        {
                            //new diaMessageY(Resources.msgAttributeError1, Resources.PromptMessage).ShowDialog();
                            DevExpress.XtraEditors.XtraMessageBox.Show($"此属性 {u} 不能重复添加", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    //不允许特殊字符或数字开头
                    if (PubFunction.StartsWithSpecialCharOrDigit(at))
                    {
                        //new diaMessageY($" {at} - " + Resources.msgStartsWithSpecialCharOrDigitError, Resources.PromptMessage).ShowDialog();
                        DevExpress.XtraEditors.XtraMessageBox.Show($" {at} - 不允许特殊字符或数字开头", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _cd.recipe_data.Add(new recipe_data()
                    {
                        parameter_name = at,
                        tag_name = tn,
                        uom = uom,
                        data_type = dt,
                        value = df
                    });

                    gridControl1.DataSource = null; // 清除当前数据源
                    gridControl1.DataSource = _cd.recipe_data; // 重新绑定数据源
                }
                else if (diaColumnSelect.tags.Count > 1)
                {
                    foreach (var tg in diaColumnSelect.tags)
                    {
                        string at = "";
                        if (tg.TagName.StartsWith(tg.CannelID + "."))
                        {
                            string tagf = tg.TagName;
                            at = tagf.Substring((tg.CannelID + ".").Length); // 根据前缀的长度移除前缀
                        }

                        string tn = tg.TagName;
                        string uom = tg.UOM;
                        string dt = DBTag.DataTypeString(tg.DataType);
                        string it = tg.isInternalValue == true ? "Yes" : "No";
                        string df = tg.DefaultValue;
                        if (string.IsNullOrEmpty(tn))
                            return;

                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            string u = Convert.ToString(gridView1.GetRowCellValue(i, "Column"));
                            string tagName = Convert.ToString(gridView1.GetRowCellValue(i, "TagName"));
                            //string dataType = Convert.ToString(gridView1.GetRowCellValue(i, "DataType"));
                            //string id = Convert.ToString(gridView1.GetRowCellValue(i, "Id"));

                            if (string.Equals(at, u, StringComparison.OrdinalIgnoreCase))
                            {
                                //new diaMessageY(Resources.msgAttributeError1, Resources.PromptMessage).ShowDialog();
                                DevExpress.XtraEditors.XtraMessageBox.Show($"此属性 {u} 不能重复添加", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        _cd.recipe_data.Add(new recipe_data()
                        {
                            parameter_name = at,
                            tag_name = tn,
                            uom = uom,
                            data_type = dt,
                            value = df
                        });

                    }

                    gridControl1.DataSource = null; // 清除当前数据源
                    gridControl1.DataSource = _cd.recipe_data; // 重新绑定数据源
                }
            }
        }

        private void btColumnUpdate_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount <= 0)
                return;

            // 获取当前选中的行
            int selectedRowHandle = gridView1.GetSelectedRows()[0];
            diaRecipeTagSelect diaColumnSelect = new diaRecipeTagSelect();
            diaColumnSelect.Atribute = gridView1.GetRowCellValue(selectedRowHandle, "parameter_name").ToString(); // 假设 Column1 对应属性
            diaColumnSelect.TagID = gridView1.GetRowCellValue(selectedRowHandle, "tag_name").ToString();    // 假设 Column3 对应 TagID
            diaColumnSelect.UOM = gridView1.GetRowCellValue(selectedRowHandle, "uom").ToString();     // 假设 Column4 对应 UOM
            diaColumnSelect.DataType = gridView1.GetRowCellValue(selectedRowHandle, "data_type").ToString(); // 假设 Column2 对应 DataType // 判断是否为内部值
            diaColumnSelect.DefaultValue = gridView1.GetRowCellValue(selectedRowHandle, "value").ToString();
            diaColumnSelect.ShowDialog();

            if (string.IsNullOrEmpty(diaColumnSelect.TagID))
                return;

            if (diaColumnSelect.DialogResult == DialogResult.Yes)
            {
                //for (int i = 0; i < _cd.recipe_data.Count; i++)
                //{
                //    if (_cd.recipe_data[i].parameter_name== diaColumnSelect.Atribute)
                //    {
                        _cd.recipe_data[selectedRowHandle] = new recipe_data()
                        {
                            parameter_name = diaColumnSelect.Atribute,
                            tag_name = diaColumnSelect.TagID,
                            uom = diaColumnSelect.UOM,
                            data_type = diaColumnSelect.DataType,
                            value = diaColumnSelect.DefaultValue
                        };
                        gridControl1.DataSource = null; // 清除当前数据源
                        gridControl1.DataSource = _cd.recipe_data; // 重新绑定数据源
                //        return;
                //    }
                //}
            }
        }

        private void btColumnDelete_Click(object sender, EventArgs e)
        {
            int[] selectedRows = gridView1.GetSelectedRows();
            if (selectedRows.Length == 0)
                return;

            string column = gridView1.GetRowCellValue(selectedRows[0], "parameter_name").ToString(); // 假设 Column1 对应属性
            if (DialogResult.Yes == DevExpress.XtraEditors.XtraMessageBox.Show($"是否删除{column}", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) // 对row进行操作
            {
                int ret = _cd.recipe_data.RemoveAll(d => d.parameter_name == column);
                gridControl1.DataSource = null; // 清除当前数据源
                gridControl1.DataSource = _cd.recipe_data; // 重新绑定数据源
            }
        }

    }
}
