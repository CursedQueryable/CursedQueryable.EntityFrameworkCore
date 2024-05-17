using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.UnitTests;

[Trait("Category", "EFCore - Unit Tests")]
public class EfCoreEntityDescriptorProviderTests
{
    private readonly EfCoreEntityDescriptorProvider _provider = new();

    [Fact]
    public void Creates_descriptor_for_matching_queryable_root()
    {
        var mockKey = new Mock<IKey>();
        mockKey
            .Setup(m => m.IsPrimaryKey())
            .Returns(true);
        mockKey
            .Setup(m => m.Properties)
            .Returns(new[] { new Mock<IProperty>().Object });

        var mockEntityType = new Mock<IEntityType>();

        mockEntityType
            .Setup(m => m.ClrType)
            .Returns(typeof(object));

        mockEntityType
            .Setup(m => m.GetKeys())
            .Returns(new[] { mockKey.Object });

        var mockQueryProvider = new Mock<IAsyncQueryProvider>();
        var rootExpression = new QueryRootExpression(mockQueryProvider.Object, mockEntityType.Object);

        var success = _provider.TryGetEntityDescriptor(rootExpression, out var descriptor);

        success.Should().BeTrue();
        descriptor.Should().BeOfType<EfCoreEntityDescriptor>();
    }

    [Fact]
    public void Does_not_create_descriptor_for_non_matching_queryable_root()
    {
        var success = _provider.TryGetEntityDescriptor(Expression.Empty(), out var descriptor);

        success.Should().BeFalse();
        descriptor.Should().BeNull();
    }
}