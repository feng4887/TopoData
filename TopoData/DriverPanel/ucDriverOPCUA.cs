using auDASLib;
using Opc.Ua;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TopoData.model;
using TopoData.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace TopoData
{
    public partial class ucDriverOPCUA : DevExpress.XtraEditors.XtraUserControl
    {
        public string EndpointUrl
        {
            get
            {
                return tbEndPoint.Text;
            }
            set
            {
                tbEndPoint.Text = value;
            }
        }

        private string _SecurityMode = "";
        public string SecurityMode
        {
            get
            {
                return _SecurityMode;
            }
            set
            {
                _SecurityMode = value;
            }
        }

        private string _securityPolicy = "";
        public string securityPolicy
        {
            get
            {
                return _securityPolicy;
            }
            set
            {
                _securityPolicy = value;
            }
        }
        private string _ApplicationName = "";
        public string ApplicationName
        {
            get
            {
                return _ApplicationName;
            }
            set
            {
                _ApplicationName = value;
            }
        }
        public string User
        {
            get
            {
                return tbUserName.Text;
            }
            set
            {
                tbUserName.Text = value;
            }
        }
        public string Psw
        {
            get
            {
                return tbPsw.Text;
            }
            set
            {
                tbPsw.Text = value;
            }
        }

        public bool UseUserLogIn
        {
            get
            {
                return cbUseUserLogIn.Checked;
            }
            set
            {
                cbUseUserLogIn.Checked = value;
            }
        }

        public void InitControl()
        {
            _rtv.Clear();
            _rtv.Add(new UACfg()
            {
                securityPolicy = _securityPolicy,
                SecurityMode = _SecurityMode,
                ApplicationName = _ApplicationName
            });


            cbEndpointItems.Properties.Items.Clear();
            cbEndpointItems.Properties.Items.Add(GetEndpoint());
            cbEndpointItems.SelectedIndex = 0;
        }

        public ucDriverOPCUA()
        {
            InitializeComponent();
        }

        private void ucDriverOPCUA_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            label3.Text = Resources.User_user;
            label4.Text = Resources.User_psd;
            btGetEndpoint.Text  = Resources.GetEndpoint;
            cbUseUserLogIn.Text = Resources.UseUserLogIn;
            btGetTag.Text       = Resources.GetTag;
        }

        private string GetEndpoint()
        {
            if (string.IsNullOrEmpty(_SecurityMode) || string.IsNullOrEmpty(_securityPolicy))
            {
                return "";
            }
            else
                return "[" + _SecurityMode + "] " + " [" + _securityPolicy + "] ";
        }

        List<UACfg> _rtv = new List<UACfg>();

        private void cbUseUserLogIn_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbUseUserLogIn.Checked)
            {
                tbUserName.Enabled = true;
                tbPsw.Enabled = true;
            }
            else
            {
                tbUserName.Enabled = false;
                tbPsw.Enabled = false;
            }
        }

        private void cbEndpointItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_rtv.Count == 0)
            {
                _SecurityMode = "";
                _securityPolicy = "";
                _ApplicationName = "";
            }
            if (cbEndpointItems.SelectedIndex >= 0)
            {
                _SecurityMode = _rtv[cbEndpointItems.SelectedIndex].SecurityMode;
                _securityPolicy = _rtv[cbEndpointItems.SelectedIndex].securityPolicy;
                _ApplicationName = _rtv[cbEndpointItems.SelectedIndex].ApplicationName;
            }
        }
        private async void btGetEndpoint_Click(object sender, EventArgs e)
        {
            cbEndpointItems.Properties.Items.Clear();
            _rtv.Clear();
            _SecurityMode = "";
            _securityPolicy = "";
            _ApplicationName = "";
            //The local discovery URL for the discovery server
            string discoveryUrl = tbEndPoint.Text;

            try
            {
                await Task.Run(() =>
                {
                    _rtv = UAOperator.GetUACfgs(tbEndPoint.Text);
                });

                if (_rtv != null || _rtv.Count > 0)
                {
                    cbEndpointItems.Properties.Items.Clear();
                    foreach (var r in _rtv)
                    {
                        string item = "[" + r.SecurityMode + "] " + " [" + r.securityPolicy + "] ";
                        int i = cbEndpointItems.Properties.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btGetTag_Click(object sender, EventArgs e)
        {
            diaOPCUATagSelect uatgselect = new diaOPCUATagSelect();
            uatgselect.mySession = null;
            uatgselect.EndpointUrl = tbEndPoint.Text;
            uatgselect.SecurityMode = _SecurityMode;
            uatgselect.SecurityPolicy = _securityPolicy;
            uatgselect.ApplicationName = _ApplicationName;

            if (string.IsNullOrEmpty(_securityPolicy) || string.IsNullOrEmpty(_SecurityMode))
            {
                //diaMessageY mb = new diaMessageY(Resources.msgUACfgError1, Resources.PromptMessage);
                // DialogResult dr = mb.ShowDialog();
                DevExpress.XtraEditors.XtraMessageBox.Show("请先获取并选择端点配置！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbUseUserLogIn.Checked)
            {
                uatgselect.User = tbUserName.Text;
                uatgselect.Psw = tbPsw.Text;
                uatgselect.UsePsw = true;
            }
            else
            {
                uatgselect.UsePsw = false;
            }
            uatgselect.TagEvent += HandleEvent;
            uatgselect.ShowDialog();
        }

        #region 事件
        /// <summary>
        /// 添加点表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleEvent(object sender, DBTagEventArgs e)
        {
            if (e.CannelType == DeviceType.OPCUA) //Opc ua
            {
                OnTagEvent(e);
            }

            else if (e.CannelType == DeviceType.ProfinetS7) //profinet
            {
            }
        }


        //// 用EventHandler<woEventArgs>委托定义事件
        public event EventHandler<DBTagEventArgs> TagEvent;

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTagEvent(DBTagEventArgs e)
        {
            TagEvent?.Invoke(this, e);
        }
        #endregion //事件

        private void cbEndpointItems_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (_rtv.Count == 0)
            {
                _SecurityMode = "";
                _securityPolicy = "";
                _ApplicationName = "";
            }
            if (cbEndpointItems.SelectedIndex >= 0)
            {
                _SecurityMode = _rtv[cbEndpointItems.SelectedIndex].SecurityMode;
                _securityPolicy = _rtv[cbEndpointItems.SelectedIndex].securityPolicy;
                _ApplicationName = _rtv[cbEndpointItems.SelectedIndex].ApplicationName;
            }
        }
    }
}
