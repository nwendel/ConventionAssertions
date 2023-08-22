namespace ConventionAssertions;

public interface IConvention<TTarget> : IConfigurableConvention<TTarget>
    where TTarget : MemberInfo
{
}
