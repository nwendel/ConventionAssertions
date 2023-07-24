using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public interface IPropertyScanner : IFluentInterface
{
    IPropertyScanner Where(Func<IPropertyFilter, bool> predicate);
}
