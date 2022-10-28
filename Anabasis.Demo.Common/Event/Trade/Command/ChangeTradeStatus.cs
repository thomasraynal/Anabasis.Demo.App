using Anabasis.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.Demo.Common.Event
{
    public class ChangeTradeStatus : BaseCommand
    {
        public TradeStatus Status { get; set; }

        public ChangeTradeStatus(string tradeId, TradeStatus tradeStatus, Guid correlationId) : base(tradeId, correlationId)
        {
            Status = tradeStatus;
        }

        public override string ToString()
        {
            return $"{nameof(ChangeTradeStatus)} - {EntityId} - {Status}";
        }

    }
}
