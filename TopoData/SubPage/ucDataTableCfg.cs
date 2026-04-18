using auDASLib;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraVerticalGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TopoData.model;
using TopoData.Properties;

namespace TopoData.SubPage
{
    public partial class ucDataTableCfg : DevExpress.XtraEditors.XtraUserControl
    {
        #region 定义
        string _TaskName = "";
        string _SampleTrig_ExpressionText = "";
        new List<DBTag> _SampleTrig_ExpressionTags = new List<DBTag>();

        #endregion

        public ucDataTableCfg()
        {
            InitializeComponent();
        }
        private void ucDataTableCfg_Load(object sender, EventArgs e)
        {
            radioGroup1.BackColor = Color.Transparent;
            HandleEvent();
            ApplyLanguage();
        }
        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            navBarControl1.ActiveGroup.Caption = Resources.DataTable;

            cbActive.Properties.OffText = Resources.Off;
            cbActive.Properties.OnText = Resources.On;
            cbRealTimeTable.Text = Resources.TimeScaleTable;
            cbExpress.Text = Resources.Expression;
            //cbUpdate.Text = Resources.Update;
            radioGroup1.Properties.Items[0].Description = Resources.CollectionRisingEdge;
            radioGroup1.Properties.Items[1].Description = Resources.CollectionConsecutive;
            btSave.Text = Resources.Save;
            btTableCreate.Text = Resources.SynchronizeTable;
            btColumnAdd.Text = Resources.Add;
            btColumnUpdate.Text = Resources.Update;
            btColumnDelete.Text = Resources.Delete;
            //btUp.Text = Resources.Up;
            //btDown.Text = Resources.Down;
            btRemoeDBDate.Text = Resources.DeleteDBDate;
            btExpress.Text = Resources.Expression;

            label8.Text = Resources.Task;
            label1.Text = Resources.DBTable;
            label2.Text = Resources.TagWO;
            //label6.Text = Resources.TitleStep;
            label9.Text = Resources.EquipmentName;
            label3.Text = Resources.BoolTrigTag;
            label4.Text = Resources.BoolTrigTag;

            //groupBox2.Text = Resources.triggering_condition;
            //groupBox4.Text = Resources.Update;
            //groupBox3.Text = Resources.LotNum;
            //groupBox1.Text = Resources.DBTable;

            //radioButtonOnTrue.Text = Resources.CollectionRisingEdge;
            //radioButtonTimeWindow.Text = Resources.CollectionConsecutive;
        }

        DataSample _cd = new DataSample();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleEvent(/*object sender, CannelEventArgs e*/)
        {
            gridControl1.DataSource = null;
            gridControl1.Refresh();

            if (ServerCfg.Instance != null && ServerCfg.Instance.dataSamples.Count > 0)
            {
                _cd = ServerCfg.Instance.dataSamples[0];
                {

                    cbActive.IsOn = _cd.bActive;

                    cbRealTimeTable.Checked = _cd.bSave2ProcessTable;
                    //radioButtonOnTrue.Checked = (cd.SampleTrig.Type == 0);

                    if (_cd.SampleTrig.Type == 0)
                        radioGroup1.SelectedIndex = 0;
                    else
                        radioGroup1.SelectedIndex = 1;
                    if (radioGroup1.SelectedIndex == 0)
                    {
                        tbTagName1.Text = _cd.SampleTrig.dataSampleTrig_OnTrue.TagName;
                        tbTagName2.Text = _cd.SampleTrig.dataSampleTrig_TimeWindow.TagName;
                    }
                    //radioButtonTimeWindow.Checked = (cd.SampleTrig.Type == 1);

                    if (radioGroup1.SelectedIndex == 1)
                    {
                        tbTagName1.Text = _cd.SampleTrig.dataSampleTrig_OnTrue.TagName;
                        tbTagName2.Text = _cd.SampleTrig.dataSampleTrig_TimeWindow.TagName;
                        tbTimeWindow.Text = _cd.SampleTrig.dataSampleTrig_TimeWindow.TimeWindow.ToString();
                    }

                    tbWoTagName.Text = _cd.wo;
                    tbTaskName.Text = _cd.TaskName;
                    _TaskName = _cd.TaskName;
                    tbTableName.Text = _cd.TableName;
                    tbEquipment.Text = _cd.EquipmentName;
                    Task1.Caption = _cd.TaskName;
                    radioGroup1.SelectedIndex = _cd.SampleTrig.Type == 0 ? 0 : 1;
                    //<表达式>
                    cbExpress.Checked = _cd.SampleTrig.bUseExpression;
                    _SampleTrig_ExpressionTags = _cd.SampleTrig.ExpressionTags;
                    _SampleTrig_ExpressionText = _cd.SampleTrig.ExpressionText;
                    if (cbExpress.Checked)
                    {
                        btExpress.Enabled = true;
                    }
                    else
                    {
                        btExpress.Enabled = false;
                    }
                    //</表达式>

                    gridControl1.DataSource = _cd.Samples;
                }
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                panel2.Enabled = true;
                panel3.Enabled = false;
            }
            else
            {
                panel2.Enabled = false;
                panel3.Enabled = true;
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTableName.Text) || string.IsNullOrWhiteSpace(tbTaskName.Text))
            {
                // new diaMessageY(Resources.msgDBTitleNotNull, Resources.PromptMessage).ShowDialog();
                DevExpress.XtraEditors.XtraMessageBox.Show("表名或者任务名不能为空。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveConfiguration();
        }

        /// <summary>
        /// 保存配置到文件
        /// </summary>
        void SaveConfiguration()
        {
            try
            {
                if (ServerCfg.Instance != null)
                {
                    DataSample cd = new DataSample();
                    foreach (var dv in ServerCfg.Instance.dataSamples)
                    {
                        if (_TaskName == dv.TaskName)
                        {
                            cd = dv;
                        }
                        else
                            continue;

                        cd.bActive = cbActive.IsOn;
                        cd.TableName = tbTableName.Text;
                        cd.TaskName = tbTaskName.Text;
                        cd.EquipmentName = tbEquipment.Text;
                        _TaskName = cd.TaskName;
                        Task1.Caption = cd.TaskName;
                        cd.bSave2ProcessTable = cbRealTimeTable.Checked;
                        List<DBTag> taglist = new List<DBTag>();
                        foreach (var x in DataImport.dicCannelTags.Values)
                        {
                            taglist.AddRange(x);
                        }
                        List<string> tagnamelist = taglist.Select(it => it.CannelID + "." + it.TagName).ToList();

                        if (radioGroup1.SelectedIndex == 0)
                        {
                            if (!cbExpress.Checked)
                            {
                                if (string.IsNullOrEmpty(tbTagName1.Text))
                                {
                                    //new diaMessageY(Resources.msgTagNameNotNull, Resources.PromptMessage).ShowDialog();
                                    //MessageBox.Show("条件通讯点名不能为空。");
                                    DevExpress.XtraEditors.XtraMessageBox.Show("表条件通讯点名不能为空。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                if (!tagnamelist.Contains(tbTagName1.Text))
                                {
                                    //new diaMessageY(Resources.msgTagNameSelectNotExist, Resources.PromptMessage).ShowDialog();
                                    //MessageBox.Show("选择的通讯点不存在, 请检查通讯配置。");
                                    DevExpress.XtraEditors.XtraMessageBox.Show("选择的通讯点不存在, 请检查通讯配置。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }

                            cd.SampleTrig.Type = 0; // on true
                            cd.SampleTrig.dataSampleTrig_OnTrue.TagName = tbTagName1.Text;
                            cd.SampleTrig.dataSampleTrig_OnTrue.CannelID = "";
                            cd.SampleTrig.dataSampleTrig_OnTrue.DataType = "0";
                        }
                        else
                        {
                            if (!cbExpress.Checked)
                            {
                                if (string.IsNullOrEmpty(tbTagName2.Text))
                                {
                                    //new diaMessageY(Resources.msgTagNameNotNull, Resources.PromptMessage).ShowDialog();
                                    //MessageBox.Show("条件通讯点名不能为空。");
                                    DevExpress.XtraEditors.XtraMessageBox.Show("条件通讯点名不能为空。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                if (!tagnamelist.Contains(tbTagName2.Text))
                                {
                                    //new diaMessageY(Resources.msgTagNameSelectNotExist, Resources.PromptMessage).ShowDialog();
                                    //MessageBox.Show("选择的通讯点不存在, 请检查通讯配置。");
                                    DevExpress.XtraEditors.XtraMessageBox.Show("选择的通讯点不存在, 请检查通讯配置。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }

                            cd.SampleTrig.Type = 1; //time window based
                            cd.SampleTrig.dataSampleTrig_TimeWindow.TagName = tbTagName2.Text;
                            cd.SampleTrig.dataSampleTrig_TimeWindow.CannelID = "";
                            cd.SampleTrig.dataSampleTrig_TimeWindow.DataType = "0";
                            cd.SampleTrig.dataSampleTrig_TimeWindow.TimeWindow = int.Parse(tbTimeWindow.Text);
                            if (cd.SampleTrig.dataSampleTrig_TimeWindow.TimeWindow <= 39)
                            {
                                //new diaMessageY(Resources.msgSampleWindowError1, Resources.PromptMessage).ShowDialog();
                                //MessageBox.Show("设定采样周期不能小于 400 ms");
                                DevExpress.XtraEditors.XtraMessageBox.Show("设定采样周期不能小于 400 ms", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        cd.SampleTrig.bUseExpression = cbExpress.Checked;
                        if (cbExpress.Checked)
                        {
                            cd.SampleTrig.ExpressionText = _SampleTrig_ExpressionText;
                            cd.SampleTrig.ExpressionTags = _SampleTrig_ExpressionTags;
                        }
                        cd.wo = tbWoTagName.Text;

                        //---------------------------
                        //存储配置文件
                        XmlHelper.XmlSerializeToFile(ServerCfg.Instance, pubDefine.Configuration, Encoding.UTF8);

                        //发送事件,更新数列表设备名
                        //OnMyEvent(new CannelEventArgs(CfgMessage.UpdateTreeSampleName, 2));
                    }
                }
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 创建表结构
        /// 同步表结构按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTableCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTableName.Text) || string.IsNullOrWhiteSpace(tbTaskName.Text))
            {
                //new diaMessageY(Resources.msgDBTitleNotNull, Resources.PromptMessage).ShowDialog();
                DevExpress.XtraEditors.XtraMessageBox.Show("表名或者任务名不能为空。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveConfiguration();

            try
            {
                //<添加采样表格数据>
                foreach (var dv in ServerCfg.Instance.dataSamples)
                {
                    if (_TaskName == dv.TaskName)
                    {
                        //<创建数据库表>
                        DBOperate dbo = new DBOperate();
                        int b = dbo.CreateOrUpdateTable(tbTableName.Text, dv);
                        dbo.Close();
                        if (b == -2)
                        {
                            //MessageBox.Show("Create table failed.", Resources.PromptMessage,
                            //        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            DevExpress.XtraEditors.XtraMessageBox.Show("创建表失败。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (b == -1)
                        {
                            //MessageBox.Show("Create table susscess.", Resources.PromptMessage,
                            //        MessageBoxButtons.OK, MessageBoxIcon.Information);

                            DevExpress.XtraEditors.XtraMessageBox.Show("创建表成功。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (b > 0)
                        {
                            //MessageBox.Show($" {b} new colums created.", Resources.PromptMessage,
                            //        MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DevExpress.XtraEditors.XtraMessageBox.Show($" 创建了 {b} 个新列。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //<创建数据库表>    
                    }
                    else
                        continue;
                }
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("创建数据库错误"+"\r\n" + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // new diaMessageY(Resources.msgCreateDBFail + "\r\n" + ex.Message, Resources.PromptMessage).ShowDialog();
            }
        }

        private void btTagSelect1_Click(object sender, EventArgs e)
        {
            diaTagSelect diaTagSelect = new diaTagSelect();
            diaTagSelect.ShowDialog();
            if (!string.IsNullOrEmpty(diaTagSelect.TagName))
            { tbTagName1.Text = diaTagSelect.TagName; }
        }

        private void btTagSelect2_Click(object sender, EventArgs e)
        {
            diaTagSelect diaTagSelect = new diaTagSelect();
            diaTagSelect.ShowDialog();
            if (!string.IsNullOrEmpty(diaTagSelect.TagName))
            { tbTagName2.Text = diaTagSelect.TagName; }
        }

        private void cbExpress_Properties_EditValueChanged(object sender, EventArgs e)
        {
            if (cbExpress.Checked && radioGroup1.SelectedIndex == 1)
            {
                tbTagName2.Enabled = false;

            }
            else
            {
                tbTagName2.Enabled = true;
            }

            if (cbExpress.Checked && radioGroup1.SelectedIndex == 0)
            {
                tbTagName1.Enabled = false;
            }
            else
            {
                tbTagName1.Enabled = true;
            }

            if (cbExpress.Checked)
            {
                btExpress.Enabled = true;
            }
            else
            {
                btExpress.Enabled = false;
            }
        }

        private void btTagSelectWo_Click(object sender, EventArgs e)
        {
            diaTagSelect diaTagSelect = new diaTagSelect();

            diaTagSelect.ShowDialog();

            if (!string.IsNullOrEmpty(diaTagSelect.TagName))
            {
                //Lot 标签只能是字符串类型
                string strtype = diaTagSelect.DataType?.ToLower() ?? "";
                if (!(strtype == "string" || strtype == "wstring"))
                {
                    //MessageBox.Show(Resources.msgWoTagTypeError, Resources.PromptMessage,
                    //        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    DevExpress.XtraEditors.XtraMessageBox.Show("工单标签类型错误, 只能是字符串类型。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                tbWoTagName.Text = diaTagSelect.TagName;
            }
        }

        private void btExpress_Click(object sender, EventArgs e)
        {
            diaExpress diaExpress = new diaExpress();
            diaExpress.ExpressionText = _SampleTrig_ExpressionText;
            diaExpress.ExpressionTags = _SampleTrig_ExpressionTags;
            diaExpress.ShowDialog();
            if (diaExpress.DialogResult == DialogResult.OK)
            {
                _SampleTrig_ExpressionText = diaExpress.ExpressionText;
                _SampleTrig_ExpressionTags = diaExpress.ExpressionTags;
            }
        }

        /// <summary>
        /// 按钮添加数据库Column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btColumnAdd_Click(object sender, EventArgs e)
        {
            diaColumnSelect diaColumnSelect = new diaColumnSelect(true);
            diaColumnSelect.ShowDialog();

            if (gridView1.RowCount > auDAServer.DACtrl.free_DataSampleColumns - 1)
            { 
                DevExpress.XtraEditors.XtraMessageBox.Show($"免费版最多支持添加 {auDAServer.DACtrl.free_DataSampleColumns} 列。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                        DevExpress.XtraEditors.XtraMessageBox.Show($" {at} - 不允许特殊字符或数字开头" , "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _cd.Samples.Add(new DSTbDefine()
                    {
                        Column = at,
                        TagName = tn,
                        UOM = uom,
                        DataType = dt,
                        isInternalValue = diaColumnSelect.isInternalValue,
                        DefaultValue = df
                    });

                    gridControl1.DataSource = null; // 清除当前数据源
                    gridControl1.DataSource = _cd.Samples; // 重新绑定数据源
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
                        _cd.Samples.Add(new DSTbDefine()
                        {
                            Column = at,
                            TagName = tn,
                            UOM = uom,
                            DataType = dt,
                            isInternalValue = diaColumnSelect.isInternalValue,
                            DefaultValue = df
                        });

                    }

                    gridControl1.DataSource = null; // 清除当前数据源
                    gridControl1.DataSource = _cd.Samples; // 重新绑定数据源
                }
            }
        }

        private void btColumnUpdate_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount <= 0)
                return;

            // 获取当前选中的行
            int selectedRowHandle = gridView1.GetSelectedRows()[0];
            diaColumnSelect diaColumnSelect = new diaColumnSelect();
            diaColumnSelect.Atribute = gridView1.GetRowCellValue(selectedRowHandle, "Column").ToString(); // 假设 Column1 对应属性
            diaColumnSelect.TagID = gridView1.GetRowCellValue(selectedRowHandle, "TagName").ToString();    // 假设 Column3 对应 TagID
            diaColumnSelect.UOM = gridView1.GetRowCellValue(selectedRowHandle, "UOM").ToString();     // 假设 Column4 对应 UOM
            diaColumnSelect.DataType = gridView1.GetRowCellValue(selectedRowHandle, "DataType").ToString(); // 假设 Column2 对应 DataType                                                                                                        // 判断是否为内部值
            string isInternalValueStr = gridView1.GetRowCellValue(selectedRowHandle, "isInternalValue").ToString();
            diaColumnSelect.isInternalValue = isInternalValueStr == "Yes" ? true : false;
            diaColumnSelect.DefaultValue = gridView1.GetRowCellValue(selectedRowHandle, "DefaultValue").ToString();
            diaColumnSelect.ShowDialog();

            if (string.IsNullOrEmpty(diaColumnSelect.TagID))
                return;

            if (diaColumnSelect.DialogResult == DialogResult.Yes)
            {
                for (int i = 0; i < _cd.Samples.Count; i++)
                {
                    if (_cd.Samples[i].Column == diaColumnSelect.Atribute)
                    {
                        _cd.Samples[i] = new DSTbDefine()
                        {
                            Column = diaColumnSelect.Atribute,
                            TagName = diaColumnSelect.TagID,
                            UOM = diaColumnSelect.UOM,
                            DataType = diaColumnSelect.DataType,
                            isInternalValue = diaColumnSelect.isInternalValue,
                            DefaultValue = diaColumnSelect.DefaultValue
                        };
                        gridControl1.DataSource = null; // 清除当前数据源
                        gridControl1.DataSource = _cd.Samples; // 重新绑定数据源
                        return;
                    }
                }
            }
        }

        private void btColumnDelete_Click(object sender, EventArgs e)
        {
            int[] selectedRows = gridView1.GetSelectedRows();
            if(selectedRows.Length == 0)
                return;

            string column = gridView1.GetRowCellValue(selectedRows[0], "Column").ToString(); // 假设 Column1 对应属性
            if (DialogResult.Yes == DevExpress.XtraEditors.XtraMessageBox.Show($"是否删除{column}", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) // 对row进行操作
            {
                int ret = _cd.Samples.RemoveAll(d => d.Column == column);
                gridControl1.DataSource = null; // 清除当前数据源
                gridControl1.DataSource = _cd.Samples; // 重新绑定数据源
            }
        }

        private void btRemoeDBDate_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }

            int[] selectedRows = gridView1.GetSelectedRows();
            string column = gridView1.GetRowCellValue(selectedRows[0], "Column").ToString(); // 假设 Column1 对应属性

            //DialogResult mb = new diaMessageYN(Resources.QstDeletDBColumn, Resources.PromptMessage).ShowDialog();
            if (DialogResult.Yes == DevExpress.XtraEditors.XtraMessageBox.Show($"是否删除{column} 及数据库表对应字段", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                try
                {

                    //<创建数据库表>
                    DBOperate dbo = new DBOperate();
                    int b = dbo.DeleteColumn(tbTableName.Text, column);
                    dbo.Close();
                    //<创建数据库表>  

                    if (b < 0)
                    {
                        // 删除选中的行
                        int ret = _cd.Samples.RemoveAll(d => d.Column == column);
                        gridControl1.DataSource = null; // 清除当前数据源
                        gridControl1.DataSource = _cd.Samples; // 重新绑定数据源
                    }
                    else
                    {
                        //new diaMessageY(Resources.DeletFail, Resources.PromptMessage).ShowDialog();
                        DevExpress.XtraEditors.XtraMessageBox.Show($"删除错误", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show($"删除错误" + ex.Message + "\r\n" + ex.InnerException, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // new diaMessageY(Resources.DeletFail + "\r\n" + ex.Message + "\r\n" + ex.InnerException, Resources.PromptMessage).ShowDialog();
                }
            }
        }
    }
}
