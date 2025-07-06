using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BigBangSimulation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// API test endpoint'i
        /// </summary>
        [HttpGet("ping")]
        public ActionResult<object> Ping()
        {
            return Ok(new
            {
                Message = "🚀 Quantum Big Bang Simulation API",
                Status = "Running",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0"
            });
        }

        /// <summary>
        /// Mock Big Bang simülasyonu
        /// </summary>
        [HttpPost("mock-bigbang")]
        public ActionResult<object> MockBigBang([FromBody] MockBigBangRequest request)
        {
            _logger.LogInformation("🚀 Mock Big Bang simülasyonu başlatılıyor");

            var particles = GenerateMockParticles(request.ParticleCount);

            var result = new
            {
                Particles = particles,
                UniverseConstants = new
                {
                    GravityStrength = 6.67430e-11,
                    LightSpeed = 299792458.0,
                    PlanckConstant = 6.62607015e-34,
                    ExpansionRate = 70.0
                },
                Metadata = new
                {
                    TotalParticles = particles.Length,
                    SimulationTime = 42.5,
                    QuantumSeed = Random.Shared.Next(1000, 9999),
                    GeneratedAt = DateTime.UtcNow,
                    Version = "1.0-mock"
                }
            };

            _logger.LogInformation("✅ Mock Big Bang simülasyonu tamamlandı");
            return Ok(result);
        }

        private object[] GenerateMockParticles(int count)
        {
            var particles = new object[count];
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                particles[i] = new
                {
                    Position = new
                    {
                        X = (random.NextDouble() - 0.5) * 20,
                        Y = (random.NextDouble() - 0.5) * 20,
                        Z = (random.NextDouble() - 0.5) * 20
                    },
                    Velocity = new
                    {
                        Vx = (random.NextDouble() - 0.5) * 4,
                        Vy = (random.NextDouble() - 0.5) * 4,
                        Vz = (random.NextDouble() - 0.5) * 4
                    },
                    Energy = random.NextDouble() * 100,
                    Mass = random.NextDouble() * 10,
                    Color = new
                    {
                        R = random.Next(0, 256),
                        G = random.Next(0, 256),
                        B = random.Next(0, 256)
                    }
                };
            }

            return particles;
        }
    }

    public class MockBigBangRequest
    {
        [Range(1, 1000, ErrorMessage = "Partikül sayısı 1 ile 1000 arasında olmalıdır.")]
        public int ParticleCount { get; set; } = 50;
    }
}
