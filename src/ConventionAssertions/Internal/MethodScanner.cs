using System.Reflection;
using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public class MethodScanner :
    IConventionTargets<MethodInfo>,
    IMethodScanner
{
    public MethodScanner(IEnumerable<Type> types)
    {
        // TODO: Non public
        Targets = types
            .SelectMany(x => x.GetMethods())
            .ToList();
    }

    public IEnumerable<MethodInfo> Targets { get; private set; }

    public IMethodScanner Where(Func<IMethodFilter, bool> predicate)
    {
        Targets = Targets
            .Select(x => new MethodFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Method)
            .ToList();

        return this;
    }
}
