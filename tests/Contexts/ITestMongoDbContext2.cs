namespace Bdaya.Abp.MongoDBMigrator.Tests.Contexts;

using Bdaya.Abp.MongoDBMigrator;
using Bdaya.Abp.MongoDBMigrator.Tests.Entities;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

[ConnectionStringName("Default")]
public interface ITestMongoDbContext2 : IAbpMongoDbContext, IAbpMigratableMongoDbContext
{
    IMongoCollection<TestEntity3> Entity3 { get; }
}
