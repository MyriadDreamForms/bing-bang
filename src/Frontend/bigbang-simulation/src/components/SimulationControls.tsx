import React, { useState } from 'react';
import type { BigBangRequestDto } from '../types/api';

interface SimulationControlsProps {
  onSimulate: (params: BigBangRequestDto) => void;
  isLoading: boolean;
  onGenerateSeed: () => void;
}

const SimulationControls: React.FC<SimulationControlsProps> = ({
  onSimulate,
  isLoading,
  onGenerateSeed
}) => {  const [params, setParams] = useState<BigBangRequestDto>({
    particleCount: 500,
    seed: 0,
    timeStep: 0.1,
    maxTime: 1.0
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSimulate(params);
  };

  const handleInputChange = (field: keyof BigBangRequestDto) => (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {
    const value = e.target.type === 'number' ? parseFloat(e.target.value) : e.target.value;
    setParams(prev => ({ ...prev, [field]: value }));
  };

  return (
    <div className="simulation-controls">
      <h3>🌌 Big Bang Simülasyon Parametreleri</h3>
      
      <form onSubmit={handleSubmit} className="controls-form">
        <div className="form-group">
          <label htmlFor="particleCount">
            ⚛️ Partikül Sayısı
            <span className="param-info">(1-10000)</span>
          </label>
          <input
            id="particleCount"
            type="number"
            min="1"
            max="10000"
            value={params.particleCount}
            onChange={handleInputChange('particleCount')}
            disabled={isLoading}
          />
        </div>        <div className="form-group">
          <label htmlFor="timeStep">
            ⏱️ Zaman Adımı
            <span className="param-info">(0.01-1.0)</span>
          </label>
          <input
            id="timeStep"
            type="number"
            min="0.01"
            max="1.0"
            step="0.01"
            value={params.timeStep}
            onChange={handleInputChange('timeStep')}
            disabled={isLoading}
          />
        </div>

        <div className="form-group">
          <label htmlFor="maxTime">
            ⏰ Maksimum Zaman
            <span className="param-info">(0.1-10.0)</span>
          </label>
          <input
            id="maxTime"
            type="number"
            min="0.1"
            max="10.0"
            step="0.1"
            value={params.maxTime}
            onChange={handleInputChange('maxTime')}
            disabled={isLoading}
          />
        </div>

        <div className="form-group">
          <label htmlFor="seed">
            🎲 Rastgele Seed
            <span className="param-info">(0 = quantum seed)</span>
          </label>
          <div className="seed-input-group">
            <input
              id="seed"
              type="number"
              value={params.seed}
              onChange={handleInputChange('seed')}
              disabled={isLoading}
            />
            <button
              type="button"
              onClick={onGenerateSeed}
              disabled={isLoading}
              className="generate-seed-btn"
              title="Kuantum seed üret"
            >
              ⚛️ Quantum
            </button>
          </div>
        </div>

        <div className="form-actions">
          <button
            type="submit"
            disabled={isLoading}
            className="simulate-btn"
          >
            {isLoading ? '🔄 Simülasyon Çalışıyor...' : '🚀 Big Bang Simülasyonu Başlat'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default SimulationControls;
