using BigBangSimulation.Application.Models;

namespace BigBangSimulation.Application.Interfaces
{
    // Kuantum hesaplama servisi interface'i
    public interface IQuantumComputingService
    {
        Task<ParticleDto[]> GenerateParticlesAsync(BigBangRequest request);
        Task<UniverseConstantsDto> GenerateUniverseConstantsAsync();
        Task<double> GenerateRandomDoubleAsync();
        Task<double> GenerateRandomRangeAsync(double min, double max);
        Task<double> GenerateGaussianRandomAsync(double mean, double stdDev);
        Task<int> GenerateQuantumSeedAsync();
    }

    // Logging servisi interface'i
    public interface ISimulationLogger
    {
        void LogSimulationStart(BigBangRequest request);
        void LogSimulationComplete(BigBangSimulationResult result);
        void LogQuantumOperation(string operation, object parameters);
        void LogError(string operation, Exception exception);
    }

    // Cache servisi interface'i (performance için)
    public interface ISimulationCache
    {
        Task<T?> GetAsync<T>(string key) where T : class;
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class;
        Task RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
    }

    // Configuration servisi interface'i
    public interface ISimulationConfiguration
    {
        int MaxParticleCount { get; }
        double MaxExplosionRadius { get; }
        TimeSpan SimulationTimeout { get; }
        bool EnableCaching { get; }
        bool EnableLogging { get; }
        string QuantumSimulatorType { get; }
    }
}
