using System;
using System.Configuration;
using System.Threading.Tasks;
using Core.Messages;
using MassTransit;
using Services.Math;

namespace DummyConsumerService.Consumers
{
    /// <summary>
    /// Dummy handler of ISomeEvent events that has a built-in chance of random failure
    /// </summary>
    public class SomeEventHandler : IConsumer<ISomeEvent>
    {
        static readonly double ChanceOfFailure;

        private readonly IGenerateRandomNumbers _randomNumberGenerator;

        public SomeEventHandler(IGenerateRandomNumbers randomNumberGenerator)
        {
            if (randomNumberGenerator == null) throw new ArgumentNullException(nameof(randomNumberGenerator));
            _randomNumberGenerator = randomNumberGenerator;
        }

        /// <summary>
        /// Static constructor that pulls our chance of failure from configuration
        /// </summary>
        static SomeEventHandler()
        {
            if (double.TryParse(ConfigurationManager.AppSettings["ChanceOfMessageHandlingFailure"], out ChanceOfFailure)) return;

            Console.WriteLine("Invalid ChanceOfMessageHandlingFailure configuration, so defaulting to 10% chance of failure.");
            ChanceOfFailure = .1;
        }

        /// <summary>
        /// Dummy handler that fails with a certain probability
        /// </summary>
        public async Task Consume(ConsumeContext<ISomeEvent> context)
        {
            //see if we should throw a random failure or let the consumption of the message succeed                       
            if (_randomNumberGenerator.GetRandomDouble() < ChanceOfFailure)
            {
                var failure = $"Random exception handling [{context.Message.What}]";
                Console.WriteLine($"{DateTime.UtcNow:u} FAILURE {failure}");
                throw new Exception(failure);
            }

            //consuming the message will just involve writing a message to the console that we got it
            await Console.Out.WriteLineAsync($"{DateTime.UtcNow:u} Handling event [{context.Message.What}] which was published at [{context.Message.When:u}]");
        }
    }
}
