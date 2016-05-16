using Core.Messages;
using MassTransit;
using System;
using System.Threading.Tasks;
using Services.Math;

namespace DummyConsumerService.Consumers
{
    /// <summary>
    /// Dummy handler of an IAppLaunched event that prints a message to the console in a random color
    /// </summary>
    public class AppLaunchedHandler : IConsumer<IAppLaunched>
    {
        IGenerateRandomColors _colorGenerator;

        public AppLaunchedHandler(IGenerateRandomColors colorGenerator)
        {
            if (colorGenerator == null) throw new ArgumentNullException(nameof(colorGenerator));
            _colorGenerator = colorGenerator;
        }

        public async Task Consume(ConsumeContext<IAppLaunched> context)
        {
            //remember our old color so we can reset it
            var oldForegroundColor = Console.ForegroundColor;

            //set the foreground color of the console with a random color
            Console.ForegroundColor = _colorGenerator.GetRandomColor();

            await Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("u")} Application [{context.Message.WhichApp}] was successfully launched at [{context.Message.When.ToString("u")}]");

            //reset the color
            Console.ForegroundColor = oldForegroundColor;
        }
    }
}
