using Anabasis.EventStore.Snapshot.SQLServer;
using Microsoft.EntityFrameworkCore;

namespace Anabasis.Demo.Actor
{
    public class TradeSQLServerAggregateSnaphotDbContextFactory : BaseAggregateSnapshotDbContextFactory<TradeAggregateSnapshotDbContext>
    {
        protected override TradeAggregateSnapshotDbContext CreateDbContextInternal(SqlServerSnapshotStoreOptions sQLServerSnapshotStoreOptions)
        {
            return new TradeAggregateSnapshotDbContext(new DbContextOptionsBuilder().UseSqlServer(sQLServerSnapshotStoreOptions.ConnectionString).Options);
        }
    }

}
