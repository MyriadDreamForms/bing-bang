// Mock data test için
import type { BigBangSimulationResult } from '../types/api';

export const mockSimulationData: BigBangSimulationResult = {
  particles: Array.from({ length: 100 }, (_, i) => ({
    id: `particle-${i}`,
    type: ['Quark', 'Lepton', 'Boson', 'Unknown'][i % 4],
    energy: Math.random() * 1000,
    position: {
      x: (Math.random() - 0.5) * 10,
      y: (Math.random() - 0.5) * 10,
      z: (Math.random() - 0.5) * 10
    },
    velocity: {
      x: (Math.random() - 0.5) * 2,
      y: (Math.random() - 0.5) * 2,
      z: (Math.random() - 0.5) * 2
    },
    mass: Math.random() * 10,
    charge: (Math.random() - 0.5) * 2,
    color: ['#ff6b35', '#4dabf7', '#51cf66', '#f06292'][i % 4]
  })),
  totalEnergy: 50000,
  totalMass: 500,
  universeConstants: {
    planckConstant: 6.626e-34,
    speedOfLight: 299792458,
    gravitationalConstant: 6.674e-11,
    hubbleConstant: 70,
    darkMatterDensity: 0.26,
    darkEnergyDensity: 0.69,
    cosmologicalConstant: 1.1e-52,
    fineStructureConstant: 0.007297
  },
  simulationTime: 123.456,
  quantumSeed: 42,
  particleTypeDistribution: {
    'Quark': 25,
    'Lepton': 25,
    'Boson': 25,
    'Unknown': 25
  },
  metadata: {
    computationTime: 123.456,
    timestamp: new Date().toISOString(),
    version: '1.0.0'
  }
};
