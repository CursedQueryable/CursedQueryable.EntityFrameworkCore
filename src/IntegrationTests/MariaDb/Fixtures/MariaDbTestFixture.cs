using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.MariaDb.Fixtures;

[CollectionDefinition(nameof(MariaDbCollection))]
public class MariaDbCollection : ICollectionFixture<MariaDbTestFixture>;

public class MariaDbTestFixture() : TestDbFixture(Options)
{
    private const string ConnectionString =
        "Server=localhost;Port=3307;Uid=root;Pwd=cqPassword!;Database=CQTest";

    private static readonly MariaDbServerVersion ServerVersion = new(new Version(11, 0, 0));

    private static DbContextOptions<TestDbContext> Options =>
        new DbContextOptionsBuilder<TestDbContext>()
            .UseMySql(ConnectionString, ServerVersion, o => o.EnableRetryOnFailure())
            .Options;
}