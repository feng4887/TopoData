
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace auDAManager.DriverPanel
{
    public partial class ucDriverModbusTCP : DevExpress.XtraEditors.XtraUserControl
    {
        public string Host
        {
            get
            {
                return tbIP.Text;
            }
            set
            {
                tbIP.Text = value;
            }
        }
        public int Port
        {
            get
            {
                int port = 0;
                if (int.TryParse(tbPort.Text, out port))
                    return port;
                else
                { 
                    MessageBox.Show("Port must be a number");
                    return 502;              
                }
            }
            set
            {
                tbPort.Text = string.Format("{0}", value);
            }
        }

        public string ParseData
        {
            get
            {

                if (cbParseData.SelectedIndex == 0)
                    return "CDAB";
                else if (cbParseData.SelectedIndex == 1)
                {
                    return "ABCD";
                }
                else
                { return "CDAB"; }
            }
            set
            {
                if (value == "CDAB")
                    cbParseData.SelectedIndex = 0;
                else if(value == "ABCD")
                    cbParseData.SelectedIndex = 1;
            }
        }

        public int DeviceAddress
       {
            get
            {
                int addr = 0;
                if (int.TryParse(tbDeviceAddress.Text, out addr))
                    return addr;
                else
                { 
                    MessageBox.Show("Device address must be a number");
                    return 0;              
                }
            }
            set
            {
                tbDeviceAddress.Text = string.Format("{0}", value);
            }
        }


        public ucDriverModbusTCP()
        {
            InitializeComponent();
        }

        private void ucDriverModbusTCP_Load(object sender, EventArgs e)
        {
            //cbParseData.SelectedIndex = 0;
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
        }
    }
}
