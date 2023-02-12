using ConventionAssertions.Internal;

namespace ConventionAssertions;

public class ConventionContext
{
    private readonly List<string> _messages = new();

    public ConventionContext(string conventionId)
    {
        GuardAgainst.NullOrWhiteSpace(conventionId);

        ConventionId = conventionId;
    }

    public IEnumerable<string> Messages => _messages.AsReadOnly();

    public string ConventionId { get; }

    [DoesNotReturn]
    public void Fail(Type type, string message)
    {
        GuardAgainst.Null(type);
        GuardAgainst.NullOrWhiteSpace(message);

        _messages.Add($"Type {type.Name} {message}");
        throw new ConventionFailedException();
    }
}
