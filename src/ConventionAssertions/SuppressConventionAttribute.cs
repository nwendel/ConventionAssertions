namespace ConventionAssertions;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class SuppressConventionAttribute : Attribute
{
    public required string Target { get; init; }

    public required string Justification { get; init; }
}
