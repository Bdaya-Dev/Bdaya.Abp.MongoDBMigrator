namespace Bdaya.Abp.MongoDBMigrator;
using Volo.Abp.MongoDB;

public static class IMigratableContextExtensions
{
    public static void ConfigureMigrations(this IMongoModelBuilder builder)
    {
        builder.Entity<MongoDBMigrationHistoryEntry>(e =>
        {
            e.CollectionName = "_MigrationHistory";
        });
    }
}
