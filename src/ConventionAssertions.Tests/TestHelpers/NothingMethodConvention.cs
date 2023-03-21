using System.Reflection;

namespace ConventionAssertions.Tests.TestHelpers;

public class NothingMethodConvention : IMethodConvention
{
    private readonly List<MethodInfo> _assertedMethods = new();

    public IEnumerable<MethodInfo> AssertedMethods => _assertedMethods;

    public void Assert(MethodInfo method, ConventionContext context)
    {
        _assertedMethods.Add(method);
    }
}
