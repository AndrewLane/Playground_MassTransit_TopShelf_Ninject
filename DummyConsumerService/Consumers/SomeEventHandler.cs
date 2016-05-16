using MassTransit;
using System;
using System.Threading.Tasks;
using Core.Messages;
using System.Security.Cryptography;
using System.Configuration;

namespace MassTransitTopShelfNinjectPlayground1.Consumer
{
    /// <summary>
    /// Dummy handler of ISomeEvent events that has a built-in chance of random failure
    /// </summary>
    public class SomeEventHandler : IConsumer<ISomeEvent>
    {
        // provider for generating random numbers
        private static RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();

        static double chanceOfFailure;

        /// <summary>
        /// Static constructor that pulls our chance of failure from configuration
        /// </summary>
        static SomeEventHandler()
        {
            if (double.TryParse(ConfigurationManager.AppSettings["ChanceOfMessageHandlingFailure"], out chanceOfFailure) == false)
            {
                Console.WriteLine("Invalid ChanceOfMessageHandlingFailure configuration, so defaulting to 10% chance of failure.");
                chanceOfFailure = .1;
            }
        }

        /// <summary>
        /// Dummy handler that fails with a certain probability
        /// </summary>
        public async Task Consume(ConsumeContext<ISomeEvent> context)
        {
            //see if we should throw a random failure or let the consumption of the message succeed                       
            if (GetRandomDouble() < chanceOfFailure)
            {
                var failure = $"Random exception handling [{context.Message.What}]";
                Console.WriteLine($"{DateTime.UtcNow.ToString("u")} FAILURE {failure}");
                throw new Exception(failure);
            }

            //consuming the message will just involve writing a message to the console that we got it
            await Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("u")} Handling event [{context.Message.What}] which was published at [{context.Message.When.ToString("u")}]");
        }

        /// <summary>
        /// Helper to generate random double's
        /// </summary>
        private double GetRandomDouble()
        {
            var byteArrayForRandomNumber = new byte[sizeof(uint)];
            cryptoProvider.GetBytes(byteArrayForRandomNumber);
            return BitConverter.ToUInt32(byteArrayForRandomNumber, startIndex: 0) / (uint.MaxValue + 1.0);
        }
    }
}
