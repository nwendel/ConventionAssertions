using System.Reflection;

namespace ConventionAssertions;

public interface IConventionTargets<T>
    where T : MemberInfo
{
    IEnumerable<T> Targets { get; }
}
