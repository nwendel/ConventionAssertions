using System.Reflection;
using ConventionAssertions.Infrastructure;

namespace ConventionAssertions.Tests.TestHelpers;

public class FailingMethodConvention : IMethodConvention
{
    public void Assert(MethodInfo method, ConventionContext context)
    {
        GuardAgainst.Null(context);

        context.Fail(method, "always fails");
    }
}
