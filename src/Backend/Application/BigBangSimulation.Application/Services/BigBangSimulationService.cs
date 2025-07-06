using BigBangSimulation.Application.Interfaces;
using BigBangSimulation.Application.Models;
using System.Diagnostics;

namespace BigBangSimulation.Application.Services
{
    public class BigBangSimulationService : IBigBangSimulationService
    {
        private readonly IQuantumComputingService _quantumService;
        private readonly ISimulationLogger _logger;
        private readonly ISimulationCache _cache;
        private readonly ISimulationConfiguration _config;

        public BigBangSimulationService(
            IQuantumComputingService quantumService,
            ISimulationLogger logger,
            ISimulationCache cache,
            ISimulationConfiguration config)
        {
            _quantumService = quantumService;
            _logger = logger;
            _cache = cache;
            _config = config;
        }

        public async Task<BigBangSimulationResult> GenerateBigBangAsync(BigBangRequest request)
        {
            _logger.LogSimulationStart(request);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Parametreleri doğrula
                ValidateRequest(request);

                // Cache kontrol et
                var cacheKey = GenerateCacheKey(request);
                if (_config.EnableCaching)
                {
                    var cachedResult = await _cache.GetAsync<BigBangSimulationResult>(cacheKey);
                    if (cachedResult != null)
                    {
                        _logger.LogSimulationComplete(cachedResult);
                        return cachedResult;
                    }
                }

                // Kuantum seed üret
                var quantumSeed = await _quantumService.GenerateQuantumSeedAsync();
                
                // Evren sabitleri üret
                var universeConstants = await _quantumService.GenerateUniverseConstantsAsync();

                // Request'e seed'i ata
                request.RandomSeed = quantumSeed;

                // Partikülleri üret
                var particles = await _quantumService.GenerateParticlesAsync(request);

                // Sonuç oluştur
                stopwatch.Stop();
                var result = new BigBangSimulationResult
                {
                    Particles = particles,
                    UniverseConstants = universeConstants,
                    Metadata = new SimulationMetadata
                    {
                        SimulationTime = DateTime.UtcNow,
                        ExecutionDuration = stopwatch.Elapsed,
                        TotalParticleCount = particles.Length,
                        TotalEnergy = particles.Sum(p => p.Energy),
                        QuantumSeed = quantumSeed,
                        SimulationId = Guid.NewGuid().ToString(),
                        AdditionalData = new Dictionary<string, object>
                        {
                            ["ParticleCount"] = request.ParticleCount,
                            ["TimeScale"] = request.TimeScale,
                            ["EnergyScale"] = request.EnergyScale,
                            ["Temperature"] = request.Temperature
                        }
                    }
                };

                // Cache'e kaydet
                if (_config.EnableCaching)
                {
                    await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(30));
                }

                _logger.LogSimulationComplete(result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateBigBangAsync", ex);
                throw;
            }
        }

        public async Task<UniverseConstantsDto> GenerateUniverseConstantsAsync()
        {
            try
            {
                var constants = await _quantumService.GenerateUniverseConstantsAsync();
                return constants;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateUniverseConstantsAsync", ex);
                throw;
            }
        }

        public async Task<int> GenerateQuantumSeedAsync()
        {
            try
            {
                return await _quantumService.GenerateQuantumSeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("GenerateQuantumSeedAsync", ex);
                throw;
            }
        }

        private void ValidateRequest(BigBangRequest request)
        {
            if (request.ParticleCount <= 0)
                throw new ArgumentException("Partikül sayısı pozitif olmalıdır.", nameof(request.ParticleCount));

            if (request.ParticleCount > _config.MaxParticleCount)
                throw new ArgumentException($"Partikül sayısı maksimum {_config.MaxParticleCount} olabilir.", nameof(request.ParticleCount));

            if (request.TimeScale <= 0)
                throw new ArgumentException("Zaman ölçeği pozitif olmalıdır.", nameof(request.TimeScale));

            if (request.EnergyScale <= 0)
                throw new ArgumentException("Enerji ölçeği pozitif olmalıdır.", nameof(request.EnergyScale));

            if (request.Temperature < 0)
                throw new ArgumentException("Sıcaklık negatif olamaz.", nameof(request.Temperature));
        }

        private string GenerateCacheKey(BigBangRequest request)
        {
            return $"bigbang_{request.ParticleCount}_{request.TimeScale}_{request.EnergyScale}_{request.Temperature}_{request.RandomSeed}";
        }
    }
}
