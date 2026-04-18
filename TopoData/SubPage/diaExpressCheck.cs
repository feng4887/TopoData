//using auDAManager.Properties;
using auDASLib;
using SqlSugar;
using SqlSugar.Extensions;
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

namespace TopoData.SubPage
{
    public partial class diaExpressCheck : DevExpress.XtraEditors.XtraForm
    {
        string _eText = "";

        /// <summary>
        /// 脚本或表达式
        /// </summary>
        public string ExpressionText
        {
            get { return _eText; }
            set { _eText = value; }
        }

        List<DBTag> _eTags = new List<DBTag>();
        public List<DBTag> ExpressionTags
        {
            get
            {
                return _eTags;
            }

            set
            {
                _eTags = value;
            }
        }

        public diaExpressCheck()
        {
            InitializeComponent();
        }
        private void diaExpressCheck_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            for (int i = 0; i < _eTags.Count; i++)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = _eTags[i].TagName;
                this.dataGridView1.Rows[index].Cells[1].Value = _eTags[i].DefaultValue;
                this.dataGridView1.Rows[index].Cells[2].Value = _eTags[i].DataType;
            }
            ApplyLanguage();
        }

        /// <summary>
        /// 应用多语言
        /// </summary>
        public void ApplyLanguage()
        {
            //this.Text = Resources.diaExpressCheck;
            //dataGridView1.Columns[0].HeaderText = Resources.TagInternal;
            //dataGridView1.Columns[1].HeaderText = Resources.Value;
            //btCheck.Text = Resources.Check;
            //btClose.Text = Resources.Close;

        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btCheck_Click(object sender, EventArgs e)
        {
            Dictionary<string, dynamic> para = new Dictionary<string, dynamic>();

            try 
            {
                // 添加行
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    var TN = row.Cells[0].Value.ToString(); //TAGNAME
                    var VL = row.Cells[1].Value == null?"" : row.Cells[1].Value.ToString(); //VALUE
                    string DT = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString(); //datatype
                    if (string.IsNullOrEmpty(VL))
                    {
                        MessageBox.Show($"[{TN}] needs a input value.", "Warning"
                            , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    dynamic value;
                    int datatype= int.Parse(DT);
                    DT= DBTag.DataTypeString(datatype);
                    switch (DT)
                    {
                        case "bool":
                            value = bool.Parse(VL) ; break;
                        case "string":
                        case "wstring":
                            value = VL; break;
                        case "byte":
                            value = byte.Parse(VL); break;
                        case "int":
                            value = int.Parse(VL); break;
                        case "float":
                            value = float.Parse(VL); break;
                        case "double":
                            value = double.Parse(VL); break;
                        default:
                            value = float.Parse(VL); break;
                    }
                    para.Add(TN, value);
                }
                //尝试解析表达式
                bool result = FleeOperator.ConditionIsOK(ExpressionText, para);

                //MessageBox.Show($"Expression Good! Result is [{result}]", "Information"
                //    , MessageBoxButtons.OK, MessageBoxIcon.Information);
                DevExpress.XtraEditors.XtraMessageBox.Show(this, $"Expression Good! Result is [{result}]",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Expression Bad :( \r\n" + ex.Message,"Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                DevExpress.XtraEditors.XtraMessageBox.Show(this, "Expression Bad :( \r\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
