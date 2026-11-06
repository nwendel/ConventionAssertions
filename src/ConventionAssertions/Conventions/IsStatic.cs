using ConventionAssertions.Reflection;

namespace ConventionAssertions.Conventions;

public class IsStatic : IConvention<Type>
{
    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (!target.IsStatic())
        {
            context.Fail("must be static");
        }
    }
}
