namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Scenarios;

public class ScenarioOptions
{
    public ScenarioOptions()
    {
    }

    public ScenarioOptions(bool useAsync, bool useCursor, Direction direction)
    {
        UseAsync = useAsync;
        UseCursor = useCursor;
        Direction = direction;
    }

    public bool UseCursor { get; }
    public Direction Direction { get; }
    public bool UseAsync { get; }
}