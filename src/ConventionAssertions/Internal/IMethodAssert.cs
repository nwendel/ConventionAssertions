using System.Reflection;

namespace ConventionAssertions.Internal;

public interface IMethodAssert : IFluentInterface
{
    void Assert<T>()
        where T : IMethodConvention, new();

    void Assert(IMethodConvention convention);

    void Assert(Action<MethodInfo, ConventionContext> assert);
}
