using System.Reflection;
using Xunit.Sdk;

namespace ConventionAssertions.Tests;

public class ConventionTests_For
{
    [Fact]
    public void Throws_on_null_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.For(
            (IConventionTargets<Type>)null!,
            a => a.Assert<NothingTypeConvention>()));
        Assert.Equal("targets", ex.ParamName);
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
    public void Throws_on_failing_convention()
    {
        Assert.Throws<XunitException>(() => Convention.For(
            new DummyTypeTargets(),
            a => a.Assert<FailingTypeConvention>()));
    }
}
