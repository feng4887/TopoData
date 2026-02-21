namespace TopoData.Page
{
    partial class ucAbout
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucAbout));
            pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)pictureEdit1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)memoEdit1.Properties).BeginInit();
            SuspendLayout();
            // 
            // pictureEdit1
            // 
            pictureEdit1.EditValue = resources.GetObject("pictureEdit1.EditValue");
            pictureEdit1.Location = new System.Drawing.Point(17, 20);
            pictureEdit1.Name = "pictureEdit1";
            pictureEdit1.Properties.AllowZoom = DevExpress.Utils.DefaultBoolean.True;
            pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            pictureEdit1.Size = new System.Drawing.Size(529, 206);
            pictureEdit1.TabIndex = 14;
            // 
            // memoEdit1
            // 
            memoEdit1.Location = new System.Drawing.Point(17, 242);
            memoEdit1.Name = "memoEdit1";
            memoEdit1.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            memoEdit1.Size = new System.Drawing.Size(529, 408);
            memoEdit1.TabIndex = 16;
            // 
            // ucAbout
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(memoEdit1);
            Controls.Add(pictureEdit1);
            Name = "ucAbout";
            Size = new System.Drawing.Size(563, 726);
            Load += ucAbout_Load;
            ((System.ComponentModel.ISupportInitialize)pictureEdit1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)memoEdit1.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
    }
}
