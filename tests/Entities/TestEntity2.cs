namespace Bdaya.Abp.MongoDBMigrator.Tests.Entities;

using Volo.Abp.Domain.Entities;

public class TestEntity2 : AggregateRoot<Guid>
{
    public required double DoubleProperty { get; set; }
}
