namespace BigBangSimulation.Quantum {
    open Microsoft.Quantum.Convert;
    open Microsoft.Quantum.Math;
    open Microsoft.Quantum.Random;
    open Microsoft.Quantum.Arrays;
    open Microsoft.Quantum.Measurement;

    // Tek bir kuantum rastgele bit üretir
    operation GenerateRandomBit() : Result {
        use qubit = Qubit();
        H(qubit);
        return M(qubit);
    }

    // Belirtilen bit sayısında rastgele sayı üretir
    operation GenerateRandomInt(bitCount : Int) : Int {
        mutable result = 0;
        for i in 0..bitCount - 1 {
            let bit = GenerateRandomBit();
            if bit == One {
                set result = result + 2^i;
            }
        }
        return result;
    }

    // 0.0 ile 1.0 arasında rastgele ondalık sayı üretir
    operation GenerateRandomDouble() : Double {
        let randomInt = GenerateRandomInt(53); // Double precision için 53 bit
        let maxValue = 2.0^53.0;
        return IntAsDouble(randomInt) / maxValue;
    }

    // Belirtilen aralıkta rastgele ondalık sayı üretir
    operation GenerateRandomRange(min : Double, max : Double) : Double {
        let randomFraction = GenerateRandomDouble();
        return min + randomFraction * (max - min);
    }

    // Belirtilen sayıda rastgele koordinat üretir (3D uzay için)
    operation GenerateRandomCoordinates(count : Int, minVal : Double, maxVal : Double) : Double[] {
        mutable coordinates = [];
        for i in 0..count - 1 {
            let coord = GenerateRandomRange(minVal, maxVal);
            set coordinates = coordinates + [coord];
        }
        return coordinates;
    }

    // Gaussian dağılımda rastgele sayı üretir (Box-Muller yöntemi)
    operation GenerateGaussianRandom(mean : Double, stdDev : Double) : Double {
        let u1 = GenerateRandomDouble();
        let u2 = GenerateRandomDouble();
        
        // Box-Muller dönüşümü
        let z0 = Sqrt(-2.0 * Log(u1)) * Cos(2.0 * PI() * u2);
        return mean + stdDev * z0;
    }

    // Partikül enerjisi için rastgele değer üretir
    operation GenerateParticleEnergy(minEnergy : Double, maxEnergy : Double) : Double {
        return GenerateRandomRange(minEnergy, maxEnergy);
    }

    // Partikül rengi için RGB değerleri üretir
    operation GenerateParticleColor() : (Int, Int, Int) {
        let r = GenerateRandomInt(8); // 8 bit = 0-255
        let g = GenerateRandomInt(8);
        let b = GenerateRandomInt(8);
        return (r, g, b);
    }
}
