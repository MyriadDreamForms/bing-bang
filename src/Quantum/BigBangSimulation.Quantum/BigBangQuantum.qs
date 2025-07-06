namespace BigBangSimulation.Quantum {
    open Microsoft.Quantum.Convert;
    open Microsoft.Quantum.Math;
    open Microsoft.Quantum.Arrays;
    open Microsoft.Quantum.Measurement;

    // Partikül verisi için struct benzeri tuple
    newtype ParticleData = (
        Position: (Double, Double, Double),
        Velocity: (Double, Double, Double), 
        Energy: Double,
        Mass: Double,
        Color: (Int, Int, Int)
    );

    // Big Bang için tek bir partikül üretir
    operation GenerateBigBangParticle(
        explosionRadius : Double, 
        energyRange : (Double, Double),
        massRange : (Double, Double)
    ) : ParticleData {
        
        // Küresel koordinatlarda rastgele pozisyon
        let theta = GenerateRandomRange(0.0, 2.0 * PI()); // Azimuth açısı
        let phi = GenerateRandomRange(0.0, PI()); // Polar açısı
        let radius = GenerateRandomRange(0.1, explosionRadius);
        
        // Kartezyen koordinatlara dönüştür
        let x = radius * Sin(phi) * Cos(theta);
        let y = radius * Sin(phi) * Sin(theta);
        let z = radius * Cos(phi);
        
        // Rastgele hız vektörü (genişleme yönünde)
        let velocityMagnitude = GenerateRandomRange(0.5, 2.0);
        let vx = velocityMagnitude * Sin(phi) * Cos(theta);
        let vy = velocityMagnitude * Sin(phi) * Sin(theta);
        let vz = velocityMagnitude * Cos(phi);
        
        // Rastgele enerji ve kütle
        let energy = GenerateRandomRange(Fst(energyRange), Snd(energyRange));
        let mass = GenerateRandomRange(Fst(massRange), Snd(massRange));
        
        // Rastgele renk
        let color = GenerateParticleColor();
        
        return ParticleData(
            (x, y, z),
            (vx, vy, vz),
            energy,
            mass,
            color
        );
    }

    // Belirtilen sayıda Big Bang partikülü üretir
    operation GenerateBigBangParticles(
        particleCount : Int,
        explosionRadius : Double,
        energyRange : (Double, Double),
        massRange : (Double, Double)
    ) : ParticleData[] {
        
        mutable particles = [];
        for i in 0..particleCount - 1 {
            let particle = GenerateBigBangParticle(explosionRadius, energyRange, massRange);
            set particles = particles + [particle];
        }
        return particles;
    }

    // Kuantum süperpozisyon etkisi ile partikül varyasyonları üretir
    operation GenerateQuantumVariations(baseParticle : ParticleData, variationCount : Int) : ParticleData[] {
        mutable variations = [];
        let (basePos, baseVel, baseEnergy, baseMass, baseColor) = baseParticle!;
        
        for i in 0..variationCount - 1 {
            // Süperpozisyon tabanlı küçük varyasyonlar
            let posVariation = 0.1;
            let velVariation = 0.05;
            
            let (x, y, z) = basePos;
            let (vx, vy, vz) = baseVel;
            
            // Gaussian gürültü ekle
            let newX = x + GenerateGaussianRandom(0.0, posVariation);
            let newY = y + GenerateGaussianRandom(0.0, posVariation);
            let newZ = z + GenerateGaussianRandom(0.0, posVariation);
            
            let newVx = vx + GenerateGaussianRandom(0.0, velVariation);
            let newVy = vy + GenerateGaussianRandom(0.0, velVariation);
            let newVz = vz + GenerateGaussianRandom(0.0, velVariation);
            
            let newEnergy = baseEnergy * (1.0 + GenerateGaussianRandom(0.0, 0.1));
            
            let variation = ParticleData(
                (newX, newY, newZ),
                (newVx, newVy, newVz),
                newEnergy,
                baseMass,
                baseColor
            );
            
            set variations = variations + [variation];
        }
        return variations;
    }

    // Kaotik kuantum sistemi ile evren sabitleri üretir
    operation GenerateUniverseConstants() : (Double, Double, Double, Double) {
        // Kuantum kaos ile evren sabitleri
        let gravityStrength = GenerateRandomRange(6.67e-11, 6.68e-11); // Gravitasyon sabiti varyasyonu
        let lightSpeed = GenerateRandomRange(2.99792458e8, 2.99792459e8); // Işık hızı varyasyonu
        let planckConstant = GenerateRandomRange(6.626e-34, 6.627e-34); // Planck sabiti varyasyonu
        let expansionRate = GenerateRandomRange(50.0, 100.0); // Hubble sabiti (km/s/Mpc)
        
        return (gravityStrength, lightSpeed, planckConstant, expansionRate);
    }

    // Kuantum entropi ile rastgele seed değeri üretir
    operation GenerateQuantumSeed() : Int {
        return GenerateRandomInt(32); // 32-bit rastgele seed
    }
}
