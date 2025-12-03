SQL Backup Tool

Windows Forms tabanlı otomatik SQL yedekleme aracıdır. Belirlenen zaman aralıklarında veritabanı yedeklerini alır, eski dosyaları temizler, işlem loglarını oluşturur ve isteğe bağlı e‑posta bildirimi gönderir.

Özellikler

Belirlenen dizine otomatik SQL yedekleme

C:/Backup altında oluşturulan klasör yapısı:

ServerName/DATE/DBName.bak

Ayarlarda belirlenen saklama süresinden (gün) eski yedekleri otomatik silme

İşlemler hakkında dahili konsol ekranına log yazma

E-posta ile hata/başarı loglarını iletme

Sistem tepsisinde çalışabilme

Durdur / Başlat / Yeniden Başlat komutları

Ayarlar penceresi üzerinden gelişmiş yapılandırma

Klasör Yapısı
C:/Backup
   └── ServerName
        └── YYYY-MM-DD
             └── DatabaseName.bak
Ayarlar Penceresi

Ayarlar bölümünde aşağıdaki parametreler yapılandırılabilir:

Bağlantı Ayarları

Connection String (sunucu, yetki bilgileri)

Yedekleme Ayarları

Yedek Klasörü (varsayılan: C:/Backup)

Hariç Tutulan Veritabanları

Saklama Süresi (gün)

Minimum Boş Alan Kontrolü (GB)

Yedekleme Sıklığı (dakika/saat)

E-posta Bildirim Ayarları

SMTP Sunucusu

SMTP Portu

SSL/TLS

Gönderen E-posta

Şifre

Alıcı E-posta

İş Akışı

Program başlatıldığında:

Saklama süresi dolmuş eski yedekleri siler.

Yedekleme zamanında:

Belirlenen sunucu ve veritabanları için .bak dosyası oluşturur.

Tarih bazlı klasör yapısı oluşturulur.

Tüm işlemler sırasında:

Dahili konsola log yazılır.

Loglar periyodik olarak e‑posta ile gönderilir.

Sistem Tepsisi Özellikleri

Program arka planda çalışabilir.

Tepsi menüsü üzerinden:

Başlat

Durdur

Yeniden Başlat komutları verilebilir.

Gereksinimler

.NET Framework / .NET 9+ (projenize göre güncelleyiniz)

SQL Server erişimi

SMTP erişimi

Kurulum

Uygulamayı çalıştırın.

Ayarlar penceresinden gerekli alanları doldurun.

Yedekleme döngüsü otomatik olarak başlayacaktır.

Loglama

Tüm yedekleme ve hata mesajları uygulama içi konsol ekranında görüntülenir.

İsteğe bağlı olarak SMTP üzerinden alıcı e‑postasına iletilir.
