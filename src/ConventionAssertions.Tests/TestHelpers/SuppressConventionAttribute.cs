using ConventionAssertions.Infrastructure;

namespace ConventionAssertions.Tests.Internal;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public sealed class SuppressConventionAttribute : Attribute
{
    public SuppressConventionAttribute(string checkId)
    {
        GuardAgainst.NullOrWhiteSpace(checkId);

        CheckId = checkId;
    }

    public string CheckId { get; }

    public string? Justification { get; set; }
}
