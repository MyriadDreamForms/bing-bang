using BigBangSimulation.Application.Interfaces;
using BigBangSimulation.Application.Models;
using BigBangSimulation.Quantum;

namespace BigBangSimulation.Infrastructure.Services;

// Kuantum hesaplama operasyonlarını yöneten servis
public class QuantumComputingService : IQuantumComputingService
{
    private readonly QuantumBigBangSimulator _quantumSimulator;

    public QuantumComputingService()
    {
        _quantumSimulator = new QuantumBigBangSimulator();
    }

    // Interface uyumlu: Partikül dizisi üretir
    public async Task<ParticleDto[]> GenerateParticlesAsync(BigBangRequest request)
    {
        var particles = await _quantumSimulator.GenerateBigBangParticlesAsync(
            request.ParticleCount, 
            request.TimeScale, 
            request.EnergyScale
        );

        return particles.Select(p => new ParticleDto
        {
            Type = p.Type,
            Position = new Vector3D { X = p.Position.X, Y = p.Position.Y, Z = p.Position.Z },
            Velocity = new Vector3D { X = p.Velocity.X, Y = p.Velocity.Y, Z = p.Velocity.Z },
            Energy = p.Energy,
            Mass = p.Mass,
            Charge = p.Charge,
            Color = $"#{p.Color.R:X2}{p.Color.G:X2}{p.Color.B:X2}",
            Size = p.Mass * 1000, // Görsel boyut için mass'i kullan
            CreationTime = p.CreatedAt.Ticks / 100 // Nanosecond'a çevir
        }).ToArray();
    }

    // Interface uyumlu: Evren sabitlerini üretir
    public async Task<UniverseConstantsDto> GenerateUniverseConstantsAsync()
    {
        var constants = await _quantumSimulator.GetUniverseConstantsAsync();
        
        return new UniverseConstantsDto
        {
            SpeedOfLight = constants.SpeedOfLight,
            PlanckConstant = constants.PlanckConstant,
            GravitationalConstant = constants.GravitationalConstant,
            FineStructureConstant = constants.FineStructureConstant,
            BoltzmannConstant = constants.BoltzmannConstant,
            AvogadroNumber = constants.AvogadroNumber,
            ElectronMass = constants.ElectronMass,
            ProtonMass = constants.ProtonMass,
            ElementaryCharge = constants.ElectronCharge,
            VacuumPermeability = 4 * Math.PI * 1e-7
        };
    }

    // Interface uyumlu: Rastgele double değer üretir
    public async Task<double> GenerateRandomDoubleAsync()
    {
        var randomInt = await _quantumSimulator.GenerateRandomNumberAsync(1000000);
        return randomInt / 1000000.0;
    }

    // Interface uyumlu: Belirli aralıkta rastgele değer üretir
    public async Task<double> GenerateRandomRangeAsync(double min, double max)
    {
        var randomDouble = await GenerateRandomDoubleAsync();
        return min + (randomDouble * (max - min));
    }

    // Interface uyumlu: Gaussian dağılımda rastgele değer üretir
    public async Task<double> GenerateGaussianRandomAsync(double mean, double stdDev)
    {
        return await _quantumSimulator.GenerateGaussianRandomAsync(mean, stdDev);
    }    // Interface uyumlu: Kuantum seed üretir (int döndürür)
    public async Task<int> GenerateQuantumSeedAsync()    {
        var randomValue = await _quantumSimulator.GenerateRandomNumberAsync(int.MaxValue);
        return randomValue;
    }
}
