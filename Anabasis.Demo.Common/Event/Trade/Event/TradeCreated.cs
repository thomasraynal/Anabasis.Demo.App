using Anabasis.Common;
using System;

namespace Anabasis.Demo.Common.Event
{
    public class TradeCreated : BaseAggregateEvent<Trade>
    {
        public TradeCreated(string tradeId, Guid correlationId) : base(tradeId, correlationId)
        {
        }

        public string CurrencyPair { get; set; }
        public double TradePrice { get; set; }
        public double MarketPrice { get; set; }
        public double PercentFromMarket { get; set; }
        public double Amount { get; set; }
        public BuyOrSell BuyOrSell { get; set; }
        public TradeStatus Status { get; set; }
        public string Desk { get; set; }
        public string Counterparty { get; set; }

        public override void Apply(Trade entity)
        {
           RandomlyThrownException.MaybeThrowSomething(0.001);

            entity.CurrencyPair = CurrencyPair;
            entity.Counterparty = Counterparty;
            entity.TradePrice = TradePrice;
            entity.MarketPrice = MarketPrice;
            entity.PercentFromMarket = PercentFromMarket;
            entity.Amount = Amount;
            entity.BuyOrSell = BuyOrSell;
            entity.Status = Status;
            entity.Desk = Desk;
            entity.Timestamp = Timestamp;
        }

        public override string ToString()
        {
            return $"{nameof(TradeCreated)}- {EntityId} - {CurrencyPair} - {Desk} - {Counterparty}";
        }
    }
}
