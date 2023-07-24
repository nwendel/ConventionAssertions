using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public interface IMethodScanner : IFluentInterface
{
    IMethodScanner Where(Func<IMethodFilter, bool> predicate);
}
