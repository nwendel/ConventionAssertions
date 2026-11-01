using System.Reflection;
using Xunit.Sdk;

namespace ConventionAssertions.Tests;

public class ConventionTests_For
{
    [Fact]
    public void Throws_on_null_scanner()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.For(
            (Func<ITypeScanner, IConventionTargets<Type>>)null!,
            a => a.Assert<NothingTypeConvention>()));
        Assert.Equal("scanner", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.For(
            (IConventionTargets<Type>)null!,
            a => a.Assert<NothingTypeConvention>()));
        Assert.Equal("targets", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_assert_action_with_scanner()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.For(
            s => s.FromAssemblyContaining<ConventionTests_For>(),
            null!));
        Assert.Equal("assertAction", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_assert_action_with_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.For(
            new DummyTypeTargets(),
            null!));
        Assert.Equal("assertAction", ex.ParamName);
    }

    [Fact]
    public void Can_assert_type_targets()
    {
        var asserted = new List<Type>();

        Convention.For(
            new DummyTypeTargets(),
            a => a.Assert((type, context) => asserted.Add(type)));

        var type = Assert.Single(asserted);
        Assert.Same(typeof(DummyTypeTargets), type);
    }

    [Fact]
    public void Can_assert_method_targets()
    {
        var asserted = new List<MethodInfo>();

        Convention.For(
            new DummyMethodTargets(),
            a => a.Assert((method, context) => asserted.Add(method)));

        var method = Assert.Single(asserted);
        Assert.Equal(nameof(DummyMethodTargets.SomeMethod), method.Name);
    }

    [Fact]
    public void Can_scan_and_assert_types()
    {
        var asserted = new List<Type>();

        Convention.For(
            s => s
                .FromAssemblyContaining<ConventionTests_For>()
                .Where(t => t.Type == typeof(ConventionTests_For)),
            a => a.Assert((type, context) => asserted.Add(type)));

        var type = Assert.Single(asserted);
        Assert.Same(typeof(ConventionTests_For), type);
    }

    [Fact]
    public void Can_scan_and_assert_methods()
    {
        var asserted = new List<MethodInfo>();

        Convention.For(
            s => s
                .FromAssemblyContaining<ConventionTests_For>()
                .Where(t => t.Type == typeof(DummyMethodTargets))
                .Methods(),
            a => a.Assert((method, context) => asserted.Add(method)));

        var method = Assert.Single(asserted);
        Assert.Equal(nameof(DummyMethodTargets.SomeMethod), method.Name);
    }

    [Fact]
    public void Can_scan_and_assert_properties()
    {
        var asserted = new List<PropertyInfo>();

        Convention.For(
            s => s
                .FromAssemblyContaining<ConventionTests_For>()
                .Where(t => t.Type == typeof(SampleId))
                .Properties(),
            a => a.Assert((property, context) => asserted.Add(property)));

        var property = Assert.Single(asserted);
        Assert.Equal(nameof(SampleId.Value), property.Name);
    }

    [Fact]
    public void Throws_on_failing_convention()
    {
        Assert.Throws<XunitException>(() => Convention.For(
            new DummyTypeTargets(),
            a => a.Assert<FailingTypeConvention>()));
    }

    [Fact]
    public void Throws_on_no_targets()
    {
        Assert.Throws<XunitException>(() => Convention.For(
            s => s
                .FromAssemblyContaining<ConventionTests_For>()
                .Where(t => false),
            a => a.Assert<NothingTypeConvention>()));
    }
}
