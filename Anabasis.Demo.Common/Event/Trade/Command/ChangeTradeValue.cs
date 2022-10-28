using Anabasis.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Event
{
    public class ChangeTradeValue : BaseCommand
    {
        public double MarketPrice { get; set; }
        public double PercentFromMarket { get; set; }

        public ChangeTradeValue(string tradeId, double marketPrice, double percentFromMarket, Guid correlationId) : base(tradeId, correlationId)
        {
            MarketPrice = marketPrice;
            PercentFromMarket = percentFromMarket;
        }
        public override string ToString()
        {
            return $"{nameof(ChangeTradeValue)} - {EntityId} - {String.Format("{0:P4}.", PercentFromMarket) }";
        }
    }
}
