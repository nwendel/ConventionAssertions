using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Tests;

public class ConventionTargetsTests_Types
{
    [Fact]
    public void Throws_on_null_assemblies()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromTypes(
            (Func<IAssemblyScanner, IAssemblyScannerFilter>)null!));
        Assert.Equal("assemblies", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_assembly_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromTypes(
            (IConventionTargets<Assembly>)null!));
        Assert.Equal("assemblies", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_type_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromTypes(
            (IConventionTargets<Type>)null!,
            t => t));
        Assert.Equal("targets", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_types_with_type_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromTypes(
            new DummyTypeTargets(),
            null!));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void Can_create_type_source()
    {
        var typeSource = ConventionTargets.FromTypes(
            a => a.FromAssemblyContaining<ConventionTargetsTests_Types>(),
            t => t.Where(x => x.Type == typeof(ConventionTargetsTests_Types)));

        var type = Assert.Single(typeSource.Targets);
        Assert.Same(typeof(ConventionTargetsTests_Types), type);
    }

    [Fact]
    public void Can_create_type_source_without_type_scan()
    {
        var typeSource = ConventionTargets.FromTypes(
            a => a.FromAssemblyContaining<ConventionTargetsTests_Types>());

        Assert.Contains(typeof(ConventionTargetsTests_Types), typeSource.Targets);
        Assert.Contains(typeof(SampleId), typeSource.Targets);
    }

    [Fact]
    public void Can_create_type_source_from_filtered_assemblies()
    {
        var typeSource = ConventionTargets.FromTypes(
            a => a
                .FromDependencyContext(DependencyContext.Load(typeof(ConventionTargetsTests_Types).Assembly)!)
                .Where(x => x.HasNamePrefix("ConventionAssertions") && !x.HasNameSegment("Tests")),
            t => t.Where(x => x.Type == typeof(Convention)));

        var type = Assert.Single(typeSource.Targets);
        Assert.Same(typeof(Convention), type);
    }

    [Fact]
    public void Can_create_type_source_from_assembly_targets()
    {
        var assemblies = ConventionTargets.FromAssemblies(a => a.FromAssemblyContaining<ConventionTargetsTests_Types>());

        var typeSource = ConventionTargets.FromTypes(
            assemblies,
            t => t.Where(x => x.Type == typeof(SampleId)));

        var type = Assert.Single(typeSource.Targets);
        Assert.Same(typeof(SampleId), type);
    }

    [Fact]
    public void Can_create_type_source_from_targets()
    {
        var shared = ConventionTargets.FromTypes(
            a => a.FromAssemblyContaining<ConventionTargetsTests_Types>(),
            t => t.Where(x => x.IsAssignableTo<IValueObject>()));

        var typeSource = ConventionTargets.FromTypes(
            shared,
            t => t.Where(x => x.Type == typeof(SampleId)));

        var type = Assert.Single(typeSource.Targets);
        Assert.Same(typeof(SampleId), type);
    }
}
