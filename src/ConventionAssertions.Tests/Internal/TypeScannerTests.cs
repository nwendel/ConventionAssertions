namespace ConventionAssertions.Tests.Internal;

public class TypeScannerTests
{
    private readonly TypeScanner _tested = new();

    private IEnumerable<Type> Targets => ((IConventionTargets<Type>)_tested).Targets;

    [Fact]
    public void Can_FromAssemblyContaining()
    {
        _tested
            .FromAssemblyContaining<TypeScannerTests>();

        Assert.Contains(typeof(TypeScannerTests), Targets);
    }

    [Fact]
    public void Can_Where()
    {
        _tested
            .FromAssemblyContaining<TypeScannerTests>()
            .Where(t => t.IsAssignableTo<TypeScannerTests>());

        Assert.Single(Targets);
        Assert.Contains(typeof(TypeScannerTests), Targets);
    }
}
