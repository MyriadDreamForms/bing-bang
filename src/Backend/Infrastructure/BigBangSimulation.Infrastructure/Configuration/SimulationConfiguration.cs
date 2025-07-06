using BigBangSimulation.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BigBangSimulation.Infrastructure.Configuration
{
    public class SimulationConfiguration : ISimulationConfiguration
    {
        private readonly IConfiguration _configuration;

        public SimulationConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }        public int MaxParticleCount => 
            int.TryParse(_configuration["Simulation:MaxParticleCount"], out var value) ? value : 10000;

        public double MaxExplosionRadius => 
            double.TryParse(_configuration["Simulation:MaxExplosionRadius"], out var value) ? value : 1000.0;

        public TimeSpan SimulationTimeout => 
            TimeSpan.FromSeconds(int.TryParse(_configuration["Simulation:TimeoutSeconds"], out var value) ? value : 300);

        public bool EnableCaching => 
            bool.TryParse(_configuration["Simulation:EnableCaching"], out var value) ? value : true;

        public bool EnableLogging => 
            bool.TryParse(_configuration["Simulation:EnableLogging"], out var value) ? value : true;

        public string QuantumSimulatorType => 
            _configuration["Simulation:QuantumSimulatorType"] ?? "LocalSimulator";
    }
}
