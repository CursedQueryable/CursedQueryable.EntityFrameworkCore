using System.Reflection;
using System.Reflection.Emit;
using CursedQueryable.Options;
using FluentAssertions;
using Xunit;

namespace CursedQueryable.EntityFrameworkCore.UnitTests;

[Trait("Category", "EFCore - Unit Tests")]
public sealed class EfCoreConfiguratorTests : IDisposable
{
    private readonly EfCoreConfigurator _configurator = new();

    public void Dispose()
    {
        GC.Collect();
    }

    [Fact]
    public void Configured_Provider_is_correct()
    {
        var options = _configurator.Configure();

        options.Provider.Should().BeOfType<EfCoreEntityDescriptorProvider>();
    }

    [Fact]
    public void Configured_NullBehaviour_default_should_be_SmallerThanNonNullable()
    {
        var options = _configurator.Configure();

        options.NullBehaviour.Should().Be(NullBehaviour.SmallerThanNonNullable);
    }

    [Fact]
    public void Configured_NullBehaviour_when_Npgsql_loaded_should_be_LargerThanNonNullable()
    {
        GenerateDynamicAssembly("Npgsql.EntityFrameworkCore");

        var options = _configurator.Configure();

        options.NullBehaviour.Should().Be(NullBehaviour.LargerThanNonNullable);
    }

    [Fact]
    public void Configured_NullBehaviour_when_Oracle_loaded_should_be_LargerThanNonNullable()
    {
        GenerateDynamicAssembly("Oracle.EntityFrameworkCore");

        var options = _configurator.Configure();

        options.NullBehaviour.Should().Be(NullBehaviour.LargerThanNonNullable);
    }

    private static void GenerateDynamicAssembly(string assemblyNameStr)
    {
        var assemblyName = new AssemblyName(assemblyNameStr);
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name!);
        var typeBuilder = moduleBuilder.DefineType("DynamicConfigurator", TypeAttributes.Public);
        typeBuilder.CreateType();
    }
}