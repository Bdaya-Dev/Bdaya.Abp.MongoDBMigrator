namespace Bdaya.Abp.MongoDBMigrator;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IBdayaAbpMongoDBDatabaseMigrator
{
    Task UpgradeDatabase(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    );
    Task DowngradeDatabase(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    );
    Task UpdateDatabaseToVersion(
        IBdayaAbpMigratableMongoDbContext context,
        int version,
        CancellationToken cancellationToken = default
    );

    Task MigrateFromConfig(
        IEnumerable<IBdayaAbpMigratableMongoDbContext> contexts,
        CancellationToken cancellationToken = default
    );
    Task MigrateFromConfig(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    );
}
