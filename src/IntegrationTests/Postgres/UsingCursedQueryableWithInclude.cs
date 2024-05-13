using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Postgres.Fixtures;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Postgres;

[Trait("Category", "EFCore - Postgres Integration Tests")]
[Collection(nameof(PostgresCollection))]
public class UsingCursedQueryableWithInclude(PostgresTestDbFixture fixture)
    : UsingCursedQueryableWithIncludeBase(fixture);