using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.MySql.Fixtures;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.MySql;

[Trait("Category", "EFCore - MySQL Integration Tests")]
[Collection(nameof(MySqlCollection))]
public class UsingCursedQueryableWithCompositePrimaryKey(MySqlTestDbFixture fixture)
    : UsingCursedQueryableWithCompositePrimaryKeyBase(fixture);