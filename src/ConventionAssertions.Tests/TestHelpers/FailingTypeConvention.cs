using ConventionAssertions.Infrastructure;

namespace ConventionAssertions.Tests.TestHelpers;

public class FailingTypeConvention : IConvention<Type>
{
    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(context);

        context.Fail(target, "always fails");
    }
}
