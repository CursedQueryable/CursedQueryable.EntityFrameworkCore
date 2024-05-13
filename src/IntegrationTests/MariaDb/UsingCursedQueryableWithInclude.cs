using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.MariaDb.Fixtures;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.MariaDb;

[Trait("Category", "EFCore - MariaDB Integration Tests")]
[Collection(nameof(MariaDbCollection))]
public class UsingCursedQueryableWithInclude(MariaDbTestFixture fixture)
    : UsingCursedQueryableWithIncludeBase(fixture);