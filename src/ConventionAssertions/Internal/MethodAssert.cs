using System.Reflection;
using ConventionAssertions.Reflection;

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

        var context = new ConventionContext();
        var suppressions = AssertHelper.FindSuppressions();

        foreach (var method in _methodSource.Methods)
        {
            if (suppressions.Contains(method.DisplayName()))
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

    public void Assert(Action<MethodInfo, ConventionContext> assert)
    {
        var convention = new MethodConventionAction(assert);
        Assert(convention);
    }
}
