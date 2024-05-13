using CursedQueryable.Paging;
using FluentAssertions;
using Polly;
using Polly.Retry;

namespace CursedQueryable.EntityFrameworkCore.IntegrationTests.Helpers.Scenarios;

public interface IScenarioRunner
{
    Task Run();
}

public class ScenarioRunner<TIn, TOut>(ScenarioContext ctx) : IScenarioRunner
    where TIn : class
    where TOut : class
{
    private readonly PageBuilder<Page<TOut>, TOut, PageOptions> _pageBuilder = new(new PageMapper<TOut>());

    // Auto-retry any transport related SqlExceptions
    private readonly ResiliencePipeline _pipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            ShouldHandle = args =>
            {
                if (args.Outcome.Exception?.Message.Contains("A transport-level error has occurred") == true)
                    return PredicateResult.True();

                return PredicateResult.False();
            },

            MaxRetryAttempts = 5,

            DelayGenerator = static args =>
            {
                var delay = args.AttemptNumber switch
                {
                    0 => TimeSpan.Zero,
                    1 => TimeSpan.FromMilliseconds(100),
                    2 => TimeSpan.FromMilliseconds(250),
                    3 => TimeSpan.FromMilliseconds(650),
                    4 => TimeSpan.FromMilliseconds(1000),
                    _ => TimeSpan.FromMilliseconds(3000)
                };

                return new ValueTask<TimeSpan?>(delay);
            }
        })
        .Build();

    public async Task Run()
    {
        await _pipeline.ExecuteAsync(async cancellationToken =>
        {
            var cursor = await GetCursor(cancellationToken);
            var baseline = GetBaseline(cursor != null ? 1 : null);

            var pageOptions = new PageOptions
            {
                Cursor = cursor,
                Direction = ctx.ScenarioOptions.Direction,
                FrameworkOptions = ctx.FrameworkOptions
            };

            Page<TOut> page;

            if (ctx.ScenarioOptions.UseAsync)
                page = await _pageBuilder.ToPageAsync(ctx.Queryable, pageOptions, cancellationToken);
            else
                // ReSharper disable once MethodHasAsyncOverloadWithCancellation
                page = _pageBuilder.ToPage(ctx.Queryable, pageOptions);

            CheckPageAgainstBaseline(page, baseline);
        });
    }

    private async Task<string?> GetCursor(CancellationToken cancellationToken)
    {
        if (!ctx.ScenarioOptions.UseCursor)
            return null;

        var pageOptions = new PageOptions
        {
            Direction = ctx.ScenarioOptions.Direction,
            FrameworkOptions = ctx.FrameworkOptions
        };

        // Get an edge/cursor for the first entry in the dataset
        var page = await _pageBuilder.ToPageAsync(ctx.Queryable, pageOptions, cancellationToken);

        var edge = ctx.ScenarioOptions.Direction == Direction.Forwards
            ? page.Edges.First()
            : page.Edges.Last();

        // Sanity check
        var baselineCtx = (ctx.Queryable, ctx.PrimaryKeys, ctx.ScenarioOptions.Direction);
        var singleQ = BaselineVisitor<TIn>.Simplify<TOut>(baselineCtx);
        var single = singleQ.FirstOrDefault();

        edge.Node.Should().BeEquivalentTo(single, cfg => cfg.IgnoringCyclicReferences());

        return edge.Cursor;
    }

    private List<TOut> GetBaseline(int? skip)
    {
        // Essentially the baseline is the same underlying queryable but rewritten to use skip/take pagination instead
        // of cursors. Ultimately there should be *no* difference between the resultant data sets.
        var baselineCtx = (ctx.Queryable, ctx.PrimaryKeys, ctx.ScenarioOptions.Direction);
        var baselineQ = BaselineVisitor<TIn>.Simplify<TOut>(baselineCtx, skip);
        var baseline = baselineQ.ToList();

        if (ctx.ScenarioOptions.Direction == Direction.Backwards)
            baseline.Reverse();

        return baseline;
    }

    private void CheckPageAgainstBaseline(Page<TOut> page, IReadOnlyCollection<TOut> baseline)
    {
        baseline.Should().NotBeNull();
        page.Should().NotBeNull();

        page.Edges.Count.Should().Be(baseline.Count);

        var first = page.Edges.FirstOrDefault();
        first?.Cursor.Should().Be(page.Info.StartCursor);
        first?.Node.Should().BeEquivalentTo(baseline.FirstOrDefault(), cfg => cfg.IgnoringCyclicReferences());

        var last = page.Edges.LastOrDefault();
        last?.Cursor.Should().Be(page.Info.EndCursor);
        last?.Node.Should().BeEquivalentTo(baseline.LastOrDefault(), cfg => cfg.IgnoringCyclicReferences());

        page.Edges
            .Select(e => e.Node)
            .Should()
            .BeEquivalentTo(baseline, cfg => cfg.IgnoringCyclicReferences());

        if (ctx.ScenarioOptions.Direction == Direction.Backwards)
        {
            page.Info.HasNextPage.Should().BeNull();
            page.Info.HasPreviousPage.Should().NotBeNull();

            if (ctx.ExpectedHasPage.HasValue)
                page.Info.HasPreviousPage.Should().Be(ctx.ExpectedHasPage);
        }
        else
        {
            page.Info.HasNextPage.Should().NotBeNull();
            page.Info.HasPreviousPage.Should().BeNull();

            if (ctx.ExpectedHasPage.HasValue)
                page.Info.HasNextPage.Should().Be(ctx.ExpectedHasPage);
        }
    }
}