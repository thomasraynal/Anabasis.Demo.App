using Anabasis.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Event
{
    public class CreateTrade : BaseCommand
    {
        public CreateTrade(string tradeId,
            string counterparty,
            string ccyPair,
            string desk,
            BuyOrSell buyOrSell,
            double marketPrice,
            double tradePrice,
            double amount,
            Guid correlationId
            ) : base(correlationId, tradeId)
        {
            Counterparty = counterparty;
            Desk = desk;
            CurrencyPair = ccyPair;
            BuyOrSell = buyOrSell;
            MarketPrice =  marketPrice;
            TradePrice = tradePrice;
            Amount = amount;
        }

        public string CurrencyPair { get; set; }
        public string Desk { get; set; }
        public string Counterparty { get; set; }
        public double TradePrice { get; set; }
        public double MarketPrice { get; set; }
        public double Amount { get; set; }
        public BuyOrSell BuyOrSell { get; set; }
        public TradeStatus Status { get; set; }


        public override string ToString()
        {
            return $"{nameof(CreateTrade)}- {EntityId} - {CurrencyPair} - {Desk} - {Counterparty}";
        }
    }
}
