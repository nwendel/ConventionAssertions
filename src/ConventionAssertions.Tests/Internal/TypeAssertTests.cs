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
    public void Can_suppress_convention()
    {
        _tested.Assert(
            (type, context) =>
            {
                context.Fail(type, "always fails");
            });
    }

    [Fact]
    [SuppressConvention(Target = "ConventionAssertions.Tests.TestHelpers.DummyTypeTargets", Justification = "For testing")]
    public void Can_suppress_convention2()
    {
        _tested.Assert<FailingTypeConvention>();
    }

    [Fact]
    public void Asdf()
    {
        _tested.Assert<HasNamespaceSegment>(x =>
        {
            x.Segment = "TestHelpers";
            x.SegmentPosition = SegmentPosition.Last;
        });
    }
}
