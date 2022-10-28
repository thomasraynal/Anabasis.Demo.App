using Anabasis.Common;
using Anabasis.Common.Configuration;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using Anabasis.EventStore.Actor;
using Anabasis.EventStore.Cache;
using Anabasis.EventStore.Snapshot;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Actor
{
    public class TradeValueUpdateService : SubscribeToAllStreamsEventStoreStatefulActor<Trade>
    {
        public TradeValueUpdateService(IActorConfigurationFactory actorConfigurationFactory, IConnectionStatusMonitor<IEventStoreConnection> connectionMonitor, ILoggerFactory loggerFactory = null, ISnapshotStore<Trade> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, IKillSwitch killSwitch = null) : base(actorConfigurationFactory, connectionMonitor, loggerFactory, snapshotStore, snapshotStrategy, killSwitch)
        {
        }

        public TradeValueUpdateService(IActorConfiguration actorConfiguration, IConnectionStatusMonitor<IEventStoreConnection> connectionMonitor, AllStreamsCatchupCacheConfiguration catchupCacheConfiguration, IEventTypeProvider eventTypeProvider, ILoggerFactory loggerFactory = null, ISnapshotStore<Trade> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, IKillSwitch killSwitch = null) : base(actorConfiguration, connectionMonitor, catchupCacheConfiguration, eventTypeProvider, loggerFactory, snapshotStore, snapshotStrategy, killSwitch)
        {
        }

        public async Task Handle(MarketDataChanged marketDataChanged)
        {

            if (!IsCaughtUp) return;

            foreach (var trade in GetCurrents().Where(trade => trade.CurrencyPair == marketDataChanged.EntityId))
            {
                var marketPrice = marketDataChanged.Bid;

                var percentFromMarket = Math.Round(((trade.TradePrice - marketPrice) / marketPrice), 4);
                var changeTradeValue = new ChangeTradeValue(trade.EntityId, marketPrice, percentFromMarket, marketDataChanged.CorrelationId);

                Logger.LogInformation($"{changeTradeValue}");

                await this.EmitEventStore(changeTradeValue);

            }

        }
    }
}
