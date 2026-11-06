using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public interface ITypeScanner : IFluentInterface
{
    ITypeScanner Where(Func<ITypeFilter, bool> predicate);
}
