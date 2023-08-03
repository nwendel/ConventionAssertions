using System.Collections;
using System.Reflection;

namespace ConventionAssertions.Tests.TestHelpers;

public sealed class DummyMethodTargets : IConventionTargets<MethodInfo>
{
    private readonly IEnumerable<MethodInfo> _methodInfos;

    public DummyMethodTargets()
    {
        _methodInfos = new[] { GetType().GetMethod(nameof(SomeMethod))! };
    }

    public static void SomeMethod()
    {
        // Never called, used for testing
    }

    IEnumerator<MethodInfo> IEnumerable<MethodInfo>.GetEnumerator() => _methodInfos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _methodInfos.GetEnumerator();
}
