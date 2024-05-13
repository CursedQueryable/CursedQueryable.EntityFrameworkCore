using CursedQueryable.EntityFrameworkCore.IntegrationTests.Database.Entities;
using CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Scenarios;
using CursedQueryable.Options;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Abstract;

public abstract class CursedQueryableTestsBase<TCat>(IReadOnlyList<string> primaryKeys, IQueryable<TCat> rootQueryable)
    where TCat : class, ICat
{
    protected virtual FrameworkOptions FrameworkOptions { get; } = new()
    {
        BadCursorBehaviour = BadCursorBehaviour.ThrowException
    };

    protected IQueryable<TCat> RootQueryable { get; } = rootQueryable;

    protected async Task RunScenario<TOut>(IQueryable<TOut> queryable, ScenarioOptions? options = null,
        Action<ScenarioContext>? opts = null) where TOut : class
    {
        var context = new ScenarioContext
        {
            Queryable = queryable,
            PrimaryKeys = primaryKeys,
            ScenarioOptions = options ?? new ScenarioOptions(),
            FrameworkOptions = FrameworkOptions
        };

        opts?.Invoke(context);

        var runnerType = typeof(ScenarioRunner<,>).MakeGenericType(typeof(TCat), typeof(TOut));
        var runner = (IScenarioRunner)Activator.CreateInstance(runnerType, [context])!;
        await runner.Run();
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Base(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable;

        await RunScenario(queryable, options, o => o.ExpectedHasPage = false);
    }

    [Theory]
    [ClassData(typeof(NoCursorScenarios))]
    public async Task Base_no_results(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Where(cat => cat.Name == "fail");

        await RunScenario(queryable, options, o => o.ExpectedHasPage = false);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Take(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Take(10);

        await RunScenario(queryable, options, o => o.ExpectedHasPage = true);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Skip(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Skip(10);

        await RunScenario(queryable, options, o => o.ExpectedHasPage = false);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Skip_Take(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Skip(10)
            .Take(10);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderBy(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderBy(cat => cat.Sex);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderByCoalesce(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderBy(cat => cat.DateOfBirth ?? DateTime.MaxValue);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderByTernary(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderBy(cat => cat.HasExpensiveTastes ? cat.Name : cat.Eman);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderBy_OrderBy(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderBy(cat => cat.Sex)
            // ReSharper disable once MultipleOrderBy - deliberate for test
            .OrderBy(cat => cat.Name);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderBy_ThenBy(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderBy(cat => cat.Sex)
            .ThenBy(cat => cat.Name);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderBy_ThenBy_Where(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderBy(cat => cat.Sex)
            .ThenBy(cat => cat.Name)
            .Where(cat => cat.Name.StartsWith("Moggy 005"));

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderByDescending(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderByDescending(cat => cat.Sex);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderByDescending_ThenByDescending(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderByDescending(cat => cat.Sex)
            .ThenByDescending(cat => cat.Name);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task OrderByDescending_ThenByDescending_Where(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .OrderByDescending(cat => cat.Sex)
            .ThenByDescending(cat => cat.Name)
            .Where(cat => cat.Name.StartsWith("Moggy 005"));

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Select(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Select(cat => cat);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task SelectP(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Select(cat => new Purrito { Cat = cat, Material = cat.HasExpensiveTastes ? "Silk" : "Cotton" });

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Select_Select(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Select(cat => cat)
            .Select(cat => cat);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Select_SelectP(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Select(cat => cat)
            .Select(cat => new Purrito { Cat = cat, Material = cat.HasExpensiveTastes ? "Silk" : "Cotton" });

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Select_SelectP_Select(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Select(cat => cat)
            .Select(cat => new Purrito { Cat = cat, Material = cat.HasExpensiveTastes ? "Silk" : "Cotton" })
            .Select(p => p.Cat);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task SelectP_Where(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Select(cat => new Purrito { Cat = cat, Material = cat.HasExpensiveTastes ? "Silk" : "Cotton" })
            .Where(p => p.Cat.Name.StartsWith("Moggy 005"));

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Where(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Where(cat => cat.Sex == Sex.Female);

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Where_Where(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Where(cat => cat.Sex == Sex.Female)
            .Where(cat => cat.Name.StartsWith("Moggy 001"));

        await RunScenario(queryable, options);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Where_Take(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Where(cat => cat.Sex == Sex.Female)
            .Take(10);

        await RunScenario(queryable, options, o => o.ExpectedHasPage = true);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Where_OrderBy_Take(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Where(cat => cat.Sex == Sex.Female)
            .OrderBy(cat => cat.Eman)
            .Take(10);

        await RunScenario(queryable, options, o => o.ExpectedHasPage = true);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Where_OrderByDescending_Take(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Where(cat => cat.Sex == Sex.Female)
            .OrderByDescending(c => c.Name)
            .Take(10);

        await RunScenario(queryable, options, o => o.ExpectedHasPage = true);
    }

    [Theory]
    [ClassData(typeof(AllScenarios))]
    public async Task Where_OrderBy_Take_SelectP(bool useAsync, bool useCursor, Direction direction)
    {
        var options = new ScenarioOptions(useAsync, useCursor, direction);

        var queryable = RootQueryable
            .Where(cat => cat.Sex == Sex.Female)
            .OrderBy(cat => cat.Eman)
            .Take(10)
            .Select(cat => new Purrito { Cat = cat, Material = cat.HasExpensiveTastes ? "Silk" : "Cotton" });

        await RunScenario(queryable, options, o => o.ExpectedHasPage = true);
    }

    protected class Purrito
    {
        public TCat Cat { get; init; } = default!;
        public string Material { get; init; } = default!;
    }
}