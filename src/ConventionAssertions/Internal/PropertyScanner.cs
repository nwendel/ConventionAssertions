using System.Reflection;
using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public class PropertyScanner :
    IConventionTargets<PropertyInfo>,
    IPropertyScanner
{
    public PropertyScanner(IEnumerable<Type> types)
    {
        GuardAgainst.Null(types);

        // TODO: Non public
        Targets = types
            .SelectMany(x => x.GetProperties())
            .ToList();
    }

    public IEnumerable<PropertyInfo> Targets { get; private set; }

    public IPropertyScanner Where(Func<IPropertyFilter, bool> predicate)
    {
        Targets = Targets
            .Select(x => new PropertyFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Property)
            .ToList();

        return this;
    }
}
