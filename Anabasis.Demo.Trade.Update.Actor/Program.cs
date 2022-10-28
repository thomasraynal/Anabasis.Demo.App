using Anabasis.Api;
using Anabasis.Common;
using Anabasis.Demo.Common.Actor;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using Anabasis.EventStore.AspNet;
using Microsoft.AspNetCore.Hosting;
using Anabasis.Demo.Common;
using Anabasis.RabbitMQ;
using Anabasis.EventStore.Cache;
using EventStore.ClientAPI;

namespace Anabasis.Demo
{
    class Program
    {
        static void Main(string[] _)
        {

            WebAppBuilder.Create<Program>(configureServiceCollection: (anabasisAppContext, serviceCollection, configurationRoot) =>
                    {
                        var (demoConfigurationOptions, connectionSettingsBuilder, eventStoreConnectionOptions) = serviceCollection.Bootstrap(configurationRoot);

                        var marketDataEventHandler = new DefaultEventTypeProvider<MarketData>(() => new[] {
                            typeof(MarketDataChanged)
                        });

                        var tradeEventHandler = new DefaultEventTypeProvider<Trade>(() => new[] {
                            typeof(TradeCreated),
                            typeof(TradeStatusChanged),
                            typeof(TradeValueChanged)
                        });

                        serviceCollection.AddWorld(eventStoreConnectionOptions.ConnectionString, connectionSettingsBuilder)

                                        .AddEventStoreStatefulActor<TradeValueUpdateService, Trade, AllStreamsCatchupCacheConfiguration > (
                                                eventTypeProvider: tradeEventHandler,
                                                getAggregateCacheConfiguration: (conf) => conf.Checkpoint = Position.Start)
                                            .WithBus<IEventStoreBus>((actor, bus) =>
                                            {
                                                actor.SubscribeToAllStreams(Position.End, eventTypeProvider: marketDataEventHandler);
                                            })
                                            .CreateActor()

                                        .AddStatelessActor<TradeGenerator>(ActorConfiguration.Default)
                                            .WithBus<IEventStoreBus>()
                                            .WithBus<IRabbitMqBus>((actor, bus) =>
                                                {
                                                    actor.SubscribeToExchange<MarketDataQuoteChanged>(StaticData.MarketDataBusOne);
                                                    actor.SubscribeToExchange<MarketDataQuoteChanged>(StaticData.MarketDataBusTwo);
                                                })
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