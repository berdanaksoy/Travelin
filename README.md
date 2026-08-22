<h1 align="center"> Travelin: Çok Dilli Tur & Rezervasyon Yönetim Sistemi </h1>

<p align="center"> Ziyaretçilerin turları keşfedip rezervasyon yaptığı, yöneticilerin ise turları, rezervasyonları ve yorumları merkezi bir panelden yönettiği; beş dil desteğine sahip, MongoDB tabanlı full-stack bir seyahat platformu. </p>

<p align="center">
  <img alt="Build" src="https://img.shields.io/badge/Build-Passing-brightgreen?style=for-the-badge">
  <img alt="Framework" src="https://img.shields.io/badge/Framework-ASP.NET%20Core%2010-512bd4?style=for-the-badge">
  <img alt="Database" src="https://img.shields.io/badge/Database-MongoDB-47A248?style=for-the-badge">
  <img alt="Architecture" src="https://img.shields.io/badge/Architecture-MVC-blue?style=for-the-badge">
</p>

---

## 🌟 Genel Bakış

**Travelin**, bir seyahat acentesinin hem müşteriye dönük vitrinini hem de arka plandaki yönetim süreçlerini tek bir platformda birleştiren kapsamlı bir tur ve rezervasyon sistemidir. Ziyaretçiler turları filtreleyip inceleyebilir, rezervasyon oluşturabilir ve yorum bırakabilir; yöneticiler ise turları, tur programlarını, kategorileri, rezervasyonları ve yorumları özel bir panel üzerinden yönetir.

Sistem, ziyaretçi arayüzünde **beş dil** (Türkçe, İngilizce, Almanca, Fransızca, İspanyolca) desteği sunar ve verilerini ilişkisel bir veritabanı yerine **MongoDB** üzerinde tutar.

### Problem

> Çoğu MVC eğitim projesi, ilişkisel bir veritabanı (SQL Server) ve tek dilli bir arayüz üzerine kuruludur. Gerçek dünyadaki bir seyahat platformu ise iki ek zorluk barındırır: içeriğin birden fazla dilde sunulması ve verinin, ilişkisel modelin sunduğu JOIN ve foreign key güvenliği olmadan — döküman tabanlı bir yapıda — modellenmesi. Bu iki konu çoğu projede ya hiç ele alınmaz ya da yüzeysel kalır.

### Çözüm

Travelin, veri katmanını **MongoDB** üzerine kurarak ilişkisel olmayan bir dünyada ilişkileri yönetmeyi hedefler: tablolar arası JOIN yerine referans ID'ler ve uygulama katmanında kurulan eşleştirmeler, foreign key kısıtları yerine bilinçli veri bütünlüğü kararları kullanılır. Arayüz katmanında ise **ASP.NET Core Localization** ile kültür-bazlı bir yapı kurularak, ziyaretçiye dönük tüm içerik beş dilde `.resx` kaynak dosyaları üzerinden sunulur. Rezervasyon tarafında ise sistem, basit bir form gönderiminin ötesine geçerek kapasite kontrolü, durum yönetimi ve otomatik e-posta bildirimi içeren gerçek bir iş akışı barındırır.

---

## ✨ Temel Özellikler

### 🌍 Çok Dilli Yapı (5 Dil)
* **Kültür-Bazlı Lokalizasyon:** Ziyaretçiye dönük tüm arayüz Türkçe, İngilizce, Almanca, Fransızca ve İspanyolca dillerinde sunulur.
* **Merkezi Kaynak Yönetimi:** Tüm metinler `.resx` kaynak dosyaları üzerinden yönetilir; sabit metin (hard-coded string) kullanımından kaçınılır.
* **Anlık Dil Değişimi:** Kullanıcı arayüz üzerinden dili değiştirebilir; seçim oturum boyunca korunur.

### 🎫 Rezervasyon & İş Akışı Yönetimi
* **Kapasite Kontrolü:** Bir tur kapasitesine ulaştığında rezervasyon butonu otomatik olarak pasifleşir; kartlarda "Kontenjan Doldu" rozeti gösterilir.
* **Tarih Denetimi:** Tarihi geçmiş turlar rezervasyona kapatılır ve görsel olarak işaretlenir.
* **Durum Yönetimi:** Her rezervasyon Beklemede, Onaylandı veya İptal Edildi durumlarında izlenir.
* **Otomatik E-posta Bildirimi:** Rezervasyon onaylandığında veya iptal edildiğinde müşteriye MailKit üzerinden bilgilendirme e-postası gönderilir.
* **Katmanlı Doğrulama:** Kapasite ve tarih denetimleri hem istemci (buton durumu) hem sunucu (GET ve POST) tarafında yapılır; istemci denetimi atlatılsa bile sunucu işlemi reddeder.

### 🗄️ MongoDB Tabanlı Veri Mimarisi
* **Döküman Tabanlı Modelleme:** Turlar, kategoriler, rezervasyonlar, yorumlar ve tur programları arası ilişkiler referans ID'ler ile kurulur.
* **Otomatik Veri Doldurma (Seeding):** Uygulama ilk açılışta boş koleksiyonları otomatik olarak gerçekçi demo verisiyle doldurur — kategoriler, 30+ tur, tur programları, yorumlar ve rezervasyonlar tek adımda hazır hale gelir.

### 📊 Excel & PDF Dışa Aktarma
* **Raporlama:** Yönetim panelinden bir tura ait rezervasyon listesi, ClosedXML ile Excel'e ve QuestPDF ile PDF'e aktarılabilir.

### 🛠️ Yönetim Paneli
* **Kapsamlı CRUD:** Turlar, kategoriler, tur programları, rezervasyonlar, yorumlar ve site ayarları için tam yönetim.
* **Filtreleme, Arama & Sayfalama:** Tüm liste ekranlarında (hem ziyaretçi hem admin) tekrar kullanılabilir ortak bir sayfalama bileşeni ve duruma özel filtreler.
* **Yorum Onay Sistemi:** Ziyaretçi yorumları, yayınlanmadan önce yönetici onayından geçer.
* **Dinamik Site Ayarları:** İletişim bilgileri, sosyal medya bağlantıları ve tanıtım videosu panelden yönetilir; footer ve iletişim sayfasına anlık yansır.

### 🗺️ Tur Detayı & Program
* **Günlük Tur Programı:** Her tur için gün gün planlanmış, akordiyon yapıda bir program.
* **Puan & Yorum:** Turların ortalama puanı ve onaylı yorumları detay sayfasında listelenir.

---

## 🛠️ Teknoloji Yığını & Mimari

Proje, veri, iş mantığı ve arayüzü birbirinden ayıran katmanlı bir **MVC (Model-View-Controller)** mimarisi üzerine kurulmuştur. İş mantığı servis katmanına taşınmış, veri taşımada DTO'lar ve nesne eşlemesinde AutoMapper kullanılmıştır.

### Kullanılan Teknolojiler

| Teknoloji | Amacı | Neden Tercih Edildi |
| :--- | :--- | :--- |
| **ASP.NET Core 10** | Ana Backend Framework | Kurumsal seviye performans, güvenlik ve platform bağımsızlık sağlar. |
| **C#** | Programlama Dili | Güçlü tip sistemi ve modern özellikler ile sürdürülebilir iş mantığı sunar. |
| **MongoDB** | Döküman Tabanlı Veritabanı | Esnek şema ve döküman tabanlı veri modelleme; ilişkisel olmayan bir yaklaşımı deneyimleme. |
| **MongoDB.Driver** | Veri Erişim Katmanı | MongoDB koleksiyonlarına tip güvenli erişim ve sorgulama. |
| **AutoMapper** | Nesne Eşleme | Entity ↔ DTO dönüşümlerini merkezi ve tekrarsız hale getirir. |
| **ASP.NET Core Localization** | Çok Dilli Yapı | `.resx` tabanlı, kültür-bazlı içerik sunumu (5 dil). |
| **MailKit** | E-posta Gönderimi | SMTP üzerinden rezervasyon bildirim e-postaları. |
| **ClosedXML** | Excel Dışa Aktarma | Bir tura ait rezervasyon verilerini `.xlsx` formatında raporlama. |
| **QuestPDF** | PDF Dışa Aktarma | Bir tura ait rezervasyon verilerini PDF formatında raporlama. |
| **Razor / ViewComponents** | Şablon Motoru | Modüler ve yeniden kullanılabilir UI bileşenleri (footer, tur listesi). |
| **Bootstrap & jQuery** | Frontend Tasarım | Responsive tasarım ve dinamik kullanıcı etkileşimi. |

---

## 📁 Proje Yapısı

```
Travelin/
├── 📄 Program.cs              # Uygulama başlangıç noktası, DI & seeding yapılandırması
├── 📄 appsettings.json        # MongoDB bağlantısı ve SMTP ayarları
├── 📂 Controllers/            # İş akışı ve request yönetimi (ziyaretçi + admin)
├── 📂 Services/               # İş mantığı katmanı (Tour, Reservation, Comment, Category...)
├── 📂 Dtos/                   # Veri transfer nesneleri (Result, Create, Update, Filter DTO'ları)
├── 📂 Entities/               # MongoDB döküman modelleri
│   ├── Tour.cs                # Tur
│   ├── Category.cs            # Kategori
│   ├── Reservation.cs         # Rezervasyon
│   ├── Comment.cs             # Yorum
│   ├── TourProgram.cs         # Günlük tur programı
│   ├── SiteSetting.cs         # Dinamik site ayarları
│   └── ReservationStatuses.cs # Rezervasyon durum sabitleri
├── 📂 Models/                 # View modelleri (HomeViewModel, CreateReservationViewModel...)
├── 📂 Mapping/                # AutoMapper profili (Entity ↔ DTO eşlemeleri)
├── 📂 Helpers/                # Yardımcı sınıflar (YouTube URL dönüştürme)
├── 📂 Settings/               # Yapılandırma ayarları (veritabanı & e-posta)
├── 📂 Seed/                   # Otomatik veri doldurma (DataSeeder)
├── 📂 ViewComponents/         # Yeniden kullanılabilir UI bileşenleri (Footer, TourList)
├── 📂 Resources/              # Lokalizasyon kaynakları (.resx — 5 dil)
├── 📄 SharedResource.cs       # Lokalizasyon için ortak kaynak referans sınıfı
├── 📂 Views/                  # Razor arayüz dosyaları (Home, Tour, Reservation, Admin, Shared)
└── 📂 wwwroot/                # Statik dosyalar (CSS, JS, görseller, şablon asset'leri)
```

---

## 🗃️ Veritabanı Mimarisi

Travelin, ilişkisel bir veritabanının aksine, koleksiyonlar arası ilişkileri referans ID'ler üzerinden kurar. Foreign key kısıtları bulunmadığından, veri bütünlüğü uygulama katmanında bilinçli olarak yönetilir.

| Koleksiyon | Açıklama |
| :--- | :--- |
| **Tours** | Tur bilgileri (başlık, ülke/şehir, kapasite, tarih, fiyat, kategori referansı) |
| **Categories** | Tur kategorileri (isim, ikon, durum) |
| **Reservations** | Rezervasyonlar (müşteri bilgisi, kişi sayısı, durum, tur referansı) |
| **Comments** | Yorumlar (puan, içerik, onay durumu, tur referansı) |
| **TourPrograms** | Turların gün gün programı (gün numarası, başlık, açıklama, tur referansı) |
| **SiteSettings** | Panelden yönetilen site geneli ayarlar (iletişim, sosyal medya, video) |

---

## 📸 Ekran Görüntüleri

### Kullanıcı Sayfaları

**Ana Sayfa:**

<img width="1554" height="6493" alt="ana_sayfa" src="https://github.com/user-attachments/assets/12e6f36d-0bd9-4b88-9a37-8667056225d1" />

<details>
<summary><strong>📸 Diğer Ekran Görüntülerini İncelemek İçin Tıklayın</strong></summary>
<br>

**Turlar Sayfası List Görünümü:**

<img width="1554" height="4531" alt="turlar_list" src="https://github.com/user-attachments/assets/0126607c-9aa1-4e3f-b765-7634f2f5b1da" />


**Turlar Sayfası Grid Görünümü:**

<img width="1554" height="4511" alt="turlar_grid" src="https://github.com/user-attachments/assets/9d807ba9-3347-4193-8aa4-5d9a8a73946b" />

**Tur Detayı Sayfası:**

<img width="1554" height="6214" alt="tur_detay" src="https://github.com/user-attachments/assets/37b036ba-f42f-4522-b45a-8663227865f9" />

**Rezervsyon Sayfası:**

<img width="1554" height="3185" alt="rezervasyon" src="https://github.com/user-attachments/assets/d0089036-c9ee-427c-9679-50e1db7e165d" />

**İletişim Sayfası:**

<img width="1554" height="2813" alt="bize_ulasin" src="https://github.com/user-attachments/assets/ac4954ae-9acd-4ee5-ab1d-6841913e5b02" />

**Hakkımızda Sayfası:**

<img width="1554" height="5080" alt="hakkimizda" src="https://github.com/user-attachments/assets/9cba262f-5f6f-4f54-b621-dbaa9380e081" />

</details>

<br>

### Admin Sayfaları

**Tur Sayfası:**

<img width="2228" height="2039" alt="adm_turlar" src="https://github.com/user-attachments/assets/621d1220-7b30-490b-a5c4-3bdbc5677510" />

<details>
<summary><strong>📸 Diğer Ekran Görüntülerini İncelemek İçin Tıklayın</strong></summary>
<br>

**Tur Ekleme/Güncelleme Sayfası:**

<img width="1580" height="2205" alt="adm_tur_ekleme_guncelleme" src="https://github.com/user-attachments/assets/94cc9b38-91bf-4775-abc2-8c8af1c2c214" />

**Kategoriler Sayfası:**

<img width="2246" height="1288" alt="adm_kategoriler" src="https://github.com/user-attachments/assets/deff1900-4350-4c33-bad3-e627d32d8368" />

**Kategori Ekleme/Güncelleme Sayfası:**

<img width="1573" height="928" alt="adm_kategori_ekleme_guncelleme" src="https://github.com/user-attachments/assets/70977d8e-2bc0-49af-bf72-5b9d8ca7a2c1" />

**Tur Programı Sayfası:**

<img width="1554" height="1944" alt="adm_programlar" src="https://github.com/user-attachments/assets/d16c19a2-8db8-4690-95ff-453b57491570" />

**Tur Programı Ekleme/Güncelleme Sayfası:**

<img width="1554" height="1205" alt="adm_program_ekleme_guncelleme" src="https://github.com/user-attachments/assets/45a2bd5f-7e1f-40a6-8895-5fc6277809e7" />

**Rezervasyon Sayfası:**

<img width="1554" height="1935" alt="adm_rezervasyonlar" src="https://github.com/user-attachments/assets/59169097-e488-46b6-b213-69d5563aaa9e" />

**Yorumlar Sayfası:**

<img width="1554" height="2375" alt="adm_yorumlar" src="https://github.com/user-attachments/assets/9b689909-1b3d-405a-9be1-7b8525edcc0d" />

**Site Ayarları Sayfası:**

<img width="1554" height="927" alt="adm_ayarlar" src="https://github.com/user-attachments/assets/a841530a-8380-4625-8242-9f28c133a6de" />

</details>

<br>

---

## 🚀 Kurulum

### Gereksinimler
* **.NET SDK 10.0** veya üstü
* **MongoDB** (yerel kurulum veya MongoDB Atlas)
* **MongoDB Compass** (opsiyonel — veriyi görsel olarak incelemek için)
* **Visual Studio 2022** veya **VS Code + C# Dev Kit**
* **SMTP Hesabı** (rezervasyon bildirim e-postaları için — Gmail App Password önerilir)

### Kurulum Adımları

1. **Repository'yi Klonlayın**
```bash
    git clone https://github.com/berdanaksoy/Travelin.git
    cd Travelin
```

2. **MongoDB Bağlantısını Ayarlayın**
    `appsettings.json` içerisindeki MongoDB bağlantı ayarlarını güncelleyin:
```json
    "DatabaseSettingsKey": {
      "ConnectionString": "mongodb://localhost:27017",
      "DatabaseName": "TravelinDb"
      ...
    }
```
    > Yerel bir MongoDB kullanıyorsanız varsayılan adres yeterlidir. MongoDB Atlas kullanıyorsanız kendi bağlantı dizenizi girin.

3. **SMTP Yapılandırması** ⚠️
    Rezervasyon bildirim e-postaları için `appsettings.json` içindeki SMTP ayarlarını **kendi bilgilerinizle** doldurun:
```json
    "EmailSettings": {
      "SenderEmail": "KENDI_EMAILINIZ@gmail.com",
      "AppPassword": "GMAIL_APP_PASSWORD"
    }
```
    > **Gmail App Password nasıl alınır?**
    > Google Hesabınız → Güvenlik → 2 Adımlı Doğrulama → Uygulama Şifreleri → "Posta" için şifre oluşturun.
    >
    > ⚠️ `AppPassword` alanını gerçek şifrenizle doldurun; repoya asla gerçek şifre ile push etmeyin.

4. **Projeyi Çalıştırın**
```bash
    dotnet run
```
    Uygulama ilk açılışta veritabanı boşsa **otomatik olarak** örnek verilerle doldurulur — kategoriler, turlar, tur programları, yorumlar ve rezervasyonlar hazır gelir. Ayrı bir script çalıştırmaya veya elle veri girmeye gerek yoktur.

    > 💡 Veriyi görsel olarak incelemek için **MongoDB Compass** ile bağlantı dizenizi kullanarak veritabanına bağlanabilirsiniz.

---

## 🔧 Kullanım

### Ziyaretçi Tarafı
* **Tur Keşfi:** Anasayfadan veya tur listesinden turları filtreleyerek (arama, ülke, kategori, tarih, sıralama) inceleyin.
* **Rezervasyon:** Tur detayından **Rezervasyon Yap** ile kişi sayısı ve iletişim bilgilerinizi girerek rezervasyon oluşturun.
* **Yorum:** Tur detayından puan ve yorum bırakın (yönetici onayı sonrası yayınlanır).
* **Dil Değişimi:** Üst menüden arayüz dilini beş dil arasından seçin.

### Yönetim Paneli
* **Turlar & Kategoriler:** Tur ve kategori ekleme, düzenleme, silme.
* **Tur Programı:** Her tur için gün gün program tanımlama.
* **Rezervasyonlar:** Rezervasyonları filtreleyip onaylama/iptal etme (müşteriye otomatik e-posta gider), Excel/PDF olarak dışa aktarma.
* **Yorumlar:** Ziyaretçi yorumlarını onaylama veya reddetme.
* **Site Ayarları:** İletişim bilgileri, sosyal medya bağlantıları ve tanıtım videosunu yönetme.

---

## 🤝 Katkıda Bulunma

Travelin'e katkılarınızı memnuniyetle karşılıyoruz!

### Nasıl Katkı Sağlanır
1. Fork alın
2. Yeni branch oluşturun (`git checkout -b feature/yeni-ozellik`)
3. Geliştirme yapın
4. Test edin
5. Commit atın (`git commit -m 'feat: yeni özellik eklendi'`)
6. Push edin (`git push origin feature/yeni-ozellik`)
7. Pull Request açın
