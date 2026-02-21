using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TopoData.SubPage
{
    public partial class diaWrite : DevExpress.XtraEditors.XtraForm
    {
        public diaWrite()
        {
            InitializeComponent();
        }

        string _strValue = string.Empty;
        public string strValue { get; set; }
        public string Address { get; set; }
        public string UOM { set { tbAddress.Text = value.ToString(); }  }
        public string TagName {
            get { return tbName.Text; }
            set 
            { 
                tbName.Text = value.ToString();
            } 
        }

        public DialogResult dret = DialogResult.No;

        private void frmWrite_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        /// <summary>
        /// 应用多语言
        /// </summary>
        public void ApplyLanguage()
        {
            //btCancel.Text = Resources.Cancel;
            //btWrite.Text  = Resources.WriteDown;
            //label3.Text   = Resources.TagName;
            //label1.Text   = Resources.UOM;
            //label2.Text   = Resources.Value;
            //this.Text     = Resources.WriteOperation;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Close();
        }

        private void btWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbValue.Text))
            { MessageBox.Show("Value cannot be empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            strValue = tbValue.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
