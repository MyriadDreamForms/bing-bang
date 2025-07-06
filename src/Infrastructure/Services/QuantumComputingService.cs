using BigBangSimulation.Application.Interfaces;
using BigBangSimulation.Quantum;

namespace BigBangSimulation.Infrastructure.Services
{
    public class QuantumComputingService : IQuantumComputingService, IDisposable
    {
        private readonly QuantumBigBangSimulator _quantumSimulator;
        private readonly ISimulationLogger _logger;
        private bool _disposed = false;

        public QuantumComputingService(ISimulationLogger logger)
        {
            _quantumSimulator = new QuantumBigBangSimulator();
            _logger = logger;
        }

        public async Task<ParticleDto[]> GenerateParticlesAsync(BigBangRequest request)
        {
            try
            {
                _logger.LogQuantumOperation("GenerateParticles", request);
                
                // Parametreleri hazırla
                var parameters = new BigBangParameters
                {
                    ParticleCount = request.ParticleCount,
                    ExplosionRadius = request.ExplosionRadius,
                    EnergyRange = (request.MinEnergy, request.MaxEnergy),
                    MassRange = (request.MinMass, request.MaxMass),
                    VariationCount = request.VariationCount
                };

                var particles = await _quantumSimulator.GenerateBigBangParticlesAsync(parameters);
                
                // ParticleData'yı ParticleDto'ya dönüştür
                return particles.Select(ConvertToParticleDto).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateParticlesAsync", ex);
                throw;
            }
        }

        public async Task<UniverseConstantsDto> GenerateUniverseConstantsAsync()
        {
            try
            {
                _logger.LogQuantumOperation("GenerateUniverseConstants", new { });
                var constants = await _quantumSimulator.GenerateUniverseConstantsAsync();
                
                return new UniverseConstantsDto
                {
                    GravityStrength = constants.GravityStrength,
                    LightSpeed = constants.LightSpeed,
                    PlanckConstant = constants.PlanckConstant,
                    ExpansionRate = constants.ExpansionRate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateUniverseConstantsAsync", ex);
                throw;
            }
        }

        public async Task<double> GenerateRandomDoubleAsync()
        {
            try
            {
                return await _quantumSimulator.GenerateRandomDoubleAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateRandomDoubleAsync", ex);
                throw;
            }
        }

        public async Task<double> GenerateRandomRangeAsync(double min, double max)
        {
            try
            {
                return await _quantumSimulator.GenerateRandomRangeAsync(min, max);
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateRandomRangeAsync", ex);
                throw;
            }
        }

        public async Task<double> GenerateGaussianRandomAsync(double mean, double stdDev)
        {
            try
            {
                return await _quantumSimulator.GenerateGaussianRandomAsync(mean, stdDev);
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateGaussianRandomAsync", ex);
                throw;
            }
        }

        public async Task<int> GenerateQuantumSeedAsync()
        {
            try
            {
                _logger.LogQuantumOperation("GenerateQuantumSeed", new { });
                return await _quantumSimulator.GenerateQuantumSeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateQuantumSeedAsync", ex);
                throw;
            }
        }

        private ParticleDto ConvertToParticleDto(ParticleData particle)
        {
            return new ParticleDto
            {
                Position = new PositionDto
                {
                    X = particle.Position.X,
                    Y = particle.Position.Y,
                    Z = particle.Position.Z
                },
                Velocity = new VelocityDto
                {
                    Vx = particle.Velocity.Vx,
                    Vy = particle.Velocity.Vy,
                    Vz = particle.Velocity.Vz
                },
                Energy = particle.Energy,
                Mass = particle.Mass,
                Color = new ColorDto
                {
                    R = particle.Color.R,
                    G = particle.Color.G,
                    B = particle.Color.B
                }
            };
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _quantumSimulator?.Dispose();
                _disposed = true;
            }
        }
    }
}
