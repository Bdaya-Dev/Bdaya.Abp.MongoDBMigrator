using System;
using Mongo2Go;

namespace Bdaya.Abp.MongoDBMigrator.Tests;

public class MongoDbFixture : IDisposable
{
    private static readonly MongoDbRunner _mongoDbRunner;
    public static readonly string ConnectionString;

    static MongoDbFixture()
    {
        _mongoDbRunner = MongoDbRunner.Start(
            singleNodeReplSet: true,
            singleNodeReplSetWaitTimeout: 20
        );
        ConnectionString = _mongoDbRunner.ConnectionString;
    }

    public void Dispose()
    {
        _mongoDbRunner?.Dispose();
        GC.SuppressFinalize(this);
    }
}
