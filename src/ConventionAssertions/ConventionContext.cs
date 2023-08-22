using ConventionAssertions.Internal;
using ConventionAssertions.Reflection;

namespace ConventionAssertions;

// TODO: Change to not require 3 overloads?
public class ConventionContext
{
    private readonly List<string> _messages = new();

    public IEnumerable<string> Messages => _messages.AsReadOnly();

    [DoesNotReturn]
    public void Fail(Type type, string message)
    {
        GuardAgainst.Null(type);
        GuardAgainst.NullOrWhiteSpace(message);

        _messages.Add($"Type {type.DisplayName()} {message}.");
        throw new ConventionFailedException();
    }

    [DoesNotReturn]
    public void Fail(MethodInfo methodInfo, string message)
    {
        GuardAgainst.Null(methodInfo);
        GuardAgainst.NullOrWhiteSpace(message);

        // TODO: Bang here should be removed?
        _messages.Add($"Method {methodInfo.DeclaringType!.DisplayName()}.{methodInfo.DisplayName()} {message}.");
        throw new ConventionFailedException();
    }

    [DoesNotReturn]
    public void Fail(PropertyInfo propertyInfo, string message)
    {
        GuardAgainst.Null(propertyInfo);
        GuardAgainst.NullOrWhiteSpace(message);

        // TODO: Bang here should be removed?
        _messages.Add($"Property {propertyInfo.DeclaringType!.DisplayName()}.{propertyInfo.Name} {message}.");
        throw new ConventionFailedException();
    }
}
