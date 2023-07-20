using ConventionAssertions.Internal;

namespace ConventionAssertions.Tests.Internal;

public class TypeScannerTests
{
    private readonly TypeScanner _tested = new();

    [Fact]
    public void Can_FromAssemblyContaining()
    {
        _tested
            .FromAssemblyContaining<TypeScannerTests>();

        Assert.Contains(typeof(TypeScannerTests), _tested.Types);
    }

    [Fact]
    public void Can_Where()
    {
        _tested
            .FromAssemblyContaining<TypeScannerTests>()
            .Where(t => t.IsAssignableTo<TypeScannerTests>());

        Assert.Single(_tested.Types);
        Assert.Contains(typeof(TypeScannerTests), _tested.Types);
    }
}
