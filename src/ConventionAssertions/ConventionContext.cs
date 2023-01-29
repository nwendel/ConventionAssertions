using ConventionAssertions.Internal;

namespace ConventionAssertions;

public class ConventionContext
{
    private readonly List<string> _messages = new();

    public IEnumerable<string> Messages => _messages;

    [DoesNotReturn]
    public void Fail(Type type, string message)
    {
        GuardAgainst.Null(type);
        GuardAgainst.NullOrWhiteSpace(message);

        _messages.Add($"Type {type.Name} {message}");
        throw new ConventionFailedException();
    }
}
