namespace Bdaya.Abp.MongoDBMigrator;

using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

[ExposeServices(typeof(IBdayaAbpMongoDBVersionedMigrator))]
public abstract class BdayaAbpMongoDBVersionedMigratorBase<T>
    : IBdayaAbpMongoDBVersionedMigrator<T>,
        IScopedDependency
    where T : IBdayaAbpMigratableMongoDbContext
{
    public abstract int? BaseVersion { get; }
    public abstract string VersionName { get; }
    public Type ContextType => typeof(T);

    public virtual Task Down(T context, IClientSessionHandle session) => Task.CompletedTask;

    Task IBdayaAbpMongoDBVersionedMigrator.Down(
        IBdayaAbpMigratableMongoDbContext context,
        IClientSessionHandle session
    )
    {
        if (context is T casted)
        {
            return Down(casted, session);
        }
        return Task.CompletedTask;
    }

    public virtual Task DownTransactioned(T context, IClientSessionHandle session) =>
        Task.CompletedTask;

    Task IBdayaAbpMongoDBVersionedMigrator.DownTransactioned(
        IBdayaAbpMigratableMongoDbContext context,
        IClientSessionHandle session
    )
    {
        if (context is T casted)
        {
            return DownTransactioned(casted, session);
        }
        return Task.CompletedTask;
    }

    public virtual Task Up(T context, IClientSessionHandle session) => Task.CompletedTask;

    Task IBdayaAbpMongoDBVersionedMigrator.Up(
        IBdayaAbpMigratableMongoDbContext context,
        IClientSessionHandle session
    )
    {
        if (context is T casted)
        {
            return Up(casted, session);
        }
        return Task.CompletedTask;
    }

    public virtual Task UpTransactioned(T context, IClientSessionHandle session) =>
        Task.CompletedTask;

    Task IBdayaAbpMongoDBVersionedMigrator.UpTransactioned(
        IBdayaAbpMigratableMongoDbContext context,
        IClientSessionHandle session
    )
    {
        if (context is T casted)
        {
            return UpTransactioned(casted, session);
        }
        return Task.CompletedTask;
    }
}
