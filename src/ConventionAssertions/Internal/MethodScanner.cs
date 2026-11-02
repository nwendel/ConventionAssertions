using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public sealed class MethodScanner : IMethodScanner
{
    private IEnumerable<MethodInfo> _methodInfos;

    public MethodScanner(IEnumerable<Type> types)
    {
        GuardAgainst.Null(types);

        // TODO: Non public
        _methodInfos = types
            .SelectMany(x => x.GetMethods())
            .Where(x => x.DeclaringType != typeof(object))
            .Where(x => !x.IsSpecialName)
            .ToList();
    }

    internal MethodScanner(IConventionTargets<MethodInfo> targets)
    {
        GuardAgainst.Null(targets);

        _methodInfos = targets.Targets.ToList();
    }

    internal IEnumerable<MethodInfo> Methods => _methodInfos;

    public IMethodScanner Where(Func<IMethodFilter, bool> predicate)
    {
        GuardAgainst.Null(predicate);

        _methodInfos = _methodInfos
            .Select(x => new MethodFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Method)
            .ToList();

        return this;
    }
}
