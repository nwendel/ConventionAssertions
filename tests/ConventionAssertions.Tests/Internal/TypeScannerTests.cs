namespace ConventionAssertions.Tests.Internal;

public class TypeScannerTests
{
    private readonly TypeScanner _tested = new(typeof(TypeScannerTests).Assembly.GetTypes());

    private IEnumerable<Type> Targets => _tested.Types;

    [Fact]
    public void Can_create()
    {
        Assert.Contains(typeof(TypeScannerTests), Targets);
    }

    [Fact]
    public void Can_Where()
    {
        _tested
            .Where(t => t.IsAssignableTo<TypeScannerTests>());

        Assert.Single(Targets);
        Assert.Contains(typeof(TypeScannerTests), Targets);
    }

    [Fact]
    public void Can_Where_more_than_once()
    {
        _tested
            .Where(t => t.IsClass)
            .Where(t => t.Type == typeof(TypeScannerTests));

        var type = Assert.Single(Targets);
        Assert.Same(typeof(TypeScannerTests), type);
    }

    [Fact]
    public void Throws_on_create_null()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new TypeScanner(null!));
        Assert.Equal("types", ex.ParamName);
    }
}
