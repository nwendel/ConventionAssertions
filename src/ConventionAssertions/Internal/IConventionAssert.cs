namespace ConventionAssertions.Internal;

public interface IConventionAssert<TTarget> : IFluentInterface
    where TTarget : MemberInfo
{
    void Assert<TConvention>(bool failOnNoTargets = true)
        where TConvention : IConvention<TTarget>, new();

    void Assert<TConvention>(Action<TConvention> configure, bool failOnNoTargets = true)
        where TConvention : IConfigurableConvention<TTarget>, new();

    void Assert(Action<TTarget, ConventionContext> assertAction, bool failOnNoTargets = true);
}
