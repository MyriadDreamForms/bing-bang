using BigBangSimulation.Application.Interfaces;
using BigBangSimulation.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BigBangSimulation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class QuantumController : ControllerBase
    {
        private readonly IBigBangSimulationService _simulationService;
        private readonly ILogger<QuantumController> _logger;

        public QuantumController(
            IBigBangSimulationService simulationService,
            ILogger<QuantumController> logger)
        {
            _simulationService = simulationService;
            _logger = logger;
        }

        /// <summary>
        /// Big Bang simülasyonu başlatır ve kuantum tabanlı partiküller üretir
        /// </summary>
        /// <param name="request">Simülasyon parametreleri</param>
        /// <returns>Partikül listesi ve evren sabitleri</returns>
        [HttpPost("bigbang")]
        [ProducesResponseType(typeof(BigBangSimulationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BigBangSimulationResult>> GenerateBigBang(
            [FromBody] BigBangRequestDto request)
        {
            try
            {
                _logger.LogInformation("🚀 Big Bang simülasyonu başlatılıyor. Partikül sayısı: {ParticleCount}", 
                    request.ParticleCount);                var simulationRequest = new BigBangRequest
                {
                    ParticleCount = request.ParticleCount,
                    TimeScale = request.TimeScale,
                    EnergyScale = request.EnergyScale,
                    Temperature = request.Temperature,
                    RandomSeed = request.RandomSeed
                };

                var result = await _simulationService.GenerateBigBangAsync(simulationRequest);                _logger.LogInformation("✅ Big Bang simülasyonu tamamlandı. " +
                    "Üretilen partikül sayısı: {ParticleCount}, Süre: {Duration}ms",
                    result.Metadata.TotalParticleCount, result.Metadata.ExecutionDuration.TotalMilliseconds);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("⚠️ Geçersiz parametre: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Big Bang simülasyonu sırasında hata oluştu");
                return StatusCode(500, new { error = "Simülasyon sırasında beklenmeyen bir hata oluştu." });
            }
        }

        /// <summary>
        /// Kuantum tabanlı evren sabitleri üretir
        /// </summary>
        /// <returns>Evren sabitleri</returns>
        [HttpGet("universe-constants")]
        [ProducesResponseType(typeof(UniverseConstantsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UniverseConstantsDto>> GenerateUniverseConstants()
        {
            try
            {
                _logger.LogInformation("⚛️ Evren sabitleri üretiliyor");

                var result = await _simulationService.GenerateUniverseConstantsAsync();

                _logger.LogInformation("✅ Evren sabitleri başarıyla üretildi");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Evren sabitleri üretilirken hata oluştu");
                return StatusCode(500, new { error = "Evren sabitleri üretilirken beklenmeyen bir hata oluştu." });
            }
        }

        /// <summary>
        /// Kuantum tabanlı rastgele seed değeri üretir
        /// </summary>
        /// <returns>Quantum seed</returns>
        [HttpGet("quantum-seed")]
        [ProducesResponseType(typeof(QuantumSeedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<QuantumSeedResponse>> GenerateQuantumSeed()
        {
            try
            {
                _logger.LogInformation("🎲 Kuantum seed üretiliyor");

                var seed = await _simulationService.GenerateQuantumSeedAsync();

                var response = new QuantumSeedResponse
                {
                    Seed = seed,
                    GeneratedAt = DateTime.UtcNow,
                    Type = "Quantum"
                };

                _logger.LogInformation("✅ Kuantum seed başarıyla üretildi: {Seed}", seed);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Kuantum seed üretilirken hata oluştu");
                return StatusCode(500, new { error = "Kuantum seed üretilirken beklenmeyen bir hata oluştu." });
            }
        }

        /// <summary>
        /// API durumunu ve sağlık kontrolünü yapar
        /// </summary>
        /// <returns>API durum bilgisi</returns>
        [HttpGet("health")]
        [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status200OK)]
        public ActionResult<HealthCheckResponse> HealthCheck()
        {
            var response = new HealthCheckResponse
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                ServiceName = "Quantum Big Bang Simulation API",
                Uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime
            };

            return Ok(response);
        }
    }

    // API DTO modelleri
    public class BigBangRequestDto
    {        [Range(1, 10000, ErrorMessage = "Partikül sayısı 1 ile 10000 arasında olmalıdır.")]
        public int ParticleCount { get; set; } = 100;

        [Range(0.1, 100.0, ErrorMessage = "Zaman ölçeği 0.1 ile 100 arasında olmalıdır.")]
        public double TimeScale { get; set; } = 1.0;

        [Range(0.1, 100.0, ErrorMessage = "Enerji ölçeği 0.1 ile 100 arasında olmalıdır.")]
        public double EnergyScale { get; set; } = 1.0;

        [Range(0, double.MaxValue, ErrorMessage = "Sıcaklık negatif olamaz.")]
        public double Temperature { get; set; } = 1000000.0; // Kelvin

        public int RandomSeed { get; set; } = 0;
    }

    public class QuantumSeedResponse
    {
        public int Seed { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string Type { get; set; } = "Quantum";
    }

    public class HealthCheckResponse
    {
        public string Status { get; set; } = "Healthy";
        public DateTime Timestamp { get; set; }
        public string Version { get; set; } = "1.0.0";
        public string ServiceName { get; set; } = "API";
        public TimeSpan Uptime { get; set; }
    }
}
