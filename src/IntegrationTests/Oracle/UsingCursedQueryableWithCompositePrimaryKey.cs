using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Oracle.Fixtures;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Oracle;

[Trait("Category", "EFCore - Oracle Integration Tests")]
[Collection(nameof(OracleCollection))]
public class UsingCursedQueryableWithCompositePrimaryKey(OracleTestDbFixture fixture)
    : UsingCursedQueryableWithCompositePrimaryKeyBase(fixture);