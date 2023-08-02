using System.Reflection;

namespace ConventionAssertions.Internal;

public interface IConventionAssert<TTarget> : IFluentInterface
    where TTarget : MemberInfo
{
    void Assert<TConvention>()
        where TConvention : IConvention<TTarget>, new();

    void Assert(IConvention<TTarget> convention);

    void Assert(Action<TTarget, ConventionContext> assert);
}
