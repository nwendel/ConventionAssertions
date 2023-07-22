using System.Reflection;

namespace ConventionAssertions.Internal;

public interface IMethodFilter : IFluentInterface
{
    MethodInfo Method { get; }

    bool IsPublic { get; }

    bool IsSpecialName { get; }

    bool HasCustomAttribute<T>()
        where T : Attribute;

    bool HasCustomAttribute(Func<Attribute, bool> predicate);
}
