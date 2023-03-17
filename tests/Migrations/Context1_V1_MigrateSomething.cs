namespace Bdaya.Abp.MongoDBMigrator.Tests.Migrations;

using Bdaya.Abp.MongoDBMigrator.Tests.Contexts;
using Bdaya.Abp.MongoDBMigrator.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

//[ExposeServices(typeof(IMongoDBVersionedMigrator))]
public class Context1_V1_MigrateSomething
    : MongoDBVersionedMigratorBase<ITestMongoDbContext1>,
        IScopedDependency
{
    public override int? BaseVersion => 1;
    public override string VersionName => "Migrate Something";

    public Context1_V1_MigrateSomething() { }

    static IMongoCollection<BsonDocument> Entity1Collection(ITestMongoDbContext1 context) =>
        context.Database.GetCollection<BsonDocument>(
            context.Entity1.CollectionNamespace.CollectionName
        );

    private readonly Guid _specificId = Guid.Parse("1f185584-91ed-4ff5-8c7c-0744101c5cf6");

    public override async Task DownTransactioned(
        ITestMongoDbContext1 context,
        IClientSessionHandle session
    )
    {
        await Entity1Collection(context)
            .DeleteOneAsync(
                new BsonDocument()
                {
                    ["_id"] = new BsonBinaryData(_specificId, GuidRepresentation.Standard)
                }
            );
    }

    public override async Task UpTransactioned(
        ITestMongoDbContext1 context,
        IClientSessionHandle session
    )
    {
        await Entity1Collection(context)
            .InsertOneAsync(
                new BsonDocument()
                {
                    ["_id"] = new BsonBinaryData(_specificId, GuidRepresentation.Standard),
                    ["IntProperty"] = 15,
                    ["StringProperty"] = "Hello"
                }
            );
    }
}
