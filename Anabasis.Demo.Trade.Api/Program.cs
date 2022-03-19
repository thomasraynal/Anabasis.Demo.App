using Anabasis.Api;
using Anabasis.Common;
using Anabasis.Demo.Common;
using Anabasis.Demo.Common.Actor;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore.AspNet;
using Microsoft.AspNetCore.Hosting;

namespace Anabasis.Demo.Api
{
    class Program
    {
        static void Main(string[] _)
        {
            WebAppBuilder.Create<Program>(
                configureServiceCollection: (anabasisAppContext, serviceCollection, configurationRoot) =>
                {

                    var (demoConfigurationOptions, connectionSettingsBuilder, eventStoreConnectionOptions) = serviceCollection.Bootstrap(configurationRoot);

                    var tradeEventHandler = new DefaultEventTypeProvider<Trade>(() => new[] {
                                    typeof(TradeCreated),
                                    typeof(TradeStatusChanged),
                                    typeof(TradeValueChanged)
                    });

                    serviceCollection.AddWorld(eventStoreConnectionOptions.ConnectionString, connectionSettingsBuilder)

                                    .AddEventStoreStatefulActor<TradeSink, Trade>(ActorConfiguration.Default)
                                        .WithReadAllFromStartCache(eventTypeProvider: tradeEventHandler)
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
