using System.Collections;

namespace ConventionAssertions.Tests.TestHelpers;

public sealed class DummyTypeTargets : IConventionTargets<Type>
{
    private readonly IEnumerable<Type> _types;

    public DummyTypeTargets()
    {
        _types = new[] { typeof(DummyTypeTargets) };
    }

    IEnumerator<Type> IEnumerable<Type>.GetEnumerator() => _types.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _types.GetEnumerator();
}
