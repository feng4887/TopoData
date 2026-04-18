using System.Drawing;
using System.Windows.Forms;

namespace TopoData.SubPage
{
    partial class diaRecipeTagSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new DevExpress.XtraEditors.LabelControl();
            tbAtribute = new DevExpress.XtraEditors.TextEdit();
            label2 = new DevExpress.XtraEditors.LabelControl();
            tbTagName = new DevExpress.XtraEditors.TextEdit();
            label3 = new DevExpress.XtraEditors.LabelControl();
            tbUOM = new DevExpress.XtraEditors.TextEdit();
            label4 = new DevExpress.XtraEditors.LabelControl();
            btOK = new DevExpress.XtraEditors.SimpleButton();
            btCancel = new DevExpress.XtraEditors.SimpleButton();
            btSelect = new DevExpress.XtraEditors.SimpleButton();
            label5 = new DevExpress.XtraEditors.LabelControl();
            tbDefaultValue = new DevExpress.XtraEditors.TextEdit();
            cbDataType = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)tbAtribute.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbTagName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbUOM.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbDefaultValue.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbDataType.Properties).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(10, 23);
            label1.Name = "label1";
            label1.Size = new Size(48, 12);
            label1.TabIndex = 0;
            label1.Text = "配方字段";
            // 
            // tbAtribute
            // 
            tbAtribute.Location = new Point(82, 21);
            tbAtribute.Margin = new Padding(3, 2, 3, 2);
            tbAtribute.Name = "tbAtribute";
            tbAtribute.Size = new Size(201, 18);
            tbAtribute.TabIndex = 1;
            // 
            // label2
            // 
            label2.Location = new Point(7, 90);
            label2.Name = "label2";
            label2.Size = new Size(48, 12);
            label2.TabIndex = 2;
            label2.Text = "数据类型";
            // 
            // tbTagName
            // 
            tbTagName.Location = new Point(82, 64);
            tbTagName.Margin = new Padding(3, 2, 3, 2);
            tbTagName.Name = "tbTagName";
            tbTagName.Properties.Appearance.BackColor = SystemColors.Window;
            tbTagName.Properties.Appearance.Options.UseBackColor = true;
            tbTagName.Properties.ReadOnly = true;
            tbTagName.Size = new Size(155, 18);
            tbTagName.TabIndex = 3;
            // 
            // label3
            // 
            label3.Location = new Point(7, 111);
            label3.Name = "label3";
            label3.Size = new Size(24, 12);
            label3.TabIndex = 4;
            label3.Text = "单位";
            // 
            // tbUOM
            // 
            tbUOM.Location = new Point(82, 108);
            tbUOM.Margin = new Padding(3, 2, 3, 2);
            tbUOM.Name = "tbUOM";
            tbUOM.Size = new Size(198, 18);
            tbUOM.TabIndex = 5;
            // 
            // label4
            // 
            label4.Location = new Point(7, 66);
            label4.Name = "label4";
            label4.Size = new Size(36, 12);
            label4.TabIndex = 6;
            label4.Text = "通讯点";
            // 
            // btOK
            // 
            btOK.Location = new Point(132, 141);
            btOK.Margin = new Padding(3, 2, 3, 2);
            btOK.Name = "btOK";
            btOK.Size = new Size(65, 21);
            btOK.TabIndex = 8;
            btOK.Text = "确认";
            btOK.Click += btOK_Click;
            // 
            // btCancel
            // 
            btCancel.Location = new Point(215, 141);
            btCancel.Margin = new Padding(3, 2, 3, 2);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(65, 21);
            btCancel.TabIndex = 9;
            btCancel.Text = "取消";
            btCancel.Click += btCancel_Click;
            // 
            // btSelect
            // 
            btSelect.Location = new Point(242, 63);
            btSelect.Margin = new Padding(3, 2, 3, 2);
            btSelect.Name = "btSelect";
            btSelect.Size = new Size(38, 17);
            btSelect.TabIndex = 10;
            btSelect.Text = "...";
            btSelect.Click += btSelect_Click;
            // 
            // label5
            // 
            label5.Location = new Point(7, 45);
            label5.Name = "label5";
            label5.Size = new Size(36, 12);
            label5.TabIndex = 14;
            label5.Text = "默认值";
            // 
            // tbDefaultValue
            // 
            tbDefaultValue.Location = new Point(82, 43);
            tbDefaultValue.Margin = new Padding(3, 2, 3, 2);
            tbDefaultValue.Name = "tbDefaultValue";
            tbDefaultValue.Size = new Size(198, 18);
            tbDefaultValue.TabIndex = 13;
            // 
            // cbDataType
            // 
            cbDataType.Location = new Point(82, 87);
            cbDataType.Name = "cbDataType";
            cbDataType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbDataType.Properties.Items.AddRange(new object[] { "bool", "int", "float", "string", "wstring", "datetime" });
            cbDataType.Size = new Size(198, 18);
            cbDataType.TabIndex = 17;
            // 
            // diaRecipeTag
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(315, 176);
            Controls.Add(cbDataType);
            Controls.Add(label5);
            Controls.Add(tbDefaultValue);
            Controls.Add(btSelect);
            Controls.Add(btCancel);
            Controls.Add(btOK);
            Controls.Add(label4);
            Controls.Add(tbUOM);
            Controls.Add(label3);
            Controls.Add(tbTagName);
            Controls.Add(label2);
            Controls.Add(tbAtribute);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "diaRecipeTag";
            StartPosition = FormStartPosition.CenterParent;
            Text = "配置";
            Load += diaColumnSelect_Load;
            ((System.ComponentModel.ISupportInitialize)tbAtribute.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbTagName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbUOM.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbDefaultValue.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbDataType.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.TextEdit tbAtribute;
        private DevExpress.XtraEditors.LabelControl label2;
        private DevExpress.XtraEditors.TextEdit tbTagName;
        private DevExpress.XtraEditors.LabelControl label3;
        private DevExpress.XtraEditors.TextEdit tbUOM;
        private DevExpress.XtraEditors.LabelControl label4;
        private DevExpress.XtraEditors.SimpleButton btOK;
        private DevExpress.XtraEditors.SimpleButton btCancel;
        private DevExpress.XtraEditors.SimpleButton btSelect;
        private DevExpress.XtraEditors.LabelControl label5;
        private DevExpress.XtraEditors.TextEdit tbDefaultValue;
        private DevExpress.XtraEditors.ComboBoxEdit cbDataType;
    }
}