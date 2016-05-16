using System;
using System.Security.Cryptography;

namespace Services.Math.Impl
{
    public class GenerateRandomNumbers : IGenerateRandomNumbers
    {
        // provider for generating random numbers
        private static RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();

        public double GetRandomDouble()
        {
            var byteArrayForRandomNumber = new byte[sizeof(uint)];
            cryptoProvider.GetBytes(byteArrayForRandomNumber);
            return BitConverter.ToUInt32(byteArrayForRandomNumber, startIndex: 0) / (uint.MaxValue + 1.0);
        }
    }
}
