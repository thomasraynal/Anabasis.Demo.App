using Anabasis.Api;
using Anabasis.Common;
using Anabasis.Demo.Common;
using Anabasis.Demo.Common.Actor;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using Anabasis.EventStore.AspNet;
using Anabasis.EventStore.Cache;
using EventStore.ClientAPI;
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

                    var marketDataEventHandler = new DefaultEventTypeProvider<MarketData>(() => new[] {
                                    typeof(MarketDataChanged)
                    });

                    serviceCollection.AddWorld(eventStoreConnectionOptions.ConnectionString, connectionSettingsBuilder)

                                    .AddEventStoreStatefulActor<TradeSink, Trade, AllStreamsCatchupCacheConfiguration>(
                                                tradeEventHandler,
                                                getAggregateCacheConfiguration: (configuration) =>
                                                {
                                                    configuration.Checkpoint = Position.Start;
                                                })
                                        .WithBus<IEventStoreBus>()
                                        .CreateActor()

                                     .AddEventStoreStatefulActor<MarketDataSink, MarketData, AllStreamsCatchupCacheConfiguration>(
                                                marketDataEventHandler,
                                                getAggregateCacheConfiguration: (configuration) =>
                                                {
                                                    configuration.Checkpoint = Position.Start;
                                                })
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
