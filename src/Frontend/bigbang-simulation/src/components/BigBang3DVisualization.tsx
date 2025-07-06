import React, { useRef, useState, Suspense } from 'react';
import { Canvas, useFrame } from '@react-three/fiber';
import { OrbitControls, Stars } from '@react-three/drei';
import * as THREE from 'three';
import type { BigBangSimulationResult } from '../types/api';

// Loading placeholder
function LoadingFallback() {
  return (
    <div style={{
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      height: '100%',
      color: '#cccccc',
      fontSize: '1.2rem'
    }}>
      <div>
        <div>🌌 3D Evren Yükleniyor...</div>
        <div style={{ fontSize: '0.9rem', marginTop: '10px', opacity: 0.7 }}>
          Three.js motoru başlatılıyor
        </div>
      </div>
    </div>
  );
}

// Big Bang patlama animasyonu
function BigBangExplosion({ isActive }: { isActive: boolean }) {
  const groupRef = useRef<THREE.Group>(null);
  const [particles, setParticles] = useState<Array<{
    position: [number, number, number];
    velocity: [number, number, number];
    color: string;
    size: number;
    energy: number;
  }>>([]);

  // Big Bang patlaması başlatıldığında partikülleri oluştur
  React.useEffect(() => {
    if (isActive) {
      const newParticles = Array.from({ length: 1000 }, () => {
        const theta = Math.random() * Math.PI * 2;
        const phi = Math.random() * Math.PI;
        const speed = 2 + Math.random() * 8;
        
        return {
          position: [0, 0, 0] as [number, number, number],
          velocity: [
            Math.sin(phi) * Math.cos(theta) * speed,
            Math.sin(phi) * Math.sin(theta) * speed,
            Math.cos(phi) * speed
          ] as [number, number, number],
          color: ['#ff4444', '#4444ff', '#44ff44', '#ffff44', '#ff44ff', '#44ffff'][Math.floor(Math.random() * 6)],
          size: 0.02 + Math.random() * 0.05,
          energy: Math.random()
        };
      });
      setParticles(newParticles);
    }
  }, [isActive]);
  // Animasyon döngüsü
  useFrame((_, delta) => {
    if (!isActive || !groupRef.current) return;

    // Partikülleri hareket ettir ve enerji kaybettir
    setParticles(prev => prev.map(particle => ({
      ...particle,
      position: [
        particle.position[0] + particle.velocity[0] * delta,
        particle.position[1] + particle.velocity[1] * delta,
        particle.position[2] + particle.velocity[2] * delta
      ] as [number, number, number],
      velocity: [
        particle.velocity[0] * 0.998, // Sürtünme etkisi
        particle.velocity[1] * 0.998,
        particle.velocity[2] * 0.998
      ] as [number, number, number],
      energy: particle.energy * 0.999
    })).filter(p => p.energy > 0.1)); // Düşük enerjili partikülleri kaldır
  });

  if (!isActive || particles.length === 0) return null;

  return (
    <group ref={groupRef}>
      {particles.map((particle, index) => (
        <mesh key={index} position={particle.position}>
          <sphereGeometry args={[particle.size, 8, 8]} />
          <meshBasicMaterial 
            color={particle.color} 
            transparent 
            opacity={particle.energy}
          />
        </mesh>
      ))}
    </group>
  );
}

// Gerçekçi partikül sistemi
function RealParticles({ data }: { data: BigBangSimulationResult | null }) {
  const groupRef = useRef<THREE.Group>(null);
  const [time, setTime] = useState(0);
  
  if (!data || !data.particles) {
    return null;
  }

  const particleCount = Math.min(data.particles.length, 800); // Performans için sınırla

  useFrame((_, delta) => {
    setTime(t => t + delta);
    if (groupRef.current) {
      groupRef.current.rotation.y += delta * 0.1;
    }
  });

  return (
    <group ref={groupRef}>
      {Array.from({ length: particleCount }, (_, index) => {
        const particle = data.particles[index] || { 
          type: 'Unknown', 
          energy: 1, 
          position: { x: 0, y: 0, z: 0 },
          color: '#ffffff'
        };
        
        // Partikül tipine göre renk
        const getParticleColor = (type: string) => {
          switch (type) {
            case 'Quark': return '#ff6b35';
            case 'Lepton': return '#4dabf7';
            case 'Boson': return '#51cf66';
            default: return '#f06292';
          }
        };

        // Enerji bazlı boyut
        const size = Math.max(0.02, Math.min(0.08, particle.energy / 10000));
        
        // Spiral genişleme hareketi
        const angle = index * 0.1 + time * 0.5;
        const radius = Math.sin(time * 0.3 + index * 0.01) * 3 + 5;
        const height = Math.cos(time * 0.2 + index * 0.05) * 2;
        
        const x = Math.cos(angle) * radius;
        const y = height;
        const z = Math.sin(angle) * radius;

        return (
          <group key={index}>
            <mesh position={[x, y, z]}>
              <sphereGeometry args={[size, 8, 8]} />
              <meshStandardMaterial 
                color={particle.color || getParticleColor(particle.type)}
                emissive={particle.color || getParticleColor(particle.type)}
                emissiveIntensity={0.3}
                transparent
                opacity={0.8}
              />
            </mesh>
            {/* Partikül izi efekti */}
            <mesh position={[x * 0.9, y * 0.9, z * 0.9]}>
              <sphereGeometry args={[size * 0.5, 6, 6]} />
              <meshBasicMaterial 
                color={particle.color || getParticleColor(particle.type)}
                transparent
                opacity={0.3}
              />
            </mesh>
          </group>
        );
      })}
    </group>
  );
}

interface BigBang3DVisualizationProps {
  data: BigBangSimulationResult | null;
}

const BigBang3DVisualization: React.FC<BigBang3DVisualizationProps> = ({ data }) => {
  const [explosionActive, setExplosionActive] = useState(false);
  const [showParticles, setShowParticles] = useState(true);
  
  // Big Bang patlaması başlat
  const triggerBigBang = () => {
    setExplosionActive(true);
    setShowParticles(false);
    setTimeout(() => {
      setExplosionActive(false);
      setShowParticles(true);
    }, 5000); // 5 saniye patlama
  };
  
  // Veri kontrolü
  if (!data) {
    return (
      <div className="visualization-container" style={{ height: '100%' }}>
        <div className="visualization-header">
          <h3>🌌 Big Bang 3D Görselleştirme</h3>
          <p>Simülasyon verisi bekleniyor...</p>
        </div>
        <div style={{ 
          display: 'flex', 
          alignItems: 'center', 
          justifyContent: 'center', 
          height: '400px',
          color: '#888',
          fontSize: '1.1rem'
        }}>
          📊 Görselleştirme için simülasyon çalıştırın
        </div>
      </div>
    );
  }
  
  return (
    <div className="visualization-container" style={{ height: '100%' }}>
      <div className="visualization-header">
        <h3>🌌 Big Bang 3D Görselleştirme</h3>
        <p>Kuantum partiküllerin 3D evren simülasyonu</p>
      </div>      {/* Kontroller */}
      <div className="visualization-controls-top">
        <div className="control-group">
          <button 
            onClick={triggerBigBang}
            disabled={explosionActive}
            style={{
              background: explosionActive ? '#666' : 'linear-gradient(45deg, #ff6b35, #f7931e)',
              color: 'white',
              border: 'none',
              padding: '10px 20px',
              borderRadius: '8px',
              fontSize: '16px',
              fontWeight: 'bold',
              cursor: explosionActive ? 'not-allowed' : 'pointer',
              boxShadow: explosionActive ? 'none' : '0 4px 15px rgba(255, 107, 53, 0.4)',
              transition: 'all 0.3s ease'
            }}
          >
            {explosionActive ? '💥 Patlama Devam Ediyor...' : '🌟 Big Bang Başlat!'}
          </button>
        </div>
        
        <div className="control-group">
          <span>📊 Partikül: {data?.particles?.length || 0}</span>
        </div>
        
        <div className="control-group">
          <span>⚡ Enerji: {data?.totalEnergy?.toFixed(0) || '0'} GeV</span>
        </div>
      </div>

      {/* 3D Canvas */}
      <div className="canvas-container">
        <Suspense fallback={<LoadingFallback />}>
          <Canvas
            camera={{ position: [5, 5, 5], fov: 75 }}
            gl={{ antialias: true, preserveDrawingBuffer: true }}
            style={{ background: 'transparent' }}
          >
            {/* Işıklandırma */}
            <ambientLight intensity={0.4} />
            <pointLight position={[10, 10, 10]} intensity={1} />
            <pointLight position={[-10, -10, -10]} intensity={0.5} color="#4444ff" />
            
            {/* Arka plan yıldızları */}
            <Stars 
              radius={30} 
              depth={50} 
              count={500} 
              factor={2} 
              saturation={0.5} 
              fade 
            />
              {/* Big Bang patlaması veya partikül sistemi */}
            {explosionActive && <BigBangExplosion isActive={explosionActive} />}
            {showParticles && !explosionActive && <RealParticles data={data} />}
            
            {/* Merkez patlama efekti */}
            <mesh position={[0, 0, 0]}>
              <sphereGeometry args={[0.1, 16, 16]} />
              <meshBasicMaterial 
                color="#ffffff" 
                transparent 
                opacity={explosionActive ? 1 : 0.3}
              />
            </mesh>
            
            {/* Kamera kontrolleri */}
            <OrbitControls
              enablePan={true}
              enableZoom={true}
              enableRotate={true}
              autoRotate={false}
              maxDistance={50}
              minDistance={2}
            />
          </Canvas>
        </Suspense>
      </div>

      {/* Bilgi paneli */}
      <div className="visualization-info">        <div className="stats-panel">
          <h4>📊 Simülasyon İstatistikleri</h4>
          <div>Toplam Partikül: {data?.particles?.length?.toLocaleString() || '0'}</div>
          <div>Ortalama Enerji: {data?.totalEnergy?.toFixed(2) || '0.00'} GeV</div>
          <div>Toplam Kütle: {data?.totalMass?.toFixed(2) || '0.00'} kg</div>
          <div>Simülasyon Zamanı: {data?.simulationTime?.toFixed(3) || '0.000'} ms</div>
        </div>

        <div className="controls-help">
          <h4>🎮 Kontroller</h4>
          <div>🖱️ Sol Tık + Sürükle: Döndür</div>
          <div>🔍 Tekerlek: Yakınlaştır/Uzaklaştır</div>
          <div>⚡ Sağ Tık + Sürükle: Kaydır</div>
          <div>🧪 Test küpü 3D render'ın çalıştığını doğrular</div>
        </div>
      </div>

      {/* Renk açıklaması */}
      <div className="visualization-controls">
        <div className="legend">
          <h4>🎨 Partikül Türleri</h4>
          <div className="legend-item">
            <div className="legend-color" style={{ background: '#ff6b35' }}></div>
            <span>Quark (Turuncu)</span>
          </div>
          <div className="legend-item">
            <div className="legend-color" style={{ background: '#4dabf7' }}></div>
            <span>Lepton (Mavi)</span>
          </div>
          <div className="legend-item">
            <div className="legend-color" style={{ background: '#51cf66' }}></div>
            <span>Boson (Yeşil)</span>
          </div>
          <div className="legend-item">
            <div className="legend-color" style={{ background: '#f06292' }}></div>
            <span>Diğer (Pembe)</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default BigBang3DVisualization;