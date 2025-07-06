# Quantum Powered Big Bang Simulation 🚀

Bu proje, Big Bang (Büyük Patlama) konseptini temel alarak kuantum algoritmalarıyla üretilen rastgele verileri C# backend üzerinden alıp, React frontend üzerinde genişleme ve partikül animasyonu olarak görselleştirir. Temel amaç: Kuantum hesaplamalarını görselleştirmek ve interaktif simülasyon oluşturmak.

---

## 📌 Proje Bileşenleri

### 1️⃣ Q# (Quantum Algorithm Layer)

- Kuantum hesaplamaları ile rastgelelik ve kaos üretilir.
- Kullanılabilecek Q# algoritmaları:
  - Rastgele enerji seviyeleri
  - Partikül konumları için rastgele dağılım
  - Süperpozisyon temelli varyasyonlar
- Microsoft Quantum Development Kit (QDK) kullanılarak geliştirilir.
- Lokal olarak QuantumSimulator ile test edilebilir.

### 2️⃣ C# ASP.NET Core Backend

- Q# algoritmalarını çağıran ve hesaplama sonuçlarını frontend'e aktaran API sağlar.
- Katmanlı Clean Architecture mimarisi uygulanır.
- Backend her animasyon başlatıldığında yeni kuantum tabanlı veri üretir.
- REST API mimarisi ile frontend'e veri sağlar.

### 3️⃣ React Frontend

- Backend API üzerinden gelen kuantum verilerini alır.
- Gelen verilerle Big Bang animasyonunu başlatır.
- Kullanılan animasyon teknolojileri:
  - **Three.js / react-three-fiber**: 3D partikül simülasyonu
  - **Canvas API**: 2D genişleme efektleri

### 4️⃣ Veri Akışı (Data Flow)

- Kullanıcı animasyonu başlatır.
- React frontend, backend'e API isteği gönderir.
- Backend, Q# algoritmasını çalıştırır.
- Q# algoritması partikül pozisyonları ve enerjileri gibi veriler üretir.
- Backend, JSON formatında veriyi frontend'e iletir.
- Frontend, gelen veriye göre Big Bang animasyonunu üretir.

---

## 🎯 Mimari Yapı

Proje Layered Clean Architecture prensibine göre inşa edilir.

### Genel Klasör Yapısı:

BigBangSimulation/
│
├── src/
│ ├── Quantum/ --> Q# Algoritmaları
│ │
│ ├── Backend/ --> ASP.NET Core Web API
│ │ ├── Application/ --> Business Logic (Kuantum çağrıları burada)
│ │ ├── Infrastructure/ --> Q# entegrasyon katmanı
│ │ ├── Controllers/ --> API Endpoint'leri
│ │ └── Program.cs --> Entry Point
│ │
│ └── Frontend/ --> React (Vite + Three.js)
│ ├── components/
│ ├── services/ --> API çağrıları
│ ├── scenes/ --> Animasyon sahneleri
│ └── App.jsx
│
└── README.md


### Katmanların Görevleri:

#### Quantum Katmanı (Q#)
- Domain katmanı gibi davranır.
- Kuantum hesaplamalarını yapar.
- Backend'e kütüphane (DLL) olarak referans verilir.

#### Backend Katmanı (ASP.NET Core)

- Application Layer:
  - Kuantum çağrılarını soyutlar.
  - İş mantığı içerir.
  - Örnek interface: `IBigBangSimulationService`

- Infrastructure Layer:
  - Q# algoritmalarını çalıştırır.
  - QuantumSimulator ile entegrasyon sağlar.

- API (Controller) Layer:
  - Frontend ile konuşur.
  - REST API sağlar. Örnek: `/api/quantum/bigbang`

#### Frontend Katmanı (React + Vite)

- API'den veriyi alır.
- Three.js/react-three-fiber ile animasyonları çizer.
- Kullanıcı etkileşimlerini yönetir.

---

## 📊 Örnek Akış

1. Kullanıcı "Big Bang Başlat" butonuna tıklar.
2. React → `/api/quantum/bigbang` endpoint'ine istek gönderir.
3. Backend → QuantumSimulator üzerinden Q# algoritmasını çalıştırır.
4. Q# algoritması:
    - `N` adet partikül için:
        - Pozisyon (x, y, z)
        - Enerji seviyesi
        - Renk parametresi
      hesaplar.
5. Backend → JSON verisi olarak frontend'e iletir.
6. Frontend → Genişleme ve partikül animasyonlarını başlatır.

---

## 🚀 Kullanım Alanları

- Kuantum fiziği ve kozmoloji eğitimleri
- Görselleştirme ve interaktif öğrenim araçları
- Bilim temalı sanat ve medya projeleri
- Deneysel oyun ve AR/VR uygulamaları

---

## 🔧 Gereksinimler

- .NET 9 SDK
- Microsoft Quantum Development Kit (QDK)
- ASP.NET Core Web API
- React (Vite + react-three-fiber)
- Three.js / Canvas API
- (Opsiyonel) Howler.js (ses efektleri için)

---

## 🧪 Geliştirme Notları

- Her animasyon çağrısı eşsiz bir evren yaratır.
- API katmanı genişlemeye uygun modüler yapıdadır.
- İleride Azure Quantum entegrasyonu yapılabilir.
- WebSocket veya SignalR ile canlı animasyon güncellemeleri mümkündür.
