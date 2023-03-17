namespace Bdaya.Abp.MongoDBMigrator.Tests.Entities;

using Volo.Abp.Domain.Entities.Auditing;

public class TestEntity3 : FullAuditedAggregateRoot<Guid>
{
    public Dictionary<string, object> DictProperty { get; set; } = new();
}
