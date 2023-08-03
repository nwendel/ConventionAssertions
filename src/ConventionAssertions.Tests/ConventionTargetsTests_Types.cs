namespace ConventionAssertions.Tests;

public class ConventionTargetsTests_Types
{
    [Fact]
    public void Throws_on_null_scanner_with_assert()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromTypes(null!));
        Assert.Equal("scannerAction", ex.ParamName);
    }

    [Fact]
    public void Can_create_type_source()
    {
        var typeSource = ConventionTargets.FromTypes(s => s
            .FromAssemblyContaining<ConventionTargetsTests_Types>()
            .Where(t => t.Type == typeof(ConventionTargetsTests_Types)));

        var type = Assert.Single(typeSource);
        Assert.Same(typeof(ConventionTargetsTests_Types), type);
    }
}
