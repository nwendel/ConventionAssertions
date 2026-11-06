using ConventionAssertions.Conventions;
using Microsoft.Extensions.DependencyModel;
using Xunit.Sdk;

namespace ConventionAssertions.Tests;

public class ConventionTargetsTests_Assemblies
{
    [Fact]
    public void Throws_on_null_assemblies()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromAssemblies(null!));
        Assert.Equal("assemblies", ex.ParamName);
    }

    [Fact]
    public void Can_create_assembly_source()
    {
        var assemblySource = ConventionTargets.FromAssemblies(a => a
            .FromAssemblyContaining<ConventionTargetsTests_Assemblies>());

        var assembly = Assert.Single(assemblySource.Targets);
        Assert.Same(typeof(ConventionTargetsTests_Assemblies).Assembly, assembly);
    }

    [Fact]
    public void Can_create_assembly_source_with_predicate()
    {
        var assemblySource = ConventionTargets.FromAssemblies(
            a => a.FromDependencyContext(DependencyContext.Load(typeof(ConventionTargetsTests_Assemblies).Assembly)!),
            x =>
                x.HasNamePrefix("ConventionAssertions") &&
                !x.HasNameSegment("Tests"));

        Assert.Contains(typeof(Convention).Assembly, assemblySource.Targets);
        Assert.DoesNotContain(typeof(ConventionTargetsTests_Assemblies).Assembly, assemblySource.Targets);
    }

    [Fact]
    public void Throws_on_null_predicate()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromAssemblies(
            a => a.FromAssemblyContaining<ConventionTargetsTests_Assemblies>(),
            null!));
        Assert.Equal("predicate", ex.ParamName);
    }

    [Fact]
    public void Can_assert_assembly_targets()
    {
        Convention.For(
            ConventionTargets.FromAssemblies(a => a.FromAssemblyContaining<ConventionTargetsTests_Assemblies>()),
            a => a.Assert<HasNoAssemblyReferenceWithNameSegment>(c => c.Segment = "Blaappen"));
    }

    [Fact]
    public void Throws_on_assert_assembly_targets()
    {
        var ex = Assert.Throws<XunitException>(() => Convention.For(
            ConventionTargets.FromAssemblies(a => a.FromAssemblyContaining<ConventionTargetsTests_Assemblies>()),
            a => a.Assert<HasNoAssemblyReferenceWithNameSegment>(c =>
            {
                c.Segment = "ConventionAssertions";
                c.SegmentPosition = SegmentPosition.First;
            })));

        Assert.Contains(
            "Assembly ConventionAssertions.Tests must not reference ConventionAssertions (ConventionAssertions.Tests -> ConventionAssertions).",
            ex.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    [SuppressConvention(Target = "ConventionAssertions.Tests", Justification = "For testing")]
    public void Can_suppress_assembly_target()
    {
        Convention.For(
            ConventionTargets.FromAssemblies(a => a.FromAssemblyContaining<ConventionTargetsTests_Assemblies>()),
            a => a.Assert((assembly, context) => context.Fail("always fails")));
    }
}
