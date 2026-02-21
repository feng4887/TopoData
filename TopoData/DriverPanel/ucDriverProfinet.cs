
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopoData.Properties;

namespace TopoData
{
    public partial class ucDriverProfinet : DevExpress.XtraEditors.XtraUserControl
    {
        public string Host
        {
            get
            {
                return tbStationIPMain.Text;
            }
            set
            {
                tbStationIPMain.Text = value;
            }
        }
        public string Rack
        {
            get
            {
                return tbStationRackMain.Text;
            }
            set
            {
                tbStationRackMain.Text = value;
            }
        }

        public string Slot
        {
            get
            {
                return tbStationSlotMain.Text;
            }
            set
            {
                tbStationSlotMain.Text = value;
            }
        }
        public ucDriverProfinet()
        {
            InitializeComponent();
        }

        private void ucDriverProfinet_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            label16.Text = Resources.PLC_Cfg_Notes;
            label2.Text  = Resources.Save_Restart;
            label19.Text = Resources.S7_400Look4Cfg;
        }
    }
}
