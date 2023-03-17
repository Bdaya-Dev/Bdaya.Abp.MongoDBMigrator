using Volo.Abp.Testing;
using Volo.Abp;
using Bdaya.Digrum.Core.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Uow;

namespace Bdaya.Abp.MongoDBMigrator.Tests;

public abstract class MongoDbTestBase : AbpIntegratedTest<MongoDbMigratorTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected virtual async Task WithUnitOfWorkAsync(Func<Task> action)
    {
        await WithUnitOfWorkAsync(new() { }, action);
    }

    protected virtual async Task WithUnitOfWorkAsync(
        AbpUnitOfWorkOptions options,
        Func<Task> action
    )
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            using (var uow = uowManager.Begin(options))
            {
                await action();

                await uow.CompleteAsync();
            }
        }
    }
}
