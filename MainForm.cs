using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BackupTool
{
    public partial class MainForm : Form
    {
        private BackupWorker? _backupWorker;

        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _trayMenu;
        private ToolStripMenuItem statusItem;

        private Icon _greenIcon;
        private Icon _redIcon;

        public MainForm()
        {
            InitializeComponent();

            CreateStatusIcons();
            InitTrayMenu();
            InitTrayIcon();
        }

        private void CreateStatusIcons()
        {
            _greenIcon = Resource.Green;
            _redIcon = Resource.Red;
        }

        private static Bitmap ResizeIconToBitmap(Icon icon, int width = 16, int height = 16)
        {
            Bitmap bmp = new(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.Clear(Color.Transparent);
                g.DrawIcon(icon, new Rectangle(0, 0, width, height));
            }
            return bmp;
        }

        internal class CustomProfessionalColors : ProfessionalColorTable
        {
            public override Color ToolStripGradientBegin => Color.White;
            public override Color ToolStripGradientEnd => Color.White;
            public override Color MenuItemSelected => Color.White;
            public override Color MenuItemSelectedGradientBegin => Color.LightGreen;
            public override Color MenuItemSelectedGradientEnd => Color.Green;
            public override Color MenuItemPressedGradientBegin => Color.GreenYellow;
            public override Color MenuItemPressedGradientEnd => Color.YellowGreen;
        }

        private void InitTrayMenu()
        {
            _trayMenu = new ContextMenuStrip
            {
                // Renkleri özelleştiren renderer'ı uygula
                RenderMode = ToolStripRenderMode.ManagerRenderMode,
                Renderer = new ToolStripProfessionalRenderer(new CustomProfessionalColors())
            };

            statusItem = new ToolStripMenuItem("Durum: Bilinmiyor")
            {
                Enabled = true,
                ImageScaling = ToolStripItemImageScaling.None
            };

            _trayMenu.Items.Add(statusItem);
            _trayMenu.Items.Add(new ToolStripSeparator());
            _trayMenu.Items.Add("Göster", null, (s, e) => ShowMainWindow());
            _trayMenu.Items.Add("Yedeklemeyi Başlat", null, (s, e) => StartBackup());
            _trayMenu.Items.Add("Yedeklemeyi Durdur", null, (s, e) => StopBackup());
            _trayMenu.Items.Add("Yeniden Başlat", null, (s, e) => RestartBackup());
            _trayMenu.Items.Add(new ToolStripSeparator());
            _trayMenu.Items.Add("Çıkış", null, (s, e) => ExitApplication());

            UpdateTrayMenuStatus();
        }

        private void InitTrayIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Resource.AppIcon,
                Text = "BackupTool",
                Visible = false,
                ContextMenuStrip = _trayMenu
            };

            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }

        private void UpdateTrayMenuStatus()
        {
            bool running = _backupWorker != null && _backupWorker.IsRunning;

            statusItem.Text = running ? "Durum: Çalışıyor" : "Durum: Durduruldu";
            statusItem.Image = running
                ? ResizeIconToBitmap(_greenIcon)
                : ResizeIconToBitmap(_redIcon);

            // ➕ Başlat/Durdur butonlarını kontrol et
            start.Enabled = !running;
            stop.Enabled = running;
        }

        private void ShowMainWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            _notifyIcon.Visible = false;
        }

        private void StartBackup()
        {
            _backupWorker?.Start();
            UpdateTrayMenuStatus();
        }

        private void StopBackup()
        {
            _backupWorker?.Stop();
            UpdateTrayMenuStatus();
        }

        private void RestartBackup()
        {
            _backupWorker?.Stop();
            _backupWorker?.Start();
            UpdateTrayMenuStatus();
        }

        private void ExitApplication()
        {
            _notifyIcon.Visible = false;
            _backupWorker?.Dispose();
            _notifyIcon.Dispose();
            Application.Exit();
        }

        private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            ShowMainWindow();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resource.AppIcon;

            if (!CheckAndEnsureSettings())
            {
                MessageBox.Show("Bazı ayarlar eksik ya da hatalı. Lütfen ayarları tamamlayın.",
                                "Eksik Ayar", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                using var settingsForm = new SettingsForm();
                settingsForm.ShowDialog(this);
                return;
            }

            string connStr = Settings.Default.ConnectionString;
            string backupFolder = Settings.Default.BackupFolder;
            string[] excludedDbs = [.. Settings.Default.ExcludedDatabases
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(db => db.Trim())];
            int retentionDays = Settings.Default.RetentionDays;
            long minFreeSpaceMB = Settings.Default.MinFreeSpaceMB;
            string smtpServer = Settings.Default.SmtpServer;
            int smtpPort = Settings.Default.SmtpPort;
            string smtpUser = Settings.Default.SmtpUser;
            string smtpPass = Settings.Default.SmtpPassword;
            string emailTo = Settings.Default.EmailTo;
            int backupFrequencyDays = Settings.Default.BackupFrequencyDays;

            _backupWorker = new BackupWorker(
                connStr,
                backupFolder,
                excludedDbs,
                retentionDays,
                minFreeSpaceMB,
                smtpServer,
                smtpPort,
                smtpUser,
                smtpPass,
                emailTo,
                backupFrequencyDays
            );

            _backupWorker.OnLog += BackupWorker_OnLog;
            _backupWorker.Start();
            UpdateTrayMenuStatus();
        }

        private void BackupWorker_OnLog(object? sender, string e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => loggerArea.AppendText(e + Environment.NewLine)));
            }
            else
            {
                loggerArea.AppendText(e + Environment.NewLine);
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            _backupWorker?.Start();
            UpdateTrayMenuStatus();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            _backupWorker?.Stop();
            UpdateTrayMenuStatus();
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            _backupWorker?.Stop();
            _backupWorker?.Start();
            UpdateTrayMenuStatus();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;

                using var dialog = new CloseOptionDialog();
                var result = dialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    if (dialog.MinimizeToTraySelected)
                    {
                        Hide();
                        _notifyIcon.Visible = true;
                        _notifyIcon.ShowBalloonTip(1000, "BackupTool", "Program sistem tepsisinde çalışmaya devam ediyor.", ToolTipIcon.Info);
                    }
                    else
                    {
                        e.Cancel = false;
                        _backupWorker?.Dispose();
                        _notifyIcon.Dispose();
                    }
                }
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void Setting_Click(object sender, EventArgs e)
        {
            using var settingsForm = new SettingsForm();

            // Ayarlar kaydedildiğinde worker'ı yeniden başlat
            settingsForm.SettingsSaved += (s, ev) =>
            {
                _backupWorker?.Stop();

                // Yeni ayarlarla yeni worker oluştur
                _backupWorker = new BackupWorker(
                    Settings.Default.ConnectionString,
                    Settings.Default.BackupFolder,
                    [.. Settings.Default.ExcludedDatabases.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(db => db.Trim())],
                    Settings.Default.RetentionDays,
                    Settings.Default.MinFreeSpaceMB,
                    Settings.Default.SmtpServer,
                    Settings.Default.SmtpPort,
                    Settings.Default.SmtpUser,
                    Settings.Default.SmtpPassword,
                    Settings.Default.EmailTo,
                    Settings.Default.BackupFrequencyDays
                );

                _backupWorker.OnLog += BackupWorker_OnLog;
                _backupWorker.Start();
                UpdateTrayMenuStatus();
            };

            settingsForm.ShowDialog(this);
        }


        private static bool CheckAndEnsureSettings()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(Settings.Default.ConnectionString))
            {
                Settings.Default.ConnectionString = "Server=localhost;Database=master;Trusted_Connection=True;";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Settings.Default.BackupFolder))
            {
                Settings.Default.BackupFolder = System.IO.Path.Combine(Application.StartupPath, "Backups");
                isValid = false;
            }

            if (Settings.Default.RetentionDays <= 0)
            {
                Settings.Default.RetentionDays = 7;
                isValid = false;
            }

            if (Settings.Default.MinFreeSpaceMB <= 0)
            {
                Settings.Default.MinFreeSpaceMB = 1024;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Settings.Default.SmtpServer))
            {
                Settings.Default.SmtpServer = "smtp.example.com";
                isValid = false;
            }

            if (Settings.Default.SmtpPort <= 0)
            {
                Settings.Default.SmtpPort = 587;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Settings.Default.SmtpUser))
            {
                Settings.Default.SmtpUser = "admin@example.com";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Settings.Default.EmailTo))
            {
                Settings.Default.EmailTo = "admin@example.com";
                isValid = false;
            }

            if (Settings.Default.BackupFrequencyDays <= 0)
            {
                Settings.Default.BackupFrequencyDays = 1;
                isValid = false;
            }

            if (Settings.Default.ExcludedDatabases == null)
            {
                Settings.Default.ExcludedDatabases = string.Empty;
                isValid = false;
            }

            if (!isValid)
            {
                Settings.Default.Save();
            }

            return isValid;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _greenIcon?.Dispose();
                _redIcon?.Dispose();
                _notifyIcon?.Dispose();
                _trayMenu?.Dispose();

                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
