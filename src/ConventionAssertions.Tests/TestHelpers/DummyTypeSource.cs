using ConventionAssertions.Tests.Internal;

namespace ConventionAssertions.Tests.TestHelpers;

[SuppressConvention(nameof(TypeAssertTests.Can_suppress_convention))]
public class DummyTypeSource : ConventionTypeSource
{
    public DummyTypeSource()
    {
        Types = new[] { typeof(DummyTypeSource) };
    }
}
