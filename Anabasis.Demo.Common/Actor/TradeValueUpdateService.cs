using Anabasis.Common;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using Anabasis.EventStore.Actor;
using Anabasis.EventStore.Factories;
using Anabasis.EventStore.Repository;
using Anabasis.EventStore.Snapshot;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Actor
{
    public class TradeValueUpdateService : BaseEventStoreStatefulActor<Trade>
    {
        public TradeValueUpdateService(IActorConfiguration actorConfiguration, IAggregateCache<Trade> eventStoreCache, ILoggerFactory loggerFactory = null) : base(actorConfiguration, eventStoreCache, loggerFactory)
        {
        }

        public TradeValueUpdateService(IEventStoreActorConfigurationFactory eventStoreCacheFactory, IConnectionStatusMonitor<IEventStoreConnection> connectionStatusMonitor, ISnapshotStore<Trade> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, ILoggerFactory loggerFactory = null) : base(eventStoreCacheFactory, connectionStatusMonitor, snapshotStore, snapshotStrategy, loggerFactory)
        {
        }

        public async Task Handle(MarketDataChanged marketDataChanged)
        {

            if (!State.IsCaughtUp) return;

            foreach (var trade in State.GetCurrents().Where(trade => trade.CurrencyPair == marketDataChanged.EntityId))
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
