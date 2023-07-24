using System.Reflection;

namespace ConventionAssertions.Internal.Filters;

public class MethodFilter : IMethodFilter
{
    public MethodFilter(MethodInfo method)
    {
        GuardAgainst.Null(method);

        Method = method;
    }

    public MethodInfo Method { get; }

    public bool IsPublic => Method.IsPublic;

    public bool IsSpecialName => Method.IsSpecialName;

    public bool HasCustomAttribute<T>()
        where T : Attribute
        => Method.GetCustomAttribute<T>() != null;

    public bool HasCustomAttribute(Func<Attribute, bool> predicate) =>
        Method.GetCustomAttributes(true)
            .OfType<Attribute>()
            .Any(a => predicate(a));
}
