using CursedQueryable.Extensions;
using CursedQueryable.Options;

namespace CursedQueryable.EntityFrameworkCore;

/// <summary>
///     Enables automatic configuration of CursedQueryable.EntityFrameworkCore within the
///     root CursedQueryable assembly.
/// </summary>
public sealed class EfCoreConfigurator : ICursedConfigurator
{
    private static IEnumerable<string> ProvidersWithLargeNulls =>
    [
        "Npgsql.EntityFrameworkCore",
        "Oracle.EntityFrameworkCore"
    ];

    /// <summary>
    ///     Returns FrameworkOptions automatically configured for use with EntityFrameworkCore.
    /// </summary>
    public FrameworkOptions Configure()
    {
        return new FrameworkOptions
        {
            Provider = new EfCoreEntityDescriptorProvider(),
            NullBehaviour = GetNullBehaviour()
        };
    }

    private static NullBehaviour GetNullBehaviour()
    {
        var referencesLargeNullProvider = AppDomain.CurrentDomain
            .GetAssemblies()
            .Any(assembly => ProvidersWithLargeNulls.Any(assembly.GetName().FullName.StartsWith));

        if (referencesLargeNullProvider)
            return NullBehaviour.LargerThanNonNullable;

        return NullBehaviour.SmallerThanNonNullable;
    }
}