namespace ConventionAssertions.Internal;

public class TypeScanner : ConventionTypeSource, ITypeScanner, ITypeScannerFilter
{
    public ITypeScannerFilter FromAssemblyContaining<T>()
    {
        Types = typeof(T).Assembly.GetTypes();
        return this;
    }

    public ITypeScannerFilter FromTypeSource(ConventionTypeSource typeSource)
    {
        GuardAgainst.Null(typeSource);

        Types = typeSource.Types;
        return this;
    }

    public ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate)
    {
        GuardAgainst.Null(predicate);

        Types = Types
            .Select(x => new TypeFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Type)
            .ToList();
        return this;
    }
}
