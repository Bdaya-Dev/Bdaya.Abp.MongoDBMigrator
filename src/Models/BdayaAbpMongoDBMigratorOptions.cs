namespace Bdaya.Abp.MongoDBMigrator;
using System.Collections.Generic;

public class BdayaAbpMongoDBMigratorOptions
{
    public BdayaAbpMongoDBMigratorBehaviors Behavior { get; set; } =
        BdayaAbpMongoDBMigratorBehaviors.Up;
    public List<string>? ContextIds { get; set; }
    public int? Version { get; set; }
}
