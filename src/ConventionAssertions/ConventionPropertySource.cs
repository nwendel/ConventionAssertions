using System.Reflection;

namespace ConventionAssertions;

public abstract class ConventionPropertySource
{
    protected ConventionPropertySource(IEnumerable<Type> types)
    {
        GuardAgainst.Null(types);

        // TODO: Non public
        Properties = types
            .SelectMany(x => x.GetProperties())
            .ToList();
    }

    public IEnumerable<PropertyInfo> Properties { get; protected set; }
}
