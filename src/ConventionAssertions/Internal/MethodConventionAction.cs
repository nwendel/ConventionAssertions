using System.Reflection;

namespace ConventionAssertions.Internal;

public class MethodConventionAction : IMethodConvention
{
    private readonly Action<MethodInfo, ConventionContext> _action;

    public MethodConventionAction(Action<MethodInfo, ConventionContext> action)
    {
        GuardAgainst.Null(action);

        _action = action;
    }

    public void Assert(MethodInfo method, ConventionContext context)
    {
        _action(method, context);
    }
}
