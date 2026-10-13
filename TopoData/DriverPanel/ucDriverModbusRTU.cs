
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace auDAManager.DriverPanel
{
    public partial class ucDriverModbusRTU : DevExpress.XtraEditors.XtraUserControl
    {
        public string ComPort
        {
            get => cbComPort.Text;
            set => cbComPort.Text = value;
        }

        public int BaudRate
        {
            get
            {
                if (int.TryParse(cbBaudRate.Text, out int b)) return b;
                return 9600;
            }
            set => cbBaudRate.Text = value.ToString();
        }

        public string ParitySetting
        {
            get => cbParity.SelectedIndex switch { 0 => "None", 1 => "Odd", 2 => "Even", _ => "None" };
            set => cbParity.SelectedIndex = value switch { "None" => 0, "Odd" => 1, "Even" => 2, _ => 0 };
        }

        public Parity Parity
        {
            get => cbParity.SelectedIndex switch { 0 => Parity.None, 1 => Parity.Odd, 2 => Parity.Even, _ => Parity.None };
            set => cbParity.SelectedIndex = value switch { Parity.None => 0, Parity.Odd => 1, Parity.Even => 2, _ => 0 };
        }

        public int DataBitsSetting
        {
            get => int.TryParse(cbDataBits.Text, out int d) ? d : 8;
            set => cbDataBits.Text = value.ToString();
        }

        public string StopBitsSetting
        {
            get => cbStopBits.SelectedIndex switch { 0 => "One", 1 => "Two", _ => "One" };
            set => cbStopBits.SelectedIndex = value switch { "One" => 0, "Two" => 1, _ => 0 };
        }

        public StopBits StopBits
        {
            get => cbStopBits.SelectedIndex switch { 0 => StopBits.One, 1 => StopBits.Two, _ => StopBits.One };
            set => cbStopBits.SelectedIndex = value switch { StopBits.One => 0, StopBits.Two => 1, _ => 0 };
        }

        public string ParseData
        {
            get => cbParseData.SelectedItem?.ToString() ?? "CDAB";
            set => cbParseData.SelectedIndex = cbParseData.Properties.Items.IndexOf(value) >= 0
                ? cbParseData.Properties.Items.IndexOf(value)
                : 0;
        }

        public int DeviceAddress
        {
            get
            {
                if (int.TryParse(tbDeviceAddress.Text, out int a)) return a;
                else
                {
                    MessageBox.Show("Address should be int value");
                    return 1;
                }
            }
            set => tbDeviceAddress.Text = value.ToString();
        }

        public ucDriverModbusRTU()
        {
            InitializeComponent();
            cbParseData.SelectedIndex = 0;  //CDAB
            cbStopBits.SelectedIndex  = 0;  //One
            cbParity.SelectedIndex    = 0;  //None
            cbBaudRate.Text  = "9600";
            cbDataBits.Text  = "8";
            cbComPort.Text   = "COM1";
            tbDeviceAddress.Text = "1";
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
        }
    }
}
