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

    internal string? Because { get; private init; }

    public void Fail(string message)
    {
        GuardAgainst.NullOrWhiteSpace(message);

        var because = Because == null ? string.Empty : $", {Because}";
        _messages.Add($"{TargetDisplayName.KindName(_target)} {TargetDisplayName.DisplayName(_target)} {message}{because}.");
    }

    internal static ConventionContext Create(object target, string? because = null)
    {
        GuardAgainst.Null(target);

        var context = target switch
        {
            MemberInfo memberInfo => new ConventionContext(memberInfo) { Because = because },
            Assembly assembly => new ConventionContext(assembly) { Because = because },
            _ => throw new NotSupportedException($"Convention target {target.GetType().FullName} is not supported"),
        };
        return context;
    }
}
