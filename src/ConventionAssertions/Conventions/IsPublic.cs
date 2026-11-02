namespace ConventionAssertions.Conventions;

public class IsPublic : IConvention<Type>
{
    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        var isPublic = target.IsPublic || target.IsNestedPublic;
        if (!isPublic)
        {
            context.Fail("must be public");
        }
    }
}
