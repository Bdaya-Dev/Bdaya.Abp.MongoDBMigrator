namespace Bdaya.Abp.MongoDBMigrator;

using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Guids;

public class BdayaAbpDefaultMongoDBDatabaseMigrator : IBdayaAbpMongoDBDatabaseMigrator
{
    private readonly ILogger<BdayaAbpDefaultMongoDBDatabaseMigrator> _logger;
    public virtual IEnumerable<IBdayaAbpMongoDBVersionedMigrator> AllVersionedMigrators { get; }

    public virtual IEnumerable<IBdayaAbpMongoDBVersionedMigrator> FilteredMigrators(
        Type contextType
    )
    {
        return AllVersionedMigrators.Where(x => x.ContextType.IsAssignableFrom(contextType));
    }

    public IGuidGenerator GuidGenerator { get; }

    public BdayaAbpMongoDBMigratorOptions Config { get; }

    public BdayaAbpDefaultMongoDBDatabaseMigrator(
        ILogger<BdayaAbpDefaultMongoDBDatabaseMigrator> logger,
        IGuidGenerator guidGenerator,
        IEnumerable<IBdayaAbpMongoDBVersionedMigrator> versionedMigrators,
        IOptions<BdayaAbpMongoDBMigratorOptions> config
    )
    {
        _logger = logger;
        GuidGenerator = guidGenerator;
        AllVersionedMigrators = versionedMigrators;
        Config = config.Value;
    }

    public async Task MigrateFromConfig(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    )
    {
        var contextIds = Config.ContextIds;
        if (contextIds != null && contextIds.Count > 0 && !contextIds.Contains(context.ContextId))
        {
            //don't handle ContextIds that aren't opted in if ContextIds exists
            return;
        }

        switch (Config.Behavior)
        {
            case BdayaAbpMongoDBMigratorBehaviors.Down:
                await DowngradeDatabase(context, cancellationToken);
                break;
            case BdayaAbpMongoDBMigratorBehaviors.Up:
                await UpgradeDatabase(context, cancellationToken);
                break;
            case BdayaAbpMongoDBMigratorBehaviors.Version:
                var version = Config.Version;
                if (!version.HasValue)
                {
                    throw new ArgumentNullException(
                        "version",
                        "for Version behavior, the Version parameter must be set"
                    );
                }
                await UpdateDatabaseToVersion(context, version.Value, cancellationToken);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public async Task MigrateFromConfig(
        IEnumerable<IBdayaAbpMigratableMongoDbContext> contexts,
        CancellationToken cancellationToken = default
    )
    {
        foreach (var item in contexts)
        {
            await MigrateFromConfig(item, cancellationToken);
        }
    }

    public async Task UpgradeDatabase(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    )
    {
        var contextId = Check.NotNullOrEmpty(context.ContextId, nameof(context.ContextId));

        var sessionOptions = new ClientSessionOptions { };
        using var session = await context.Database.Client.StartSessionAsync(
            sessionOptions,
            cancellationToken
        );

        _logger.LogInformation("Upgrading context {context}, reading history entries...", context);

        var entries = await context.MigrationHistory
            .AsQueryable(session)
            .Where(x => x.ContextId == contextId)
            .OrderByDescending(x => x.Version)
            .ToListAsync(cancellationToken: cancellationToken);
        _logger.LogInformation(
            "Upgrading context {context}, history entries found {count}",
            context,
            entries.Count
        );
        var latestEntry = entries.FirstOrDefault();
        var currentVersion = latestEntry?.Version ?? -1;
        _logger.LogInformation(
            "Upgrading context {context}, latest entry: {currentVersion}",
            context,
            currentVersion
        );
        await DoUpgrade(context, session, currentVersion, null, cancellationToken);
    }

    public async Task DowngradeDatabase(
        IBdayaAbpMigratableMongoDbContext context,
        CancellationToken cancellationToken = default
    )
    {
        var contextId = Check.NotNullOrEmpty(context.ContextId, nameof(context.ContextId));

        var sessionOptions = new ClientSessionOptions { };
        using var session = await context.Database.Client.StartSessionAsync(
            sessionOptions,
            cancellationToken
        );

        _logger.LogInformation(
            "Downgrading context {context}, reading history entries...",
            context
        );
        var entries = await context.MigrationHistory
            .AsQueryable(session)
            .Where(x => x.ContextId == contextId)
            .OrderByDescending(x => x.Version)
            .ToListAsync(cancellationToken: cancellationToken);
        _logger.LogInformation(
            "Downgrading context {context}, history entries found {count}",
            context,
            entries.Count
        );
        var latestEntry = entries.FirstOrDefault();
        var currentVersion = latestEntry?.Version ?? -1;
        _logger.LogInformation(
            "Downgrading context {context}, latest entry: {currentVersion}",
            context,
            currentVersion
        );
        var possibleDownMigration = FilteredMigrators(context.GetType())
            .FirstOrDefault(x => x.BaseVersion == currentVersion);
        _logger.LogInformation(
            "Downgrading context {context}, possible down migration: {possibleDownMigration}",
            context,
            possibleDownMigration == null ? "none" : possibleDownMigration.BaseVersion
        );
        if (possibleDownMigration == null)
        {
            return;
        }
        try
        {
            await possibleDownMigration.Down(context, session);
            session.StartTransaction();
            await possibleDownMigration.DownTransactioned(context, session);
            if (possibleDownMigration.BaseVersion is int migratorVersion)
            {
                await context.MigrationHistory.DeleteOneAsync(
                    session,
                    x => x.Version == migratorVersion && x.ContextId == contextId,
                    cancellationToken: cancellationToken
                );
            }
            await session.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            if (session.IsInTransaction)
            {
                await session.AbortTransactionAsync(cancellationToken);
            }
            _logger.LogException(ex);
            throw;
        }
        _logger.LogInformation(
            "Downgrading context {context}, To version: {version} Done successfully.",
            context,
            possibleDownMigration.BaseVersion
        );
    }

    public async Task UpdateDatabaseToVersion(
        IBdayaAbpMigratableMongoDbContext context,
        int version,
        CancellationToken cancellationToken = default
    )
    {
        var contextId = Check.NotNullOrEmpty(context.ContextId, nameof(context.ContextId));

        var sessionOptions = new ClientSessionOptions { };
        using var session = await context.Database.Client.StartSessionAsync(
            sessionOptions,
            cancellationToken
        );

        _logger.LogInformation("Migrating context {context}, reading history entries...", context);
        var entries = await context.MigrationHistory
            .AsQueryable(session)
            .Where(x => x.ContextId == contextId)
            .OrderByDescending(x => x.Version)
            .ToListAsync(cancellationToken: cancellationToken);
        _logger.LogInformation(
            "Migrating context {context}, history entries found {count}",
            context,
            entries.Count
        );
        var latestEntry = entries.FirstOrDefault();
        var currentVersion = latestEntry?.Version ?? -1;
        var isUpgrade = version > currentVersion;
        _logger.LogInformation(
            "Migrating context {context}, latest entry: {currentVersion} ({isUpgrade})",
            context,
            currentVersion,
            currentVersion == version
                ? "No Change"
                : isUpgrade
                    ? "Upgrade"
                    : "Downgrade"
        );
        if (version == currentVersion)
        {
            return;
        }

        if (isUpgrade)
        {
            await DoUpgrade(
                context,
                session,
                currentVersion: currentVersion,
                targetVersion: version,
                cancellationToken
            );
        }
        else
        {
            var possibleDownMigrations = FilteredMigrators(context.GetType())
                .Where(
                    x =>
                        !x.BaseVersion.HasValue
                        || (x.BaseVersion.Value <= currentVersion && x.BaseVersion.Value > version)
                )
                .OrderByDescending(x => x.BaseVersion);
            _logger.LogInformation(
                "Downgrading context {context}, possible down migrations: {possibleDownMigrations}",
                context,
                possibleDownMigrations.Any()
                    ? possibleDownMigrations.Select(x => x.BaseVersion).JoinAsString(", ")
                    : "none"
            );
            if (!possibleDownMigrations.Any())
            {
                return;
            }

            var latestVersion = currentVersion;
            foreach (var migrator in possibleDownMigrations)
            {
                try
                {
                    await migrator.Down(context, session);
                    session.StartTransaction();
                    await migrator.DownTransactioned(context, session);
                    if (migrator.BaseVersion is int migratorVersion)
                    {
                        await context.MigrationHistory.DeleteOneAsync(
                            session,
                            x => x.ContextId == contextId && x.Version == migratorVersion,
                            cancellationToken: cancellationToken
                        );
                        latestVersion = migratorVersion - 1;
                    }
                    await session.CommitTransactionAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    if (session.IsInTransaction)
                    {
                        await session.AbortTransactionAsync(cancellationToken);
                    }
                    _logger.LogException(ex);
                    throw;
                }
            }
            _logger.LogInformation(
                "Downgrading context {context}, To version: {version} Done successfully.",
                context,
                latestVersion
            );
        }
    }

    private async Task DoUpgrade(
        IBdayaAbpMigratableMongoDbContext context,
        IClientSessionHandle session,
        int currentVersion,
        int? targetVersion,
        CancellationToken cancellationToken
    )
    {
        //unversioned migrations happen after versioned migrations
        var possibleUpMigrations = FilteredMigrators(context.GetType())
            .Where(
                x =>
                    !x.BaseVersion.HasValue
                    || (
                        x.BaseVersion.Value > currentVersion
                        && (!targetVersion.HasValue || x.BaseVersion.Value <= targetVersion.Value)
                    )
            )
            .OrderBy(x => x.BaseVersion ?? int.MaxValue);

        _logger.LogInformation(
            "Upgrading context {context}, possible up migrations: {possibleUpMigrations}",
            context,
            possibleUpMigrations.Any()
                ? possibleUpMigrations.Select(x => x.BaseVersion).JoinAsString(", ")
                : "none"
        );
        if (!possibleUpMigrations.Any())
        {
            return;
        }

        var latestVersion = currentVersion;
        foreach (var migrator in possibleUpMigrations)
        {
            try
            {
                await migrator.Up(context, session);
                session.StartTransaction();
                await migrator.UpTransactioned(context, session);
                if (migrator.BaseVersion is int migratorVersion)
                {
                    await context.MigrationHistory.InsertOneAsync(
                        session,
                        new BdayaAbpMongoDBMigrationHistoryEntry(id: GuidGenerator.Create())
                        {
                            CreatedAt = DateTime.UtcNow,
                            ContextId = context.ContextId,
                            VersionName = migrator.VersionName ?? string.Empty,
                            Version = latestVersion = migratorVersion
                        },
                        cancellationToken: cancellationToken
                    );
                }
                await session.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                if (session.IsInTransaction)
                {
                    await session.AbortTransactionAsync(cancellationToken);
                }
                _logger.LogException(ex);
                throw;
            }
        }
        _logger.LogInformation(
            "Upgrading context {context}, To version: {version} Done successfully.",
            context,
            latestVersion
        );
        return;
    }
}
