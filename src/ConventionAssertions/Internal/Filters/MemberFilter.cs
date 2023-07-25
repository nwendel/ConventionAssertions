using System.Reflection;

namespace ConventionAssertions.Internal.Filters;

public abstract class MemberFilter : IMemberFilter
{
    protected abstract MemberInfo Member { get; }

    public bool HasCustomAttribute<T>()
        where T : Attribute
        => Member.GetCustomAttribute<T>() != null;

    public bool HasCustomAttribute(Func<Attribute, bool> predicate) =>
        Member.GetCustomAttributes(true)
            .OfType<Attribute>()
            .Any(a => predicate(a));
}
