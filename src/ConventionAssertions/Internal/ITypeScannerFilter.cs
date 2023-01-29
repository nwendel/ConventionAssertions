namespace ConventionAssertions.Internal;

public interface ITypeScannerFilter
{
    ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate);
}
