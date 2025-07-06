import React from 'react';
import type { BigBangSimulationResult } from '../types/api';

interface SimulationResultsProps {
  result: BigBangSimulationResult | null;
  error: string | null;
}

const SimulationResults: React.FC<SimulationResultsProps> = ({ result, error }) => {
  if (error) {
    return (
      <div className="simulation-results error">
        <h3>❌ Simülasyon Hatası</h3>
        <p className="error-message">{error}</p>
      </div>
    );
  }

  if (!result) {
    return (
      <div className="simulation-results placeholder">
        <h3>🌌 Simülasyon Sonuçları</h3>
        <p>Simülasyon sonuçları burada görüntülenecek...</p>
      </div>
    );
  }

  const { particles, universeConstants, metadata } = result;
  
  // Güvenlik kontrolü
  if (!particles || !Array.isArray(particles) || particles.length === 0) {
    return (
      <div className="simulation-results error">
        <h3>⚠️ Uyarı</h3>
        <p>Simülasyon sonucunda partikül verisi bulunamadı.</p>
      </div>
    );
  }
  
  // Partikül türlerine göre grupla
  const particleTypes = particles.reduce((acc, particle) => {
    acc[particle.type] = (acc[particle.type] || 0) + 1;
    return acc;
  }, {} as Record<string, number>);
  // Enerji istatistikleri
  const energies = particles.map(p => p.energy).filter(e => e != null);
  const totalEnergy = energies.reduce((sum, e) => sum + e, 0);
  const avgEnergy = energies.length > 0 ? totalEnergy / energies.length : 0;
  const maxEnergy = energies.length > 0 ? Math.max(...energies) : 0;
  const minEnergy = energies.length > 0 ? Math.min(...energies) : 0;

  // Kütle istatistikleri
  const masses = particles.map(p => p.mass).filter(m => m != null);
  const totalMass = masses.reduce((sum, m) => sum + m, 0);
  const avgMass = masses.length > 0 ? totalMass / masses.length : 0;

  return (
    <div className="simulation-results">
      <h3>✅ Simülasyon Tamamlandı</h3>
        {/* Genel Bilgiler */}
      <div className="result-section">
        <h4>📊 Genel İstatistikler</h4>
        <div className="stats-grid">
          <div className="stat-item">
            <span className="stat-label">⚛️ Toplam Partikül:</span>
            <span className="stat-value">{particles.length.toLocaleString()}</span>
          </div>          <div className="stat-item">
            <span className="stat-label">⏱️ Hesaplama Süresi:</span>
            <span className="stat-value">{metadata?.computationTime?.toFixed(2) || '0.00'} ms</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">🎲 Quantum Seed:</span>
            <span className="stat-value">{result.quantumSeed || 'N/A'}</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">🕐 Zaman Damgası:</span>
            <span className="stat-value">{metadata?.timestamp ? new Date(metadata.timestamp).toLocaleTimeString() : 'N/A'}</span>
          </div>
        </div>
      </div>

      {/* Partikül Türleri */}
      <div className="result-section">
        <h4>🔬 Partikül Türleri</h4>
        <div className="particle-types">
          {Object.entries(particleTypes).map(([type, count]) => (
            <div key={type} className="particle-type">
              <span className="type-name">{type}</span>
              <span className="type-count">{count}</span>              <span className="type-percentage">
                ({particles.length > 0 ? ((count / particles.length) * 100).toFixed(1) : '0.0'}%)
              </span>
            </div>
          ))}
        </div>
      </div>

      {/* Enerji İstatistikleri */}
      <div className="result-section">
        <h4>⚡ Enerji Analizi</h4>
        <div className="stats-grid">
          <div className="stat-item">
            <span className="stat-label">Toplam Enerji:</span>
            <span className="stat-value">{totalEnergy.toExponential(2)} J</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">Ortalama Enerji:</span>
            <span className="stat-value">{avgEnergy.toExponential(2)} J</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">Maksimum Enerji:</span>
            <span className="stat-value">{maxEnergy.toExponential(2)} J</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">Minimum Enerji:</span>
            <span className="stat-value">{minEnergy.toExponential(2)} J</span>
          </div>
        </div>
      </div>

      {/* Kütle İstatistikleri */}
      <div className="result-section">
        <h4>⚖️ Kütle Analizi</h4>
        <div className="stats-grid">
          <div className="stat-item">
            <span className="stat-label">Toplam Kütle:</span>
            <span className="stat-value">{totalMass.toExponential(2)} kg</span>
          </div>
          <div className="stat-item">
            <span className="stat-label">Ortalama Kütle:</span>
            <span className="stat-value">{avgMass.toExponential(2)} kg</span>
          </div>
        </div>
      </div>

      {/* Evren Sabitleri */}
      <div className="result-section">
        <h4>🌌 Evren Sabitleri</h4>
        <div className="constants-grid">
          <div className="constant-item">
            <span className="constant-name">Işık Hızı (c):</span>
            <span className="constant-value">{universeConstants.speedOfLight.toExponential(3)} m/s</span>
          </div>
          <div className="constant-item">
            <span className="constant-name">Planck Sabiti (h):</span>
            <span className="constant-value">{universeConstants.planckConstant.toExponential(3)} J⋅s</span>
          </div>
          <div className="constant-item">
            <span className="constant-name">Gravitasyonel Sabit (G):</span>
            <span className="constant-value">{universeConstants.gravitationalConstant.toExponential(3)} m³⋅kg⁻¹⋅s⁻²</span>
          </div>
          <div className="constant-item">
            <span className="constant-name">İnce Yapı Sabiti (α):</span>
            <span className="constant-value">{universeConstants.fineStructureConstant.toFixed(6)}</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SimulationResults;
