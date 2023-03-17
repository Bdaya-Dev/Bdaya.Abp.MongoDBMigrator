namespace Bdaya.Abp.MongoDBMigrator;
using System.Threading;
using System.Threading.Tasks;

public interface IMongoDBMigrator
{
    public Task UpgradeDatabase(
        IAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    );
    public Task DowngradeDatabase(
        IAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    );
    public Task UpdateDatabaseToVersion(
        IAbpMigratableMongoDbContext context,
        int version,
        CancellationToken cancellationToken = default
    );
}
