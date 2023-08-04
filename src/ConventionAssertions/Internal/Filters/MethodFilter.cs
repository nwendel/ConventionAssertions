namespace ConventionAssertions.Internal.Filters;

public class MethodFilter : MemberFilter, IMethodFilter
{
    public MethodFilter(MethodInfo method)
    {
        GuardAgainst.Null(method);

        Method = method;
    }

    public MethodInfo Method { get; }

    public bool IsPublic => Method.IsPublic;

    protected override MemberInfo Member => Method;
}
