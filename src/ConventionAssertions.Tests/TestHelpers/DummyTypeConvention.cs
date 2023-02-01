namespace ConventionAssertions.Tests.TestHelpers;

public class DummyTypeConvention : ITypeConvention
{
    private readonly List<Type> _assertedTypes = new();

    public IEnumerable<Type> AssertedTypes => _assertedTypes;

    public void Assert(Type type, ConventionContext context)
    {
        _assertedTypes.Add(type);
    }
}
