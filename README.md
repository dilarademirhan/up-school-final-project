# Final Project - Crawler

Bu proje, rakip bir e-ticaret sitesinden ürün verilerini otomatik olarak çekmeyi ve crawler olaylarını gerçek zamanlı olarak takip etmeyi amaçlayan bir web uygulamasıdır.

## 🔑 Özellikler

### Web Scraping
- E-ticaret sitesinden ürün bilgilerini otomatik çekme
- Ürün adı, fiyat, indirim durumu, resim URL'leri gibi detayları toplama
- Selenium ile web scraping işlemleri

### Kullanıcı Yönetimi
- Microsoft Identity ile kullanıcı kimlik doğrulama
- Geleneksel ve Google ile giriş seçenekleri
- JWT token tabanlı güvenlik
- Kullanıcı oturum yönetimi

### Sipariş Yönetimi
- Yeni sipariş oluşturma
- Sipariş detaylarını görüntüleme
- Sipariş silme (soft ve hard delete)
- Sipariş durumu takibi

### Gerçek Zamanlı İletişim
- SignalR ile canlı veri aktarımı
- Crawler durumu takibi
- Anlık bildirimler

### Bildirim Sistemi
- E-posta bildirimleri
- Toast bildirimleri
- Kullanıcı tercihlerine göre bildirim yönetimi

### Veri Yönetimi
- Excel'e veri aktarma
- Ürün verilerini görüntüleme ve filtreleme
- Veritabanı yönetimi

## 🚀 Teknolojiler

### Frontend
- React.js
- JavaScript
- Material-UI (@mui)
- SignalR (Gerçek zamanlı iletişim için)
- Axios (API istekleri için)
- React Router (Sayfa yönlendirmeleri için)
- JWT Authentication

### Backend
- .NET 7
- Clean Architecture
- Entity Framework Core
- MySQL
- SignalR (Gerçek zamanlı iletişim için)
- Selenium (Web scraping için)
- MediatR (CQRS implementasyonu için)
- FluentValidation
- Microsoft Identity (Kullanıcı yönetimi için)
- JWT Authentication
- Swagger (API dokümantasyonu için)



## 🛠️ Kurulum

### Frontend Kurulumu

1. Frontend dizinine gidin:
```bash
cd final-project-frontend
```

2. Bağımlılıkları yükleyin:
```bash
npm install
```

3. Geliştirme sunucusunu başlatın:
```bash
npm run dev
```

### Backend Kurulumu

1. Backend dizinine gidin:
```bash
cd FinalProject/WebApi
```

2. Bağımlılıkları yükleyin:
```bash
dotnet restore
```

3. Veritabanını oluşturun:
```bash
dotnet ef database update
```

4. Uygulamayı başlatın:
```bash
dotnet run
```


## 🔒 Güvenlik

- JWT tabanlı kimlik doğrulama
- CORS politikaları
- Güvenli API endpoint'leri
- Şifreleme ve hash'leme
- Korumalı rotalar

## 📝 API Dokümantasyonu

Backend uygulaması çalıştığında, Swagger UI üzerinden API dokümantasyonuna erişebilirsiniz:
```
http://localhost:5000/swagger
```
