namespace ConventionAssertions.Rules;

public class IsInternal : ITypeConvention
{
    public void Assert(Type type, ConventionContext context)
    {
        GuardAgainst.Null(type);
        GuardAgainst.Null(context);

        var isInternal = type.IsNotPublic || type.IsNestedAssembly;
        if (!isInternal)
        {
            context.Fail(type, "must be internal");
        }
    }
}
