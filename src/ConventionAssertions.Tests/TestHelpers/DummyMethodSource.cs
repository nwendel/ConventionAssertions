namespace ConventionAssertions.Tests.TestHelpers;

public class DummyMethodSource : ConventionMethodSource
{
    public DummyMethodSource()
        : base(new[] { typeof(DummyMethodSource) })
    {
        Methods = new[] { GetType().GetMethod(nameof(SomeMethod))! };
    }

    public static void SomeMethod()
    {
        // Never called, used for testing
    }
}
