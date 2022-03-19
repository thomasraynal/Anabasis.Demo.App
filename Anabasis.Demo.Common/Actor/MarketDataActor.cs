using Anabasis.Common;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using Anabasis.EventStore.Actor;
using Anabasis.EventStore.Factories;
using Anabasis.EventStore.Snapshot;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Actor
{
    public class MarketDataActor : BaseEventStoreStatefulActor<MarketData>
    {
        public MarketDataActor(IActorConfiguration actorConfiguration, IAggregateCache<MarketData> eventStoreCache, ILoggerFactory loggerFactory = null) : base(actorConfiguration, eventStoreCache, loggerFactory)
        {
        }

        public MarketDataActor(IEventStoreActorConfigurationFactory eventStoreCacheFactory, IConnectionStatusMonitor<IEventStoreConnection> connectionStatusMonitor, ISnapshotStore<MarketData> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, ILoggerFactory loggerFactory = null) : base(eventStoreCacheFactory, connectionStatusMonitor, snapshotStore, snapshotStrategy, loggerFactory)
        {
        }

        public async Task Handle(MarketDataQuoteChanged marketDataQuoteChanged)
        {
            var marketDataChanged = new MarketDataChanged(marketDataQuoteChanged.CurrencyPair, marketDataQuoteChanged.CorrelationId)
            {
                Bid = marketDataQuoteChanged.Bid,
                Offer = marketDataQuoteChanged.Offer,
            };

            Logger.LogInformation($"{marketDataChanged}");

            await this.EmitEventStore(marketDataChanged);
        }
    }
}
