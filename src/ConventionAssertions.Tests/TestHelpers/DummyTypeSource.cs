namespace ConventionAssertions.Tests.TestHelpers;

public class DummyTypeSource : ConventionTypeSource
{
    public DummyTypeSource()
    {
        Types = new[] { typeof(DummyTypeSource) };
    }
}
