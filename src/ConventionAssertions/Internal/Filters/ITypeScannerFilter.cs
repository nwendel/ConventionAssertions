namespace ConventionAssertions.Internal.Filters;

public interface ITypeScannerFilter : IFluentInterface
{
    ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate);
}
