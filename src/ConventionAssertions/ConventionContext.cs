using System.Reflection;
using ConventionAssertions.Internal;
using ConventionAssertions.Reflection;

namespace ConventionAssertions;

public class ConventionContext
{
    private readonly List<string> _messages = new();

    public IEnumerable<string> Messages => _messages.AsReadOnly();

    [DoesNotReturn]
    public void Fail(Type type, string message)
    {
        GuardAgainst.Null(type);
        GuardAgainst.NullOrWhiteSpace(message);

        _messages.Add($"Type {type.DisplayName()} {message}");
        throw new ConventionFailedException();
    }

    [DoesNotReturn]
    public void Fail(MethodInfo methodInfo, string message)
    {
        GuardAgainst.Null(methodInfo);
        GuardAgainst.NullOrWhiteSpace(message);

        _messages.Add($"Type {methodInfo.DeclaringType!.DisplayName()}.{methodInfo.Name} {message}");
        throw new ConventionFailedException();
    }
}
