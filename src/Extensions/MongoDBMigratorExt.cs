namespace Microsoft.Extensions.DependencyInjection;

using Bdaya.Abp.MongoDBMigrator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

public static class MongoDBMigratorDependencyInjectionExt
{
    //private static async Task WrapMigratorAndContext<TContext>(
    //    this IServiceProvider serviceProvider,
    //    Func<TContext, IMongoDBMigrator, CancellationToken, Task> action
    //)
    //    where TContext : IAbpMigratableMongoDbContext
    //{
    //    var cancellationTokenProvider =
    //        serviceProvider.GetRequiredService<ICancellationTokenProvider>();
    //    using var scope = serviceProvider.CreateScope();
    //    var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    //    using var uow = uowManager.Begin(new AbpUnitOfWorkOptions() { });

    //    var migrator = serviceProvider.GetRequiredService<IMongoDBMigrator>();
    //    //using var scope = context.ServiceProvider.CreateAsyncScope();
    //    var dbContextProvider = serviceProvider.GetRequiredService<
    //        IMongoDbContextProvider<TContext>
    //    >();
    //    var dbContext = await dbContextProvider.GetDbContextAsync(cancellationTokenProvider.Token);
    //    //Upgrade to latest version
    //    await action(dbContext, migrator, cancellationTokenProvider.Token);

    //    await uow.CompleteAsync();
    //}

    //public static async Task MigrateContextAsync<TContext>(this IServiceProvider serviceProvider)
    //    where TContext : IAbpMigratableMongoDbContext
    //{
    //    await serviceProvider.WrapMigratorAndContext<TContext>(
    //        async (context, migrator, cancellationToken) =>
    //        {
    //            await migrator.UpgradeDatabase(context, cancellationToken);
    //        }
    //    );
    //}

}
