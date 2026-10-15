using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using auDASLib;
using OpcDaNetCore.Utilities;
using TopoData.model;
using TopoData.Properties;

namespace TopoData
{
    public partial class ucDriverOPCDA : DevExpress.XtraEditors.XtraUserControl
    {
        private const int ClassNotRegisteredError = unchecked((int)0x80040154);

        public string Endpoint
        {
            get
            {
                return cbEndPoint.Text;
            }
            set
            {
                cbEndPoint.Text = value;
            }
        }

        public string HostName
        {
            get
            {
                return tbHostName.Text;
            }
            set
            {
                tbHostName.Text = value;
            }
        }


        public ucDriverOPCDA()
        {
            InitializeComponent();
        }

        private void ucDriverOPCDA_Load(object sender, EventArgs e)
        {
            InitControl();
        }

        /// <summary>
        /// 初始化 OPC DA 配置控件。
        /// </summary>
        public void InitControl()
        {
            if (string.IsNullOrWhiteSpace(tbHostName.Text))
                tbHostName.Text = "localhost";
        }

        /// <summary>
        /// 多语言切换
        /// </summary>
        public void ApplyLanguage()
        {
            btGetServer.Text = "枚举服务";
            btGetTag.Text = Resources.GetTag;
            label1.Text = Resources.HostName;
            label16.Text = "OPC Server：";
        }

        /// <summary>
        /// 根据主机名枚举 OPC DA 服务，并更新服务下拉列表。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btGetServer_Click(object sender, EventArgs e)
        {
            string hostName = HostName.Trim();
            if (string.IsNullOrWhiteSpace(hostName))
            {
                MessageBox.Show("请输入 OPC DA 服务器主机名。", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btGetServer.Enabled = false;
            UseWaitCursor = true;

            try
            {
                var serverNames = (await BrowseOpcDaServers.BrowseServersAsync(hostName))
                    .Select(server => server.ServerName)
                    .Where(serverName => !string.IsNullOrWhiteSpace(serverName))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(serverName => serverName)
                    .ToList();

                if (IsDisposed || !IsHandleCreated)
                    return;

                if (serverNames.Count == 0)
                {
                    MessageBox.Show($"主机 {hostName} 未发现 OPC DA 服务。", Resources.PromptMessage,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string currentEndpoint = Endpoint.Trim();
                cbEndPoint.BeginUpdate();
                try
                {
                    cbEndPoint.Items.Clear();
                    cbEndPoint.Items.AddRange(serverNames.ToArray());
                }
                finally
                {
                    cbEndPoint.EndUpdate();
                }

                int currentIndex = serverNames.FindIndex(serverName =>
                    string.Equals(serverName, currentEndpoint, StringComparison.OrdinalIgnoreCase));
                cbEndPoint.SelectedIndex = currentIndex >= 0 ? currentIndex : 0;
                cbEndPoint.DroppedDown = true;
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    string errorMessage = IsOpcCoreComponentRegistrationError(ex)
                        ? "无法使用 OPCEnum。请确认程序与 OPC Core Components 的位数一致，并确认目标主机已安装并注册 OPCEnum。本项目应使用 win-x86 版本运行。"
                        : $"枚举 OPC DA 服务失败：{ex.Message}";
                    MessageBox.Show(errorMessage, Resources.PromptMessage,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (!IsDisposed)
                {
                    UseWaitCursor = false;
                    btGetServer.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 判断异常是否由 OPC Core Components 或 OPCEnum 未按当前进程位数注册导致。
        /// </summary>
        /// <param name="exception">服务枚举异常。</param>
        /// <returns>属于 COM 类未注册错误时返回 true。</returns>
        private static bool IsOpcCoreComponentRegistrationError(Exception exception)
        {
            Exception? currentException = exception;
            while (currentException != null)
            {
                if (currentException.HResult == ClassNotRegisteredError ||
                    currentException.Message.Contains("0x80040154", StringComparison.OrdinalIgnoreCase))
                    return true;

                currentException = currentException.InnerException;
            }

            return false;
        }

        #region 事件
        /// <summary>
        /// 添加点表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleEvent(object sender, DBTagEventArgs e)
        {
            if (e.CannelType == DeviceType.OPCDA) //Opc da
            {
                OnTagEvent(e);
            }
        }


        // 用EventHandler<woEventArgs>委托定义事件
        public event EventHandler<DBTagEventArgs>? TagEvent;

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTagEvent(DBTagEventArgs e)
        {
            TagEvent?.Invoke(this, e);
        }
        #endregion //事件

        /// <summary>
        /// 打开 OPC DA 点位选择对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btGetTag_Click(object sender, EventArgs e)
        {
            string hostName = HostName.Trim();
            string endpoint = Endpoint.Trim();

            if (string.IsNullOrWhiteSpace(hostName))
            {
                MessageBox.Show("请输入 OPC DA 服务器主机名。", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                MessageBox.Show("请输入 OPC DA Server 名称（ProgID）。", Resources.PromptMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var tagSelect = new diaOPCDATagSelect
            {
                HostName = hostName,
                Endpoint = endpoint
            };
            tagSelect.TagEvent += HandleEvent;
            tagSelect.ShowDialog(this);
            tagSelect.TagEvent -= HandleEvent;
        }
    }
}
