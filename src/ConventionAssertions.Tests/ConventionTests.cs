using ConventionAssertions.Internal;
using ConventionAssertions.Rules;

namespace ConventionAssertions.Tests;

public class ConventionTests
{
    [Fact]
    public void Throws_on_null_scanner()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            (Action<ITypeScanner>)null!,
            x => x.Assert<NoPublicConstructors>()));
        Assert.Equal("scanner", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_scanner_with_assert()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(null!));
        Assert.Equal("scanner", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_typesource()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            (ConventionTypeSource)null!,
            x => x.Assert<NoPublicConstructors>()));
        Assert.Equal("typeSource", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_assert()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            new DummyTypeSource(),
            null!));
        Assert.Equal("assert", ex.ParamName);
    }

    [Fact]
    public void Can_assert_type_source_single_type()
    {
        var asserted = new List<Type>();

        Convention.ForTypes(
            new DummyTypeSource(),
            x => x.Assert((type, context) => asserted.Add(type)));

        var type = Assert.Single(asserted);
        Assert.Same(typeof(DummyTypeSource), type);
    }

    private class DummyTypeSource : ConventionTypeSource
    {
        public DummyTypeSource()
        {
            Types = new[] { typeof(DummyTypeSource) };
        }
    }
}
