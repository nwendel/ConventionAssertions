using System.Reflection;

namespace ConventionAssertions.Tests.TestHelpers;

public class DummyMethodTargets : IConventionTargets<MethodInfo>
{
    public DummyMethodTargets()
    {
        Targets = new[] { GetType().GetMethod(nameof(SomeMethod))! };
    }

    public IEnumerable<MethodInfo> Targets { get; private set; }

    public static void SomeMethod()
    {
        // Never called, used for testing
    }
}
