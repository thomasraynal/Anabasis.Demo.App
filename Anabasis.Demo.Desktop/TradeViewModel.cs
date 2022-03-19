using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.Demo.Desktop
{
    public class TradeViewModel : ReactiveObject, IEquatable<TradeViewModel>
    {

        public TradeViewModel(Trade trade)
        {
            TradeId = trade.EntityId;
            Counterparty = trade.Counterparty;
            Desk = trade.Desk;
            CurrencyPair = trade.CurrencyPair;
            Status = trade.Status;
            MarketPrice = trade.MarketPrice;
            TradePrice = trade.TradePrice;
            Amount = trade.Amount;
            PercentFromMarket = trade.PercentFromMarket;
            Timestamp = trade.Timestamp;

            var position = 0;

            TradeEvents = trade.AppliedEvents.Select(appliedEvent => new TradeEventViewModel(position++, appliedEvent)).ToArray();

        }

        private TradeEventViewModel[] _tradeEventViewModels;
        public TradeEventViewModel[] TradeEvents
        {
            get => _tradeEventViewModels;
            set => this.RaiseAndSetIfChanged(ref _tradeEventViewModels, value);
        }

        private string _tradeId;
        public string TradeId
        {
            get => _tradeId;
            set => this.RaiseAndSetIfChanged(ref _tradeId, value);
        }

        private string _counterparty;
        public string Counterparty
        {
            get => _counterparty;
            set => this.RaiseAndSetIfChanged(ref _counterparty, value);
        }

        private string _desk;
        public string Desk
        {
            get => _desk;
            set => this.RaiseAndSetIfChanged(ref _desk, value);
        }

        private string _currencyPair;
        public string CurrencyPair
        {
            get => _currencyPair;
            set => this.RaiseAndSetIfChanged(ref _currencyPair, value);
        }

        private TradeStatus _status;
        public TradeStatus Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }

        private BuyOrSell _buyOrSell;
        public BuyOrSell BuyOrSell
        {
            get => _buyOrSell;
            set => this.RaiseAndSetIfChanged(ref _buyOrSell, value);
        }

        private double _marketPrice;
        public double MarketPrice
        {
            get => _marketPrice;
            set => this.RaiseAndSetIfChanged(ref _marketPrice, value);
        }

        private double _tradePrice;
        public double TradePrice
        {
            get => _tradePrice;
            set => this.RaiseAndSetIfChanged(ref _tradePrice, value);
        }

        private double _amount;
        public double Amount
        {
            get => _amount;
            set => this.RaiseAndSetIfChanged(ref _amount, value);
        }

        private double _percentFromMarket;
        public double PercentFromMarket
        {
            get => _percentFromMarket;
            set => this.RaiseAndSetIfChanged(ref _percentFromMarket, value);
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get => _timestamp;
            set => this.RaiseAndSetIfChanged(ref _timestamp, value);
        }

        public bool Equals(TradeViewModel other)
        {
            return this.TradeId == other.TradeId;
        }
    }
}
