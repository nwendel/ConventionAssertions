using ConventionAssertions.Reflection;

namespace ConventionAssertions;

public class ConventionContext
{
    private readonly MemberInfo _target;
    private readonly List<string> _messages = new();

    public ConventionContext(MemberInfo target)
    {
        GuardAgainst.Null(target);

        _target = target;
    }

    public IEnumerable<string> Messages => _messages.AsReadOnly();

    public void Fail(string message)
    {
        GuardAgainst.NullOrWhiteSpace(message);

        _messages.Add($"{_target.KindName()} {_target.DisplayName()} {message}.");
    }
}
