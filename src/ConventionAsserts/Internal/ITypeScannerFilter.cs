namespace ConventionAsserts.Internal;

public interface ITypeScannerFilter
{
    ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate);
}
