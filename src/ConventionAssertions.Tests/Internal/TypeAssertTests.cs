using ConventionAssertions.Conventions;
using Xunit.Sdk;

namespace ConventionAssertions.Tests.Internal;

public class TypeAssertTests
{
    private readonly ConventionAssert<Type> _tested = new(new DummyTypeTargets());

    [Fact]
    public void Can_assert_generic_overload()
    {
        _tested.Assert<NothingTypeConvention>();
    }

    /*
    [Fact]
    public void Can_assert_instance_overload()
    {
        _tested.Assert(new NothingTypeConvention());
    }
    */

    [Fact]
    public void Throws_on_assert_generic_overload()
    {
        Assert.Throws<XunitException>(() => _tested.Assert<FailingTypeConvention>());
    }

    /*
    [Fact]
    public void Throws_on_assert_instance_overload()
    {
        Assert.Throws<XunitException>(() => _tested.Assert(new FailingTypeConvention()));
    }
    */

    [Fact]
    [SuppressConvention(Target = "ConventionAssertions.Tests.TestHelpers.DummyTypeTargets", Justification = "For testing")]
    public void Can_suppress_convention_action()
    {
        _tested.Assert(
            (type, context) =>
            {
                context.Fail("always fails");
            });
    }

    [Fact]
    [SuppressConvention(Target = "ConventionAssertions.Tests.TestHelpers.DummyTypeTargets", Justification = "For testing")]
    public void Can_suppress_convention_generic_overload()
    {
        _tested.Assert<FailingTypeConvention>();
    }

    [Fact]
    public void Can_assert_configured_convention()
    {
        _tested.Assert<HasNamespaceSegment>(x =>
        {
            x.Segment = "TestHelpers";
            x.SegmentPosition = SegmentPosition.Last;
        });
    }

    [Fact]
    public void Can_assert_generic_overload_because()
    {
        var ex = Assert.Throws<XunitException>(() => _tested.Assert<FailingTypeConvention>(because: "some reason"));

        Assert.EndsWith("DummyTypeTargets always fails, some reason.", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Can_assert_configured_convention_because()
    {
        var ex = Assert.Throws<XunitException>(() => _tested.Assert<HasNamespaceSegment>(
            x => x.Segment = "Missing",
            because: "some reason"));

        Assert.EndsWith("must belong to a namespace which contains a segment Missing, some reason.", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Can_assert_action_because()
    {
        var ex = Assert.Throws<XunitException>(() => _tested.Assert(
            (type, context) =>
            {
                context.Fail("first");
                context.Fail("second");
            },
            because: "some reason"));

        Assert.Contains("DummyTypeTargets first, some reason.", ex.Message, StringComparison.Ordinal);
        Assert.Contains("DummyTypeTargets second, some reason.", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Can_assert_without_because()
    {
        var ex = Assert.Throws<XunitException>(() => _tested.Assert<FailingTypeConvention>());

        Assert.EndsWith("DummyTypeTargets always fails.", ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Throws_on_assert_whitespace_because(string because)
    {
        Assert.Throws<ArgumentNullException>(() => _tested.Assert<FailingTypeConvention>(because: because));
    }
}
