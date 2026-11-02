namespace ConventionAssertions.Conventions;

public class IsSealed : IConvention<Type>
{
    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (!target.IsSealed)
        {
            context.Fail("must be sealed");
        }
    }
}
