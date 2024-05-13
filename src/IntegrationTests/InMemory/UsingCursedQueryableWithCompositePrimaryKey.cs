using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Entities;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.InMemory.Fixtures;
using CursedQueryable.Options;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.InMemory;

[Trait("Category", "EFCore - InMemory Integration Tests")]
[Collection(nameof(InMemoryCollection))]
public class UsingCursedQueryableWithCompositePrimaryKey(InMemoryTestDbFixture fixture)
    : CursedQueryableTestsBase<ManyKeyedCat>(PrimaryKeys, GetRootQueryable(fixture))
{
    private static readonly IReadOnlyList<string> PrimaryKeys = new[]
    {
        nameof(ManyKeyedCat.Id1),
        nameof(ManyKeyedCat.Id2),
        nameof(ManyKeyedCat.Id3)
    };

    protected override FrameworkOptions FrameworkOptions =>
        new(base.FrameworkOptions)
        {
            Provider = new EfCoreEntityDescriptorProvider()
        };

    private static IQueryable<ManyKeyedCat> GetRootQueryable(TestDbFixture fixture)
    {
        return fixture
            .DbContext
            .ManyKeyedCats;
    }
}