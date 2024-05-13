using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using CursedQueryable.Options;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Oracle.Fixtures;

[CollectionDefinition(nameof(OracleCollection))]
public class OracleCollection : ICollectionFixture<OracleTestDbFixture>;

public class OracleTestDbFixture() : TestDbFixture(Options, DropTables)
{
    private const string ConnectionString =
        "Data Source=localhost;User Id=system;Password=cqPassword!";

    private static DbContextOptions<TestDbContext> Options =>
        new DbContextOptionsBuilder<TestDbContext>()
            .UseOracle(ConnectionString)
            .Options;

    public override NullBehaviour NullBehaviour => NullBehaviour.LargerThanNonNullable;

    private static void DropTables(TestDbContext dbContext)
    {
        // Because .EnsureDeleted() does nothing
        dbContext.Database.ExecuteSqlRaw($"drop table if exists \"SYSTEM\".\"{nameof(TestDbContext.ManyKeyedCats)}\"");
        dbContext.Database.ExecuteSqlRaw($"drop table if exists \"SYSTEM\".\"{nameof(TestDbContext.Kittens)}\"");
        dbContext.Database.ExecuteSqlRaw($"drop table if exists \"SYSTEM\".\"{nameof(TestDbContext.Cats)}\"");
    }
}