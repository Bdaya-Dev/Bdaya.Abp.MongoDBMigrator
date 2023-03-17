namespace Bdaya.Abp.MongoDBMigrator.Tests.Contexts;

using Bdaya.Abp.MongoDBMigrator;
using Volo.Abp.MongoDB;
using MongoDB.Driver;
using Bdaya.Abp.MongoDBMigrator.Tests.Entities;
using Volo.Abp.Data;

[ConnectionStringName("Default")]
public class TestMongoDbContext1 : AbpMongoDbContext, ITestMongoDbContext1
{
    public string ContextId => "TestContext1";
    public IMongoCollection<MongoDBMigrationHistoryEntry> MigrationHistory =>
        Collection<MongoDBMigrationHistoryEntry>();

    public IMongoCollection<TestEntity1> Entity1 => Collection<TestEntity1>();
    public IMongoCollection<TestEntity2> Entity2 => Collection<TestEntity2>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);
        modelBuilder.ConfigureMigrations();
    }
}
