using System.Reflection;

namespace ConventionAssertions.Tests.TestHelpers;

public class NothingMethodConvention : IConvention<MethodInfo>
{
    private readonly List<MethodInfo> _assertedMethods = new();

    public IEnumerable<MethodInfo> AssertedMethods => _assertedMethods;

    public void Assert(MethodInfo target, ConventionContext context)
    {
        _assertedMethods.Add(target);
    }
}
