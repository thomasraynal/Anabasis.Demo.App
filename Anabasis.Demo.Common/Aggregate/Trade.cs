using Anabasis.Common;
using Anabasis.EventStore.Shared;
using System;

namespace Anabasis.Demo
{
    public class Trade : BaseAggregate
    {
        public Trade()
        {
        }

        public Trade(Trade trade, TradeStatus tradeStatus)
        {
            EntityId = trade.EntityId;
            Counterparty = trade.Counterparty;
            CurrencyPair = trade.CurrencyPair;
            Desk = trade.Desk;
            Status = tradeStatus;
            BuyOrSell = trade.BuyOrSell;
            MarketPrice = TradePrice = trade.TradePrice;
            Amount = trade.Amount;
            PercentFromMarket = trade.PercentFromMarket;
            Timestamp = trade.Timestamp;

        }

        public Trade(string tradeId, 
            string counterparty, 
            string desk, 
            string ccyPair, 
            TradeStatus status, 
            BuyOrSell buySell, 
            double tradePrice, 
            int amount)
        {
            EntityId = tradeId;
            Counterparty = counterparty;
            CurrencyPair = ccyPair;
            Desk = desk;
            Status = status;
            BuyOrSell = buySell;
            MarketPrice = TradePrice = tradePrice;
            Amount = amount;
            PercentFromMarket = 0;
            Timestamp = DateTime.UtcNow;
        }

        public Trade(string tradeId, 
            string counterparty, 
            string desk, 
            string ccyPair, 
            TradeStatus status, 
            BuyOrSell buySell,
            double tradePrice, 
            int amount, 
            DateTime timeStamp)
        {
            EntityId = tradeId;
            Counterparty = counterparty;
            Desk = desk;
            CurrencyPair = ccyPair;
            Status = status;
            BuyOrSell = buySell;
            MarketPrice = TradePrice = tradePrice;
            Amount = amount;
            PercentFromMarket = 0;
            Timestamp = timeStamp;
        }

        public string CurrencyPair { get; set; }
        public string Desk { get; set; }
        public string Counterparty { get; set; }
        public double TradePrice { get; set; }
        public double MarketPrice { get; set; }
        public double PercentFromMarket { get; set; }
        public double Amount { get; set; }
        public BuyOrSell BuyOrSell { get; set; }
        public TradeStatus Status { get; set; }
        public DateTime Timestamp { get; set; }



    }
}
