using Anabasis.Api;
using Anabasis.Common;
using Anabasis.Demo.Common.Actor;
using Anabasis.EventStore.AspNet;
using Microsoft.AspNetCore.Hosting;
using Anabasis.Demo.Common;
using Anabasis.RabbitMQ;
using Anabasis.EventStore;

namespace Anabasis.Demo
{
    class Program
    {
        static void Main(string[] _)
        {

            WebAppBuilder.Create<Program>(
                    configureServiceCollection: (anabasisAppContext, serviceCollection, configurationRoot) =>
                    {

                        var (demoConfigurationOptions, connectionSettingsBuilder, eventStoreConnectionOptions) = serviceCollection.Bootstrap(configurationRoot);

                        serviceCollection.AddWorld(eventStoreConnectionOptions.ConnectionString, connectionSettingsBuilder)

                                        .AddStatelessActor<MarketDataGenerator>(ActorConfiguration.Default)
                                            .WithBus<IRabbitMqBus>()
                                            .WithBus<IEventStoreBus>()
                                            .CreateActor();
                    },
                    configureApplicationBuilder: (anabasisAppContext, app) =>
                    {
                        app.UseWorld();
                    })
                    .Build()
                    .Run();
        }
    }
}
