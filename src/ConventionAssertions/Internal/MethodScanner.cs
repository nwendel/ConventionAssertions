using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public class MethodScanner : ConventionMethodSource, IMethodScanner
{
    public MethodScanner(IEnumerable<Type> types)
        : base(types)
    {
    }

    public IMethodScanner Where(Func<IMethodFilter, bool> predicate)
    {
        Methods = Methods
            .Select(x => new MethodFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Method)
            .ToList();

        return this;
    }
}
