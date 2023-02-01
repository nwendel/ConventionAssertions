namespace ConventionAssertions.Rules;

public class IsNotNestedInternal : ITypeConvention
{
    public void Assert(Type type, ConventionContext context)
    {
        GuardAgainst.Null(type);
        GuardAgainst.Null(context);

        var isInternal = type.IsNotPublic && !type.IsNested;
        if (!isInternal)
        {
            context.Fail(type, "must be internal");
        }
    }
}
