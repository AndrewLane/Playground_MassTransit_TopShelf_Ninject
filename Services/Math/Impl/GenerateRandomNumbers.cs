using System;
using System.Security.Cryptography;

namespace Services.Math.Impl
{
    internal class GenerateRandomNumbers : IGenerateRandomNumbers
    {
        // provider for generating random numbers
        private static readonly RNGCryptoServiceProvider CryptoProvider = new RNGCryptoServiceProvider();

        public double GetRandomDouble()
        {
            var byteArrayForRandomNumber = new byte[sizeof(uint)];
            CryptoProvider.GetBytes(byteArrayForRandomNumber);
            return BitConverter.ToUInt32(byteArrayForRandomNumber, startIndex: 0) / (uint.MaxValue + 1.0);
        }

        public int GetRandomInt(int inclusiveMin, int inclusiveMax)
        {
            if (inclusiveMin > inclusiveMax) throw new ArgumentOutOfRangeException(nameof(inclusiveMax));

            if (inclusiveMin == inclusiveMax) return inclusiveMin;

            int totalPossibleValues = inclusiveMax - inclusiveMin + 1;
            return Convert.ToInt32(System.Math.Floor((GetRandomDouble() * totalPossibleValues))) + inclusiveMin;                
        }
    }
}
