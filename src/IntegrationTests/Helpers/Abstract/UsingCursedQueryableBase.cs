using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Entities;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;
using CursedQueryable.Options;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;

public abstract class UsingCursedQueryableBase(TestDbFixture fixture)
    : CursedQueryableTestsBase<Cat>(PrimaryKeys, GetRootQueryable(fixture))
{
    private static readonly IReadOnlyList<string> PrimaryKeys = new[]
    {
        nameof(Cat.Id)
    };

    protected override FrameworkOptions FrameworkOptions =>
        new(base.FrameworkOptions)
        {
            NullBehaviour = fixture.NullBehaviour,
            Provider = new EfCoreEntityDescriptorProvider()
        };

    private static IQueryable<Cat> GetRootQueryable(TestDbFixture fixture)
    {
        return fixture
            .DbContext
            .Cats;
    }
}