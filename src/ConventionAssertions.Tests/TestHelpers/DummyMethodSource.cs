namespace ConventionAssertions.Tests.TestHelpers;

public class DummyMethodSource : ConventionMethodSource
{
    public DummyMethodSource()
        : base(new[] { typeof(DummyMethodSource) })
    {
        Methods = new[] { GetType().GetMethods()[0] };
    }
}
