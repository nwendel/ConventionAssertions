using ConventionAssertions.Internal;
using ConventionAssertions.Tests.TestHelpers;
using Xunit.Sdk;

namespace ConventionAssertions.Tests.Internal;

public class TypeAssertTests
{
    private readonly TypeAssert _tested = new(new DummyTypeSource());

    [Fact]
    public void Can_assert_generic_overload()
    {
        _tested.Assert<DummyTypeConvention>();
    }

    [Fact]
    public void Can_assert_instance_overload()
    {
        _tested.Assert(new DummyTypeConvention());
    }

    [Fact]
    public void Throws_on_assert_generic_overload()
    {
        Assert.Throws<XunitException>(() => _tested.Assert<FailingTypeConvention>());
    }

    [Fact]
    public void Throws_on_assert_instance_overload()
    {
        Assert.Throws<XunitException>(() => _tested.Assert(new FailingTypeConvention()));
    }

    private class DummyTypeConvention : ITypeConvention
    {
        public void Assert(Type type, ConventionContext context)
        {
            // Convetion should always succeed
        }
    }

    private class FailingTypeConvention : ITypeConvention
    {
        public void Assert(Type type, ConventionContext context)
        {
            context.Fail(type, "always fails");
        }
    }
}
