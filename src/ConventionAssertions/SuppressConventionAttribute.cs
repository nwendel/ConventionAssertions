namespace ConventionAssertions;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class SuppressConventionAttribute : Attribute
{
    required public Type TargetType { get; set; }

    public string? MethodName { get; set; }

    required public string Justification { get; set; }
}
