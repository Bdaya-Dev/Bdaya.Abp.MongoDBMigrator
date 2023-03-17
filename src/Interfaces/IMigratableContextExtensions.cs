namespace Bdaya.Abp.MongoDBMigrator;
using Volo.Abp.MongoDB;

public static class IMigratableContextExtensions
{
    public static void ConfigureMigrations(this IMongoModelBuilder builder)
    {
        builder.Entity<BdayaAbpMongoDBMigrationHistoryEntry>(e =>
        {
            e.CollectionName = "_MigrationHistory";
        });
    }
}
