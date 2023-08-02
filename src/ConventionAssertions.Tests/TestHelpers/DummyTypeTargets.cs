namespace ConventionAssertions.Tests.TestHelpers;

public class DummyTypeTargets : IConventionTargets<Type>
{
    public DummyTypeTargets()
    {
        Targets = new[] { typeof(DummyTypeTargets) };
    }

    public IEnumerable<Type> Targets { get; private set; }
}
