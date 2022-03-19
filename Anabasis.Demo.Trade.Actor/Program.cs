using Anabasis.Api;
using Anabasis.Common;
using Anabasis.Demo.Actor;
using Anabasis.Demo.Common.Actor;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using Anabasis.EventStore.AspNet;
using Anabasis.EventStore.Snapshot;
using Anabasis.EventStore.Snapshot.SQLServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Anabasis.Demo.Common;

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

                        var isProduction = anabasisAppContext.Environment == AnabasisEnvironment.Production;

                        //no sqlserver in kube righ now
                        if (!isProduction)
                        {
                            var sqlServerSnapshotStoreOptions = serviceCollection.WithConfiguration<SqlServerSnapshotStoreOptions>(configurationRoot);
                            serviceCollection.AddSingleton<ISnapshotStore<Trade>, TradeSQLServerAggregateSnapshotStore>();
                            serviceCollection.AddSingleton<ISnapshotStrategy, DefaultSnapshotStrategy>();
                            serviceCollection.AddDbContextFactory<TradeAggregateSnapshotDbContext>(options => options.UseSqlServer(sqlServerSnapshotStoreOptions.ConnectionString));
                        }

                        var tradeCommandHandler = new DefaultEventTypeProvider<Trade>(() => new[] { 
                            typeof(CreateTrade),
                            typeof(ChangeTradeStatus),
                            typeof(ChangeTradeValue)
                        });

                        var tradeEventHandler = new DefaultEventTypeProvider<Trade>(() => new[] {
                            typeof(TradeCreated),
                            typeof(TradeStatusChanged),
                            typeof(TradeValueChanged)
                        });

                        serviceCollection.AddWorld(eventStoreConnectionOptions.ConnectionString, connectionSettingsBuilder)

                                        .AddEventStoreStatefulActor<TradeActor, Trade>(ActorConfiguration.Default)
                                            .WithReadAllFromStartCache(
                                                eventTypeProvider: tradeEventHandler,
                                                catchupEventStoreCacheConfigurationBuilder: (configuration) =>
                                                {
                                                    configuration.UseSnapshot = !isProduction;
                                                })
                                            .WithBus<IEventStoreBus>((actor, bus) =>
                                            {
                                                actor.SubscribeFromEndToAllStreams(eventTypeProvider: tradeCommandHandler);
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
