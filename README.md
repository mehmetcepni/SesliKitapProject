# 📚 SesliKitapWeb

Sesli kitapların yönetilebileceği, kullanıcıların yorum yapabileceği ve AI destekli içerik moderasyonuna sahip modern bir web uygulaması.

## 🎯 Özellikler

- ✅ **Kitap Yönetimi**: Kitap ekleme, düzenleme, silme ve kategorilendirme
- ✅ **Kullanıcı Sistemi**: Güvenli kayıt/giriş sistemi, profil yönetimi
- ✅ **Sosyal Özellikler**: Kullanıcı takip sistemi, takip istekleri (Pending/Accepted/Rejected)
- ✅ **Yorum Sistemi**: Kitap yorumları ve puanlama
- ✅ **AI Özellikleri**: Duygu analizi, içerik moderasyonu (ML.NET + Hugging Face)
- ✅ **Akıllı Öneriler**: Kişiselleştirilmiş kitap önerileri
- ✅ **Okuma Geçmişi**: Kullanıcı okuma takibi
- ✅ **Admin Paneli**: Kitap ve kategori yönetimi

## 🛠️ Teknolojiler

- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core** - ORM
- **SQL Server** - Veritabanı
- **ASP.NET Identity** - Kimlik doğrulama
- **ML.NET** - Makine öğrenmesi
- **Hugging Face API** - NLP servisleri
- **Bootstrap 5** - UI framework

## 📦 Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/mehmetcepni/SesliKitapWebv-3.git
cd SesliKitapWebv-3
```

2. Bağımlılıkları yükleyin:
```bash Politika
dotnet restore
```

3. `appsettings.json` dosyasında veritabanı bağlantısını yapılandırın

4. Veritabanını oluşturun:
```bash
dotnet ef database update
```

5. Uygulamayı çalıştırın:
```bash
dotnet run
```

6. Tarayıcıda açın: `http://localhost:5206`

## 🔐 Varsayılan Admin

- **Email**: admin@gmail.com
- **Şifre**: Admin123!

## 📝 API Yapılandırması

`appsettings.json` dosyasına Hugging Face API anahtarını ekleyin (opsiyonel):

```json
{
  "HuggingFace": {
    "ApiToken": "YOUR_API_KEY"
  }
}
```

## 📊 Veritabanı Tabloları

- Books, Categories, Reviews
- UserBooks, UserReadingHistory
- UserFollows (Takip sistemi)
- ApplicationUser, AspNetRoles, AspNetUserRoles

## 👥 Roller

- **Admin**: Kitap ve kategori yönetimi
- **User**: Kitap okuma, yorum yazma, kullanıcı takip etme

## 📞 İletişim

**Mehmet Cepni**  
Email: mehmetcepni343@gmail.com  
GitHub: [@mehmetcepni](https://github.com/mehmetcepni)  
Proje: [SesliKitapWebv-3](https://github.com/mehmetcepni/SesliKitapWebv-3)

## 📄 Lisans

MIT License

Bağımlılıkları yükleyin:
dotnet restore

appsettings.json dosyasında veritabanı bağlantısını yapılandırın

Veritabanını oluşturun:
dotnet ef database update

Uygulamayı çalıştırın:
dotnet run

Tarayıcıda açın: http://localhost:5206


🔐 Varsayılan Admin
Email: admin@gmail.com
Şifre: Admin123!
📝 API Yapılandırması
appsettings.json dosyasına Hugging Face API anahtarını ekleyin (opsiyonel):
{
  "HuggingFace": {
    "ApiToken": "YOUR_API_KEY"
  }
}

📊 Veritabanı Tabloları
Books, Categories, Reviews
UserBooks, UserReadingHistory
UserFollows (Takip sistemi)
ApplicationUser, AspNetRoles, AspNetUserRoles
👥 Roller
Admin: Kitap ve kategori yönetimi
User: Kitap okuma, yorum yazma, kullanıcı takip etme
📞 İletişim
Mehmet Cepni
Email: mehmetcepni343@gmail.com
GitHub: @mehmetcepni
Proje: SesliKitapWebv-3

📄 Lisans
MIT License
