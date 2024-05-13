using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.SqlServer.Fixtures;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.SqlServer;

[Trait("Category", "EFCore - SQL Server Integration Tests")]
[Collection(nameof(SqlServerCollection))]
public class UsingCursedQueryableWithInclude(SqlServerTestDbFixture fixture)
    : UsingCursedQueryableWithIncludeBase(fixture);