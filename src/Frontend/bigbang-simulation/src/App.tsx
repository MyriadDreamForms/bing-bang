import { useState, useEffect } from 'react';
import SimulationControls from './components/SimulationControls';
import SimulationResults from './components/SimulationResults';
import BigBang3DVisualization from './components/BigBang3DVisualization';
import ErrorBoundary from './components/ErrorBoundary';
import BigBangApiService from './services/apiService';
import { mockSimulationData } from './utils/mockData';
import type { BigBangSimulationResult, BigBangRequestDto } from './types/api';
import './App.css';

function App() {
  const [simulationResult, setSimulationResult] = useState<BigBangSimulationResult | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [apiStatus, setApiStatus] = useState<'checking' | 'connected' | 'disconnected'>('checking');
  const [activeTab, setActiveTab] = useState<'results' | 'visualization'>('visualization'); // 3D görselleştirmeyi varsayılan yap

  // Sayfa yüklendiğinde API durumunu kontrol et
  useEffect(() => {
    console.log('🚀 App başlatılıyor, API durumu kontrol ediliyor...');
    checkApiStatus();
  }, []);

  const checkApiStatus = async () => {
    console.log('📡 API durumu kontrol ediliyor...');
    setApiStatus('checking');
    try {
      const healthResponse = await BigBangApiService.healthCheck();
      console.log('✅ API bağlantısı başarılı:', healthResponse);
      setApiStatus('connected');
    } catch (error) {
      console.error('❌ API bağlantı hatası:', error);
      setApiStatus('disconnected');
    }
  };

  const handleSimulate = async (params: BigBangRequestDto) => {
    console.log('🌌 Big Bang simülasyonu başlatılıyor:', params);
    setIsLoading(true);
    setError(null);
    
    try {
      // Eğer seed 0 ise, önce quantum seed üret
      if (params.seed === 0) {
        console.log('🎲 Quantum seed üretiliyor...');
        const seedResponse = await BigBangApiService.generateQuantumSeed();
        params.seed = seedResponse.seed;
        console.log('✅ Quantum seed üretildi:', seedResponse.seed);
      }
      
      // Big Bang simülasyonunu çalıştır
      console.log('🚀 Big Bang API çağrısı yapılıyor...');
      const result = await BigBangApiService.generateBigBang(params);
      console.log('✅ Big Bang simülasyonu tamamlandı:', result);
      
      setSimulationResult(result);
      setActiveTab('results'); // Sonuçlar sekmesine geç
      
    } catch (error) {
      console.error('❌ Simülasyon hatası:', error);
      const errorMessage = error instanceof Error ? error.message : 'Bilinmeyen bir hata oluştu';
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  const handleGenerateSeed = async () => {
    try {
      const seedResponse = await BigBangApiService.generateQuantumSeed();
      // Seed'i form kontrollerine gönder (bu implementasyon eksik, form state'ini yukarı taşımak gerekir)
      console.log('Generated quantum seed:', seedResponse.seed);
    } catch (error) {
      console.error('Seed üretme hatası:', error);
    }
  };

  // Mock data test fonksiyonu
  const loadMockData = () => {
    console.log('🧪 Mock data yükleniyor...');
    setSimulationResult(mockSimulationData);
    setActiveTab('visualization');
    setError(null);
    console.log('✅ Mock data yüklendi:', mockSimulationData);
  };

  return (
    <ErrorBoundary>
      <div className="app">
        <header className="app-header">
          <h1>🌌 Quantum Big Bang Simulation</h1>
        <p>Kuantum algoritmaları ile Big Bang simülasyonu ve 3D görselleştirme</p>
        
        <div className={`api-status ${apiStatus}`}>
          <span className="status-indicator"></span>
          <span className="status-text">
            {apiStatus === 'checking' && 'API durumu kontrol ediliyor...'}
            {apiStatus === 'connected' && 'API bağlantısı aktif'}
            {apiStatus === 'disconnected' && 'API bağlantısı yok'}
          </span>
          {apiStatus === 'disconnected' && (
            <button onClick={checkApiStatus} className="retry-btn">
              🔄 Yeniden Dene
            </button>
          )}
        </div>
        
        {/* 3D Test Butonu */}
        <div style={{ marginTop: '15px' }}>
          <button 
            onClick={loadMockData}
            style={{
              padding: '10px 20px',
              background: '#4dabf7',
              color: 'white',
              border: 'none',
              borderRadius: '6px',
              cursor: 'pointer',
              fontSize: '0.9rem'
            }}
          >
            🧪 3D Test Verisi Yükle (Mock)
          </button>
        </div>
      </header>

      <main className="app-main">
        <div className="controls-section">
          <SimulationControls
            onSimulate={handleSimulate}
            isLoading={isLoading}
            onGenerateSeed={handleGenerateSeed}
          />
        </div>

        <div className="results-section">
          <div className="tab-container">
            <div className="tab-buttons">
              <button
                className={`tab-button ${activeTab === 'results' ? 'active' : ''}`}
                onClick={() => setActiveTab('results')}
                disabled={isLoading}
              >
                📊 Simülasyon Sonuçları
              </button>
              <button
                className={`tab-button ${activeTab === 'visualization' ? 'active' : ''}`}
                onClick={() => setActiveTab('visualization')}
                disabled={!simulationResult || isLoading}
              >
                🌌 3D Görselleştirme
              </button>
            </div>

            <div className="tab-content" style={{ position: 'relative' }}>
              {/* Loading overlay */}
              {isLoading && (
                <div className="loading-overlay">
                  <div className="loading-content">
                    <div className="loading-spinner"></div>
                    <h3>🌌 Big Bang Simülasyonu Çalışıyor</h3>
                    <p>Kuantum partiküller oluşturuluyor...</p>
                  </div>
                </div>
              )}
              
              <div style={{ display: activeTab === 'results' ? 'block' : 'none' }}>
                <SimulationResults result={simulationResult} error={error} />
              </div>
              
              <div style={{ display: activeTab === 'visualization' ? 'block' : 'none' }}>
                {simulationResult && (
                  <BigBang3DVisualization
                    data={simulationResult}
                  />
                )}
              </div>
            </div>
          </div>
        </div>
      </main>

      <footer className="app-footer">
        <p>
          Powered by <strong>Q# Quantum Computing</strong> + <strong>React</strong> + <strong>Three.js</strong>
        </p>
        <p>
          Big Bang simülasyonu kuantum rastgelelik algoritmaları kullanılarak gerçekleştirilir.
        </p>
      </footer>
      </div>
    </ErrorBoundary>
  );
}

export default App;
