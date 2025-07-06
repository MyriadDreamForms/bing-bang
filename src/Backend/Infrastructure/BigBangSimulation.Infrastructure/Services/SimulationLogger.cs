using BigBangSimulation.Application.Interfaces;
using BigBangSimulation.Application.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BigBangSimulation.Infrastructure.Services
{
    public class SimulationLogger : ISimulationLogger
    {
        private readonly ILogger<SimulationLogger> _logger;

        public SimulationLogger(ILogger<SimulationLogger> logger)
        {
            _logger = logger;
        }

        public void LogSimulationStart(BigBangRequest request)
        {
            _logger.LogInformation("🚀 Big Bang simülasyonu başlatıldı. Parametreler: {Parameters}", 
                JsonSerializer.Serialize(request));
        }

        public void LogSimulationComplete(BigBangSimulationResult result)
        {
            _logger.LogInformation("✅ Big Bang simülasyonu tamamlandı. " +
                "Partikül sayısı: {ParticleCount}, " +
                "Simülasyon süresi: {SimulationTime}ms, " +
                "Quantum Seed: {QuantumSeed}", 
                result.Metadata.TotalParticleCount,
                result.Metadata.SimulationTime,
                result.Metadata.QuantumSeed);
        }

        public void LogQuantumOperation(string operation, object parameters)
        {
            _logger.LogDebug("⚛️ Kuantum operasyonu: {Operation}, Parametreler: {Parameters}", 
                operation, JsonSerializer.Serialize(parameters));
        }

        public void LogError(string operation, Exception exception)
        {
            _logger.LogError(exception, "❌ Hata oluştu. Operasyon: {Operation}", operation);
        }
    }
}
