using System.Reflection;

namespace ConventionAssertions;

public interface IMethodConvention
{
    void Assert(MethodInfo method, ConventionContext context);
}
