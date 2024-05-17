using System.Reflection;
using CursedQueryable.EntityDescriptors;
using Microsoft.EntityFrameworkCore.Query;

namespace CursedQueryable.EntityFrameworkCore;

/// <summary>
/// </summary>
/// <param name="root">The root expression for any IQueryable originating from Entity Framework Core.</param>
public sealed class EfCoreEntityDescriptor(QueryRootExpression root) : IEntityDescriptor
{
    /// <summary>
    ///     The CLR type of the root entity.
    /// </summary>
    public Type Type { get; } = root.EntityType.ClrType;

    /// <summary>
    ///     A collection of all the properties belonging to the CLR type that Entity Framework has marked as
    ///     comprising the entity's Primary Key.
    /// </summary>
    public IReadOnlyCollection<PropertyInfo> PrimaryKeyComponents { get; } =
        root.EntityType
            .GetKeys()
            .Single(key => key.IsPrimaryKey())
            .Properties
            .Select(property => property.PropertyInfo!)
            .ToArray();
}