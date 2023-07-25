using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public class PropertyScanner : ConventionPropertySource, IPropertyScanner
{
    public PropertyScanner(IEnumerable<Type> types)
        : base(types)
    {
    }

    public IPropertyScanner Where(Func<IPropertyFilter, bool> predicate)
    {
        Properties = Properties
            .Select(x => new PropertyFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Property)
            .ToList();

        return this;
    }
}
