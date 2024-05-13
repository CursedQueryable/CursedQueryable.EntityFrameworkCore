using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.SqlServer.Fixtures;

[CollectionDefinition(nameof(SqlServerCollection))]
public class SqlServerCollection : ICollectionFixture<SqlServerTestDbFixture>;

public class SqlServerTestDbFixture() : TestDbFixture(Options)
{
    private const string ConnectionString =
        "Server=localhost;User ID=sa;Password=cqPassword!;Database=CQTest;TrustServerCertificate=True";

    private static DbContextOptions<TestDbContext> Options =>
        new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlServer(ConnectionString, o => o.EnableRetryOnFailure())
            .Options;
}