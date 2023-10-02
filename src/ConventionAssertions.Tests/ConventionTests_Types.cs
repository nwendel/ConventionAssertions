namespace ConventionAssertions.Tests;

public class ConventionTests_Types
{
    [Fact]
    public void Throws_on_null_scanner_action()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            null!,
            x => x.Assert<NothingTypeConvention>()));
        Assert.Equal("scanner", ex.ParamName);
    }

    /*
    [Fact]
    public void Throws_on_null_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            (IConventionTargets<Type>)null!,
            x => x.Assert<NothingTypeConvention>()));
        Assert.Equal("targets", ex.ParamName);
    }
    */

    /*
    [Fact]
    public void Throws_on_null_assert_action()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            new DummyTypeTargets(),
            null!));
        Assert.Equal("assertAction", ex.ParamName);
    }
    */

    /*
    [Fact]
    public void Can_assert_targets_single_type()
    {
        var asserted = new List<Type>();

        Convention.ForTypes(
            new DummyTypeTargets(),
            x => x.Assert(
                (type, context) => asserted.Add(type)));

        var type = Assert.Single(asserted);
        Assert.Same(typeof(DummyTypeTargets), type);
    }
    */

    /*
    [Fact]
    public void Can_scan_and_assert()
    {
        var convention = new NothingTypeConvention();

        Convention.ForTypes(
            s => s
                .FromAssemblyContaining<ConventionTests_Types>()
                .Where(t => t.Type == typeof(ConventionTests_Types)),
            x => x.Assert<>(convention));

        var type = Assert.Single(convention.AssertedTypes);
        Assert.Same(typeof(ConventionTests_Types), type);
    }
    */
}
