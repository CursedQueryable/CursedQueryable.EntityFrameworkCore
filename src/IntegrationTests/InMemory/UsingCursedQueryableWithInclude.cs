using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Entities;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.InMemory.Fixtures;
using CursedQueryable.Options;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.InMemory;

[Trait("Category", "EFCore - InMemory Integration Tests")]
[Collection(nameof(InMemoryCollection))]
public class UsingCursedQueryableWithInclude(InMemoryTestDbFixture fixture)
    : CursedQueryableWithKittensTestsBase(PrimaryKeys, GetRootQueryable(fixture))
{
    private static readonly IReadOnlyList<string> PrimaryKeys = new[]
    {
        nameof(Cat.Id)
    };

    protected override FrameworkOptions FrameworkOptions =>
        new(base.FrameworkOptions)
        {
            Provider = new EfCoreEntityDescriptorProvider()
        };

    private static IQueryable<Cat> GetRootQueryable(TestDbFixture fixture)
    {
        return fixture
            .DbContext
            .Cats
            .AsNoTrackingWithIdentityResolution()
            .Include(cat => cat.Kittens);
    }
}