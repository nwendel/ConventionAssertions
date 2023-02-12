namespace ConventionAssertions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public sealed class SuppressConventionAttribute : Attribute
{
    public SuppressConventionAttribute(string id)
    {
        GuardAgainst.NullOrWhiteSpace(id);

        Id = id;
    }

    public string Id { get; }

    public string? Justification { get; set; }
}
