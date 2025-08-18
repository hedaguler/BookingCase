# 🏨 BookingCase — Otel Arama & Rezervasyon Örneği

> ASP.NET Core MVC ile **Booking.com (RapidAPI)** uç noktalarını kullanan, **Sona** temasıyla zenginleştirilmiş mini bir otel arama uygulaması.
> Modern arayüz, hızlı arama ve temiz mimari! ✨

---

## 🚀 Özellikler

* 🔎 **Arama Formu (Hero)** — Şehir, tarih, kişi/oda ve para birimi seçimi
* 🗺️ **Şehir → `dest_id` Dönüşümü** (Booking API `searchDestination`)
* 🏨 **Otel Sonuçları** — Fotoğraf, isim, adres, puan ve fiyat kartları
* 🧾 **Detay Sayfası** — Çoklu görseller, giriş/çıkış saatleri, puan ve özet
* 🧩 **Sona Tema Bileşenleri**

  * ℹ️ About Us
  * 🧰 What We Do (mini **carousel**)
  * 💬 Testimonials
  * 📰 Hotel News (Blog grid)
* 🌐 **Çoklu Para Birimi** (USD, TRY, …)
* 🧼 **Temiz & Genişletilebilir Kod** — Services, ViewModels, MVC ayrımı
* 🛡️ **Quota Koruması** — 429 (Too Many Requests) için kullanıcı dostu uyarı

---

## 🧱 Teknolojiler

* **.NET 8** + **ASP.NET Core MVC**
* **HttpClientFactory** (isimli client)
* **Newtonsoft.Json (JObject/JArray)**
* **Bootstrap 5 / jQuery** (Sona temasının statik öğeleri)

---

## 📦 Hızlı Kurulum

> Aşağıdaki adımlar Windows/macOS/Linux’ta çalışır. Visual Studio (2022+) ile de doğrudan F5 yapabilirsiniz.

### 1) Depoyu Al

```bash
git clone https://github.com/hedaguler/BookingCase.git
cd BookingCase
```

### 2) Gerekli SDK

* [.NET 8 SDK](https://dotnet.microsoft.com/) kurulu olmalı.

Doğrula:

```bash
dotnet --version
```

### 3) RapidAPI Gizli Anahtarlarını Tanımla

**Önerilen Yol: .NET User Secrets**

> Proje **web projesi** klasöründe çalıştırın:

```bash
dotnet user-secrets init
dotnet user-secrets set "RapidApi:Key" "BURAYA_RAPIDAPI_KEY"
dotnet user-secrets set "RapidApi:Host" "booking-com15.p.rapidapi.com"
```

> Alternatif: Ortam değişkenleri
> `RAPIDAPI__KEY` ve `RAPIDAPI__HOST` olarak da ayarlayabilirsiniz.

### 4) Çalıştır

```bash
dotnet restore
dotnet build
dotnet run
```

Uygulama tipik olarak `https://localhost:7079` altında açılır. 🌐

---

## 🧭 API Entegrasyonu (Özet)

Uygulama, `Program.cs` içinde **isimli HttpClient** kaydeder:

```csharp
builder.Services.AddHttpClient("booking", client =>
{
    var host = builder.Configuration["RapidApi:Host"] ?? "booking-com15.p.rapidapi.com";
    client.BaseAddress = new Uri($"https://{host}/");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", host);

    var key = builder.Configuration["RapidApi:Key"];
    if (!string.IsNullOrWhiteSpace(key))
        client.DefaultRequestHeaders.Add("x-rapidapi-key", key);
});
```

### Kullanılan Uç Noktalar

* 🔹 `GET api/v1/hotels/searchDestination` → **şehir** → `dest_id`
* 🔹 `GET api/v1/hotels/searchHotels` → **otel listeleme** / detay bilgiler

> **Not:** Ücretsiz RapidAPI paketlerinde **429 (Too Many Requests)** limiti görülebilir.
> Bu durumda kısa süre bekleyip tekrar deneyin.

---

## 🗂️ Proje Yapısı

```
BookingCase/
├─ Controllers/
│  └─ HomeController.cs
├─ Models/
│  └─ ViewModels/ (HotelCardViewModel, HotelDetailViewModel, SearchFormViewModel ...)
├─ Services/
│  ├─ IBookingApiService.cs
│  └─ BookingApiService.cs
├─ Views/
│  ├─ Home/ (Index, Results, Details ...)
│  └─ Shared/ (_Layout, _ValidationScriptsPartial ...)
├─ wwwroot/
│  ├─ sona/ (tema dosyaları: css, js, img)
│  └─ site.css (küçük özelleştirmeler)
└─ Program.cs
```

---

## 🖼️ Ekran Görüntüleri

> (İsterseniz repoda `docs/` klasörü açıp görselleri ekleyin)

* **Ana Sayfa / Arama Formu**
* **Otel Sonuçları**
* **Detay Sayfası**
* **What We Do Carousel**, **Testimonials**, **Blog**

---

## 🛠️ Geliştirme İpuçları

* 🧪 Yeni şehir ararken **dest\_id** dinamik olarak bulunur; servis `query`/`name` parametrelerini birlikte gönderir.
* 🌍 `currency_code`, `languagecode`, `units` gibi parametreler **opsiyonel** ve tema/metinlere göre ayarlanabilir.
* 🧯 Aşırı istek durumunda 429 hatası yakalanıp kullanıcı dostu mesaj gösterilir.

---

## 🧭 Yol Haritası

* [ ] Favori oteller / basit sepet
* [ ] Harita entegrasyonu
* [ ] Basit kimlik doğrulama
* [ ] Çoklu dil desteği (tr/en)

---

## 🤝 Katkı

Katkılar memnuniyetle kabul edilir! PR açmadan önce kısa bir **issue** oluşturursanız harika olur. 🙏

---

## ⚖️ Lisans

Bu proje **MIT** lisansı ile sunulmaktadır.
Tasarım (Sona) ve Booking.com API kullanım koşulları ilgili sağlayıcılara aittir.

---

## 📬 İletişim

* GitHub: [@hedaguler](https://github.com/hedaguler)
* Proje: **BookingCase**

> Beğendiysen ⭐ vermeyi unutma!
> Güzel konaklamalar, temiz kodlar! ✈️🏨💻



https://github.com/user-attachments/assets/f17b5184-6e73-4258-b101-03191e43cb64

<img width="607" height="665" alt="image" src="https://github.com/user-attachments/assets/fd03f671-a020-4093-a9f9-2909252c175c" />


<img width="481" height="575" alt="Ekran görüntüsü 2025-08-19 000146" src="https://github.com/user-attachments/assets/dfe961a3-e85c-4210-81b7-11d4136488b1" />
<img width="607" height="665" alt="image" src="https://github.com/user-attachments/assets/a075267d-2764-4895-bf31-e10fd88770e1" />
<img width="535" height="816" alt="Ekran görüntüsü 2025-08-19 000109" src="https://github.com/user-attachments/assets/7c9de722-cd6a-4dfd-af22-6ae76cfb38b4" />
<img width="607" height="665" alt="image" src="https://github.com/user-attachments/assets/300aa15d-4254-4f87-88b3-aab89da959cc" />
<img width="476" height="787" alt="Ekran görüntüsü 2025-08-19 000332" src="https://github.com/user-attachments/assets/39f80198-8d9c-4f83-990d-a5458df83dfe" />
<img width="607" height="665" alt="image" src="https://github.com/user-attachments/assets/45f6d97e-430f-4f3b-a626-0938148f5b30" />
<img width="512" height="662" alt="Ekran görüntüsü 2025-08-19 000406" src="https://github.com/user-attachments/assets/16b89993-fcf2-476b-9175-e1c1076a41f3" />
<img width="607" height="665" alt="image" src="https://github.com/user-attachments/assets/ad91217f-6923-4a36-966f-32360bc7db49" />
<img width="607" height="665" alt="Ekran görüntüsü 2025-08-19 000520" src="https://github.com/user-attachments/assets/7ffb45e6-7bcf-4395-baf1-f8a960fa1de0" />
