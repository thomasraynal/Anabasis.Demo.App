using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.Demo.Desktop
{
    public class MarketDataViewModel: ReactiveObject, IEquatable<MarketDataViewModel>
    {

        public MarketDataViewModel(MarketData marketData)
        {
            CcyPair = marketData.EntityId;
            Bid = marketData.Bid;
            Offer = marketData.Offer;
        }

        private string _ccyPair;
        public string CcyPair
        {
            get => _ccyPair;
            set => this.RaiseAndSetIfChanged(ref _ccyPair, value);
        }

        private double _bid;
        public double Bid
        {
            get => _bid;
            set => this.RaiseAndSetIfChanged(ref _bid, value);
        }

        private double _offer;
        public double Offer
        {   
            get => _offer;
            set => this.RaiseAndSetIfChanged(ref _offer, value);
        }

        public bool Equals(MarketDataViewModel other)
        {
            return CcyPair == other.CcyPair;
        }
    }
}
