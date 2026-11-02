using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public interface ITypeScannerFilter : IFluentInterface
{
    ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate);
}
