using ConventionAssertions.Internal;
using ConventionAssertions.Tests.TestHelpers;

namespace ConventionAssertions.Tests;

public class ConventionTests_Types
{
    [Fact]
    public void Throws_on_null_scanner()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            (Action<ITypeScanner>)null!,
            x => x.Assert<NothingTypeConvention>()));
        Assert.Equal("typeScanner", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_scanner_with_assert()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(null!));
        Assert.Equal("typeScanner", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_type_source()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForTypes(
            (ConventionTypeSource)null!,
            x => x.Assert<NothingTypeConvention>()));
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
            x => x.Assert(
                (type, context) => asserted.Add(type)));

        var type = Assert.Single(asserted);
        Assert.Same(typeof(DummyTypeSource), type);
    }

    [Fact]
    public void Can_create_type_source()
    {
        var typeSource = Convention.ForTypes(s => s
            .FromAssemblyContaining<ConventionTests_Types>()
            .Where(t => t.Type == typeof(ConventionTests_Types)));

        var type = Assert.Single(typeSource.Types);
        Assert.Same(typeof(ConventionTests_Types), type);
    }

    [Fact]
    public void Can_scan_and_assert()
    {
        var convention = new NothingTypeConvention();

        Convention.ForTypes(
            s => s
                .FromAssemblyContaining<ConventionTests_Types>()
                .Where(t => t.Type == typeof(ConventionTests_Types)),
            x => x.Assert(convention));

        var type = Assert.Single(convention.AssertedTypes);
        Assert.Same(typeof(ConventionTests_Types), type);
    }
}
