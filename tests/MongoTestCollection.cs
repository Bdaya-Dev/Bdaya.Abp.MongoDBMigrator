using Xunit;

namespace Bdaya.Abp.MongoDBMigrator.Tests;

[CollectionDefinition(Name)]
public class MongoTestCollection : ICollectionFixture<MongoDbFixture>
{
    public const string Name = "MongoDB Collection";
}
