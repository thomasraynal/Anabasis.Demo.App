using Anabasis.Common;
using Anabasis.Common.Configuration;
using Anabasis.Demo.Common.Event;
using Anabasis.EventStore;
using Anabasis.EventStore.Actor;
using Anabasis.EventStore.Cache;
using Anabasis.EventStore.Snapshot;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Actor
{
    public class TradeActor : SubscribeToAllStreamsEventStoreStatefulActor<Trade>
    {
        public TradeActor(IActorConfigurationFactory actorConfigurationFactory, IConnectionStatusMonitor<IEventStoreConnection> connectionMonitor, ILoggerFactory loggerFactory = null, ISnapshotStore<Trade> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, IKillSwitch killSwitch = null) : base(actorConfigurationFactory, connectionMonitor, loggerFactory, snapshotStore, snapshotStrategy, killSwitch)
        {
        }

        public TradeActor(IActorConfiguration actorConfiguration, IConnectionStatusMonitor<IEventStoreConnection> connectionMonitor, AllStreamsCatchupCacheConfiguration catchupCacheConfiguration, IEventTypeProvider eventTypeProvider, ILoggerFactory loggerFactory = null, ISnapshotStore<Trade> snapshotStore = null, ISnapshotStrategy snapshotStrategy = null, IKillSwitch killSwitch = null) : base(actorConfiguration, connectionMonitor, catchupCacheConfiguration, eventTypeProvider, loggerFactory, snapshotStore, snapshotStrategy, killSwitch)
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
