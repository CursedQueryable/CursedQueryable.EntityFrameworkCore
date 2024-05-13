using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using CursedQueryable.Options;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Postgres.Fixtures;

[CollectionDefinition(nameof(PostgresCollection))]
public class PostgresCollection : ICollectionFixture<PostgresTestDbFixture>;

public class PostgresTestDbFixture() : TestDbFixture(Options)
{
    private const string ConnectionString =
        "Host=localhost;Username=postgres;Password=cqPassword!;Database=CQTest";

    private static DbContextOptions<TestDbContext> Options =>
        new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(ConnectionString, o => o.EnableRetryOnFailure())
            .Options;

    public override NullBehaviour NullBehaviour => NullBehaviour.LargerThanNonNullable;
}