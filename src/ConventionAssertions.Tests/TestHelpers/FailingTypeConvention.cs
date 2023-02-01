using ConventionAssertions.Infrastructure;

namespace ConventionAssertions.Tests.TestHelpers;

public class FailingTypeConvention : ITypeConvention
{
    public void Assert(Type type, ConventionContext context)
    {
        GuardAgainst.Null(context);

        context.Fail(type, "always fails");
    }
}
