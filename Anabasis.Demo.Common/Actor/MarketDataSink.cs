using Anabasis.Common;
using Anabasis.Common.Configuration;
using Anabasis.EventStore.Actor;
using Anabasis.EventStore.Cache;
using Anabasis.EventStore.Snapshot;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;

namespace Anabasis.Demo.Common.Actor
{
    public class MarketDataSink : SubscribeToAllStreamsEventStoreStatefulActor<MarketData>
    {
        public MarketDataSink(IActorConfigurationFactory actorConfigurationFactory, IConnectionStatusMonitor<IEventStoreConnection> connectionMonitor, ILoggerFactory loggerFactory = null, ISnapshotStore<MarketData> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, IKillSwitch killSwitch = null) : base(actorConfigurationFactory, connectionMonitor, loggerFactory, snapshotStore, snapshotStrategy, killSwitch)
        {
        }

        public MarketDataSink(IActorConfiguration actorConfiguration, IConnectionStatusMonitor<IEventStoreConnection> connectionMonitor, AllStreamsCatchupCacheConfiguration catchupCacheConfiguration, IEventTypeProvider eventTypeProvider, ILoggerFactory loggerFactory = null, ISnapshotStore<MarketData> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, IKillSwitch killSwitch = null) : base(actorConfiguration, connectionMonitor, catchupCacheConfiguration, eventTypeProvider, loggerFactory, snapshotStore, snapshotStrategy, killSwitch)
        {
        }
    }
}
