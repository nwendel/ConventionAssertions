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
    public void Can_Where()
    {
        _tested
            .FromDependencyContext(DependencyContext.Load(TestAssembly)!)
            .Where(x => x.HasNamePrefix("ConventionAssertions") && !x.HasNameSegment("Tests"));

        Assert.Contains(typeof(Convention).Assembly, Targets);
        Assert.DoesNotContain(TestAssembly, Targets);
        Assert.All(Targets, x => Assert.StartsWith("ConventionAssertions", x.GetName().Name, StringComparison.Ordinal));
    }

    [Fact]
    public void Can_Where_more_than_once()
    {
        _tested
            .FromDependencyContext(DependencyContext.Load(TestAssembly)!)
            .Where(x => x.HasNamePrefix("ConventionAssertions"))
            .Where(x => x.HasNameSegment("Tests"));

        var assembly = Assert.Single(Targets);
        Assert.Same(TestAssembly, assembly);
    }

    [Fact]
    public void Can_Where_before_loading()
    {
        // Where runs on the names, so an assembly that is filtered out is never loaded
        _tested
            .FromDependencyContext(MissingAssemblyContext())
            .Where(x => !x.HasNamePrefix("Does.Not.Exist"));

        Assert.Empty(Targets);
    }

    [Fact]
    public void Throws_on_loading_missing_assembly()
    {
        _tested
            .FromDependencyContext(MissingAssemblyContext());

        Assert.Throws<FileNotFoundException>(() => Targets);
    }

    [Fact]
    public void Throws_on_FromDependencyContext_null()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => _tested.FromDependencyContext(null!));
        Assert.Equal("dependencyContext", ex.ParamName);
    }

    [Fact]
    public void Throws_on_Where_null()
    {
        _tested.FromAssemblyContaining<AssemblyScannerTests>();

        var ex = Assert.Throws<ArgumentNullException>(() => _tested.Where(null!));
        Assert.Equal("predicate", ex.ParamName);
    }

    private static DependencyContext MissingAssemblyContext()
    {
        var library = new RuntimeLibrary(
            "project",
            "Does.Not.Exist",
            "1.0.0",
            string.Empty,
            [new RuntimeAssetGroup(string.Empty, "Does.Not.Exist.dll")],
            [],
            [],
            [],
            serviceable: false);

        return new DependencyContext(
            new TargetInfo(".NETCoreApp,Version=v8.0", null, null, isPortable: true),
            CompilationOptions.Default,
            [],
            [library],
            []);
    }
}
