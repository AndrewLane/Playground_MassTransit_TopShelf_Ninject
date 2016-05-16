using System.Reflection;
using Topshelf;
using MassTransit;
using Core;

namespace DummyConsumerService
{
    /// <summary>
    /// TopShelf service that hosts our dummy Windows service that consumes messages
    /// </summary>
    public class Program
    {
        public static int Main()
        {
            return (int)HostFactory.Run(cfg => cfg.Service(x => new DummyConsumerService()));
        }
    }

    internal class DummyConsumerService : ServiceControl
    {
        IBusControl _busControl;

        public bool Start(HostControl hostControl)
        {
            //bootstrap MassTransit to consume messages from this service
            _busControl = NinjectMassTransitBootstrapper.Bootstrap(MessagingPreference.Consumer, Assembly.GetExecutingAssembly());
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _busControl?.Stop();
            return true;
        }
    }
}
