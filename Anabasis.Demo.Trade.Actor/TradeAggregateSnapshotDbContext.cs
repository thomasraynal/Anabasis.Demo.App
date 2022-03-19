using Anabasis.Common;
using Anabasis.EventStore.Snapshot.SQLServer;
using Microsoft.EntityFrameworkCore;

namespace Anabasis.Demo.Actor
{
    public class TradeAggregateSnapshotDbContext : BaseAggregateSnapshotDbContext<AggregateSnapshot>
    {
        public TradeAggregateSnapshotDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
