using Anabasis.Common;
using Anabasis.EventStore.Shared;
using System;

namespace Anabasis.Demo.Common.Event
{
    public class TradeValueChanged : BaseAggregateEvent<Trade>
    {
        public double MarketPrice { get; set; }
        public double PercentFromMarket { get; set; }

        public TradeValueChanged(string tradeId, Guid correlationId) : base(tradeId, correlationId)
        {
        }

        public override void Apply(Trade entity)
        {
            RandomlyThrownException.MaybeThrowSomething(0.001);

            entity.MarketPrice = MarketPrice;
            entity.PercentFromMarket = PercentFromMarket;
        }

        public override string ToString()
        {
            return $"{nameof(TradeValueChanged)} - {EntityId} - {String.Format("{0:P2}.", PercentFromMarket) }";
        }
    }
}
