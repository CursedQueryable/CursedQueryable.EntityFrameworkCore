using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Entities;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Scenarios;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;

public abstract class CursedQueryableWithKittensTestsBase(
    IReadOnlyList<string> primaryKeys,
    IQueryable<Cat> rootQueryable)
    : CursedQueryableTestsBase<Cat>(primaryKeys, rootQueryable)
{
    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task SelectP_with_Kittens(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Take(10)
            .Select(cat => new KittenPurrito
            {
                CatName = cat.Name,
                KittenNames = cat.Kittens.Select(kitten => kitten.Name),
                Material = cat.HasExpensiveTastes ? "Silk" : "Cotton"
            });

        await RunScenario(queryable, options);
    }

    private class KittenPurrito
    {
        public string CatName { get; set; } = default!;
        public IEnumerable<string> KittenNames { get; set; } = default!;
        public string Material { get; set; } = default!;
    }
}