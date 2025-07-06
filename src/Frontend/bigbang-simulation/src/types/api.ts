// Big Bang simülasyon API tipleri

export interface BigBangRequestDto {
  particleCount: number;
  seed: number;
  timeStep: number;
  maxTime: number;
}

export interface Particle {
  id: string;
  type: string;
  energy: number;
  position: {
    x: number;
    y: number;
    z: number;
  };
  velocity: {
    x: number;
    y: number;
    z: number;
  };
  mass: number;
  charge: number;
  color: string;
}

export interface UniverseConstants {
  planckConstant: number;
  speedOfLight: number;
  gravitationalConstant: number;
  hubbleConstant: number;
  darkMatterDensity: number;
  darkEnergyDensity: number;
  cosmologicalConstant: number;
  fineStructureConstant: number;
}

export interface BigBangSimulationResult {
  particles: Particle[];
  totalEnergy: number;
  totalMass: number;
  universeConstants: UniverseConstants;
  simulationTime: number;
  quantumSeed: number;
  particleTypeDistribution: Record<string, number>;
  metadata: {
    computationTime: number;
    timestamp: string;
    version: string;
  };
}

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  error?: string;
  timestamp: string;
}

export interface HealthCheckResult {
  status: string;
  timestamp: string;
  services: {
    quantum: boolean;
    database: boolean;
    api: boolean;
  };
}

export interface UniverseConstantsDto {
  planckConstant: number;
  speedOfLight: number;
  gravitationalConstant: number;
  hubbleConstant: number;
  darkMatterDensity: number;
  darkEnergyDensity: number;
  cosmologicalConstant: number;
  fineStructureConstant: number;
}

export interface QuantumSeedResponse {
  seed: number;
  timestamp: string;
  entropy: number;
}

export interface HealthCheckResponse {
  status: string;
  timestamp: string;
  services: {
    quantum: boolean;
    database: boolean;
    api: boolean;
  };
}
