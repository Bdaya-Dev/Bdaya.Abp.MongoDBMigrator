namespace Bdaya.Abp.MongoDBMigrator.Tests.Migrations;

using Bdaya.Abp.MongoDBMigrator.Tests.Contexts;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

//[ExposeServices(typeof(IMongoDBVersionedMigrator))]
public class Context2_V0_Initial
    : BdayaAbpMongoDBVersionedMigratorBase<ITestMongoDbContext2>,
        IScopedDependency
{
    public override int? BaseVersion => 0;
    public override string VersionName => "Initial";

    public override Task Down(ITestMongoDbContext2 context, IClientSessionHandle session)
    {
        throw new NotImplementedException();
    }
}
