using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public interface ITypeScannerFilter : IFluentInterface, IConventionTargets<Type>
{
    ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate);
}
