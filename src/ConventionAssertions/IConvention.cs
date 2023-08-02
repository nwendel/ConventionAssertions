using System.Reflection;

namespace ConventionAssertions;

public interface IConvention<T>
    where T : MemberInfo
{
    void Assert(T target, ConventionContext context);
}
