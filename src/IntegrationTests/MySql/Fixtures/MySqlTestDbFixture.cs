extern alias MySql;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.MySql.Fixtures;

[CollectionDefinition(nameof(MySqlCollection))]
public class MySqlCollection : ICollectionFixture<MySqlTestDbFixture>;

public class MySqlTestDbFixture() : TestDbFixture(Options)
{
    private const string ConnectionString =
        "Server=localhost;Uid=root;Pwd=cqPassword!;Database=CQTest";

    private static DbContextOptions<TestDbContext> Options =>
        MySql::Microsoft.EntityFrameworkCore.MySQLDbContextOptionsExtensions.UseMySQL(
                new DbContextOptionsBuilder<TestDbContext>(), ConnectionString, o => o.EnableRetryOnFailure())
            .Options;
}