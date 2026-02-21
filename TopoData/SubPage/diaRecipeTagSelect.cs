//using auDAManager.Properties;
using auDASLib;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;



namespace TopoData.SubPage
{
    public partial class diaRecipeTagSelect : DevExpress.XtraEditors.XtraForm
    {
        public string TagID { get { return tbTagName.Text; } set { tbTagName.Text = value; } }
        public string UOM { get { return tbUOM.Text; } set { tbUOM.Text = value; } }
        public string DataType { get { return cbDataType.Text; } set { cbDataType.Text = value; } }
        public string Atribute { get { return tbAtribute.Text; } set { tbAtribute.Text = value; } }
        public string DefaultValue { get; set; } = "";

        public List<DBTag> tags = new List<DBTag>();

        /// <summary>
        /// 是一个内存点
        /// </summary>
        public bool isInternalValue { get; set; } = false;
        bool _MultiSelect;
        public diaRecipeTagSelect(bool MultiSelect = false)
        {
            InitializeComponent();
            _MultiSelect = MultiSelect;
        }
        private void diaColumnSelect_Load(object sender, EventArgs e)
        {
               
            tbDefaultValue.Text = DefaultValue;
            ApplyLanguage();
        }

        /// <summary>
        /// 应用多语言
        /// </summary>
        public void ApplyLanguage()
        {
            //this.Text = Resources.Config;
            //btOK.Text = Resources.OK;
            //btCancel.Text = Resources.Close;
            //rbDevice.Text = Resources.TagDevice;
            //rbInternal.Text = Resources.TagInternal;

            //label5.Text = Resources.ValueDefault;
            //label4.Text = Resources.TagName;
            //label2.Text = Resources.DataType;
            //label3.Text = Resources.UOM;
            //label1.Text = Resources.TableColName;
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            diaTagSelect diaTagSelect = new diaTagSelect(_MultiSelect);
            diaTagSelect.ShowDialog();
            tags = diaTagSelect.tags;
            if (diaTagSelect.tags.Count == 1)
            {
                if (!string.IsNullOrEmpty(diaTagSelect.TagName))
                {
                    tbTagName.Text = diaTagSelect.TagName;
                    tbUOM.Text = diaTagSelect.UOM;
                    cbDataType.Text = diaTagSelect.DataType;

                }
                
                tbAtribute.Enabled = true;
            }
            else if (diaTagSelect.tags.Count > 1)
            {
                
                tbTagName.Text = string.Join(";", tags.Select(tag => tag.TagName));
                tbAtribute.Text = "";
                tbAtribute.Enabled = false;
                tbUOM.Text = "";
                cbDataType.Text = "";
            }
            else
            {
                tags.Clear();
                tbAtribute.Enabled = true;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.No;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbAtribute.Text) && tags.Count == 1)
                {
                    MessageBox.Show("数据表字段名不能为空");
                    return;
                }
                bool isNumeric = Regex.IsMatch(tbAtribute.Text, @"^\d");
                if (isNumeric && tags.Count == 1)
                {
                    MessageBox.Show("字段不能以数字开头");
                    return;
                }
                if (string.IsNullOrWhiteSpace(cbDataType.Text) && tags.Count == 1)
                {
                    MessageBox.Show("请选择一个数据类型");
                    return;
                }



                DefaultValue = tbDefaultValue.Text;

                TagID = tbTagName.Text;
                UOM = tbUOM.Text;
                DataType = cbDataType.Text;
                Atribute = tbAtribute.Text;


                {
                    //<check datatype>
                    dynamic value;
                    //int datatype = int.Parse(DataType);
                    //DBTag.DataTypeString(datatype);
                    switch (DataType)
                    {
                        case "bool":
                            value = bool.Parse(DefaultValue); break;
                        case "string":
                        case "wstring":
                            value = DefaultValue; break;
                        case "byte":
                            value = byte.Parse(DefaultValue); break;
                        case "int":
                            value = int.Parse(DefaultValue); break;
                        case "float":
                            value = float.Parse(DefaultValue); break;
                        case "double":
                            value = double.Parse(DefaultValue); break;
                        default:
                            value = float.Parse(DefaultValue); break;
                    }
                    //</check datatype>
                }


                this.Close();
                this.DialogResult = DialogResult.Yes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.DialogResult = DialogResult.No;
            }
        }

    }
}
