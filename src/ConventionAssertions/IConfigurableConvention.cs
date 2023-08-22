namespace ConventionAssertions;

public interface IConfigurableConvention<TTarget>
    where TTarget : MemberInfo
{
    void Assert(TTarget target, ConventionContext context);
}
