namespace Bdaya.Abp.MongoDBMigrator;

using MongoDB.Driver;
using Volo.Abp.MongoDB;

public interface IAbpMigratableMongoDbContext : IAbpMongoDbContext
{
    string ContextId { get; }
    IMongoCollection<MongoDBMigrationHistoryEntry> MigrationHistory { get; }
}
