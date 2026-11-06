using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Tests.Internal;

public class AssemblyScannerTests
{
    private static readonly Assembly TestAssembly = typeof(AssemblyScannerTests).Assembly;

    private readonly AssemblyScanner _tested = new();

    private IEnumerable<Assembly> Targets => _tested.Assemblies;

    [Fact]
    public void Can_FromAssemblyContaining()
    {
        _tested
            .FromAssemblyContaining<AssemblyScannerTests>();

        var assembly = Assert.Single(Targets);
        Assert.Same(TestAssembly, assembly);
    }

    [Fact]
    public void Can_FromDependencyContext()
    {
        _tested
            .FromDependencyContext(DependencyContext.Load(TestAssembly)!);

        Assert.Contains(TestAssembly, Targets);
        Assert.Contains(typeof(Convention).Assembly, Targets);
    }

    [Fact]
    public void Can_FromDependencyContext_with_predicate()
    {
        _tested
            .FromDependencyContext(DependencyContext.Load(TestAssembly)!, x => x.HasNamePrefix("ConventionAssertions") && !x.HasNameSegment("Tests"));

        Assert.Contains(typeof(Convention).Assembly, Targets);
        Assert.DoesNotContain(TestAssembly, Targets);
        Assert.All(Targets, x => Assert.StartsWith("ConventionAssertions", x.GetName().Name, StringComparison.Ordinal));
    }

    [Fact]
    public void Can_Where()
    {
        _tested
            .FromDependencyContext(DependencyContext.Load(TestAssembly)!)
            .Where(x => x.HasNameSegment("Tests") && x.HasNamePrefix("ConventionAssertions"));

        var assembly = Assert.Single(Targets);
        Assert.Same(TestAssembly, assembly);
    }

    [Fact]
    public void Throws_on_FromDependencyContext_null()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => _tested.FromDependencyContext(null!));
        Assert.Equal("dependencyContext", ex.ParamName);
    }
}
