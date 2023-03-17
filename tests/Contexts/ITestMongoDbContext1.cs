namespace Bdaya.Abp.MongoDBMigrator.Tests.Contexts;

using Bdaya.Abp.MongoDBMigrator;
using Bdaya.Abp.MongoDBMigrator.Tests.Entities;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

[ConnectionStringName("Default")]
public interface ITestMongoDbContext1 : IAbpMongoDbContext, IBdayaAbpMigratableMongoDbContext
{
    IMongoCollection<TestEntity1> Entity1 { get; }
    IMongoCollection<TestEntity2> Entity2 { get; }
}
