namespace ConventionAssertions.Conventions;

public class AsyncMethodHasAsyncSuffix : IConvention<MethodInfo>
{
    private const string _suffix = "Async";

    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (!IsAwaitable(target.ReturnType))
        {
            return;
        }

        if (!target.Name.EndsWith(_suffix, StringComparison.Ordinal))
        {
            context.Fail($"returns an awaitable type and must have name ending with {_suffix}");
        }
    }

    private static bool IsAwaitable(Type type)
    {
        if (typeof(Task).IsAssignableFrom(type) || type == typeof(ValueTask))
        {
            return true;
        }

        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueTask<>);
    }
}
