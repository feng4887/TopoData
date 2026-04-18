using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopoData.Page
{
    public partial class ucAbout : DevExpress.XtraEditors.XtraUserControl
    {
        public ucAbout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            //label1.Text = Resources.CannelName;
            //label2.Text = Resources.DriverType;
            //groupBox1.Text = Resources.CannelCfg;
            //cbActive.Text = Resources.Enable;
            //btClear.Text = Resources.Clear;
            //btConfirm.Text = Resources.Save;
            //btExport.Text = Resources.Export;
            //btImport.Text = Resources.Import;
            //if (cbCannel.SelectedItem?.ToString() == "Siemens Profinet")
            //{
            //    ucProfinet.ApplyLanguage();
            //}
            //else if (cbCannel.SelectedItem?.ToString() == "OPC UA")
            //{
            //    ucOPCUA.ApplyLanguage();
            //}
        }

        private void ucAbout_Load(object sender, EventArgs e)
        {
            memoEdit1.Text = "版本号：TopoData V1.0.0\r\n\r\n" 
                             +"开发者：Topotech团队\r\n\r\n" 
                             +"说明：\r\n\r\n" 
                             +"1. 这是 TopoData技术预览版，完全免费，此版本通过了一定的测试，可以在生产环境中使用，目前仍存在一些已知问题/错误，如果对生产造成损失开发团队不负责，" 
                             +"您可以发邮件到\r\n\r\n" + " 【feng4887@hotmail】 \r\n\r\n" + "与我保持联系，我会尽快帮您解决问题。\r\n\r\n" 
                             +"2. 为了保证类似商业软件权益，本软件\r\n"
                             +" a.只提供了OPCUA和Profinet通讯协议;\r\n"
                             +" b.只能创建一个设备，点数不超过50个;\r\n"
                             +" c.数据存储表只有一个，并且字段不超过20个。\r\n\r\n" 
                             +"最后：\r\n" 
                             +"1. 如果您有好的建议或意见，欢迎随时与我们联系，我们会认真考虑您的建议，并在后续版本中进行改进。\r\n"
                             + "2. 感谢您使用TopoData数据采集与管理系统！";
        }
    }
}
