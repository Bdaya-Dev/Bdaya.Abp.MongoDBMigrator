namespace Bdaya.Abp.MongoDBMigrator.Tests.Contexts;

using Bdaya.Abp.MongoDBMigrator;
using Volo.Abp.MongoDB;
using MongoDB.Driver;
using Bdaya.Abp.MongoDBMigrator.Tests.Entities;
using Volo.Abp.Data;

[ConnectionStringName("Default")]
public class TestMongoDbContext2 : AbpMongoDbContext, ITestMongoDbContext2
{
    public string ContextId => "TestContext2";
    public IMongoCollection<MongoDBMigrationHistoryEntry> MigrationHistory =>
        Collection<MongoDBMigrationHistoryEntry>();

    public IMongoCollection<TestEntity3> Entity3 => Collection<TestEntity3>();
}
