using System.Reflection;

namespace ConventionAssertions.Tests.TestHelpers;

public sealed class DummyMethodTargets : IConventionTargets<MethodInfo>
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Investigate later")]
    private readonly IEnumerable<MethodInfo> _methodInfos;

    public DummyMethodTargets()
    {
        _methodInfos = new[] { GetType().GetMethod(nameof(SomeMethod))! };
    }

    IEnumerable<MethodInfo> IConventionTargets<MethodInfo>.Targets => _methodInfos;

    public static void SomeMethod()
    {
        // Never called, used for testing
    }
}
