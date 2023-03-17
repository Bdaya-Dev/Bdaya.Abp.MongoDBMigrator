namespace Bdaya.Abp.MongoDBMigrator;

using MongoDB.Driver;
using Volo.Abp.MongoDB;

public interface IBdayaAbpMigratableMongoDbContext : IAbpMongoDbContext
{
    string ContextId { get; }
    IMongoCollection<BdayaAbpMongoDBMigrationHistoryEntry> MigrationHistory { get; }
}
