using auDASLib;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGauges.Core.Model;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraNavBar;
using DevExpress.XtraPrinting.HtmlExport.Native;
using DevExpress.XtraRichEdit.Import.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.Properties;
using auDAServer;
using TopoData.model;
namespace TopoData.Page
{
    public partial class ucCannelCfg : DevExpress.XtraEditors.XtraUserControl
    {
        #region [定义]
        private ucDriverProfinet ucProfinet = new ucDriverProfinet();
        private ucDriverOPCUA ucOPCUA = new ucDriverOPCUA();
        private ucDriverEmpty ucDriverEmpty = new ucDriverEmpty();

        /// <summary>
        /// 默认设备
        /// </summary>
        private string _CannelID { get; set; }
        private DeviceType _devType = DeviceType.OPCUA;
        #endregion //[定义]

        #region [form]
        public ucCannelCfg()
        {
            InitializeComponent();
        }

        private void ucCannelCfg_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            InitDriverCannel();
            SetupGridView();
            if (!(ServerCfg.Instance == null || ServerCfg.Instance.cDevices == null)
                && ServerCfg.Instance.cDevices.Count > 0)
            {
                _CannelID = ServerCfg.Instance.cDevices[0].CannelID;
                HandleEvent(this, new CannelEventArgs(_CannelID, 1)); //初始化
            }
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            labelControl1.Text = Resources.CannelName;
            labelControl2.Text = Resources.DriverType;
            labelControl3.Text = Resources.Description;
            navBarControl1.ActiveGroup.Caption = Resources.Equipments;
            cbActive.Properties.OffText = Resources.Off;
            cbActive.Properties.OnText = Resources.On;
            groupControl1.Text = Resources.EquipmentCfguration;
            groupControl2.Text = Resources.TagList;
            //groupBox1.Text = Resources.CannelCfg;
            cbActive.Text = Resources.Enable;
            //btClear.Text = Resources.Clear;
            btConfirm.Text = Resources.Save;
            btExport.Text = Resources.Export;
            btImport.Text = Resources.Import;
            btDelete.Text = Resources.Delete;
            if (cbCannel.SelectedItem?.ToString() == "Siemens Profinet")
            {
                ucProfinet.ApplyLanguage();
            }
            else if (cbCannel.SelectedItem?.ToString() == "OPC UA")
            {
                ucOPCUA.ApplyLanguage();
            }
        }

        /// <summary>
        /// 增加驱动，这个函数要增加
        /// </summary>
        private void InitDriverCannel()
        {
            cbCannel.SelectedIndex = 0;
            ucProfinet.Parent = pnCannel;
            ucOPCUA.Parent = pnCannel;
            //ucDriverEmpty.Parent = pnCannel;
            //ucDriverEmpty.Visible = false;
            ucProfinet.Visible = false;
            ucOPCUA.Visible = true;
            ucOPCUA.TagEvent += UcOPCUA_TagEvent;
        }
        #endregion //[form]

        #region 保存更新事件
        // 用EventHandler<woEventArgs>委托定义事件
        public event EventHandler<CannelEventArgs> MyEvent;

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMyEvent(CannelEventArgs e)
        {
            MyEvent?.Invoke(this, e);
        }

        public void HandleEvent(object sender, CannelEventArgs e)
        {
            string _CannelID = "";
            //if (e.Type == 1)
            {
                this.Enabled = true;
                tbCannelID.Text = e.Message;

                _CannelID = e.Message;
                if (ServerCfg.Instance != null && ServerCfg.Instance.cDevices.Count > 0)
                {

                    foreach (ConnctDevice cd in ServerCfg.Instance.cDevices)
                    {
                        if (e.Message == cd.CannelID)
                        {
                            //**************************************
                            //驱动不同，添加更多代码
                            //**************************************
                            if (cd.devType == DeviceType.ProfinetS7)
                            {
                                ucProfinet.Parent = pnCannel;
                                ucProfinet.Slot = cd.IpCfg.Slot.ToString();
                                ucProfinet.Host = cd.IpCfg.Host.ToString();
                                ucProfinet.Rack = cd.IpCfg.Rack.ToString();
                                cbCannel.SelectedItem = "Siemens Profinet";
                            }
                            else if (cd.devType == DeviceType.OPCUA)
                            {
                                ucOPCUA.EndpointUrl = cd.UAParamter.EndpointUrl.ToString();
                                ucOPCUA.securityPolicy = cd.UAParamter.securityPolicy.ToString();
                                ucOPCUA.SecurityMode = cd.UAParamter.SecurityMode.ToString();
                                ucOPCUA.UseUserLogIn = cd.UAParamter.UseUserLogIn;
                                ucOPCUA.User = cd.UAParamter.User;
                                ucOPCUA.Psw = cd.UAParamter.Psw;
                                ucOPCUA.ApplicationName = cd.UAParamter.ApplicationName;
                                ucOPCUA.InitControl();
                                cbCannel.SelectedItem = "OPC UA";
                            }
                            navBarItem1.Caption = cd.CannelID; // 更新导航栏名称
                            tbDescription.Text = cd.Description;
                            cbActive.IsOn = cd.iActive == 1? true : false;
                            UpdateSheet(cd.CannelID);
                            break;
                        }
                    }
                }
            }

            //else if (e.Type == 101) //删除
            //{
            //    this.Enabled = false;
            //    tbCannelID.Text = "";
            //    cbCannel.SelectedIndex = -1;
            //    _CannelID = "";
            //    ucDriverEmpty.Visible = true;
            //    ucProfinet.Visible = false;
            //    ucOPCUA.Visible = false;
            //    ClearAllSheet();

            // }
        }
        #endregion //事件

        #region [数据操作]
        /// <summary>
        /// 接收点表增删改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcOPCUA_TagEvent(object? sender, model.DBTagEventArgs e)
        {

            string taskName = "";// cbRealtimeTask.Text;
            ConnctDevice _connctDevice = null;

            // ①获取当前通道信息，第一个通道
            if (ServerCfg.Instance == null || ServerCfg.Instance.cDevices == null)
                return;

            if (ServerCfg.Instance.cDevices.Count > 0)
            {
                _CannelID = ServerCfg.Instance.cDevices[0].CannelID;
                _devType = ServerCfg.Instance.cDevices[0].devType;
                _connctDevice = ServerCfg.Instance.cDevices[0];

            }
            else
                return;

            if (string.IsNullOrEmpty(_CannelID))
                return;


            if (DataImport.dicCannelTags.ContainsKey(_CannelID))
            {
                var existing = DataImport.dicCannelTags[_CannelID];
                var existingNames = new HashSet<string>(existing.Select(t => t.TagName), StringComparer.OrdinalIgnoreCase);

                // 去重并区分重复/待添加
                var incomingDistinct = e.Tags
                    .Where(t => !string.IsNullOrEmpty(t.TagName))
                    .GroupBy(t => t.TagName, StringComparer.OrdinalIgnoreCase)
                    .Select(g => g.First())
                    .ToList();

                var duplicates = incomingDistinct
                    .Where(t => existingNames.Contains(t.TagName))
                    .Select(t => t.TagName)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var toAdd = incomingDistinct
                    .Where(t => !existingNames.Contains(t.TagName))
                    .ToList();

                if (duplicates.Count > 0)
                {
                    var dupList = string.Join(",", duplicates);
                    //var msg = $"下列 TagName 已存在：{Environment.NewLine}{dupList}{Environment.NewLine}{Environment.NewLine}将添加其余 {toAdd.Count} 个不重复项，是否继续？";
                    //var msg = $"{Resources.msgTag1}{Environment.NewLine}{dupList}{Environment.NewLine}{Environment.NewLine}{Resources.msgTag2} {toAdd.Count} {Resources.msgTag3}";
                    //diaMessageYN mb = new diaMessageYN(msg, Resources.PromptMessage);

                }

                if (toAdd.Count > 0)
                {
                    // 确保新增项的 CannelID/TagId 正确
                    foreach (var tag in toAdd)
                    {
                        tag.CannelID = _CannelID;
                        tag.TagId = $"{_CannelID}.{tag.TagName}";
                    }

                    

                        int mm = DataImport.dicCannelTags[_CannelID].Count;
                        if (DataImport.dicCannelTags[_CannelID].Count + toAdd.Count > auDAServer.DACtrl.free_CannelTags)
                        {
                            int removecount = DataImport.dicCannelTags[_CannelID].Count + toAdd.Count - DACtrl.free_CannelTags;
                            int removeat = toAdd.Count - removecount;
                            for (int i = toAdd.Count - 1; i >= removeat; i--)
                            {
                                toAdd.RemoveAt(i);
                            }

                            if(removecount > 0)
                                DevExpress.XtraEditors.XtraMessageBox.Show($"免费版本导入变量点数量不能超过 {DACtrl.free_CannelTags} 个。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                    existing.AddRange(toAdd);
                }
            }
            else
            {
                // 新增整个列表，去重并修正 CannelID/TagId
                var incomingDistinct = e.Tags
                    .Where(t => !string.IsNullOrEmpty(t.TagName))
                    .GroupBy(t => t.TagName, StringComparer.OrdinalIgnoreCase)
                    .Select(g => g.First())
                    .ToList();

                foreach (var tag in incomingDistinct)
                {
                    tag.CannelID = _CannelID;
                    tag.TagId = $"{_CannelID}.{tag.TagName}";
                }

                //----------------------------------------------------------------------
                // 免费版本 行数限制
                if (DACtrl.free_Version)
                {
                    if (DataImport.dicCannelTags.ContainsKey(_CannelID))
                    {
                        if (DataImport.dicCannelTags[_CannelID].Count + incomingDistinct.Count > DACtrl.free_CannelTags)
                        {
                            int removecount = DataImport.dicCannelTags[_CannelID].Count + incomingDistinct.Count - DACtrl.free_CannelTags;
                            for (int i = incomingDistinct.Count - 1; i >= removecount; i--)
                            {
                                incomingDistinct.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        DataImport.dicCannelTags.Add(_CannelID, new List<DBTag>());

                        if (DataImport.dicCannelTags[_CannelID].Count + incomingDistinct.Count > DACtrl.free_CannelTags)
                        {
                            int removecount = DataImport.dicCannelTags[_CannelID].Count + incomingDistinct.Count - DACtrl.free_CannelTags;
                            for (int i = incomingDistinct.Count - 1; i >= removecount; i--)
                            {
                                incomingDistinct.RemoveAt(i);
                            }
                        }

                    }

                }
                try
                {
                    //DataImport.dicCannelTags.Add(_CannelID, incomingDistinct);    

                    //foreach (var tag in incomingDistinct)
                    //{
                    DataImport.dicCannelTags[_CannelID].AddRange(incomingDistinct);

                    // }
                }
                  catch
                { }

            }

            UpdateSheet(_CannelID);
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <param name="CannelID"></param>
        /// <returns></returns>
        private List<DBTag> GetDBTagsFromSheet(string CannelID)
        {
            List<DBTag> tags = new List<DBTag>();
            if (string.IsNullOrEmpty(_CannelID))
                return tags;

            int LstRowsCount = gridView1.RowCount;
            var processedTagNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < LstRowsCount - 1; i++)
            {
                var tagName = gridView1.GetRowCellValue(i, "TagName").ToString();
                var address = gridView1.GetRowCellValue(i, "Address").ToString();
                if (string.IsNullOrEmpty(tagName) || string.IsNullOrEmpty(address))
                    continue;

                // 检查是否已处理过相同的 TagName
                if (processedTagNames.Contains(tagName))
                    continue; // 跳过重复的

                processedTagNames.Add(tagName);

                string dataTypeStr = gridView1.GetRowCellValue(i, "DataType").ToString();
                var uom         = gridView1.GetRowCellValue(i, "UOM")?.ToString();
                var description = gridView1.GetRowCellValue(i, "Description")?.ToString();
                int dataType = -1;
                try
                { dataType = int.Parse(dataTypeStr); }
                catch
                { }
                tags.Add(new DBTag()
                {
                    CannelID = _CannelID,
                    TagId = $"{_CannelID}.{tagName}",
                    TagName = tagName,
                    Address = address,
                    DataType = dataType,//DBTag.GetDataType(dataTypeStr),
                    UOM = uom,
                    Description = description
                });
            }
            return tags;
        }

        private void updateSheetRowsByFreeversion(int max_row)
        {
            GridView view = gridView1;

            if (view.RowCount > max_row)
            {
                for (int i = view.RowCount - 1; i >= max_row; i--)
                {
                    view.DeleteRow(i);
                }
            }

        }

        #endregion [数据操作]

        #region [grid operation]
        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }


        public void SetupGridView()
        {
            RepositoryItemLookUpEdit repoLookUp = new RepositoryItemLookUpEdit();
            repoLookUp.DataSource = new List<DataTypeItem>
            {
                new DataTypeItem { Id = 0, Name = "bool" },
                new DataTypeItem { Id = 1, Name = "string" },
                new DataTypeItem { Id = 2, Name = "wstring" },
                new DataTypeItem { Id = 3, Name = "byte" },
                new DataTypeItem { Id = 4, Name = "int16" },
                new DataTypeItem { Id = 5, Name = "word" },
                new DataTypeItem { Id = 6, Name = "int" },
                new DataTypeItem { Id = 10, Name = "float" },
                new DataTypeItem { Id = 11, Name = "double" }
            };

            repoLookUp.DisplayMember = "Name";
            repoLookUp.ValueMember = "Id";
            repoLookUp.NullText = "";
            repoLookUp.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            gridControl1.RepositoryItems.Add(repoLookUp);
            gridView1.Columns["DataType"].ColumnEdit = repoLookUp;
            repoLookUp.PopulateColumns();
            repoLookUp.Columns["Id"].Visible = false;

            gridView1.IndicatorWidth = 40;

            // 设置新行位置在底部
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;

            // 启用自动添加新行
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            gridView1.OptionsBehavior.Editable = true;

            // 处理新行初始化事件
            gridView1.InitNewRow += GridView1_InitNewRow;
            gridView1.RowUpdated += GridView1_RowUpdated;
            gridView1.ValidateRow += GridView1_ValidateRow;
            gridView1.RowDeleting += GridView1_RowDeleting; // 处理行删除事件
        }

        private void GridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            // 新增行
            if (e.Row is DBTag model)
            {
                // 1. 保存到数据库
                //SaveToDatabase(model);
            }
        }

        private void GridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            // 预设新行数据
            view.SetRowCellValue(e.RowHandle, "UOM", "-");
        }

        private void GridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            var row = e.Row as DBTag;
            if (string.IsNullOrWhiteSpace(row.TagName))
            {
                e.Valid = false;
                e.ErrorText = "Name 不能为空";
            }
        }

        private void ClearAllSheet()
        {
            DataTable dt = new DataTable();
            this.gridControl1.DataSource = dt;
            gridControl1.Refresh();
            //TagList.Clear();
        }

        private void UpdateSheet(string CannelID)
        {
            ClearAllSheet();

            //<填写点表>
            if (DataImport.dicCannelTags.Count > 0 && DataImport.dicCannelTags.ContainsKey(CannelID))
            {
                int x = DataImport.dicCannelTags.Count;
                int y = DataImport.dicCannelTags[CannelID].Count;   
                BindingList<DBTag> TagList = new BindingList<DBTag>();
                foreach (var tag in DataImport.dicCannelTags[CannelID])
                {
                    TagList.Add(tag);
                }
                gridControl1.DataSource = TagList;
            }
            //</填写点表>
        }
        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "DataType")
            {
                if (e.Value != null)
                {
                    try
                    { 
                        int i = Convert.ToInt32(e.Value);
                        e.DisplayText = DBTag.DataTypeString(i);                    
                    }
                    catch
                    { }
                }
            }
        }
        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView1_RowDeleting(object sender, RowDeletingEventArgs e)
        {
            DBTag row = gridView1.GetRow(e.RowHandle) as DBTag;
            if (row == null) return;

            // 示例：已提交的记录禁止删除
            //if (row.Status == 1)
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("已提交的数据不允许删除");
            //    e.Cancel = true;
            //    return;
            //}

            //// 数据库删除（或软删除）
            //DeleteFromDb(row.Id);
        }


        #endregion //[grid operation]

        #region [button action]

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCannelID.Text))
                return;

            if (ServerCfg.Instance != null)
            {
                ConnctDevice cd = new ConnctDevice();
                foreach (var dv in ServerCfg.Instance.cDevices)
                {
                    if (_CannelID == dv.CannelID)
                    {
                        cd = dv;
                    }
                    else
                        continue;

                    cd.CannelID    = tbCannelID.Text;
                    cd.Description = tbDescription.Text;
                    cd.iActive    = cbActive.IsOn ? 1 : 0;

                    //<更改点表>
                    if (!DataImport.dicCannelTags.ContainsKey(_CannelID))
                    {
                        if (!DataImport.dicCannelTags.ContainsKey(cd.CannelID))
                            DataImport.dicCannelTags.Add(cd.CannelID, GetDBTagsFromSheet(cd.CannelID));
                        else
                            DataImport.dicCannelTags[cd.CannelID] = GetDBTagsFromSheet(cd.CannelID);
                    }
                    else
                    {
                        //1. DataImport.CannelTags.Keys[_CannelID] = cd.CannelID;
                        //更改Cannel键值
                        DataImport.dicCannelTags = DataImport.dicCannelTags.ToDictionary(k => k.Key == _CannelID ? tbCannelID.Text : k.Key, k => k.Value);

                        //2. 保存点表
                        DataImport.dicCannelTags[tbCannelID.Text] = GetDBTagsFromSheet(tbCannelID.Text);

                    }

                    List<DBTag> tosave = new List<DBTag>();
                    foreach (var t in DataImport.dicCannelTags)
                    {
                        tosave.AddRange(t.Value);
                    }

                    //----------------------------------------------------------------------
                    // 免费版本 列数限制
                    if (DACtrl.free_Version)
                    {                           

                        if (tosave.Count > DACtrl.free_CannelTags)
                        {
                            for (int i = tosave.Count - 1; i >= DACtrl.free_CannelTags; i--)
                            {
                                tosave.RemoveAt(i);
                            }

                            DevExpress.XtraEditors.XtraMessageBox.Show($"免费版本导入变量点数量不能超过 {DACtrl.free_CannelTags} 个。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            updateSheetRowsByFreeversion(DACtrl.free_CannelTags);
                            DataImport.dicCannelTags[tbCannelID.Text] = GetDBTagsFromSheet(tbCannelID.Text);
                        }
                    }

                    XmlHelper.XmlSerializeToFile(tosave, pubDefine.TagDefine, Encoding.UTF8);
                    dv.CannelID = tbCannelID.Text;
                    //</更改点表>

                    _CannelID = dv.CannelID;
                    navBarItem1.Caption = dv.CannelID; // 更新导航栏名称

                    //***************************************
                    //3. 保存 Siemens Profinet配置
                    if (cbCannel.SelectedItem != null &&
                        cbCannel.SelectedItem.ToString() == "Siemens Profinet")
                    {
                        cd.Address = "1";
                        cd.devType = DeviceType.ProfinetS7;
                        cd.SampleInterval = 20;
                        cd.SampleQty = 10;

                        bool v = PubFunction.IsValidIP(ucProfinet.Host);
                        if (!v)
                        {
                            //new diaMessageY(Resources.msgIPWrong, Resources.msgSaveError).ShowDialog();
                            DevExpress.XtraEditors.XtraMessageBox.Show($"IP 地址错误。", "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        int rack = 0;
                        int slot = 0;
                        try
                        {
                            rack = int.Parse(ucProfinet.Rack);
                            slot = int.Parse(ucProfinet.Slot);
                        }
                        catch
                        {
                            //new diaMessageY(Resources.msgRSError, Resources.msgSaveError).ShowDialog();
                            DevExpress.XtraEditors.XtraMessageBox.Show($"解析profinet rack/slot地址错误。", "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        cd.IpCfg = new IpCfg() { Host = ucProfinet.Host, Rack = rack, Slot = slot };
                        cd.UAParamter = null;

                    }
                    //***************************************
                    //4 保存 OPC UA 配置
                    else if (cbCannel.SelectedItem != null &&
                            cbCannel.SelectedItem.ToString() == "OPC UA")
                    {
                        cd.Address = "2";
                        cd.devType = DeviceType.OPCUA;
                        cd.SampleInterval = 20;
                        cd.SampleQty = 10;
                        cd.IpCfg = null;
                        cd.UAParamter = new UACfg()
                        {
                            EndpointUrl = ucOPCUA.EndpointUrl,
                            securityPolicy = ucOPCUA.securityPolicy,
                            SecurityMode = ucOPCUA.SecurityMode,
                            Psw = ucOPCUA.Psw,
                            User = ucOPCUA.User,
                            UseUserLogIn = ucOPCUA.UseUserLogIn,
                            ApplicationName = ucOPCUA.ApplicationName,
                        };
                    }

                    //5. 保存
                    XmlHelper.XmlSerializeToFile(ServerCfg.Instance, pubDefine.Configuration, Encoding.UTF8);


                    //6. 发送事件,更新数列表设备名
                    OnMyEvent(new CannelEventArgs(CfgMessage.UpdateTreeItemName, 1));
                }
            }

        }

        private void btImport_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(tbCannelID.Text)) return;
            string fileName1 = "";
            XtraOpenFileDialog dlgOpenFile = new XtraOpenFileDialog();
            dlgOpenFile.Filter = "excel.xlsx|*.xlsx";


            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                fileName1 = dlgOpenFile.FileName;
            }

            DataTable dtt = new DataTable();
            if (!File.Exists(fileName1))
            {
                return;
            }
            else
            {
                try
                {
                    dtt = ExcelHelper.Import(fileName1);



                    //----------------------------------------------------------------------
                    // 免费版本 列数限制
                    if (DACtrl.free_Version)
                    {
                        if (dtt == null ) return;

                        if (dtt.Rows.Count > DACtrl.free_CannelTags)
                        { 
                            for (int i = dtt.Rows.Count - 1; i >= DACtrl.free_CannelTags; i--)
                            {
                                dtt.Rows.RemoveAt(i);
                            }
                            MessageBox.Show($"免费版本导入变量点数量不能超过 {DACtrl.free_CannelTags} 个。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    if (dtt.Rows.Count > 0)
                    {
                        List<DBTag> Tags = new List<DBTag>();

                        // 方案1A：使用 HashSet 记录已处理的 TagName
                        var processedTagNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                        //**************************************
                        //驱动不同，添加更多代码
                        //**************************************
                        if (cbCannel.SelectedItem.ToString() == "Siemens Profinet")
                        {
                            foreach (DataRow x in dtt.Rows)
                            {
                                // 检查是否已处理过相同的 TagName
                                if (processedTagNames.Contains(x[0].ToString()))
                                    continue; // 跳过重复的

                                processedTagNames.Add(x[0].ToString());

                                Tags.Add(new DBTag()
                                {
                                    TagId = tbCannelID.Text + "." + x[0].ToString(),
                                    Address = x[1].ToString(),
                                    DataType = DBTag.GetDataType(x[2].ToString()),
                                    CannelID = tbCannelID.Text,
                                    TagName = x[0].ToString(),
                                    UOM = x[3].ToString(),
                                    Description = x[4].ToString()
                                });
                            }
                        }
                        else if (cbCannel.SelectedItem.ToString() == "OPC UA")
                        {

                            int ccount = dtt.Columns.Count;
                            if (ccount >= 3)
                                foreach (DataRow x in dtt.Rows)
                                {
                                    // 检查是否已处理过相同的 TagName
                                    if (processedTagNames.Contains(x[0].ToString()))
                                        continue; // 跳过重复的

                                    processedTagNames.Add(x[0].ToString());

                                    Tags.Add(new DBTag()
                                    {
                                        TagId = tbCannelID.Text + "." + x[0].ToString(),
                                        Address = x[1].ToString(),
                                        DataType = DBTag.GetDataType(x[2].ToString()),
                                        CannelID = tbCannelID.Text,
                                        TagName = x[0].ToString(),
                                        UOM = x[3].ToString(),
                                        Description = x[4].ToString()
                                    });
                                }
                            else
                                foreach (DataRow x in dtt.Rows)
                                {
                                    Tags.Add(new DBTag()
                                    {
                                        TagId = tbCannelID.Text + "." + x[0].ToString(),
                                        Address = x[1].ToString(),
                                        CannelID = tbCannelID.Text,
                                        TagName = x[0].ToString(),
                                        DataType = -1
                                    });
                                }
                        }

                        if (Tags.Count > 0)
                        {
                            Tags.RemoveAll(tag => tag.TagName == "");
                            Tags = Tags.OrderBy(it => it.TagName).ToList();
                        }

                        if (DataImport.dicCannelTags.ContainsKey(tbCannelID.Text))
                        {
                            DataImport.dicCannelTags.Remove(tbCannelID.Text);
                        }

                        DataImport.ImportCannelTags(Tags);
                        List<DBTag> tosave = new List<DBTag>();
                        foreach (var t in DataImport.dicCannelTags)
                        {
                            tosave.AddRange(t.Value);
                        }
                        XmlHelper.XmlSerializeToFile(tosave, pubDefine.TagDefine, Encoding.UTF8);

                        UpdateSheet(tbCannelID.Text);
                        //MessageBox.Show($"导入变量点{Tags.Count}个。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //new diaMessageY(Resources.msgImportTag1 + Tags.Count + Resources.msgExport2, Resources.PromptMessage).ShowDialog();
                        DevExpress.XtraEditors.XtraMessageBox.Show($"导入变量点{Tags.Count}个。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Excel 导入失败：" + ex.Message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //new diaMessageY(Resources.msgImportExcelFail + "\r\n" + ex.Message + "\r\n" + ex.InnerException, Resources.PromptMessage).ShowDialog();

                    DevExpress.XtraEditors.XtraMessageBox.Show("Excel 导入失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCannelID.Text)) return;

            List<DBTag> list = new List<DBTag>();
            string localFilePath; //获得文件路径 
            string fileNameExt;  //获取文件名，不带路径
            //*********************************************************************

            //string localFilePath, fileNameExt, newFileName, FilePath; 
            XtraSaveFileDialog sfd = new XtraSaveFileDialog();

            //设置文件类型 
            sfd.Filter = "Excel.xlsx|*.xlsx";

            //设置默认文件类型显示顺序 
            sfd.FilterIndex = 1;

            //保存对话框是否记忆上次打开的目录 
            sfd.RestoreDirectory = true;

            //点了保存按钮进入 
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                localFilePath = sfd.FileName.ToString(); //获得文件路径 
                fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1); //获取文件名，不带路径
            }
            else
            {
                localFilePath = ""; fileNameExt = "";
            }

            if (string.IsNullOrEmpty(localFilePath))
                return;

            try
            {
                DataTable dtl = new DataTable("Table1");

                //**************************************
                //驱动不同，添加更多代码
                //**************************************
                if (cbCannel.SelectedItem.ToString() == "Siemens Profinet")
                {
                    dtl.Columns.Add("TagId", typeof(string));
                    dtl.Columns.Add("Address", typeof(string));
                    dtl.Columns.Add("DataType", typeof(string));
                    dtl.Columns.Add("UOM", typeof(string));
                    dtl.Columns.Add("Description", typeof(string));
                    if (File.Exists(pubDefine.TagDefine))
                    {
                        List<DBTag> yy = XmlHelper.XmlDeserializeFromFile<List<DBTag>>(pubDefine.TagDefine, Encoding.UTF8);

                        var xx = new List<DBTag>();
                        if (yy != null && yy.Count > 0)
                            xx = yy.Where(it => it.CannelID == tbCannelID.Text).ToList();

                        if (xx.Count > 0)
                        {

                            for (int i = 0; i < xx.Count; i++)
                            {
                                if (xx != null && xx.Count > 0)
                                {
                                    string dp = "";
                                    dp = DBTag.DataTypeString(xx[i].DataType);
                                    dtl.Rows.Add(xx[i].TagName, xx[i].Address, dp, xx[i].UOM, xx[i].Description);
                                }
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("System will export demo tag list.", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return;

                            dtl.Rows.Add("A_trg", "DB300.DBX0.0", "bool", "-", "");
                            dtl.Rows.Add("A_Line", "DB300.DBD4", "int", "-", "");
                            dtl.Rows.Add("D_NetWeight", "DB300.DBD48", "float", "-", "");
                            dtl.Rows.Add("Temp1", "DB301.string0.10", "string", "-", "");
                            dtl.Rows.Add("Temp2", "DB301.wstring12.10", "wstring", "-", "");
                            dtl.Rows.Add("state", "DB302.DBW0", "word", "-", "");
                        }
                    }

                    else
                    {
                        if (MessageBox.Show("System will export demo tag list.", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return;

                        dtl.Rows.Add("A_trg", "DB300.DBX0.0", "bool", "-", "");
                        dtl.Rows.Add("A_trg", "DB300.DBX0.0", "bool", "-", "");
                        dtl.Rows.Add("A_Line", "DB300.DBD4", "int", "-", "");
                        dtl.Rows.Add("D_NetWeight", "DB300.DBD48", "float", "-", "");
                        dtl.Rows.Add("Temp1", "DB301.string0.10", "string", "-", "");
                        dtl.Rows.Add("Temp2", "DB301.wstring12.10", "wstring", "-", "");
                    }
                }
                else if (cbCannel.SelectedItem.ToString() == "OPC UA")
                {
                    dtl.Columns.Add("TagId", typeof(string));
                    dtl.Columns.Add("Address", typeof(string));
                    dtl.Columns.Add("DataType", typeof(string));
                    dtl.Columns.Add("UOM", typeof(string));
                    dtl.Columns.Add("Description", typeof(string));

                    if (File.Exists(pubDefine.TagDefine))
                    {
                        List<DBTag> yy = XmlHelper.XmlDeserializeFromFile<List<DBTag>>(pubDefine.TagDefine, Encoding.UTF8);
                        var xx = new List<DBTag>();
                        if (yy != null && yy.Count > 0)
                            xx = yy.Where(it => it.CannelID == tbCannelID.Text).ToList();

                        if (xx.Count > 0)
                        {
                            for (int i = 0; i < xx.Count; i++)
                            {
                                if (xx != null && xx.Count > 0)
                                {
                                    string dp = "";
                                    dtl.Rows.Add(xx[i].TagName, xx[i].Address, DBTag.DataTypeString(xx[i].DataType), xx[i].UOM, xx[i].Description);
                                }
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("System will export demo tag list.", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return;

                            dtl.Rows.Add("cip_CipPress", "ns=2;s=模拟器示例.函数.Random1", "float", "-", "");
                            dtl.Rows.Add("cip_CipTemp", "ns=2;s=模拟器示例.函数.Random2", "float", "-", "");
                            dtl.Rows.Add("pkg_Bucket", "ns=2;s=模拟器示例.函数.Sine1", "string", "-", "");
                        }
                    }

                    else
                    {
                        if (MessageBox.Show("System will export demo tag list.", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            return;

                        dtl.Rows.Add("cip_CipPress", "ns=2;s=模拟器示例.函数.Random1", "-", "");
                        dtl.Rows.Add("cip_CipTemp", "ns=2;s=模拟器示例.函数.Random2", "-", "");
                        dtl.Rows.Add("pkg_Bucket", "ns=2;s=模拟器示例.函数.Sine1", "-", "");
                    }
                }

                try
                {
                    ExcelHelper eh = new ExcelHelper();

                    eh.OutputRoot = localFilePath;
                    eh.Ds.Tables.Add(dtl.Copy());
                    eh.SaveDs();
                    //MessageBox.Show("共导出 " + dtl.Rows.Count.ToString() + " 条数据。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //new diaMessageY(Resources.msgExport1 + dtl.Rows.Count.ToString() + Resources.msgExport2, Resources.PromptMessage).ShowDialog();
                    DevExpress.XtraEditors.XtraMessageBox.Show("共导出 " + dtl.Rows.Count.ToString() + " 条数据。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    //new diaMessageY(Resources.msgExportExcelFail + "\r\n" + ex.Message + "\r\n" + ex.InnerException, Resources.PromptMessage).ShowDialog();
                    DevExpress.XtraEditors.XtraMessageBox.Show("导出失败" + "\r\n" + ex.Message + "\r\n" + ex.InnerException, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cbCannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                //**************************************
                //驱动不同，添加更多代码
                //**************************************
                if (cbCannel.SelectedIndex < 0)
                    return;

                if (cbCannel.SelectedItem.ToString() == "Siemens Profinet")
                {
                    ucDriverEmpty.Visible = false;
                    ucProfinet.Visible = true;
                    ucOPCUA.Visible = false;
                    ucProfinet.ApplyLanguage();
                }

                else if (cbCannel.SelectedItem.ToString() == "OPC UA")
                {
                    ucDriverEmpty.Visible = false;
                    ucProfinet.Visible = false;
                    ucOPCUA.Visible = true;
                    ucOPCUA.ApplyLanguage();
                }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            int[] rowHandles = gridView1.GetSelectedRows();
            if (rowHandles.Length == 0) return;

            // 必须排序（从大到小）
            Array.Sort(rowHandles);
            Array.Reverse(rowHandles);

            var result = DevExpress.XtraEditors.XtraMessageBox.Show($"确认删除选中的 {rowHandles.Length} 行？",
                  "批量删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                gridView1.BeginUpdate();
                try
                {
                    foreach (int rowHandle in rowHandles)
                    {

                        try 
                        { 
                            gridView1.DeleteRow(rowHandle);                       
                            DataImport.dicCannelTags[_CannelID].RemoveAt(rowHandle);                        
                        }
                        catch { }

                    }
                }
                finally
                {
                    gridView1.EndUpdate();
                }
            }
        }
        #endregion  //[button action


    }
}
