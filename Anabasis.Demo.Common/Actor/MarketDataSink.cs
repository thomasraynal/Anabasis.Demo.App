using Anabasis.Common;
using Anabasis.EventStore.Actor;
using Anabasis.EventStore.Factories;
using Anabasis.EventStore.Snapshot;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;

namespace Anabasis.Demo.Common.Actor
{
    public class MarketDataSink : BaseEventStoreStatefulActor<MarketData>
    {
        public MarketDataSink(IActorConfiguration actorConfiguration, IAggregateCache<MarketData> eventStoreCache, ILoggerFactory loggerFactory = null) : base(actorConfiguration, eventStoreCache, loggerFactory)
        {
        }

        public MarketDataSink(IEventStoreActorConfigurationFactory eventStoreCacheFactory, IConnectionStatusMonitor<IEventStoreConnection> connectionStatusMonitor, ISnapshotStore<MarketData> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, ILoggerFactory loggerFactory = null) : base(eventStoreCacheFactory, connectionStatusMonitor, snapshotStore, snapshotStrategy, loggerFactory)
        {
        }
    }
}
