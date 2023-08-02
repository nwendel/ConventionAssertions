namespace ConventionAssertions.Rules;

public class IsAbstractOrSealed : IConvention<Type>
{
    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (target.IsAbstract || target.IsSealed)
        {
            return;
        }

        context.Fail(target, $"must be abstract or sealed");
    }
}
