namespace ConventionAssertions.Internal;

public interface IConventionAssert<TTarget> : IFluentInterface
    where TTarget : MemberInfo
{
    void Assert<TConvention>()
        where TConvention : IConvention<TTarget>, new();

    void Assert<TConvention>(Action<TConvention> configure)
        where TConvention : IConfigurableConvention<TTarget>, new();

    void Assert(Action<TTarget, ConventionContext> assertAction);
}
