﻿using Bdaya.Abp.MongoDBMigrator.Tests.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Linq;
using Volo.Abp.MongoDB;
using Volo.Abp.Testing;
using Xunit;

namespace Bdaya.Abp.MongoDBMigrator.Tests;

[Collection(MongoTestCollection.Name)]
public class CreationTests : MongoDbTestBase
{
    [Fact]
    public void AllServicesAreRegistered()
    {
        _ = ServiceProvider.GetRequiredService<IMongoDbContextProvider<ITestMongoDbContext1>>();
        var _context1 = ServiceProvider.GetRequiredService<ITestMongoDbContext1>();
        var _context2 = ServiceProvider.GetRequiredService<ITestMongoDbContext2>();
        var migrator = ServiceProvider.GetRequiredService<IBdayaAbpMongoDBDatabaseMigrator>();

        //var allMigrators = ServiceProvider.GetRequiredService(typeof(IMongoDBMigrator<>));

        Assert.IsType<TestMongoDbContext1>(_context1);
        Assert.IsType<TestMongoDbContext2>(_context2);
        Assert.IsType<BdayaAbpDefaultMongoDBDatabaseMigrator>(migrator);

        var castedMigrator = (BdayaAbpDefaultMongoDBDatabaseMigrator)migrator;

        var c1Migrators = ServiceProvider
            .GetServices<IBdayaAbpMongoDBVersionedMigrator>()
            .OfType<IBdayaAbpMongoDBVersionedMigrator<ITestMongoDbContext1>>()
            .ToList();
        var c2Migrators = ServiceProvider
            .GetServices<IBdayaAbpMongoDBVersionedMigrator>()
            .OfType<IBdayaAbpMongoDBVersionedMigrator<ITestMongoDbContext2>>()
            .ToList();

        c1Migrators.Count.ShouldBe(3);
        c2Migrators.Count.ShouldBe(1);

        castedMigrator.AllVersionedMigrators.ShouldBe(
            Enumerable.Concat<IBdayaAbpMongoDBVersionedMigrator>(c1Migrators, c2Migrators)
        );
    }
}
