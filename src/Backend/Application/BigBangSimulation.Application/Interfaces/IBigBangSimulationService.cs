using BigBangSimulation.Application.Models;

namespace BigBangSimulation.Application.Interfaces
{
    // Partikül simülasyon servisi interface'i
    public interface IBigBangSimulationService
    {
        Task<BigBangSimulationResult> GenerateBigBangAsync(BigBangRequest request);
        Task<UniverseConstantsDto> GenerateUniverseConstantsAsync();
        Task<int> GenerateQuantumSeedAsync();
    }
}
