using Microsoft.Quantum.Simulation.Simulators;
using Microsoft.Quantum.Runtime;

namespace BigBangSimulation.Quantum;

// Q# ile C# arasındaki köprü sınıfı
public class QuantumBigBangSimulator
{
    private readonly QuantumSimulator _simulator;

    public QuantumBigBangSimulator()
    {
        _simulator = new QuantumSimulator();
    }    // Kuantum rastgele sayı üretir
    public async Task<int> GenerateRandomNumberAsync(int maxValue)
    {
        if (maxValue <= 0) return 0;
        
        // Geçici çözüm: Q# integration sonrası geliştirilecek
        await Task.Delay(1); // Async pattern için
        
        // Kriptografik güvenli rastgele sayı üreteci kullan
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var value = Math.Abs(BitConverter.ToInt32(bytes, 0));
        return value % maxValue;
    }

    // Gaussian dağılım rastgele sayı üretir
    public async Task<double> GenerateGaussianRandomAsync(double mean, double stdDev)
    {
        await Task.Delay(1); // Async pattern için
        
        // Box-Muller transformation ile Gaussian dağılım
        var u1 = new Random().NextDouble();
        var u2 = new Random().NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }

    // Big Bang partiküllerini üretir
    public async Task<ParticleData[]> GenerateBigBangParticlesAsync(int count, double timeScale, double energyScale)
    {
        var particles = new List<ParticleData>();
        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            // Partikül tipi belirle
            var particleTypes = new[] { "quark", "lepton", "boson" };
            var type = particleTypes[await GenerateRandomNumberAsync(3)];

            // Pozisyon (merkezi patlama)
            var radius = await GenerateGaussianRandomAsync(0, 0.1 * timeScale);
            var theta = await GenerateRandomNumberAsync(360) * Math.PI / 180;
            var phi = await GenerateRandomNumberAsync(180) * Math.PI / 180;

            var x = radius * Math.Sin(phi) * Math.Cos(theta);
            var y = radius * Math.Sin(phi) * Math.Sin(theta);
            var z = radius * Math.Cos(phi);

            // Hız (dışarı doğru)
            var speed = await GenerateGaussianRandomAsync(0.3, 0.1) * timeScale;
            var vx = speed * Math.Sin(phi) * Math.Cos(theta);
            var vy = speed * Math.Sin(phi) * Math.Sin(theta);
            var vz = speed * Math.Cos(phi);

            // Enerji ve küttle
            var energy = await GenerateGaussianRandomAsync(1.0, 0.3) * energyScale;
            var mass = energy / (299792458.0 * 299792458.0); // E=mc²

            // Renk (enerji seviyesine bağlı)
            var colorIntensity = Math.Max(0, Math.Min(1, energy / energyScale));
            var r = (byte)(255 * colorIntensity);
            var g = (byte)(128 + 127 * (1 - colorIntensity));
            var b = (byte)(255 * (1 - colorIntensity));

            particles.Add(new ParticleData
            {
                Id = Guid.NewGuid().ToString(),
                Type = type,
                Position = new Vector3(x, y, z),
                Velocity = new Vector3(vx, vy, vz),
                Energy = energy,
                Mass = mass,
                Charge = type == "lepton" ? -1.602e-19 : (await GenerateRandomNumberAsync(3) - 1) * 1.602e-19 / 3,
                Color = new ColorData { R = r, G = g, B = b, A = 255 },
                CreatedAt = DateTime.UtcNow
            });
        }

        return particles.ToArray();
    }

    // Evren sabitlerini döndürür
    public async Task<UniverseConstants> GetUniverseConstantsAsync()
    {
        // Kuantum varyasyonları ekle
        var speedOfLightVariation = await GenerateGaussianRandomAsync(1.0, 0.0001);
        var planckVariation = await GenerateGaussianRandomAsync(1.0, 0.0001);

        return new UniverseConstants
        {
            SpeedOfLight = 299792458.0 * speedOfLightVariation,
            PlanckConstant = 6.62607015e-34 * planckVariation,
            GravitationalConstant = 6.67430e-11,
            FineStructureConstant = 7.2973525693e-3,
            BoltzmannConstant = 1.380649e-23,
            AvogadroNumber = 6.02214076e23,
            ElectronMass = 9.1093837015e-31,
            ProtonMass = 1.67262192369e-27,
            ElectronCharge = 1.602176634e-19
        };
    }

    public void Dispose()
    {
        _simulator?.Dispose();
    }
}

// Veri modelleri
public class ParticleData
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Vector3 Position { get; set; } = new();
    public Vector3 Velocity { get; set; } = new();
    public double Energy { get; set; }
    public double Mass { get; set; }
    public double Charge { get; set; }
    public ColorData Color { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class Vector3
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Vector3() { }
    public Vector3(double x, double y, double z)
    {
        X = x; Y = y; Z = z;
    }
}

public class ColorData
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; } = 255;
}

public class UniverseConstants
{
    public double SpeedOfLight { get; set; }
    public double PlanckConstant { get; set; }
    public double GravitationalConstant { get; set; }
    public double FineStructureConstant { get; set; }
    public double BoltzmannConstant { get; set; }
    public double AvogadroNumber { get; set; }
    public double ElectronMass { get; set; }
    public double ProtonMass { get; set; }
    public double ElectronCharge { get; set; }
}
