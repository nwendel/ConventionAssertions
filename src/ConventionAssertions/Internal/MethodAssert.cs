using System.Diagnostics;
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

    public void Assert(IMethodConvention convention)
    {
        GuardAgainst.Null(convention);

        var suppressions = FindSuppressions();

        var context = new ConventionContext();
        foreach (var method in _methodSource.Methods)
        {
            if (suppressions.Contains((method.DeclaringType!, method.Name)))
            {
                continue;
            }

            try
            {
                convention.Assert(method, context);
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

    // TODO: This method is very similar to the one in TypeAssert, refactor?
    private static HashSet<(Type Type, string? MethodName)> FindSuppressions()
    {
        // TODO: This assumes first method found here has the suppression attributes,
        //       not sure what to do long term
        var frame = new StackTrace().GetFrames()
            .SkipWhile(x => x.GetMethod()?.DeclaringType?.Assembly == typeof(Convention).Assembly)
            .First();
        var method = frame.GetMethod()!;
        var attributes = method.GetCustomAttributes<SuppressConventionAttribute>();

        var types = attributes
            .Select(x => (x.TargetType, x.MethodName))
            .ToHashSet();
        return types;
    }
}
