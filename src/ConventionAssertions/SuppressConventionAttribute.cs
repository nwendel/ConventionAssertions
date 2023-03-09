namespace ConventionAssertions;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class SuppressConventionAttribute : Attribute
{
    required public string Target { get; init; }

    required public string Justification { get; init; }
}
