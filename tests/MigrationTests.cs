using Bdaya.Abp.MongoDBMigrator.Tests.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Shouldly;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;
using Xunit.Abstractions;

namespace Bdaya.Abp.MongoDBMigrator.Tests;

[Collection(MongoTestCollection.Name)]
public class MigrationTests : MongoDbTestBase
{
    private readonly IMongoDbContextProvider<ITestMongoDbContext1> _context1Provider;
    private readonly IMongoDbContextProvider<ITestMongoDbContext2> _context2Provider;
    private readonly IMongoDBMigrator _migrator;

    public MigrationTests()
    {
        _migrator = ServiceProvider.GetRequiredService<IMongoDBMigrator>();

        _context1Provider = ServiceProvider.GetRequiredService<
            IMongoDbContextProvider<ITestMongoDbContext1>
        >();
        _context2Provider = ServiceProvider.GetRequiredService<
            IMongoDbContextProvider<ITestMongoDbContext2>
        >();
    }

    [Fact]
    public async Task EmptyAtStart()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var _context1 = await _context1Provider.GetDbContextAsync();
            var history = await _context1.MigrationHistory.AsQueryable().ToListAsync();
            history.ShouldBeEmpty();
        });
    }

    [Fact]
    public async Task TestUpgradeC1Migration()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var _context1 = await _context1Provider.GetDbContextAsync();
            await _migrator.UpgradeDatabase(_context1); //From V-1 To V2
            var history = await _context1.MigrationHistory
                .AsQueryable()
                .Where(x => x.ContextId == _context1.ContextId)
                .ToListAsync();
            history.Count.ShouldBe(3);
            var latest = history.OrderByDescending(x => x.Version).First();
            latest.Version.ShouldBe(2);
            latest.VersionName.ShouldBe("Migrate Another Thing");
            latest.ContextId.ShouldBe(_context1.ContextId);

            var entity1 = await _context1.Entity1.AsQueryable().ToListAsync();
            entity1.Count.ShouldBe(1);
        });
    }

    [Fact]
    public async Task TestDowngradeC1Migration()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var _context1 = await _context1Provider.GetDbContextAsync();
            await _migrator.UpgradeDatabase(_context1); //From V-1 to V2
            await _migrator.DowngradeDatabase(_context1); //from V2 to V1
            var history = await _context1.MigrationHistory
                .AsQueryable()
                .Where(x => x.ContextId == _context1.ContextId)
                .ToListAsync();
            history.Count.ShouldBe(2);
            var latest = history.OrderByDescending(x => x.Version).First();
            latest.Version.ShouldBe(1);
            latest.VersionName.ShouldBe("Migrate Something");
            latest.ContextId.ShouldBe(_context1.ContextId);

            var entity1 = await _context1.Entity1.AsQueryable().ToListAsync();
            entity1.Count.ShouldBe(1);
            await _migrator.DowngradeDatabase(_context1); // from V1 to V0
            entity1 = await _context1.Entity1.AsQueryable().ToListAsync();
            entity1.Count.ShouldBe(0);
        });
    }

    [Fact]
    public async Task TestUpgradeC2Migration()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var _context2 = await _context2Provider.GetDbContextAsync();
            await _migrator.UpgradeDatabase(_context2); //From V-1 to V0
            var history = await _context2.MigrationHistory
                .AsQueryable()
                .Where(x => x.ContextId == _context2.ContextId)
                .ToListAsync();
            history.Count.ShouldBe(1);
            var latest = history.OrderByDescending(x => x.Version).First();
            latest.Version.ShouldBe(0);
            latest.VersionName.ShouldBe("Initial");
            latest.ContextId.ShouldBe(_context2.ContextId);
        });
    }

    [Fact]
    public async Task TestUpdateC1ToVersion()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var _context1 = await _context1Provider.GetDbContextAsync();
            await _migrator.UpdateDatabaseToVersion(_context1, 1); //From V-1 to V1
            var history = await _context1.MigrationHistory
                .AsQueryable()
                .Where(x => x.ContextId == _context1.ContextId)
                .ToListAsync();
            history.Count.ShouldBe(2);
            var latest = history.OrderByDescending(x => x.Version).First();
            latest.Version.ShouldBe(1);
            latest.VersionName.ShouldBe("Migrate Something");
            latest.ContextId.ShouldBe(_context1.ContextId);

            var entity1 = await _context1.Entity1.AsQueryable().ToListAsync();
            entity1.Count.ShouldBe(1);
        });
        await WithUnitOfWorkAsync(async () =>
        {
            var _context1 = await _context1Provider.GetDbContextAsync();
            await _migrator.UpdateDatabaseToVersion(_context1, 0); //From V1 to V0
            var history = await _context1.MigrationHistory
                .AsQueryable()
                .Where(x => x.ContextId == _context1.ContextId)
                .ToListAsync();
            history.Count.ShouldBe(1);
            var latest = history.OrderByDescending(x => x.Version).First();
            latest.Version.ShouldBe(0);
            latest.VersionName.ShouldBe("Initial");
            latest.ContextId.ShouldBe(_context1.ContextId);

            var entity1 = await _context1.Entity1.AsQueryable().ToListAsync();
            entity1.Count.ShouldBe(0);
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var _context1 = await _context1Provider.GetDbContextAsync();
            await _migrator
                .UpdateDatabaseToVersion(_context1, -1)
                .ShouldThrowAsync<NotImplementedException>(); //From V0 to V-1
            var history = await _context1.MigrationHistory
                .AsQueryable()
                .Where(x => x.ContextId == _context1.ContextId)
                .ToListAsync();
            history.Count.ShouldBe(1);
            var latest = history.OrderByDescending(x => x.Version).First();
            latest.Version.ShouldBe(0);
            latest.VersionName.ShouldBe("Initial");
            latest.ContextId.ShouldBe(_context1.ContextId);
        });
    }
}
