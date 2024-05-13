using CursedQueryable.Options;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Scenarios;

public class ScenarioContext
{
    public IQueryable Queryable { get; init; } = default!;
    public IReadOnlyCollection<string> PrimaryKeys { get; init; } = default!;
    public FrameworkOptions FrameworkOptions { get; init; } = default!;
    public ScenarioOptions ScenarioOptions { get; set; } = new();
    public bool? ExpectedHasPage { get; set; }
}