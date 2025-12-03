namespace BackupTool
{
    partial class CloseOptionDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.RadioButton radioMinimizeTray;
        private System.Windows.Forms.RadioButton radioClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labelInfo;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.radioMinimizeTray = new System.Windows.Forms.RadioButton();
            this.radioClose = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(12, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(262, 15);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "Program kapatılırken ne yapmak istediğinizi seçin:";
            // 
            // radioMinimizeTray
            // 
            this.radioMinimizeTray.AutoSize = true;
            this.radioMinimizeTray.Location = new System.Drawing.Point(12, 40);
            this.radioMinimizeTray.Name = "radioMinimizeTray";
            this.radioMinimizeTray.Size = new System.Drawing.Size(163, 19);
            this.radioMinimizeTray.TabIndex = 1;
            this.radioMinimizeTray.TabStop = true;
            this.radioMinimizeTray.Text = "Sistem tepsisinde çalışmaya devam et";
            this.radioMinimizeTray.UseVisualStyleBackColor = true;
            // 
            // radioClose
            // 
            this.radioClose.AutoSize = true;
            this.radioClose.Location = new System.Drawing.Point(12, 65);
            this.radioClose.Name = "radioClose";
            this.radioClose.Size = new System.Drawing.Size(61, 19);
            this.radioClose.TabIndex = 2;
            this.radioClose.TabStop = true;
            this.radioClose.Text = "Kapat";
            this.radioClose.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(116, 110);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 27);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "Tamam";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(197, 110);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            // 
            // CloseOptionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 149);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.radioMinimizeTray);
            this.Controls.Add(this.radioClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CloseOptionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kapatma Seçenekleri";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
