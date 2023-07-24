using System.Reflection;

namespace ConventionAssertions.Internal.Filters;

public interface IMethodFilter : IMemberFilter
{
    MethodInfo Method { get; }

    bool IsPublic { get; }
}
