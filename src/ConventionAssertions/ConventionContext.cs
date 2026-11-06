using ConventionAssertions.Reflection;

namespace ConventionAssertions;

public class ConventionContext
{
    private readonly object _target;
    private readonly List<string> _messages = new();

    public ConventionContext(MemberInfo target)
    {
        GuardAgainst.Null(target);

        _target = target;
    }

    public ConventionContext(Assembly target)
    {
        GuardAgainst.Null(target);

        _target = target;
    }

    public IEnumerable<string> Messages => _messages.AsReadOnly();

    public void Fail(string message)
    {
        GuardAgainst.NullOrWhiteSpace(message);

        _messages.Add($"{TargetDisplayName.KindName(_target)} {TargetDisplayName.DisplayName(_target)} {message}.");
    }

    internal static ConventionContext Create(object target)
    {
        GuardAgainst.Null(target);

        return target switch
        {
            MemberInfo memberInfo => new ConventionContext(memberInfo),
            Assembly assembly => new ConventionContext(assembly),
            _ => throw new NotSupportedException($"Convention target {target.GetType().FullName} is not supported"),
        };
    }
}
