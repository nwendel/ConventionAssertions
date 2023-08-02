namespace ConventionAssertions.Conventions;

public class HasNoPublicConstructors : IConvention<Type>
{
    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (target.GetConstructors().Any())
        {
            context.Fail(target, "must not have a public constructor");
        }
    }
}
