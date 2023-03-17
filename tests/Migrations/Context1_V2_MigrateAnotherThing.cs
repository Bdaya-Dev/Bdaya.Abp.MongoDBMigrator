namespace Bdaya.Abp.MongoDBMigrator.Tests.Migrations;

using Bdaya.Abp.MongoDBMigrator.Tests.Contexts;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

//[ExposeServices(typeof(IMongoDBVersionedMigrator))]
public class Context1_V2_MigrateAnotherThing
    : BdayaAbpMongoDBVersionedMigratorBase<ITestMongoDbContext1>,
        IScopedDependency
{
    public override int? BaseVersion { get; } = 2;
    public override string VersionName { get; } = "Migrate Another Thing";
}
