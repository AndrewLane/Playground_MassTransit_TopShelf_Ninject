using MassTransit;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Configuration;
using System.Reflection;

namespace Core
{
    /// <summary>
    /// Helper class to do all the setup necessary for MassTransit that uses Ninject for its container
    /// </summary>
    public static class NinjectMassTransitBootstrapper
    {
        private static StandardKernel _kernel;

        /// <summary>
        /// Create the NInject kernel and, if appropriate, bind all the IConsumer objects in the passed assembly.
        /// Create an IBus object and bind all its variant interfaces (ISendEndpointProvider, IBus, IBusControl).
        /// Start the message bus and return it.
        /// </summary>
        public static IBusControl Bootstrap(MessagingPreference messagingPreference, Assembly assemblyWithConsumers = null)
        {
            _kernel = new StandardKernel();

            if (messagingPreference == MessagingPreference.Consumer && assemblyWithConsumers != null)
            {
                // register the consumers in the assemblyWithConsumers
                _kernel.Bind(x =>
                {
                    x.From(assemblyWithConsumers)
                    .SelectAllClasses()
                    .InheritedFrom<IConsumer>()
                    .BindToSelf();
                });
            }

            var bus = messagingPreference == MessagingPreference.Consumer ?
                //our convention for naming the queue will be to use the name of the assembly with the consumers
                CreateBus(hasReceiveEndPoint: true, queueNameFunc: () => assemblyWithConsumers.GetName().Name) :
                CreateBus(hasReceiveEndPoint: false);

            //From the MT docs: http://docs.masstransit-project.com/en/latest/migrating/
            //"Also, IBus is really just a collection of other interfaces. In this case, it’s unlikely that any part of the an application would ever need to
            //take a dependency on IBus directly, but should instead opt for a narrower interface, such as ISendEndpointProvider or IPublishEndpoint.
            //Each has a particular usefulness, but should only be used in cases where there is not an existing context which can be used."
            _kernel.Bind<ISendEndpointProvider, IBus, IBusControl>().ToMethod(x => bus);
            bus.Start();
            return bus;
        }

        /// <summary>
        /// Helper method to create our IBusControl object.  The configuration depends on whether we have a receive end point or not.  If we are consuming messages, we'll require a 
        /// Func which we'll invoke to figure out what to name the queue.
        /// </summary>
        private static IBusControl CreateBus(bool hasReceiveEndPoint, Func<string> queueNameFunc = null)
        {
            var rabbitMqInfo = GetRabbitMqInfoFromConfiguration();

            return Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var host = configurator.Host(new Uri($"rabbitmq://{rabbitMqInfo.Host}/"), h =>
                {
                    h.Username(rabbitMqInfo.Username);
                    h.Password(rabbitMqInfo.Password);
                });

                if (hasReceiveEndPoint)
                {
                    //convention is that the queue name will be the name of whatever assembly consumes the message
                    configurator.ReceiveEndpoint(host: host, queueName: queueNameFunc(), configure: endPointConfigurator =>
                    {
                        endPointConfigurator.LoadFrom(_kernel);

                        //retry policy is to retry 5 times, the first after 1 second, the next 2 seconds after that, the next 3 seconds after that, etc.
                        //with this policy, a poison message will take about 15 minutes to make it to the _error queue if there's no other congestion in the queue

                        //todo: make this configurable?
                        endPointConfigurator.UseRetry(Retry.Incremental(retryLimit: 5, initialInterval: TimeSpan.FromSeconds(1), intervalIncrement: TimeSpan.FromSeconds(1)));
                    });
                }
            });
        }

        /// <summary>
        /// Helper method to pull our RabbitMQ information from app settings
        /// </summary>
        private static RabbitMqInfo GetRabbitMqInfoFromConfiguration()
        {
            return new RabbitMqInfo
            {
                Host = GetAppSettingWithValidation("RabbitMQ.Host"),
                Username = GetAppSettingWithValidation("RabbitMQ.Username"),
                Password = GetAppSettingWithValidation("RabbitMQ.Password")
            };
        }

        /// <summary>
        /// Helper method to pull data out of app settings and make sure it's non-blank before returning it
        /// </summary>
        private static string GetAppSettingWithValidation(string appSetting)
        {
            var setting = ConfigurationManager.AppSettings[appSetting];

            //throw an exception if we're missing configuration data
            if (String.IsNullOrWhiteSpace(setting)) throw new ConfigurationErrorsException($"Missing {appSetting} configuration");

            return setting;
        }
    }
}
