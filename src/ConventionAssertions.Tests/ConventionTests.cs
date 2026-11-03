using System.Reflection;
using Xunit.Sdk;

namespace ConventionAssertions.Tests;

public class ConventionTests
{
    private static readonly IConventionTargets<Type> TwoTypes = ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<ConventionTests>()
        .Where(t => t.Type == typeof(SampleId) || t.Type == typeof(PublicClass)));

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
    public void Can_scan_and_assert_types()
    {
        var asserted = new List<Type>();

        Convention.For(
            ConventionTargets.FromTypes(s => s
                .FromAssemblyContaining<ConventionTests>()
                .Where(t => t.Type == typeof(ConventionTests))),
            a => a.Assert((type, context) => asserted.Add(type)));

        var type = Assert.Single(asserted);
        Assert.Same(typeof(ConventionTests), type);
    }

    [Fact]
    public void Throws_on_no_targets()
    {
        Assert.Throws<XunitException>(() => Convention.For(
            ConventionTargets.FromTypes(s => s
                .FromAssemblyContaining<ConventionTests>()
                .Where(t => false)),
            a => a.Assert<NothingTypeConvention>()));
    }

    [Fact]
    public void Reports_single_violation()
    {
        var ex = Assert.Throws<XunitException>(() => Convention.For(
            new DummyTypeTargets(),
            a => a.Assert<FailingTypeConvention>()));

        Assert.Equal($"Found 1.{Environment.NewLine}Type ConventionAssertions.Tests.TestHelpers.DummyTypeTargets always fails.", ex.Message);
    }

    [Fact]
    public void Reports_multiple_violations()
    {
        var ex = Assert.Throws<XunitException>(() => Convention.For(
            TwoTypes,
            a => a.Assert<FailingTypeConvention>()));

        Assert.StartsWith("Found 2.", ex.Message, StringComparison.Ordinal);
        Assert.Contains("Type ConventionAssertions.Tests.TestHelpers.SampleId always fails.", ex.Message, StringComparison.Ordinal);
        Assert.Contains("Type ConventionAssertions.Tests.TestHelpers.PublicClass always fails.", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    [SuppressConvention(Target = "ConventionAssertions.Tests.TestHelpers.SampleId", Justification = "For testing")]
    [SuppressConvention(Target = "ConventionAssertions.Tests.TestHelpers.PublicClass", Justification = "For testing")]
    public void Can_suppress_violations()
    {
        Convention.For(
            TwoTypes,
            a => a.Assert<FailingTypeConvention>());
    }

    [Fact]
    [SuppressConvention(Target = "ConventionAssertions.Tests.TestHelpers.SampleId", Justification = "For testing")]
    public void Reports_violations_not_suppressed()
    {
        // Called directly, since suppressions are read from the method calling Convention.For
        XunitException? ex = null;
        try
        {
            Convention.For(
                TwoTypes,
                a => a.Assert<FailingTypeConvention>());
        }
        catch (XunitException e)
        {
            ex = e;
        }

        Assert.NotNull(ex);
        Assert.StartsWith("Found 1.", ex.Message, StringComparison.Ordinal);
        Assert.Contains("PublicClass", ex.Message, StringComparison.Ordinal);
    }
}
