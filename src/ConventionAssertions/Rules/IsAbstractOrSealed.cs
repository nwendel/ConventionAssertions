namespace ConventionAssertions.Rules;

public class IsAbstractOrSealed : ITypeConvention
{
    public void Assert(Type type, ConventionContext context)
    {
        GuardAgainst.Null(type);
        GuardAgainst.Null(context);

        if (type.IsAbstract || type.IsSealed)
        {
            return;
        }

        context.Fail(type, $"must be abstract or sealed");
    }
}
