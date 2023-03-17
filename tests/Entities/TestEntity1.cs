namespace Bdaya.Abp.MongoDBMigrator.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

public class TestEntity1 : Entity<Guid>
{
    public TestEntity1()
        : base() { }

    public TestEntity1(Guid id)
        : base(id) { }

    public required string StringProperty { get; set; }
    public required int IntProperty { get; set; }
}
