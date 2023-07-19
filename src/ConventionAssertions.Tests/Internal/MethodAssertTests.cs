using ConventionAssertions.Internal;
using ConventionAssertions.Tests.TestHelpers;
using Xunit.Sdk;

namespace ConventionAssertions.Tests.Internal;

public class MethodAssertTests
{
    private readonly MethodAssert _tested = new(new DummyMethodSource());

    [Fact]
    public void Can_assert_generic_overload()
    {
        _tested.Assert<NothingMethodConvention>();
    }

    [Fact]
    public void Can_assert_instance_overload()
    {
        _tested.Assert(new NothingMethodConvention());
    }

    [Fact]
    public void Throws_on_assert_generic_overload()
    {
        Assert.Throws<XunitException>(() => _tested.Assert<FailingMethodConvention>());
    }

    [Fact]
    public void Throws_on_assert_instance_overload()
    {
        Assert.Throws<XunitException>(() => _tested.Assert(new FailingMethodConvention()));
    }
}
