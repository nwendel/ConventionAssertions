using System.Reflection;
using ConventionAssertions.Infrastructure;

namespace ConventionAssertions.Tests.TestHelpers;

public class FailingMethodConvention : IConvention<MethodInfo>
{
    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(context);

        context.Fail(target, "always fails");
    }
}
