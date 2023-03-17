namespace Bdaya.Abp.MongoDBMigrator;

using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

[ExposeServices(typeof(IMongoDBVersionedMigrator))]
public abstract class MongoDBVersionedMigratorBase<T>
    : IMongoDBVersionedMigrator<T>,
        IScopedDependency
    where T : IAbpMigratableMongoDbContext
{
    public abstract int? BaseVersion { get; }
    public abstract string VersionName { get; }
    public Type ContextType => typeof(T);

    public virtual Task Down(T context, IClientSessionHandle session) => Task.CompletedTask;

    Task IMongoDBVersionedMigrator.Down(
        IAbpMigratableMongoDbContext context,
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

    Task IMongoDBVersionedMigrator.DownTransactioned(
        IAbpMigratableMongoDbContext context,
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

    Task IMongoDBVersionedMigrator.Up(
        IAbpMigratableMongoDbContext context,
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

    Task IMongoDBVersionedMigrator.UpTransactioned(
        IAbpMigratableMongoDbContext context,
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
