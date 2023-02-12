using ConventionAssertions.Tests.Internal;

namespace ConventionAssertions.Tests.TestHelpers;

[SuppressConvention(nameof(TypeAssertTests.Can_suppress_convention), Justification = "For unit test")]
public class DummyTypeSource : ConventionTypeSource
{
    public DummyTypeSource()
    {
        Types = new[] { typeof(DummyTypeSource) };
    }
}
