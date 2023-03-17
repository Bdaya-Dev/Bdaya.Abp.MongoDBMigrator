namespace Bdaya.Abp.MongoDBMigrator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

public class NullMongoDBMigtrator : IMongoDBMigrator
{
    public Task DowngradeDatabase(
        IAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }

    public Task UpdateDatabaseToVersion(
        IAbpMigratableMongoDbContext context,
        int version,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }

    public Task UpgradeDatabase(
        IAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    )
    {
        return Task.CompletedTask;
    }
}
