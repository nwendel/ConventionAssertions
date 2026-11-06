namespace ConventionAssertions.Internal;

public interface IConventionAssert<TTarget> : IFluentInterface
    where TTarget : notnull
{
    void Assert<TConvention>(bool failOnNoTargets = true, string? because = null)
        where TConvention : IConvention<TTarget>, new();

    void Assert<TConvention>(Action<TConvention> configure, bool failOnNoTargets = true, string? because = null)
        where TConvention : IConfigurableConvention<TTarget>, new();

    void Assert(Action<TTarget, ConventionContext> assertAction, bool failOnNoTargets = true, string? because = null);
}
