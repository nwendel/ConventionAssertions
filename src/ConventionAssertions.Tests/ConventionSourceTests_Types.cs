namespace ConventionAssertions.Tests;

public class ConventionSourceTests_Types
{
    [Fact]
    public void Throws_on_null_scanner_with_assert()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionSource.FromTypes(null!));
        Assert.Equal("typeScanner", ex.ParamName);
    }

    [Fact]
    public void Can_create_type_source()
    {
        var typeSource = ConventionSource.FromTypes(s => s
            .FromAssemblyContaining<ConventionSourceTests_Types>()
            .Where(t => t.Type == typeof(ConventionSourceTests_Types)));

        var type = Assert.Single(typeSource.Types);
        Assert.Same(typeof(ConventionSourceTests_Types), type);
    }
}
