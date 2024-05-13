using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Data;
using CursedQueryable.Options;
using Microsoft.EntityFrameworkCore;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Fixtures;

public abstract class TestDbFixture : IDisposable
{
    protected TestDbFixture(DbContextOptions<TestDbContext> options, Action<TestDbContext>? beforeEnsureCreated = null)
    {
        DbContext = new TestDbContext(options);
        DbContext.Database.EnsureDeleted();
        beforeEnsureCreated?.Invoke(DbContext);
        DbContext.Database.EnsureCreated();

        var strategy = DbContext.Database.CreateExecutionStrategy();

        strategy.Execute(() =>
        {
            using var transaction = DbContext.Database.BeginTransaction();

            List<object> data =
            [
                .. TestData.GenerateCats(),
                .. TestData.GenerateManyKeyedCats()
            ];

            DbContext.AddRange(data);
            DbContext.SaveChanges();

            transaction.Commit();
        });
    }

    public TestDbContext DbContext { get; }
    public virtual NullBehaviour NullBehaviour => NullBehaviour.SmallerThanNonNullable;

    public void Dispose()
    {
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}