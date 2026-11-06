namespace ConventionAssertions.Internal.Filters;

public interface IMethodFilter : IMemberFilter
{
    MethodInfo Method { get; }

    bool IsPublic { get; }

    bool IsAwaitable { get; }

    bool IsVoid { get; }

    bool HasAnyNamePrefix(params string[] prefixes);
}
