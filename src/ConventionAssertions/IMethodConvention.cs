using System.Reflection;

namespace ConventionAssertions;

public interface IMethodConvention
{
    string CheckId => GetType().Name;

    void Assert(MethodInfo method, ConventionContext context);
}
