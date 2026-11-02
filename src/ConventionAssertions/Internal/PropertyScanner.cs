using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public sealed class PropertyScanner : IPropertyScanner
{
    private IEnumerable<PropertyInfo> _propertyInfos;

    public PropertyScanner(IEnumerable<Type> types)
    {
        GuardAgainst.Null(types);

        // TODO: Non public
        _propertyInfos = types
            .SelectMany(x => x.GetProperties())
            .ToList();
    }

    internal PropertyScanner(IConventionTargets<PropertyInfo> targets)
    {
        GuardAgainst.Null(targets);

        _propertyInfos = targets.Targets.ToList();
    }

    internal IEnumerable<PropertyInfo> Properties => _propertyInfos;

    public IPropertyScanner Where(Func<IPropertyFilter, bool> predicate)
    {
        GuardAgainst.Null(predicate);

        _propertyInfos = _propertyInfos
            .Select(x => new PropertyFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Property)
            .ToList();

        return this;
    }
}
