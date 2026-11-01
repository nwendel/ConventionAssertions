namespace ConventionAssertions;

public interface IConventionTargets<T>
    where T : MemberInfo
{
    internal IEnumerable<T> Targets { get; }
}
