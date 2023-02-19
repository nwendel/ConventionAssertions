using System.Reflection;

namespace ConventionAssertions.Internal;

public class MethodFilter : IMethodFilter
{
    public MethodFilter(MethodInfo method)
    {
        GuardAgainst.Null(method);

        Method = method;
    }

    public MethodInfo Method { get; }

    public bool HasAttribute<T>()
        where T : Attribute
        => Method.GetCustomAttribute<T>() != null;
}
