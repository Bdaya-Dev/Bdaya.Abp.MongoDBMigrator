namespace Bdaya.Abp.MongoDBMigrator;

using MongoDB.Driver;
using System;
using System.Threading.Tasks;

public interface IMongoDBVersionedMigrator
{
    int? BaseVersion { get; }
    string? VersionName { get; }
    Type ContextType { get; }

    /// <summary>
    /// Migrates from BaseVersion - 1 to BaseVersion
    /// </summary>
    /// <returns></returns>
    Task Up(IAbpMigratableMongoDbContext context, IClientSessionHandle session);

    /// <summary>
    /// Migrates from BaseVersion - 1 to BaseVersion within a transaction
    /// </summary>
    /// <returns></returns>
    Task UpTransactioned(IAbpMigratableMongoDbContext context, IClientSessionHandle session);

    /// <summary>
    /// Migrates from BaseVersion to BaseVersion - 1
    /// </summary>
    /// <returns></returns>
    Task Down(IAbpMigratableMongoDbContext context, IClientSessionHandle session);

    /// <summary>
    /// Migrates from BaseVersion to BaseVersion - 1
    /// </summary>
    /// <returns></returns>
    Task DownTransactioned(IAbpMigratableMongoDbContext context, IClientSessionHandle session);
}

public interface IMongoDBVersionedMigrator<T> : IMongoDBVersionedMigrator
    where T : IAbpMigratableMongoDbContext
{
    /// <summary>
    /// Migrates from BaseVersion - 1 to BaseVersion
    /// </summary>
    /// <returns></returns>
    Task Up(T context, IClientSessionHandle session);

    /// <summary>
    /// Migrates from BaseVersion - 1 to BaseVersion within a transaction
    /// </summary>
    /// <returns></returns>
    Task UpTransactioned(T context, IClientSessionHandle session);

    /// <summary>
    /// Migrates from BaseVersion to BaseVersion - 1
    /// </summary>
    /// <returns></returns>
    Task Down(T context, IClientSessionHandle session);

    /// <summary>
    /// Migrates from BaseVersion to BaseVersion - 1
    /// </summary>
    /// <returns></returns>
    Task DownTransactioned(T context, IClientSessionHandle session);
}
