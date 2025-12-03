using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BackupTool
{
    public class BackupWorker(
        string connectionString,
        string backupFolder,
        string[] excludedDatabases,
        int retentionDays,
        long minFreeDiskSpaceMB,
        string smtpServer,
        int smtpPort,
        string emailUsername,
        string emailPassword,
        string emailRecipient,
        int backupFrequencyDays) : IDisposable
    {
        private readonly object _workLock = new();
        private System.Threading.Timer? _timer;

        private readonly string _connectionString = connectionString;
        private readonly string _backupFolder = backupFolder;
        private readonly string[] _excludedDatabases = excludedDatabases;
        private readonly int _retentionDays = retentionDays;
        private readonly long _minFreeDiskSpaceMB = minFreeDiskSpaceMB;
        private readonly string _smtpServer = smtpServer;
        private readonly int _smtpPort = smtpPort;
        private readonly int _backupFrequencyDays = backupFrequencyDays;
        private readonly string _emailUsername = emailUsername;
        private readonly string _emailPassword = emailPassword;
        private readonly string _emailRecipient = emailRecipient;

        public bool IsRunning { get; private set; }
        public event EventHandler<string>? OnLog;

        public void Start()
        {
            if (IsRunning) return;

            _timer = new System.Threading.Timer(_ => DoWork(), null, TimeSpan.Zero, TimeSpan.FromDays(_backupFrequencyDays));
            IsRunning = true;
            Log("BackupWorker başladı.");
        }

        public void Stop()
        {
            if (!IsRunning) return;

            _timer?.Dispose();
            _timer = null;
            IsRunning = false;
            Log("BackupWorker durduruldu.");
        }

        private void DoWork()
        {
            var lockTaken = false;
            try
            {
                Monitor.TryEnter(_workLock, ref lockTaken);
                if (!lockTaken)
                {
                    Log("Önceki yedekleme işlemi halen devam ediyor, yeni tetikleme atlandı.");
                    return;
                }

                var drive = new DriveInfo(Path.GetPathRoot(_backupFolder)!);
                var availableMB = drive.AvailableFreeSpace / (1024 * 1024);

                if (availableMB < _minFreeDiskSpaceMB)
                {
                    Log($"Yeterli disk alanı yok ({availableMB} MB). Yedekleme yapılmayacak.");
                    SendAlertEmail(availableMB);
                    return;
                }

                var successDbs = new List<string>();
                var skippedDbs = new List<string>();
                var failedDbs = new List<string>();

                if (!Directory.Exists(_backupFolder))
                    Directory.CreateDirectory(_backupFolder);

                DeleteOldBackups();

                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var getDatabasesCmd = new SqlCommand("SELECT name FROM sys.databases WHERE database_id > 4", connection);
                using var reader = getDatabasesCmd.ExecuteReader();
                while (reader.Read())
                {
                    var dbName = reader.GetString(0);
                    if (_excludedDatabases.Contains(dbName, StringComparer.OrdinalIgnoreCase))
                        continue;

                    try
                    {
                        var builder = new SqlConnectionStringBuilder(_connectionString);
                        var serverName = builder.DataSource.Replace("\\", "_").Replace(":", "_");
                        var dateFolder = Path.Combine(_backupFolder, serverName, DateTime.Now.ToString("yyyy-MM-dd"));

                        if (!Directory.Exists(dateFolder))
                            Directory.CreateDirectory(dateFolder);

                        var filePath = Path.Combine(dateFolder, $"{dbName}.bak");

                        if (File.Exists(filePath))
                        {
                            skippedDbs.Add(dbName);
                            Log($"ℹ {dbName} veritabanı için yedek zaten mevcut. Atlandı.");
                            continue;
                        }

                        var backupSql = $"""
                    BACKUP DATABASE [{dbName}] TO DISK = N'{filePath}'
                    WITH INIT, FORMAT, NAME = N'{dbName} Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10
                    """;

                        using var backupConn = new SqlConnection(_connectionString);
                        using var backupCmd = new SqlCommand(backupSql, backupConn);
                        backupConn.Open();
                        backupCmd.ExecuteNonQuery();

                        successDbs.Add(dbName);
                        Log($"✔ {dbName} veritabanı yedeklendi.");
                    }
                    catch (Exception ex)
                    {
                        failedDbs.Add(dbName);
                        Log($"❌ Hata: {dbName} yedeklenemedi. {ex.Message}");
                    }
                }

                if (successDbs.Count > 0)
                {
                    Log($"✅ Başarıyla yedeklenen veritabanı sayısı: {successDbs.Count}");
                    SendSuccessEmail(successDbs);
                }

                if (skippedDbs.Count > 0)
                    Log($"ℹ Atlanan (zaten yedeği olan) veritabanı sayısı: {skippedDbs.Count}");

                if (failedDbs.Count > 0)
                {
                    Log($"❌ Yedeklenemeyen veritabanı sayısı: {failedDbs.Count}");
                    SendFailureEmail(failedDbs);
                }

                if (successDbs.Count == 0 && skippedDbs.Count == 0 && failedDbs.Count == 0)
                    Log("ℹ Yedeklenecek veritabanı bulunamadı.");
            }
            catch (Exception ex)
            {
                Log("🔴 Genel hata: " + ex.Message);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(_workLock);
            }
        }

        private void DeleteOldBackups()
        {
            try
            {
                if (_retentionDays <= 0)
                {
                    Log("ℹ Eski yedek silme devre dışı bırakılmış (retentionDays <= 0).");
                    return;
                }

                Log("📁 Eski yedek temizleme işlemi başladı...");
                var files = Directory.GetFiles(_backupFolder, "*.bak", SearchOption.AllDirectories);
                int deletedCount = 0, keptCount = 0;

                foreach (var file in files)
                {
                    try
                    {
                        var fi = new FileInfo(file);
                        if (fi.LastWriteTime.Date < DateTime.Now.Date.AddDays(-_retentionDays))
                        {
                            fi.Delete();
                            deletedCount++;
                            Log($"🗑 Silindi: {file}");
                        }
                        else
                        {
                            keptCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"❗ {file} silinemedi: {ex}");
                    }
                }

                Log($"🧾 Silinen dosya: {deletedCount}, Tutulan dosya: {keptCount}");

                // 📁 Tarih klasörlerini kontrol et
                var dateFolders = Directory.GetDirectories(_backupFolder, "*", SearchOption.AllDirectories);
                int deletedFolders = 0;

                foreach (var folder in dateFolders)
                {
                    try
                    {
                        // Klasör adı geçerli bir tarih mi?
                        var folderName = Path.GetFileName(folder);
                        if (DateTime.TryParseExact(folderName, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var folderDate))
                        {
                            if (folderDate.Date < DateTime.Now.Date.AddDays(-_retentionDays))
                            {
                                // Klasör boş mu (tüm .bak'ler silinmiş mi)?
                                if (!Directory.EnumerateFileSystemEntries(folder).Any())
                                {
                                    Directory.Delete(folder, false);
                                    deletedFolders++;
                                    Log($"🗂 Silinen boş tarih klasörü: {folder}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"❗ Klasör silinemedi: {folder} - {ex.Message}");
                    }
                }

                Log($"📦 Silinen klasör sayısı: {deletedFolders}");
            }
            catch (Exception ex)
            {
                Log("🔴 DeleteOldBackups genel hatası: " + ex);
            }
        }



        private void SendAlertEmail(long availableMB)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(_connectionString);
                var serverName = builder.DataSource;

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_emailUsername));
                message.To.Add(MailboxAddress.Parse(_emailRecipient));
                message.Subject = "Disk Alanı Uyarısı";
                message.Body = new TextPart("plain")
                {
                    Text = $"Sql veri tabanı yedeği için disk alanı yetersiz: {availableMB} MB\nSunucu: {serverName}"
                };

                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(_emailUsername, _emailPassword);
                client.Send(message);
                client.Disconnect(true);

                Log("Disk alanı uyarısı e-postası gönderildi.");
            }
            catch (Exception ex)
            {
                Log("Disk alanı e-postası gönderilemedi: " + ex.Message);
            }
        }

        private void SendSuccessEmail(List<string> successDbs)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_emailUsername));
                message.To.Add(MailboxAddress.Parse(_emailRecipient));
                message.Subject = "SQL Yedekleme Başarılı";

                message.Body = new TextPart("plain")
                {
                    Text = $"Aşağıdaki veritabanları başarıyla yedeklendi:\n\n" + string.Join("\n", successDbs)
                };

                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(_emailUsername, _emailPassword);
                client.Send(message);
                client.Disconnect(true);

                Log("Başarılı yedekler için e-posta gönderildi.");
            }
            catch (Exception ex)
            {
                Log("Başarılı yedek e-postası gönderilemedi: " + ex.Message);
            }
        }

        private void SendFailureEmail(List<string> failedDbs)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_emailUsername));
                message.To.Add(MailboxAddress.Parse(_emailRecipient));
                message.Subject = "SQL Yedekleme Hataları";

                message.Body = new TextPart("plain")
                {
                    Text = $"Aşağıdaki veritabanları yedeklenemedi:\n\n" + string.Join("\n", failedDbs)
                };

                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(_emailUsername, _emailPassword);
                client.Send(message);
                client.Disconnect(true);

                Log("Yedekleme hataları için e-posta gönderildi.");
            }
            catch (Exception ex)
            {
                Log("Yedekleme hata e-postası gönderilemedi: " + ex.Message);
            }
        }

        private void Log(string message)
        {
            var timestamped = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            OnLog?.Invoke(this, timestamped);
            OnLog?.Invoke(this, new string('─', 85));
        }

        public void Dispose() => Stop();
    }
}
