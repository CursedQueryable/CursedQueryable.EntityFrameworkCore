using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Entities;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using CursedQueryable.Options;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;

public abstract class UsingCursedQueryableWithCompositePrimaryKeyBase(TestDbFixture fixture)
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
            NullBehaviour = fixture.NullBehaviour,
            Provider = new EfCoreEntityDescriptorProvider()
        };

    private static IQueryable<ManyKeyedCat> GetRootQueryable(TestDbFixture fixture)
    {
        return fixture
            .DbContext
            .ManyKeyedCats;
    }
}