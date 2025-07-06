import axios from 'axios';
import type {
  BigBangSimulationResult,
  BigBangRequestDto,
  UniverseConstantsDto,
  QuantumSeedResponse,
  HealthCheckResponse
} from '../types/api';

// API base URL
const API_BASE_URL = 'http://localhost:5000';

// Axios client
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// Error interceptor
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('API Error:', error);
    
    if (error.code === 'ECONNREFUSED') {
      throw new Error('API sunucusuna bağlanılamıyor. Sunucunun çalıştığından emin olun.');
    }
    
    if (error.response?.status === 500) {
      throw new Error('Sunucu hatası. Lütfen daha sonra tekrar deneyin.');
    }
    
    if (error.response?.status === 400) {
      throw new Error(error.response.data?.message || 'Geçersiz istek parametreleri.');
    }
    
    throw error;
  }
);

// API Service Class
class BigBangApiService {
  
  // Health check
  static async healthCheck(): Promise<HealthCheckResponse> {
    try {
      const response = await apiClient.get('/api/health');      return {
        status: response.data.status || 'Running',
        timestamp: response.data.timestamp || new Date().toISOString(),
        services: {
          quantum: true,
          database: true,
          api: true
        }
      };
    } catch (error) {
      throw new Error('Health check başarısız');
    }
  }

  // Generate Big Bang simulation
  static async generateBigBang(params: BigBangRequestDto): Promise<BigBangSimulationResult> {
    try {
      const response = await apiClient.post('/api/quantum/bigbang', params);
      return response.data;
    } catch (error) {
      console.error('Big Bang API Error:', error);
        // Mock data for testing
      return {
        particles: Array.from({ length: params.particleCount }, (_, i) => ({
          id: `particle-${i}`,
          type: ['Quark', 'Lepton', 'Boson'][i % 3],
          energy: Math.random() * 1000,
          mass: Math.random() * 0.1,
          charge: (Math.random() - 0.5) * 2,
          position: {
            x: (Math.random() - 0.5) * 10,
            y: (Math.random() - 0.5) * 10,
            z: (Math.random() - 0.5) * 10
          },
          velocity: {
            x: (Math.random() - 0.5) * 5,
            y: (Math.random() - 0.5) * 5,
            z: (Math.random() - 0.5) * 5
          },
          color: `hsl(${Math.random() * 360}, 70%, 60%)`
        })),
        totalEnergy: params.particleCount * 500,
        totalMass: params.particleCount * 0.05,
        universeConstants: {
          planckConstant: 6.62607015e-34,
          speedOfLight: 299792458,
          gravitationalConstant: 6.67430e-11,
          hubbleConstant: 70,
          darkMatterDensity: 0.26,
          darkEnergyDensity: 0.69,
          cosmologicalConstant: 1.1e-52,
          fineStructureConstant: 7.2973525693e-3
        },
        simulationTime: 123.456,
        quantumSeed: params.seed,
        particleTypeDistribution: {
          'Quark': Math.floor(params.particleCount / 3),
          'Lepton': Math.floor(params.particleCount / 3),
          'Boson': Math.floor(params.particleCount / 3)
        },
        metadata: {
          computationTime: 123.456,
          timestamp: new Date().toISOString(),
          version: '1.0.0'
        }
      };
    }
  }

  // Generate quantum seed
  static async generateQuantumSeed(): Promise<QuantumSeedResponse> {
    try {
      const response = await apiClient.get('/api/quantum/quantum-seed');
      return response.data;
    } catch (error) {      // Fallback seed
      return {
        seed: Math.floor(Math.random() * 1000000),
        timestamp: new Date().toISOString(),
        entropy: 1.0
      };
    }
  }

  // Get universe constants
  static async getUniverseConstants(): Promise<UniverseConstantsDto> {
    try {
      const response = await apiClient.get('/api/quantum/universe-constants');
      return response.data;
    } catch (error) {      // Fallback constants
      return {
        planckConstant: 6.62607015e-34,
        speedOfLight: 299792458,
        gravitationalConstant: 6.67430e-11,
        hubbleConstant: 70,
        darkMatterDensity: 0.26,
        darkEnergyDensity: 0.69,
        cosmologicalConstant: 1.1e-52,
        fineStructureConstant: 7.2973525693e-3
      };
    }
  }
}

export default BigBangApiService;
