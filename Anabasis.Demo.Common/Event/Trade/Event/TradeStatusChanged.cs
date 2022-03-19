using Anabasis.Common;
using Anabasis.EventStore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Event
{
    public class TradeStatusChanged : BaseAggregateEvent<Trade>
    {
        public TradeStatus Status { get; set; }

        public TradeStatusChanged(string tradeId, Guid correlationId) : base(tradeId, correlationId)
        {
        }

        public override void Apply(Trade entity)
        {
            RandomlyThrownException.MaybeThrowSomething(0.001);

            entity.Status = Status;
        }

        public override string ToString()
        {
            return $"{nameof(TradeStatusChanged)} - {EntityId} - {Status}";
        }
    }
}
