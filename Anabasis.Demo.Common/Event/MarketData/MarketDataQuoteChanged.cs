using Anabasis.RabbitMQ;
using Anabasis.RabbitMQ.Event;
using System;

namespace Anabasis.Demo.Common.Event
{
    public class MarketDataQuoteChanged : BaseRabbitMqEvent
    {
        public MarketDataQuoteChanged(Guid eventID, Guid correlationId) : base(eventID, correlationId)
        {

        }

        [RoutingPosition(0)]
        public string CurrencyPair { get; set; }

        public double Bid { get; set; }
        public double Offer { get; set; }

    }
}
