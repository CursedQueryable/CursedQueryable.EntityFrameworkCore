using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using CursedQueryable.EntityDescriptors;
using Microsoft.EntityFrameworkCore.Query;

namespace CursedQueryable.EntityFrameworkCore;

/// <summary>
///     Provides EfCoreEntityDescriptor instances for IQueryable expressions.
/// </summary>
public sealed class EfCoreEntityDescriptorProvider : IEntityDescriptorProvider
{
    /// <summary>
    ///     Tries to get an EfCoreEntityDescriptor for a given expression.
    /// </summary>
    public bool TryGetEntityDescriptor(Expression expression,
        [MaybeNullWhen(false)] out IEntityDescriptor entityDescriptor)
    {
        if (expression is EntityQueryRootExpression { QueryProvider: not null } root)
        {
            entityDescriptor = new EfCoreEntityDescriptor(root);
            return true;
        }

        entityDescriptor = null;
        return false;
    }
}