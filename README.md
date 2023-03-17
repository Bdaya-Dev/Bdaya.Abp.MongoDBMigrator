# Bdaya.Abp.MongoDBMigrator

An ABP Module that simulates EntityFramework's Migration flow for MongoDB.

Can be used in applications or modules.

# Add the module as a dependency
1. Import via nuget: https://www.nuget.org/packages/Bdaya.Abp.MongoDBMigrator/
2. in your `.MongoDB` module, depend on it: `[DependsOn(typeof(BdayaAbpMongoDBMigratorModule))]`
# How to use:
## 1. Make your contexts migratable
Implement `IBdayaAbpMigratableMongoDbContext` in the contexts

```csharp
using Bdaya.Abp.MongoDBMigrator;

public class MyDBContext : AbpMongoDbContext, IBdayaAbpMigratableMongoDbContext
{
    //your usual collections...

    //Define a context Id to be persisted in the database to uniquely identify the
    //current version of the context
    public string ContextId { get; } = "MyContext";
    //This collection stores version history of all contexts
    public IMongoCollection<BdayaAbpMongoDBMigrationHistoryEntry> MigrationHistory => Collection<BdayaAbpMongoDBMigrationHistoryEntry>();
    
    protected override void CreateModel(IMongoModelBuilder modelBuilder) 
    {
        //This configures the `MigrationHistory` collection to be named "_MigrationHistory"
        modelBuilder.ConfigureMigrations();
        //your usual configuration...
    }
}
```

## 2. Define your migrations

in your `.MongoDB` layer project, create a folder that contains all your migrations (similar to EF).

Assume you want to define migrations for `MyDBContext`:

```csharp 
public class MyDBContext_Initial_V0 : BdayaAbpMongoDBVersionedMigratorBase<MyDBContext>, IScopedDependency
{
    ///An incremental version for your migration
    public override int? BaseVersion { get; } = 0;

    ///An optional name for this version
    public override string VersionName { get; } = "Initial";

    public override async Task Up(MyDBContext context, IClientSessionHandle session)
    {
        //Executed when upgrading the database to the next version
        //Suitable for creating indexes
    }

    public override async Task UpTransactioned(MyDBContext context, IClientSessionHandle session)
    {
        //Executed when upgrading the database to the next version using a transaction
        //Suitable for adding data
    }

    public override async Task Down(MyDBContext context, IClientSessionHandle session)
    {
        //Executed when downgrading the database to the previous version
        //Suitable for deleting indexes
    }

    public override async Task DownTransactioned(MyDBContext context, IClientSessionHandle session)
    {
        //Executed when downgrading the database to the previous version using a transaction
        //Suitable for removing data
    }
}
```

## 3. Change your auto generated SchemaMigrator file
all startup projects come with a predefined SchemaMigrator class for mongodb `MongoDb[PROJECTNAME]DbSchemaMigrator`, change it like this:
1. get the migrator instance via Dependency Injection
```csharp
private readonly IBdayaAbpMongoDBDatabaseMigrator _mongoDBMigrator;    

public MongoDbBLCIRMDbSchemaMigrator(IBdayaAbpMongoDBDatabaseMigrator mongoDBMigrator, /*your other imports...*/)
{
    //your other assignments...
    _mongoDBMigrator = mongoDBMigrator;
}
```
2. call the migrator
```csharp
public async Task MigrateAsync()
{
    //other migration code...
    //add this to the very bottom
    await _mongoDBMigrator.MigrateFromConfig(dbContexts.OfType<IBdayaAbpMigratableMongoDbContext>());
}
```

## 4. Configure the migration behavior
in your `.DBMigrator` module, you can control how the migration is going to work.
this package supports 3 behaviors of migrations:
* `Up`: upgrades the database to the latest version
* `Down`: downgrades the database **one** version lower
* `Version`: updates the database (either by upgrading or downgrading) to a specific version

you can control which behavior you want by configuring the `BdayaAbpMongoDBMigratorOptions` object by adding this to your `[ProjectName]DbMigratorModule.ConfigureServices`:

```csharp
//configure dynamically by using appsettings.json
var section = context.Services.GetConfiguration().GetSection("Migrations");
Configure<BdayaAbpMongoDBMigratorOptions>(section);
//OR configure statically
Configure<BdayaAbpMongoDBMigratorOptions>(opt =>
{
    // supports Up, Down or Version
    opt.Behavior = BdayaAbpMongoDBMigratorBehaviors.Up;

    // the context Ids to include in migration, if set to null or empty, will migrate all contexts
    opt.ContextIds = new() { "MyContext", "MyOtherContext" }; 

    // only usable when behavior is set to Version
    //opt.Version = 5; 
});
```


### Notes:
1. The difference between normal and `Transactioned` methods, is that when an operation in a transactioned context fails, it will roll back all other operations done in the same method.

    This doesn't work for some mongodb operations (e.g. operations that invlove Indexes).
2. You can define any number of migrations to any number of contexts
3. a single migration can handle multiple contexts, but you will have to implment `IBdayaAbpMongoDBVersionedMigrator` instead of `BdayaAbpMongoDBVersionedMigratorBase<TContext>`.
