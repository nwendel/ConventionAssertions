namespace ConventionAssertions.Tests.TestHelpers;

public class NothingTypeConvention : IConvention<Type>
{
    private readonly List<Type> _assertedTypes = new();

    public IEnumerable<Type> AssertedTypes => _assertedTypes;

    public void Assert(Type target, ConventionContext context)
    {
        _assertedTypes.Add(target);
    }
}
