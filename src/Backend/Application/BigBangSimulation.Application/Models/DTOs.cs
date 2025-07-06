namespace BigBangSimulation.Application.Models
{
    // Big Bang simülasyon isteği DTO'su
    public class BigBangRequest
    {
        public int ParticleCount { get; set; } = 1000;
        public double TimeScale { get; set; } = 1.0;
        public double EnergyScale { get; set; } = 1.0;
        public double Temperature { get; set; } = 1000000.0; // Kelvin
        public int RandomSeed { get; set; }
    }

    // Partikül verileri DTO'su
    public class ParticleDto
    {
        public string Type { get; set; } = string.Empty; // "quark", "lepton", "boson"
        public double Energy { get; set; }
        public double Mass { get; set; }
        public double Charge { get; set; }
        public Vector3D Position { get; set; } = new();
        public Vector3D Velocity { get; set; } = new();
        public string Color { get; set; } = "#FFFFFF";
        public double Size { get; set; }
        public long CreationTime { get; set; } // Nanosecond
    }

    // 3D vektör DTO'su
    public class Vector3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    // Evren sabitleri DTO'su
    public class UniverseConstantsDto
    {
        public double SpeedOfLight { get; set; } // c = 299,792,458 m/s
        public double PlanckConstant { get; set; } // h = 6.626 × 10^-34 J⋅s
        public double GravitationalConstant { get; set; } // G = 6.674 × 10^-11 m³⋅kg⁻¹⋅s⁻²
        public double FineStructureConstant { get; set; } // α ≈ 1/137
        public double ElectronMass { get; set; } // me = 9.109 × 10^-31 kg
        public double ProtonMass { get; set; } // mp = 1.673 × 10^-27 kg
        public double ElementaryCharge { get; set; } // e = 1.602 × 10^-19 C
        public double BoltzmannConstant { get; set; } // kB = 1.381 × 10^-23 J/K
        public double AvogadroNumber { get; set; } // NA = 6.022 × 10^23 mol⁻¹
        public double VacuumPermeability { get; set; } // μ₀ = 4π × 10^-7 H/m
    }

    // Big Bang simülasyon sonucu DTO'su
    public class BigBangSimulationResult
    {
        public ParticleDto[] Particles { get; set; } = Array.Empty<ParticleDto>();
        public UniverseConstantsDto UniverseConstants { get; set; } = new();
        public SimulationMetadata Metadata { get; set; } = new();
    }

    // Simülasyon meta verileri DTO'su
    public class SimulationMetadata
    {
        public DateTime SimulationTime { get; set; } = DateTime.UtcNow;
        public TimeSpan ExecutionDuration { get; set; }
        public int TotalParticleCount { get; set; }
        public double TotalEnergy { get; set; }
        public int QuantumSeed { get; set; }
        public string SimulationId { get; set; } = Guid.NewGuid().ToString();
        public Dictionary<string, object> AdditionalData { get; set; } = new();
    }
}
