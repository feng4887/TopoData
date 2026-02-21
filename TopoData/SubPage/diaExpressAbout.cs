//using auDAManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopoData.SubPage
{
    public partial class diaExpressAbout : DevExpress.XtraEditors.XtraForm
    {
        public diaExpressAbout()
        {
            InitializeComponent();
        }

        private void btYes_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void diaExpressAbout_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            //btYes.Text = Resources.Close;
            //label1.Text = Resources.msgExample1;
            //label2.Text = Resources.msgExample2;
        }
    }
}
