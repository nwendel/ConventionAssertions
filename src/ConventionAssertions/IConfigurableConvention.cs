namespace ConventionAssertions;

public interface IConfigurableConvention<TTarget>
    where TTarget : notnull
{
    void Assert(TTarget target, ConventionContext context);
}
