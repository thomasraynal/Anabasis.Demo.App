using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace Anabasis.Demo.Desktop
{
    public class CurrencyPairPositionViewModel : ReactiveObject, IDisposable
    {
        private readonly IDisposable _cleanUp;
        private TradesPosition _position;

        public CurrencyPairPositionViewModel(IGroup<Trade, string, string> tradesByCurrencyPair)
        {
            CcyPair = tradesByCurrencyPair.Key;

            _cleanUp = tradesByCurrencyPair.Cache.Connect()
                .ToCollection()
                .Select(query =>
                {
                    var buy = query.Where(trade => trade.BuyOrSell == BuyOrSell.Buy).Sum(trade => trade.Amount);
                    var sell = query.Where(trade => trade.BuyOrSell == BuyOrSell.Sell).Sum(trade => trade.Amount);
                    var count = query.Count;

                    return new TradesPosition(buy, sell, count);
                })
                .Subscribe(position => Position = position);
        }

        public TradesPosition Position
        {
            get => _position;
            set => this.RaiseAndSetIfChanged(ref _position, value);
        }

        public string CcyPair { get; }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
