using System.Reflection;
using ConventionAssertions.Reflection;

namespace ConventionAssertions.Internal;

public class ConventionAssert<TTarget> : IConventionAssert<TTarget>
    where TTarget : MemberInfo
{
    private readonly IConventionTargets<TTarget> _targets;

    public ConventionAssert(IConventionTargets<TTarget> targets)
    {
        GuardAgainst.Null(targets);

        _targets = targets;
    }

    public void Assert<T>()
        where T : IConvention<TTarget>, new()
    {
        var convention = new T();
        Assert(convention);
    }

    public void Assert(IConvention<TTarget> convention)
    {
        GuardAgainst.Null(convention);

        var context = new ConventionContext();
        var suppressions = AssertHelper.FindSuppressions();

        foreach (var target in _targets)
        {
            if (suppressions.Contains(target.DisplayName()))
            {
                continue;
            }

            try
            {
                convention.Assert(target, context);
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

    public void Assert(Action<TTarget, ConventionContext> assertAction)
    {
        var convention = new ConventionAction(assertAction);
        Assert(convention);
    }

    private sealed class ConventionAction : IConvention<TTarget>
    {
        private readonly Action<TTarget, ConventionContext> _action;

        public ConventionAction(Action<TTarget, ConventionContext> action)
        {
            GuardAgainst.Null(action);

            _action = action;
        }

        public void Assert(TTarget target, ConventionContext context)
        {
            _action(target, context);
        }
    }
}
