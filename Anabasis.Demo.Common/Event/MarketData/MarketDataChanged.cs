using Anabasis.Common;
using Anabasis.EventStore.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.Demo.Common.Event
{
    public class MarketDataChanged : BaseAggregateEvent<MarketData>
    {
        public double Bid { get; set; }
        public double Offer { get; set; }

        public MarketDataChanged(string ccyPair, Guid correlationId) : base(ccyPair, correlationId)
        {
        }

        public override void Apply(MarketData entity)
        {
            entity.Bid = Bid;
            entity.Offer = Offer;
        }
    }
}
