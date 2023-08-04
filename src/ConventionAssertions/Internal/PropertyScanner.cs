using System.Collections;
using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public sealed class PropertyScanner :
    IConventionTargets<PropertyInfo>,
    IPropertyScanner
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

    public IPropertyScanner Where(Func<IPropertyFilter, bool> predicate)
    {
        _propertyInfos = _propertyInfos
            .Select(x => new PropertyFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Property)
            .ToList();

        return this;
    }

    IEnumerator<PropertyInfo> IEnumerable<PropertyInfo>.GetEnumerator() => _propertyInfos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _propertyInfos.GetEnumerator();
}
