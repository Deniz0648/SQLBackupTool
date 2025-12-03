using System;
using System.Linq;
using System.Windows.Forms;
using BackupTool;

namespace BackupTool
{
    public partial class SettingsForm : Form
    {

        public event EventHandler? SettingsSaved;
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {

            this.Icon = Resource.settings;

            txtConnectionString.Text = Settings.Default.ConnectionString;
            txtBackupFolder.Text = Settings.Default.BackupFolder;
            txtExcludedDatabases.Text = Settings.Default.ExcludedDatabases ?? "";

            numRetentionDays.Minimum = 1;
            numRetentionDays.Maximum = 365;
            numRetentionDays.Value = Math.Min(Math.Max(Settings.Default.RetentionDays, numRetentionDays.Minimum), numRetentionDays.Maximum);

            numMinFreeSpace.Minimum = 0;
            numMinFreeSpace.Maximum = 204800;
            numMinFreeSpace.Value = Math.Min(Math.Max(Settings.Default.MinFreeSpaceMB, numMinFreeSpace.Minimum), numMinFreeSpace.Maximum);

            numBackupFrequency.Minimum = 1;
            numBackupFrequency.Maximum = 365;
            numBackupFrequency.Value = Math.Min(Math.Max(Settings.Default.BackupFrequencyDays, numBackupFrequency.Minimum), numBackupFrequency.Maximum);
                        
            txtSmtpServer.Text = Settings.Default.SmtpServer;

            numSmtpPort.Minimum = 1;
            numSmtpPort.Maximum = 65535;
            numSmtpPort.Value = Math.Min(Math.Max(Settings.Default.SmtpPort, numSmtpPort.Minimum), numSmtpPort.Maximum);

            txtSmtpUser.Text = Settings.Default.SmtpUser;
            txtSmtpPassword.Text = Settings.Default.SmtpPassword;
            txtEmailTo.Text = Settings.Default.EmailTo;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Settings.Default.ConnectionString = txtConnectionString.Text;
            Settings.Default.BackupFolder = txtBackupFolder.Text;
            Settings.Default.ExcludedDatabases = txtExcludedDatabases.Text;
            Settings.Default.RetentionDays = (int)numRetentionDays.Value;
            Settings.Default.MinFreeSpaceMB = (long)numMinFreeSpace.Value;
            Settings.Default.SmtpServer = txtSmtpServer.Text;
            Settings.Default.SmtpPort = (int)numSmtpPort.Value;
            Settings.Default.SmtpUser = txtSmtpUser.Text;
            Settings.Default.SmtpPassword = txtSmtpPassword.Text;
            Settings.Default.EmailTo = txtEmailTo.Text;
            Settings.Default.BackupFrequencyDays = (int)numBackupFrequency.Value;

            Settings.Default.Save();
            MessageBox.Show("Ayarlar kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SettingsSaved?.Invoke(this, EventArgs.Empty);

            this.Close(); // Formu kapat
        }

    }
}
