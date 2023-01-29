namespace ConventionAssertions.Internal;

public interface ITypeScannerFilter : IFluentInterface
{
    ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate);
}
