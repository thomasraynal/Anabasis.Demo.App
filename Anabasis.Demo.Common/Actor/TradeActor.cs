using Anabasis.Common;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using Anabasis.EventStore.Actor;
using Anabasis.EventStore.Factories;
using Anabasis.EventStore.Repository;
using Anabasis.EventStore.Snapshot;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Actor
{
    public class TradeActor : BaseEventStoreStatefulActor<Trade>
    {
        public TradeActor(IActorConfiguration actorConfiguration, IAggregateCache<Trade> eventStoreCache, ILoggerFactory loggerFactory = null) : base(actorConfiguration, eventStoreCache, loggerFactory)
        {
        }

        public TradeActor(IEventStoreActorConfigurationFactory eventStoreCacheFactory, IConnectionStatusMonitor<IEventStoreConnection> connectionStatusMonitor, ISnapshotStore<Trade> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, ILoggerFactory loggerFactory = null) : base(eventStoreCacheFactory, connectionStatusMonitor, snapshotStore, snapshotStrategy, loggerFactory)
        {
        }

        public async Task Handle(CreateTrade createTrade)
        {
            var tradeCreated = new TradeCreated(createTrade.EntityId, createTrade.CorrelationId)
            {
                Counterparty = createTrade.Counterparty,
                CurrencyPair = createTrade.CurrencyPair,
                Desk = createTrade.Desk,
                BuyOrSell = createTrade.BuyOrSell,
                MarketPrice = createTrade.MarketPrice,
                TradePrice = createTrade.TradePrice,
                Amount = createTrade.Amount,
                Status = TradeStatus.Live
                
            };

            Logger.LogInformation($"{tradeCreated}");

            await this.EmitEventStore(tradeCreated);
        }

        public async Task Handle(ChangeTradeStatus updateTradeStatus)
        {
            var tradeCreated = new TradeStatusChanged(updateTradeStatus.EntityId, updateTradeStatus.CorrelationId)
            {
                Status = updateTradeStatus.Status
            };

            Logger.LogInformation($"{tradeCreated}");

            await this.EmitEventStore(tradeCreated);
        }

        public async Task Handle(ChangeTradeValue updateTradeValue)
        {
            var tradeValueChanged = new TradeValueChanged(updateTradeValue.EntityId, updateTradeValue.CorrelationId)
            {
                MarketPrice = updateTradeValue.MarketPrice,
                PercentFromMarket = updateTradeValue.PercentFromMarket
            };

            Logger.LogInformation($"{tradeValueChanged}");

            await this.EmitEventStore(tradeValueChanged);
        }
    }
}
