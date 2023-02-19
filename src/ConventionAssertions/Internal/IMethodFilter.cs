using System.Reflection;

namespace ConventionAssertions.Internal;

public interface IMethodFilter : IFluentInterface
{
    MethodInfo Method { get; }

    bool HasAttribute<T>()
        where T : Attribute;
}
