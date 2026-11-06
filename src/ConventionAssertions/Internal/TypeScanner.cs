using ConventionAssertions.Internal.Filters;
using TypeFilter = ConventionAssertions.Internal.Filters.TypeFilter;

namespace ConventionAssertions.Internal;

public sealed class TypeScanner : ITypeScanner
{
    private IEnumerable<Type> _types;

    public TypeScanner(IEnumerable<Type> types)
    {
        GuardAgainst.Null(types);

        _types = types.ToList();
    }

    internal IEnumerable<Type> Types => _types;

    public ITypeScanner Where(Func<ITypeFilter, bool> predicate)
    {
        GuardAgainst.Null(predicate);

        _types = _types
            .Select(x => new TypeFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Type)
            .ToList();
        return this;
    }
}
