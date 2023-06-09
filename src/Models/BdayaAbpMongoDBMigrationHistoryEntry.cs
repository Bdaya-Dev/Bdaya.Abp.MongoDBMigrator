﻿namespace Bdaya.Abp.MongoDBMigrator;
using System;
using Volo.Abp.Domain.Entities;

public class BdayaAbpMongoDBMigrationHistoryEntry : Entity<Guid>
{
    public BdayaAbpMongoDBMigrationHistoryEntry(Guid id)
        : base(id) { }

    public required string ContextId { get; set; }
    public required int Version { get; set; }
    public required string VersionName { get; set; }
    public required DateTime CreatedAt { get; set; }
}
