using System.Reflection;

namespace ConventionAssertions;

public abstract class ConventionMethodSource
{
    protected ConventionMethodSource(IEnumerable<Type> types)
    {
        // TODO: Non public
        Methods = types
            .SelectMany(x => x.GetMethods())
            .ToList();
    }

    public IEnumerable<MethodInfo> Methods { get; protected set; }
}
