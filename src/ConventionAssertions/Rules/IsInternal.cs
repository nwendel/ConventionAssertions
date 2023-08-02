namespace ConventionAssertions.Rules;

public class IsInternal : IConvention<Type>
{
    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        var isInternal = target.IsNotPublic || target.IsNestedAssembly;
        if (!isInternal)
        {
            context.Fail(target, "must be internal");
        }
    }
}
