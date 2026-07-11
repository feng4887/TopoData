using auDASLib;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using ScintillaNET;
using StdLog4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TopoData.Properties;


namespace TopoData.SubPage
{
    public partial class ucScripts : DevExpress.XtraEditors.XtraUserControl
    {
        private string _currentScriptName = "";
        private string _expressionText = "";
        private List<DBTag> _expressionTags = new List<DBTag>();
        private bool _navBarEventRegistered = false;

        public ucScripts()
        {
            InitializeComponent();
            cbExpress.Properties.EditValueChanged += cbExpress_Properties_EditValueChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeScriptPage();
        }

        /// <summary>
        /// 应用界面文字。
        /// </summary>
        public void ApplyLanguage()
        {


            barButtonAdd.Caption      = Resources.Add;
            barButtonRemove.Caption   = Resources.Delete;
            barButtonItemSave.Caption = Resources.Save;
            barButtonRun.Caption      = "Run";
            DataTableList.Caption     = "Scripts";
            groupControl1.Text        = Resources.BasicInfo;
            groupControl2.Text        = Resources.ScriptTrig;
            label1.Text                 = Resources.ScriptName;
            labelControl1.Text          = Resources.Description;
            label3.Text                 = Resources.BoolTrigTag;
            cbActive.Properties.OffText = Resources.Disable;
            cbActive.Properties.OnText  = Resources.Enable;
            cbExpress.Text              = Resources.Expression; 
            btExpress.Text              = Resources.Expression; 
        }

        private void InitializeScriptPage()
        {
            navBarControl1.LargeImages = imageList1;
            navBarControl1.SmallImages = imageList1;
            navBarControl1.LinkSelectionMode = LinkSelectionModeType.OneInGroup;

            if (!_navBarEventRegistered)
            {
                navBarControl1.LinkClicked += NavBarControl1_LinkClicked;
                _navBarEventRegistered = true;
            }

            ApplyLanguage();
            LoadScriptConfig();
            _currentScriptName = "";
            RefreshScriptItems();
            UpdateExpressionControlState();
            ConfigurePythonEditor();
        }

        /// <summary>
        /// <summary>
        /// 配置 Scintilla Python lexer 与代码编辑器行为。
        /// </summary>
        private void ConfigurePythonEditor()
        {
            memoEditScriptText.LexerName = "python";
            memoEditScriptText.WrapMode = WrapMode.None;
            memoEditScriptText.TabWidth = 4;
            memoEditScriptText.IndentWidth = 4;
            memoEditScriptText.UseTabs = false;
            memoEditScriptText.IndentationGuides = IndentView.LookBoth;
            memoEditScriptText.Margins[0].Type = MarginType.Number;
            memoEditScriptText.Margins[0].Width = 40;

            memoEditScriptText.Styles[Style.Default].Font = "Consolas";
            memoEditScriptText.Styles[Style.Default].Size = 10;
            memoEditScriptText.Styles[Style.Default].ForeColor = Color.Black;
            memoEditScriptText.StyleClearAll();

            memoEditScriptText.Styles[Style.Python.CommentLine].ForeColor = Color.FromArgb(0, 128, 0);
            memoEditScriptText.Styles[Style.Python.CommentBlock].ForeColor = Color.FromArgb(0, 128, 0);
            memoEditScriptText.Styles[Style.Python.String].ForeColor = Color.FromArgb(163, 21, 21);
            memoEditScriptText.Styles[Style.Python.Character].ForeColor = Color.FromArgb(163, 21, 21);
            memoEditScriptText.Styles[Style.Python.Triple].ForeColor = Color.FromArgb(163, 21, 21);
            memoEditScriptText.Styles[Style.Python.TripleDouble].ForeColor = Color.FromArgb(163, 21, 21);
            memoEditScriptText.Styles[Style.Python.Word].ForeColor = Color.FromArgb(0, 0, 255);
            memoEditScriptText.Styles[Style.Python.Word].Bold = true;
            memoEditScriptText.Styles[Style.Python.Word2].ForeColor = Color.FromArgb(121, 94, 38);
            memoEditScriptText.Styles[Style.Python.Number].ForeColor = Color.FromArgb(9, 134, 88);
            memoEditScriptText.Styles[Style.Python.Decorator].ForeColor = Color.FromArgb(128, 0, 128);

            memoEditScriptText.SetKeywords(0, "and as assert async await break class continue def del elif else except False finally for from global if import in is lambda None nonlocal not or pass raise return True try while with yield");
            memoEditScriptText.SetKeywords(1, "abs all any bin bool bytearray bytes callable chr classmethod compile complex delattr dict dir divmod enumerate eval filter float format frozenset getattr globals hasattr hash help hex id input int isinstance issubclass iter len list locals map max memoryview min next object oct open ord pow print property range repr reversed round set setattr slice sorted staticmethod str sum super tuple type vars zip");
        }

        private void LoadScriptConfig()
        {
            try
            {
                if (File.Exists(PythonScriptConfig.ScriptPathDefine))
                {
                    PythonScriptConfig.Instance = XmlHelper.XmlDeserializeFromFile<PythonScriptConfig>(PythonScriptConfig.ScriptPathDefine, Encoding.UTF8) ?? new PythonScriptConfig();
                }
                else
                {
                    PythonScriptConfig.Instance = new PythonScriptConfig();
                }

                if (PythonScriptConfig.Instance.Scripts == null)
                    PythonScriptConfig.Instance.Scripts = new List<PythonScriptItem>();
            }
            catch (Exception ex)
            {
                Log4Helper.Instance.Error("[Python Script] Load script configuration failed.", ex);
                PythonScriptConfig.Instance = new PythonScriptConfig();
                XtraMessageBox.Show($"加载 Python 脚本配置失败: {ex.Message}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveScriptConfig()
        {
            string configFolder = Path.GetDirectoryName(PythonScriptConfig.ScriptPathDefine);
            if (!string.IsNullOrEmpty(configFolder))
                Directory.CreateDirectory(configFolder);

            PythonScriptConfig.Instance.last_modify_time = DateTime.Now;
            XmlHelper.XmlSerializeToFile(PythonScriptConfig.Instance, PythonScriptConfig.ScriptPathDefine, Encoding.UTF8);
        }

        private void RefreshScriptItems()
        {
            DataTableList.ItemLinks.Clear();
            navBarControl1.Items.Clear();

            foreach (var script in PythonScriptConfig.Instance.Scripts)
            {
                NavBarItem item = CreateScriptNavItem(script);
                navBarControl1.Items.Add(item);
                DataTableList.ItemLinks.Add(item);
            }

            if (PythonScriptConfig.Instance.Scripts.Count > 0)
            {
                int selectedIndex = PythonScriptConfig.Instance.Scripts.FindIndex(it =>
                    string.Equals(it.ScriptName, _currentScriptName, StringComparison.OrdinalIgnoreCase));
                if (selectedIndex < 0)
                    selectedIndex = 0;

                navBarControl1.ActiveGroup = DataTableList;
                DataTableList.SelectedLinkIndex = selectedIndex;

                LoadScriptToPanel(PythonScriptConfig.Instance.Scripts[selectedIndex]);
            }
            else
            {
                ClearScriptPanel();
            }
        }

        private NavBarItem CreateScriptNavItem(PythonScriptItem script)
        {
            NavBarItem item = new NavBarItem();
            item.Caption = script.ScriptName;
            item.Name = script.ScriptName;
            item.Tag = script.ScriptName;
            item.LargeImageIndex = 0;
            item.SmallImageIndex = 0;
            item.ImageOptions.LargeImageSize = new System.Drawing.Size(32, 32);
            item.AllowAutoSelect = true;
            return item;
        }

        private void ClearScriptPanel()
        {
            _currentScriptName = "";
            _expressionText = "";
            _expressionTags = new List<DBTag>();
            tbTaskName.Text = "";
            tbDescription.Text = "";
            tbTagName1.Text = "";
            cbActive.IsOn = true;
            cbExpress.Checked = false;
            memoEditScriptText.Text = "";
            UpdateExpressionControlState();
        }

        private void LoadScriptToPanel(PythonScriptItem script)
        {
            if (script == null)
            {
                ClearScriptPanel();
                return;
            }

            _currentScriptName = script.ScriptName;
            tbTaskName.Text = script.ScriptName;
            tbDescription.Text = script.Description;
            cbActive.IsOn = script.bActive;
            tbTagName1.Text = script.Trigger?.TagName ?? "";
            cbExpress.Checked = script.Trigger?.bUseExpression ?? false;
            _expressionText = script.Trigger?.ExpressionText ?? "";
            _expressionTags = script.Trigger?.ExpressionTags ?? new List<DBTag>();
            memoEditScriptText.Text = script.ScriptText;
            UpdateExpressionControlState();
        }

        private int GetNextScriptNumber()
        {
            int maxNum = 0;
            foreach (var script in PythonScriptConfig.Instance.Scripts)
            {
                System.Text.RegularExpressions.Match match = Regex.Match(script.ScriptName ?? "", @"PythonScript(\d+)", RegexOptions.IgnoreCase);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int num))
                    maxNum = Math.Max(maxNum, num);
            }
            return maxNum + 1;
        }

        /// <summary>
        /// Add script item to configuration, refresh navBarControl1.
        /// </summary>
        private void barButtonAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!TryBuildScriptFromPanel(out PythonScriptItem script))
                return;

            bool duplicate = PythonScriptConfig.Instance.Scripts.Any(it =>
                string.Equals(it.ScriptName, script.ScriptName, StringComparison.OrdinalIgnoreCase));

            if (duplicate)
            {
                XtraMessageBox.Show("脚本名不能重复。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PythonScriptConfig.Instance.Scripts.Add(script);
            _currentScriptName = script.ScriptName;

            try
            {
                SaveScriptConfig();
                RefreshScriptItems();
                XtraMessageBox.Show("脚本添加成功。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log4Helper.Instance.Error("[Python Script] Add script failed.", ex);
                XtraMessageBox.Show($"添加脚本失败: {ex.Message}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Remove script from configuration, refresh navBarControl1.
        /// </summary>
        private void barButtonRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentScriptName))
                return;

            if (DialogResult.Yes != XtraMessageBox.Show($"是否删除 {_currentScriptName}", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;

            PythonScriptConfig.Instance.Scripts.RemoveAll(it => string.Equals(it.ScriptName, _currentScriptName, StringComparison.OrdinalIgnoreCase));
            _currentScriptName = "";

            try
            {
                SaveScriptConfig();
                RefreshScriptItems();
            }
            catch (Exception ex)
            {
                Log4Helper.Instance.Error("[Python Script] Remove script failed.", ex);
                XtraMessageBox.Show($"删除脚本失败: {ex.Message}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Update script item to configuration, refresh navBarControl1.
        /// </summary>
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!TryBuildScriptFromPanel(out PythonScriptItem script))
                return;

            PythonScriptItem oldScript = PythonScriptConfig.Instance.Scripts
                .FirstOrDefault(it => string.Equals(it.ScriptName, _currentScriptName, StringComparison.OrdinalIgnoreCase));

            if (oldScript == null)
            {
                PythonScriptConfig.Instance.Scripts.Add(script);
            }
            else
            {
                int index = PythonScriptConfig.Instance.Scripts.IndexOf(oldScript);
                PythonScriptConfig.Instance.Scripts[index] = script;
            }

            _currentScriptName = script.ScriptName;

            try
            {
                SaveScriptConfig();
                RefreshScriptItems();
                XtraMessageBox.Show("脚本保存成功。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log4Helper.Instance.Error("[Python Script] Save script failed.", ex);
                XtraMessageBox.Show($"保存脚本失败: {ex.Message}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barButtonRun_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!TryBuildScriptFromPanel(out PythonScriptItem script, false))
                return;

            Cursor = Cursors.WaitCursor;
            try
            {
                PythonScriptExecuteResult result = PythonScriptExecutor.Execute(script);
                if (result.IsSuccess)
                {
                    string output = string.IsNullOrWhiteSpace(result.Output) ? "脚本执行成功。" : result.Output;
                    XtraMessageBox.Show(output, "Python", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show($"脚本执行失败: {result.ErrorMessage}", "Python", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Select a bool tag for script item.
        /// </summary>
        private void btTagSelect1_Click(object sender, EventArgs e)
        {
            if (cbExpress.Checked)
            {
                XtraMessageBox.Show("使用表达式时不需要选择单个 bool 触发点。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            diaTagSelect diaTagSelect = new diaTagSelect();
            diaTagSelect.ShowDialog();

            if (string.IsNullOrEmpty(diaTagSelect.TagName))
                return;

            if (!string.Equals(diaTagSelect.DataType, "bool", StringComparison.OrdinalIgnoreCase))
            {
                XtraMessageBox.Show("脚本触发点必须是 bool 类型。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tbTagName1.Text = diaTagSelect.TagName;
        }

        /// <summary>
        /// Open script trigger in expression editor.
        /// </summary>
        private void btExpress_Click(object sender, EventArgs e)
        {
            if (!cbExpress.Checked)
            {
                XtraMessageBox.Show("请先勾选使用表达式。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            diaExpress diaExpress = new diaExpress();
            diaExpress.ExpressionText = _expressionText;
            diaExpress.ExpressionTags = _expressionTags;
            diaExpress.ShowDialog();

            if (diaExpress.DialogResult == DialogResult.OK)
            {
                _expressionText = diaExpress.ExpressionText;
                _expressionTags = diaExpress.ExpressionTags;
            }
        }

        private void NavBarControl1_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            string itemName = Convert.ToString(e.Link.Item.Tag) ?? e.Link.Item.Name;
            PythonScriptItem script = PythonScriptConfig.Instance.Scripts
                .FirstOrDefault(it => string.Equals(it.ScriptName, itemName, StringComparison.OrdinalIgnoreCase));

            LoadScriptToPanel(script);
        }

        private void cbExpress_Properties_EditValueChanged(object sender, EventArgs e)
        {
            UpdateExpressionControlState();
        }

        private void UpdateExpressionControlState()
        {
            bool useExpression = cbExpress.Checked;
            tbTagName1.Enabled = !useExpression;
            btTagSelect1.Enabled = !useExpression;
            btExpress.Enabled = useExpression;
        }

        private bool TryBuildScriptFromPanel(out PythonScriptItem script, bool validateTrigger = true)
        {
            script = new PythonScriptItem();
            string scriptName = tbTaskName.Text.Trim();
            // Python 缩进属于脚本语义，正文必须逐字符保存，不能调用 Trim。
            string scriptText = memoEditScriptText.Text;

            if (string.IsNullOrWhiteSpace(scriptName))
            {
                XtraMessageBox.Show("脚本名不能为空。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (PubFunction.StartsWithSpecialCharOrDigit(scriptName))
            {
                XtraMessageBox.Show("脚本名不允许特殊字符或数字开头。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool duplicate = PythonScriptConfig.Instance.Scripts.Any(it =>
                !string.Equals(it.ScriptName, _currentScriptName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(it.ScriptName, scriptName, StringComparison.OrdinalIgnoreCase));

            if (duplicate)
            {
                XtraMessageBox.Show("脚本名不能重复。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(scriptText))
            {
                XtraMessageBox.Show("Python 脚本内容不能为空。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (validateTrigger)
            {
                if (!cbExpress.Checked && !ValidateBoolTag(tbTagName1.Text.Trim()))
                    return false;

                if (cbExpress.Checked && string.IsNullOrWhiteSpace(_expressionText))
                {
                    XtraMessageBox.Show("表达式不能为空。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            script = new PythonScriptItem()
            {
                ScriptName = scriptName,
                Description = tbDescription.Text,
                bActive = cbActive.IsOn,
                ScriptText = scriptText,
                last_modify_time = DateTime.Now,
                Trigger = new PythonScriptTrigger()
                {
                    TagName = tbTagName1.Text.Trim(),
                    bUseExpression = cbExpress.Checked,
                    ExpressionText = _expressionText,
                    ExpressionTags = new List<DBTag>(_expressionTags ?? new List<DBTag>())
                }
            };

            return true;
        }

        private bool ValidateBoolTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                XtraMessageBox.Show("bool 触发点不能为空。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            DBTag tag = FindTag(tagName);
            if (tag == null)
            {
                XtraMessageBox.Show("选择的通讯点不存在, 请检查通讯配置。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (tag.DataType != 0)
            {
                XtraMessageBox.Show("脚本触发点必须是 bool 类型。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private DBTag FindTag(string tagName)
        {
            foreach (DBTag tag in GetAllTags())
            {
                string fullName = $"{tag.CannelID}.{tag.TagName}";
                if (string.Equals(tag.TagId, tagName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fullName, tagName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(tag.TagName, tagName, StringComparison.OrdinalIgnoreCase))
                    return tag;
            }

            return null;
        }

        private List<DBTag> GetAllTags()
        {
            List<DBTag> tags = new List<DBTag>();

            if (DataImport.dicCannelTags != null && DataImport.dicCannelTags.Count > 0)
            {
                foreach (var item in DataImport.dicCannelTags.Values)
                    tags.AddRange(item);
            }

            if (File.Exists(pubDefine.TagDefine))
            {
                try
                {
                    List<DBTag> fileTags = XmlHelper.XmlDeserializeFromFile<List<DBTag>>(pubDefine.TagDefine, Encoding.UTF8);
                    if (fileTags != null)
                        tags.AddRange(fileTags);
                }
                catch (Exception ex)
                {
                    Log4Helper.Instance.Error("[Python Script] Read tag definition failed.", ex);
                }
            }

            return tags;
        }

        /// <summary>
        /// 弹出diaTagSelect对话框，选择一个读标签并插入到光标所在位置脚本配置中。
        /// 格式是：Read[CannelID.TagName]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddReadTag_Click(object sender, EventArgs e)
        {
            if (!TrySelectTag(out string cannelId, out string tagName))
                return;

            InsertTextAtCursor(memoEditScriptText, $"Read[{GetScriptTagName(cannelId, tagName)}]");
        }

        /// <summary>
        /// 弹出diaTagSelect对话框，选择一个写标签并插入到光标所在位置脚本配置中。
        /// 格式是：Write[CannelID.TagName]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddWriteTag_Click(object sender, EventArgs e)
        {
            if (!TrySelectTag(out string cannelId, out string tagName))
                return;

            InsertTextAtCursor(memoEditScriptText, $"Write[{GetScriptTagName(cannelId, tagName)}]");
        }

        /// <summary>
        /// 弹出标签选择对话框，返回选中的通道与点名称。
        /// </summary>
        /// <param name="cannelId">选中的通道ID。</param>
        /// <param name="tagName">选中的点名称。</param>
        /// <returns>选中有效点返回 true，否则返回 false。</returns>
        private bool TrySelectTag(out string cannelId, out string tagName)
        {
            cannelId = "";
            tagName = "";

            diaTagSelect diaTagSelect = new diaTagSelect();
            diaTagSelect.ShowDialog();

            if (string.IsNullOrEmpty(diaTagSelect.TagName))
                return false;

            if (string.IsNullOrEmpty(diaTagSelect.Cannel))
            {
                XtraMessageBox.Show("所选标签缺少通道信息，请检查通讯点配置。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            cannelId = diaTagSelect.Cannel;
            tagName = diaTagSelect.TagName;
            return true;
        }

        private static string GetScriptTagName(string cannelId, string tagName)
        {
            string prefix = cannelId + ".";
            return tagName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                ? tagName
                : prefix + tagName;
        }

        /// <summary>
        /// 在 Scintilla 当前选区或光标位置插入指定文本。
        /// </summary>
        /// <param name="editor">目标编辑控件。</param>
        /// <param name="text">要插入的文本。</param>
        private void InsertTextAtCursor(Scintilla editor, string text)
        {
            if (editor == null || string.IsNullOrEmpty(text))
                return;

            editor.ReplaceSelection(text);
            editor.Focus();
        }
    }
}
