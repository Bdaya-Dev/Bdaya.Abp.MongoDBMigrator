using System;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Bdaya.Abp.MongoDBMigrator.Tests;
using Microsoft.Extensions.DependencyInjection;
using Bdaya.Abp.MongoDBMigrator.Tests.Contexts;
using Bdaya.Abp.MongoDBMigrator;

namespace Bdaya.Digrum.Core.MongoDB;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAuthorizationModule),
    typeof(BdayaAbpMongoDBMigratorModule)
)]
public class MongoDbMigratorTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var stringArray = MongoDbFixture.ConnectionString.Split('?');
        var connectionString =
            stringArray[0].EnsureEndsWith('/')
            + "Db_"
            + Guid.NewGuid().ToString("N")
            + "/?"
            + stringArray[1];

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = connectionString;
        });
        context.Services.AddAlwaysAllowAuthorization();
        context.Services.AddMongoDbContext<TestMongoDbContext1>(options =>
        {
            options.AddDefaultRepositories<ITestMongoDbContext1>();
        });
        context.Services.AddMongoDbContext<TestMongoDbContext2>(options =>
        {
            options.AddDefaultRepositories<ITestMongoDbContext2>();
        });
    }
}
