SQL Backup Tool
Windows Forms tabanlı otomatik SQL yedekleme aracıdır. Belirlenen zaman aralıklarında veritabanı yedeklerini alır, eski dosyaları temizler, işlem loglarını oluşturur ve isteğe bağlı e‑posta bildirimi gönderir.
________________________________________
Özellikler
•	Belirlenen dizine otomatik SQL yedekleme
•	C:/Backup altında oluşturulan klasör yapısı:
o	ServerName/DATE/DBName.bak
•	Ayarlarda belirlenen saklama süresinden (gün) eski yedekleri otomatik silme
•	İşlemler hakkında dahili konsol ekranına log yazma
•	E-posta ile hata/başarı loglarını iletme
•	Sistem tepsisinde çalışabilme
o	Durdur / Başlat / Yeniden Başlat komutları
•	Ayarlar penceresi üzerinden gelişmiş yapılandırma
________________________________________
Klasör Yapısı
C:/Backup
   └── ServerName
        └── YYYY-MM-DD
             └── DatabaseName.bak
________________________________________
Ayarlar Penceresi
Ayarlar bölümünde aşağıdaki parametreler yapılandırılabilir:
Bağlantı Ayarları
•	Connection String (sunucu, yetki bilgileri)
Yedekleme Ayarları
•	Yedek Klasörü (varsayılan: C:/Backup)
•	Hariç Tutulan Veritabanları
•	Saklama Süresi (gün)
•	Minimum Boş Alan Kontrolü (GB)
•	Yedekleme Sıklığı (dakika/saat)
E-posta Bildirim Ayarları
•	SMTP Sunucusu
•	SMTP Portu
•	SSL/TLS
•	Gönderen E-posta
•	Şifre
•	Alıcı E-posta
________________________________________
İş Akışı
1.	Program başlatıldığında:
o	Saklama süresi dolmuş eski yedekleri siler.
2.	Yedekleme zamanında:
o	Belirlenen sunucu ve veritabanları için .bak dosyası oluşturur.
o	Tarih bazlı klasör yapısı oluşturulur.
3.	Tüm işlemler sırasında:
o	Dahili konsola log yazılır.
o	Loglar periyodik olarak e‑posta ile gönderilir.
________________________________________
Sistem Tepsisi Özellikleri
•	Program arka planda çalışabilir.
•	Tepsi menüsü üzerinden:
o	Başlat
o	Durdur
o	Yeniden Başlat komutları verilebilir.
________________________________________
Gereksinimler
•	.NET Framework / .NET 9
•	SQL Server erişimi
•	SMTP erişimi
________________________________________
Kurulum
1.	Uygulamayı çalıştırın.
2.	Ayarlar penceresinden gerekli alanları doldurun.
3.	Yedekleme döngüsü otomatik olarak başlayacaktır.
________________________________________
Loglama
•	Tüm yedekleme ve hata mesajları uygulama içi konsol ekranında görüntülenir.
•	İsteğe bağlı olarak SMTP üzerinden alıcı e‑postasına iletilir.
