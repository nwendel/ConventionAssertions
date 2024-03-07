using System.Collections;

namespace ConventionAssertions.Tests.TestHelpers;

public sealed class DummyTypeTargets : IConventionTargets<Type>
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Investigate later")]
    private readonly IEnumerable<Type> _types;

    public DummyTypeTargets()
    {
        _types = new[] { typeof(DummyTypeTargets) };
    }

    IEnumerator<Type> IEnumerable<Type>.GetEnumerator() => _types.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _types.GetEnumerator();
}
