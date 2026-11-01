using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public sealed class MethodScanner :
    IConventionTargets<MethodInfo>,
    IMethodScanner
{
    private IEnumerable<MethodInfo> _methodInfos;

    public MethodScanner(IEnumerable<Type> types)
    {
        // TODO: Non public
        _methodInfos = types
            .SelectMany(x => x.GetMethods())
            .ToList();
    }

    IEnumerable<MethodInfo> IConventionTargets<MethodInfo>.Targets => _methodInfos;

    public IMethodScanner Where(Func<IMethodFilter, bool> predicate)
    {
        _methodInfos = _methodInfos
            .Select(x => new MethodFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Method)
            .ToList();

        return this;
    }
}
