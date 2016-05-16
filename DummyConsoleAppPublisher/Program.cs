using Core;
using Core.Messages;
using System;
using System.Threading.Tasks;

namespace DummyConsoleAppPublisher
{
    /// <summary>
    /// Dummy console app that will publish a bunch of ISomeEvent messages in a loop, with some delay in between each publish
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //ask the user how long to wait in between publishing each message
            Console.WriteLine("Enter milliseconds to wait between publishes (must be positive):");
            var delayInput = Console.ReadLine();
            int millisecondsBetweenPublishes;
            if (int.TryParse(delayInput, out millisecondsBetweenPublishes) == false || millisecondsBetweenPublishes <= 0)
            {
                Console.WriteLine("Bad input received, so using default of 1000 milliseconds");
                millisecondsBetweenPublishes = 1000;
            }

            //ask the user many messages to publish before dying
            Console.WriteLine("Enter max messages to publish (use a negative number for an infinite loop):");
            var maxMessagesInput = Console.ReadLine();
            int maxMessagesToPublish;
            if (int.TryParse(maxMessagesInput, out maxMessagesToPublish) == false)
            {
                Console.WriteLine("Bad input received, so assuming infinite loop.");
                maxMessagesToPublish = -1;
            }

            //ask the user what the payload of the message should be
            Console.WriteLine("Enter a name for your messages.");
            var str = Console.ReadLine();

            //bootstrap our message bus that will be publishing messages only
            var bus = NinjectMassTransitBootstrapper.Bootstrap(MessagingPreference.PublisherOnly);

            int counter = 0;
            while (true)
            {
                counter++;
                var eventToPublish = new SomeEvent { What = $"{str} #{counter}", When = DateTime.UtcNow };
                bus.Publish(eventToPublish);

                Console.WriteLine($"{DateTime.UtcNow.ToString("u")} Just published [{eventToPublish.What}]");
                Task.Delay(millisecondsBetweenPublishes).Wait();

                //see if we're done
                if (maxMessagesToPublish > 0 && counter >= maxMessagesToPublish) break;
            }

            Console.WriteLine("Attempting to stop bus...");
            bus.Stop();
            Console.WriteLine("Exiting");
        }
    }
}
