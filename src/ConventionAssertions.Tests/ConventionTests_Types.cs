using Xunit.Sdk;

namespace ConventionAssertions.Tests;

public class ConventionTests_Types
{
    [Fact]
    public void Throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            null!,
            a => a.Assert<NothingTypeConvention>()));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_assert_action()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            s => s.FromAssemblyContaining<ConventionTests_Types>(),
            null!));
        Assert.Equal("assertAction", ex.ParamName);
    }

    [Fact]
    public void Can_scan_and_assert()
    {
        var asserted = new List<Type>();

        Convention.ForTypes(
            s => s
                .FromAssemblyContaining<ConventionTests_Types>()
                .Where(t => t.Type == typeof(ConventionTests_Types)),
            a => a.Assert((type, context) => asserted.Add(type)));

        var type = Assert.Single(asserted);
        Assert.Same(typeof(ConventionTests_Types), type);
    }

    [Fact]
    public void Throws_on_no_targets()
    {
        Assert.Throws<XunitException>(() => Convention.ForTypes(
            s => s
                .FromAssemblyContaining<ConventionTests_Types>()
                .Where(t => false),
            a => a.Assert<NothingTypeConvention>()));
    }
}
