namespace Bdaya.Abp.MongoDBMigrator.Tests.Migrations;

using Bdaya.Abp.MongoDBMigrator.Tests.Contexts;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

//[ExposeServices(typeof(IMongoDBVersionedMigrator))]
public class Context1_V0_Initial
    : MongoDBVersionedMigratorBase<ITestMongoDbContext1>,
        IScopedDependency
{
    public override int? BaseVersion => 0;
    public override string VersionName => "Initial";

    public override Task Down(ITestMongoDbContext1 context, IClientSessionHandle session)
    {
        throw new NotImplementedException();
    }
}
