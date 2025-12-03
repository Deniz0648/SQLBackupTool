namespace BackupTool
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtConnectionString;
        private TextBox txtBackupFolder;
        private TextBox txtExcludedDatabases;
        private NumericUpDown numRetentionDays;
        private NumericUpDown numMinFreeSpace;
        private TextBox txtSmtpServer;
        private NumericUpDown numSmtpPort;
        private TextBox txtSmtpUser;
        private TextBox txtSmtpPassword;
        private TextBox txtEmailTo;
        private Button btnSave;
        private NumericUpDown numBackupFrequency;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtConnectionString = new TextBox();
            txtBackupFolder = new TextBox();
            txtExcludedDatabases = new TextBox();
            numBackupFrequency = new NumericUpDown();
            numRetentionDays = new NumericUpDown();
            numMinFreeSpace = new NumericUpDown();
            txtSmtpServer = new TextBox();
            numSmtpPort = new NumericUpDown();
            txtSmtpUser = new TextBox();
            txtSmtpPassword = new TextBox();
            txtSmtpPassword.PasswordChar = '*';
            txtEmailTo = new TextBox();
            btnSave = new Button();

            ((System.ComponentModel.ISupportInitialize)numBackupFrequency).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRetentionDays).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinFreeSpace).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSmtpPort).BeginInit();

            SuspendLayout();

            int y = 10;
            int spacing = 30;

            AddControl(txtConnectionString, "Bağlantı Dizesi", ref y, spacing);
            AddControl(txtBackupFolder, "Yedek Klasörü", ref y, spacing);
            AddControl(txtExcludedDatabases, "Hariç Veritabanları", ref y, spacing);
            AddControl(numRetentionDays, "Saklama Süresi (gün)", ref y, spacing);
            AddControl(numMinFreeSpace, "Minimum Boş Alan (MB)", ref y, spacing);
            AddControl(numBackupFrequency, "Yedekleme Sıklığı (gün)", ref y, spacing);
            AddControl(txtSmtpServer, "SMTP Sunucu", ref y, spacing);
            AddControl(numSmtpPort, "SMTP Port", ref y, spacing);
            AddControl(txtSmtpUser, "SMTP Kullanıcı", ref y, spacing);
            AddControl(txtSmtpPassword, "SMTP Şifre", ref y, spacing);
            AddControl(txtEmailTo, "E-posta Alıcı", ref y, spacing);

            btnSave.Text = "Kaydet";
            btnSave.Width = 100;
            btnSave.Left = 370;
            btnSave.Top = y + 10;
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);

            this.Text = "Ayarlar";
            this.ClientSize = new Size(500, btnSave.Bottom + 20);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            ((System.ComponentModel.ISupportInitialize)numBackupFrequency).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRetentionDays).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinFreeSpace).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSmtpPort).EndInit();

            ResumeLayout(false);
        }




        private void AddControl(Control control, string label, ref int y, int spacing)
        {
            Label lbl = new Label { Text = label, Left = 10, Top = y + 3, Width = 150 };
            control.Left = 170;
            control.Top = y;
            control.Width = 300;
            this.Controls.Add(lbl);
            this.Controls.Add(control);
            y += spacing;
        }

    }

}