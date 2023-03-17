namespace Bdaya.Abp.MongoDBMigrator;

using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

[DependsOn(typeof(AbpMongoDbModule))]
public class BdayaAbpMongoDBMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped(typeof(IMongoDBMigrator), typeof(DefaultMongoDBMigrator));
    }
}
