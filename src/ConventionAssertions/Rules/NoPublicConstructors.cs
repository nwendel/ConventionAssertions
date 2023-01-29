namespace ConventionAssertions.Rules;

public class NoPublicConstructors : ITypeConvention
{
    public void Assert(Type type, ConventionContext context)
    {
        GuardAgainst.Null(type);
        GuardAgainst.Null(context);

        if (type.GetConstructors().Any())
        {
            context.Fail(type, "must not have a public constructor");
        }
    }
}
