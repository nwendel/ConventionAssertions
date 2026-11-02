using System.Runtime.CompilerServices;

namespace ConventionAssertions.Conventions;

public class VoidMethodIsNotAsync : IConvention<MethodInfo>
{
    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (target.ReturnType != typeof(void))
        {
            return;
        }

        var isAsync = target.GetCustomAttribute<AsyncStateMachineAttribute>() != null;
        if (isAsync)
        {
            context.Fail("must not be async void");
        }
    }
}
