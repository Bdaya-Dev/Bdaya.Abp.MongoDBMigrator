namespace Bdaya.Abp.MongoDBMigrator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

public class BdayaAbpNullMongoDBDatabaseMigtrator : IBdayaAbpMongoDBDatabaseMigrator
{
    public Task DowngradeDatabase(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }

    public Task MigrateFromConfig(
        IEnumerable<IBdayaAbpMigratableMongoDbContext> contexts,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }

    public Task MigrateFromConfig(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }

    public Task UpdateDatabaseToVersion(
        IBdayaAbpMigratableMongoDbContext context,
        int version,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }

    public Task UpgradeDatabase(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }
}
