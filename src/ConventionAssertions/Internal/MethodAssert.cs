using System.Reflection;

namespace ConventionAssertions.Internal;

public class MethodAssert : IMethodAssert
{
    private readonly ConventionMethodSource _methodSource;

    public MethodAssert(ConventionMethodSource methodSource)
    {
        GuardAgainst.Null(methodSource);

        _methodSource = methodSource;
    }

    public void Assert<T>()
        where T : IMethodConvention, new()
    {
        var convention = new T();
        Assert(convention);
    }

    // TODO: This is also mostly duplicated...
    public void Assert(IMethodConvention convention)
    {
        GuardAgainst.Null(convention);

        var context = new ConventionContext();
        foreach (var type in _methodSource.Methods)
        {
            if (HasSuppression(type, convention.CheckId))
            {
                continue;
            }

            try
            {
                convention.Assert(type, context);
            }
            catch (ConventionFailedException)
            {
                // Ignore this exception
            }
        }

        var messages = context.Messages;
        if (messages.Any())
        {
            var message = string.Join(Environment.NewLine, messages);

            TestFramework.Throw(message);
        }
    }

    // TODO: This almost duplicated...
    private static bool HasSuppression(MethodInfo methodInfo, string checkId)
    {
        var attributes = methodInfo.GetCustomAttributes(false);
        var filtered = attributes
            .Select(x => (Attribute: x, TypeName: x.GetType().Name, PropertyInfo: x.GetType().GetProperty(nameof(ITypeConvention.CheckId))))
            .Where(x => x.TypeName == "SuppressConventionAttribute" && x.PropertyInfo != null)
            .ToList();
        var hasSuppression = filtered
            .Any(x => (string?)x.PropertyInfo!.GetValue(x.Attribute) == checkId);
        return hasSuppression;
    }
}
