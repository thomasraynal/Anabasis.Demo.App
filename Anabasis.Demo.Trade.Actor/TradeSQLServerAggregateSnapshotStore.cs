using Anabasis.Common;
using Anabasis.EventStore.Snapshot.SQLServer;
using Microsoft.EntityFrameworkCore;

namespace Anabasis.Demo.Actor
{
    public class TradeSQLServerAggregateSnapshotStore : BaseEntityFrameworkSnapshotStore<Trade, TradeAggregateSnapshotDbContext, AggregateSnapshot>
    {
        public TradeSQLServerAggregateSnapshotStore(IDbContextFactory<TradeAggregateSnapshotDbContext> aggregateSnapshotDbContextFactory) : base(aggregateSnapshotDbContextFactory)
        {
        }
    }
}
