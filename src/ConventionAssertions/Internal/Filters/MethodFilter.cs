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

    public bool IsAwaitable
    {
        get
        {
            var returnType = Method.ReturnType;
            if (typeof(Task).IsAssignableFrom(returnType) || returnType == typeof(ValueTask))
            {
                return true;
            }

            return returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ValueTask<>);
        }
    }

    public bool IsVoid => Method.ReturnType == typeof(void);

    protected override MemberInfo Member => Method;
}
