//using auDAManager.Properties;
using auDASLib;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using TopoData.model;

namespace TopoData.SubPage
{
    public partial class diaExpress : DevExpress.XtraEditors.XtraForm
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

        public diaExpress()
        {
            InitializeComponent();
        }

        private void diaExpress_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = _eText;
            listView1.Items.Clear();
            foreach (var tag in _eTags)
            {
                System.Windows.Forms.ListViewItem lvi = new System.Windows.Forms.ListViewItem();
                lvi.Text = tag.TagName;
                lvi.SubItems.Add(tag.Address);
                lvi.SubItems.Add(DBTag.DataTypeString(tag.DataType));
                this.listView1.Items.Add(lvi);
            }

            ApplyLanguage();
        }

        /// <summary>
        /// 应用多语言
        /// </summary>
        public void ApplyLanguage()
        {
            //this.Text = Resources.diaExpress;
            //listView1.Columns[0].Text = Resources.TagInternal;
            //listView1.Columns[1].Text = Resources.Address;
            //listView1.Columns[2].Text = Resources.DataType;
            //groupBox1.Text = Resources.TagInternalList;
            //groupBox2.Text = Resources.Expression;
            //btSave.Text = Resources.Save;
            //btClose.Text = Resources.Close;

        }

        private static readonly string[] keywords = { "error", "warning", "info" };
        private static readonly string pattern = string.Join("|", keywords.Select(keyword => $"(?i){Regex.Escape(keyword)}"));
        private List<Tuple<int, int, System.Drawing.Color>> coloredRanges = new List<Tuple<int, int, System.Drawing.Color>>();


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //// Calculate the starting position of the current line.  
            //int start = 0, end = 0;
            //for (start = richTextBox1.SelectionStart - 1; start > 0; start--)
            //{
            //    if (richTextBox1.Text[start] == '\n') { start++; break; }
            //}
            //// Calculate the end position of the current line.  
            //for (end = richTextBox1.SelectionStart; end < richTextBox1.Text.Length; end++)
            //{
            //    if (richTextBox1.Text[end] == '\n') break;
            //}
            //// Extract the current line that is being edited.  
            //String line = richTextBox1.Text.Substring(start, end - start);
            //// Backup the users current selection point.  
            //int selectionStart = richTextBox1.SelectionStart;
            //int selectionLength = richTextBox1.SelectionLength;
            //// Split the line into tokens.  
            //Regex r = new Regex("([ \\t{}();])");
            //string[] tokens = r.Split(line);
            //int index = start;
            //foreach (string token in tokens)
            //{
            //    // Set the token's default color and font.  
            //    richTextBox1.SelectionStart = index;
            //    richTextBox1.SelectionLength = token.Length;
            //    richTextBox1.SelectionColor = System.Drawing.Color.Black;
            //    richTextBox1.SelectionFont = new Font("Courier New", 10,
            //    FontStyle.Regular);
            //    // Check whether the token is a keyword.   
            //    String[] keywords = { "public", "void", "using", "static", "class" };
            //    for (int i = 0; i < keywords.Length; i++)
            //    {
            //        if (keywords[i] == token)
            //        {
            //            // Apply alternative color and font to highlight keyword.   
            //            richTextBox1.SelectionColor = System.Drawing.Color.Blue;
            //            richTextBox1.SelectionFont = new Font("Courier New", 10,
            //            FontStyle.Bold);
            //            break;
            //        }

            //        index += token.Length;
            //    }
            //    // Restore the users current selection point.   
            //    richTextBox1.SelectionStart = selectionStart;
            //    richTextBox1.SelectionLength = selectionLength;
            //}

        }


        private void RecolorText(string text, int start, int end)
        {
            MatchCollection matches = Regex.Matches(text.Substring(start), pattern);

            int lastIndex = start;

            foreach (Match match in matches)
            {
                int matchStart = match.Index + start; // Adjust for the substring offset
                int matchLength = match.Length;

                // Select and color the match
                richTextBox1.Select(matchStart, matchLength);
                System.Drawing.Color color;
                switch (match.Value.ToLower())
                {
                    case "error":
                        color = System.Drawing.Color.Red;
                        break;
                    case "warning":
                        color = System.Drawing.Color.Orange;
                        break;
                    case "info":
                        color = System.Drawing.Color.Blue;
                        break;
                    default:
                        color = System.Drawing.Color.Black; // Should never reach here due to the fixed keywords list
                        break;
                }

                richTextBox1.SelectionColor = color;

                // Update the list of colored ranges
                coloredRanges.Add(Tuple.Create(matchStart, matchLength, color));

                // Update lastIndex for the next iteration (if needed, though it's not strictly necessary here)
                lastIndex = matchStart + matchLength;
            }

            // Color any remaining text after the last match (if any) to black
            if (lastIndex < end)
            {
                richTextBox1.Select(lastIndex, end - lastIndex);
                richTextBox1.SelectionColor = System.Drawing.Color.Black;

                // Note: We don't add this range to coloredRanges because it's default (black) and not tracked.
            }
        }

        private void UpdateColoredRanges(string text)
        {
            // This method can be used to adjust the coloredRanges list based on the current text,
            // but in this simplified example, we're assuming that recoloring from the end is sufficient.
            // If more complex text manipulations are needed, this method should be expanded to handle those cases.

            // For now, we'll just clear the list and rely on RecolorText to rebuild it.
            // In a real-world scenario, you might want to optimize this to avoid unnecessary re-coloring.
            coloredRanges.Clear();

            // Optionally, you could re-apply colors based on the current text and saved ranges,
            // but that would require more complex logic to handle overlapping ranges, deletions, etc.
        }

        // Optional: Handle contents resized event (not strictly necessary for this example)
        //private void RichTextBox1_ContentsResized(object sender, ContentResizedEventArgs e)
        //{
        //    // This can be used to handle cases where the RichTextBox control resizes due to text changes,
        //    // but it's not directly related to the coloring logic and is thus optional.
        //}

        void aaa(string x)
        {
            ArrayList list = getIndexArray(richTextBox1.Text, x);
            for (int i = 0; i < list.Count; i++)
            {
                int index = (int)list[i];
                richTextBox1.Select(index, x.Length);
                richTextBox1.SelectionColor = System.Drawing.Color.Green;
            }
            richTextBox1.ForeColor = System.Drawing.Color.Black;
        }

        private ArrayList getIndexArray(String inputStr, String findStr)

        {
            ArrayList list = new ArrayList();
            int start = 0;
            while (start < inputStr.Length)
            {
                int index = inputStr.IndexOf(findStr, start);
                if (index >= 0)

                {
                    list.Add(index);
                    start = index + findStr.Length;
                }
                else
                {
                    break;
                }
            }
            return list;
        }


        /// <summary>
        /// 把指定文字修改为指定颜色
        /// </summary>
        /// <param name="specialWord"></param>
        /// <param name="color"></param>
        private void ChangeColorForSpecialWord(string specialWord, System.Drawing.Color color)
        {
            if (string.IsNullOrEmpty(specialWord))
                return;

            int pos = 0;
            do
            {
                if (pos != 0 && pos + specialWord.Length < richTextBox1.Text.Length)
                    pos += specialWord.Length;//跳过要查找词继续找

                //查找指定词的位置
                pos = richTextBox1.Find(specialWord, pos, RichTextBoxFinds.None);
                if (pos > 0)
                {
                    richTextBox1.Select(pos, specialWord.Length);
                    richTextBox1.SelectionColor = color;
                }
                if (pos + specialWord.Length > richTextBox1.Text.Length - 1)
                    break;
            } while (pos >= 0 && pos < richTextBox1.Text.Length);
        }

        /// <summary>
        /// 把包含指定文字的行修改为指定颜色
        /// </summary>
        /// <param name="specialText"></param>
        /// <param name="color"></param>
        private void ChangeColorForSpecialText(string specialText, System.Drawing.Color color)
        {
            if (string.IsNullOrEmpty(specialText))
                return;

            int pos = 0;
            int lineNum;
            List<int> lst = new List<int>();
            do
            {
                if (pos != 0 && pos + specialText.Length < richTextBox1.Text.Length)
                    pos += specialText.Length;//跳过要查找文字继续找

                //查找指定文本的位置
                pos = richTextBox1.Find(specialText, pos, RichTextBoxFinds.None);

                if (pos > 0)
                {
                    //根据文本位置返回它所在的行号
                    lineNum = richTextBox1.GetLineFromCharIndex(pos);
                    lst.Add(lineNum);
                }
                if (pos + specialText.Length > richTextBox1.Text.Length - 1)
                    break;
            } while (pos >= 0 && pos < richTextBox1.Text.Length);

            for (int i = 0; i < lst.Count; i++)
            {
                SelectLine(lst[i]);
                richTextBox1.SelectionColor = color;
            }
        }

        //选中一行，lineNum 为行号
        private void SelectLine(int lineNum)
        {
            if (lineNum < 0)
                return;

            //根据行号返回该行第一个字符的索引
            int start = this.richTextBox1.GetFirstCharIndexFromLine(lineNum);
            int end = this.richTextBox1.GetFirstCharIndexFromLine(++lineNum);
            if (start == -1)
                return;
            else if (end == -1)//如果 end 超出文本长度，则用文本长度 - start
                end = this.richTextBox1.TextLength - start;
            else
                end = end - start;
            this.richTextBox1.Select(start, end);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            diaColumnSelect diaColumnSelect = new diaColumnSelect();
            diaColumnSelect.ShowDialog();
            string at = diaColumnSelect.Atribute;
            string tn = diaColumnSelect.TagID;
            string uom = diaColumnSelect.UOM;
            string dt = diaColumnSelect.DataType;
            //string x = diaColumnSelect.AccessibleDefaultActionDescription

            if (string.IsNullOrEmpty(tn))
                return;

            bool isNumeric = Regex.IsMatch(at, @"^\d");
            if (isNumeric)
            {
                MessageBox.Show("字段不能以数字开头");
                return;
            }

            // Define a regex pattern to match only letters, digits, and underscores.
            string pattern = @"^[a-zA-Z0-9_]+$";
            bool reb = Regex.IsMatch(at, pattern);
            if (!reb)
            {
                MessageBox.Show("变量名只允许由字母数值和_组成");
                return;
            }

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string ParaName = listView1.Items[i].Text;
                string TagAddr = listView1.Items[i].SubItems[0].Text;

                if (at == ParaName)
                {
                    MessageBox.Show("此属性不能重复添加");
                    return;
                }
            }
            System.Windows.Forms.ListViewItem lvi = new System.Windows.Forms.ListViewItem();
            lvi.Text = at;
            lvi.SubItems.Add(tn);
            lvi.SubItems.Add(dt);
            this.listView1.Items.Add(lvi);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            _eText = richTextBox1.Text;
            _eTags.Clear();
            foreach (System.Windows.Forms.ListViewItem a in listView1.Items)
            {
                _eTags.Add(new DBTag() { TagName = a.Text, Address = a.SubItems[1].Text, DataType = DBTag.GetDataType(a.SubItems[2].Text) });
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btRemoveTag_Click(object sender, EventArgs e)
        {
            // 删除选中的行
            foreach (System.Windows.Forms.ListViewItem item in listView1.SelectedItems)
            {
                listView1.Items.Remove(item);
            }
        }

        private void btSelectTag_Click(object sender, EventArgs e)
        {
            string tn = "";
            if (listView1.SelectedItems.Count > 0)
            {
                tn = listView1.SelectedItems[0].Text;
                int cursorPosition = richTextBox1.SelectionStart;
                string currentText = richTextBox1.Text;
                richTextBox1.Text = currentText.Substring(0, cursorPosition) + " " + tn + " " + currentText.Substring(cursorPosition);
                // 移动光标到插入文本的末尾
                richTextBox1.SelectionStart = cursorPosition + tn.Length;
                richTextBox1.ScrollToCaret(); // 确保视图滚动到光标位置
            }
        }

        private void btAblut_Click(object sender, EventArgs e)
        {
            diaExpressAbout diaExpressAbout = new diaExpressAbout();
            diaExpressAbout.ShowDialog();
        }

        private void btCheck_Click(object sender, EventArgs e)
        {
            _eText = richTextBox1.Text;
            _eTags.Clear();
            foreach (System.Windows.Forms.ListViewItem a in listView1.Items)
            {
                _eTags.Add(new DBTag() { TagName = a.Text, Address = a.SubItems[1].Text, DataType = DBTag.GetDataType(a.SubItems[2].Text) });
            }
            diaExpressCheck diaExpressCheck = new diaExpressCheck();
            diaExpressCheck.ExpressionTags = _eTags;
            diaExpressCheck.ExpressionText = _eText;
            diaExpressCheck.ShowDialog();
        }
    }
}
