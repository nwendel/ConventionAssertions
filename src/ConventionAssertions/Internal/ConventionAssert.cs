using System.Reflection;
using ConventionAssertions.Reflection;

namespace ConventionAssertions.Internal;

public class ConventionAssert<TTarget> : IConventionAssert<TTarget>
    where TTarget : MemberInfo
{
    private readonly IConventionTargets<TTarget> _typeSource;

    public ConventionAssert(IConventionTargets<TTarget> typeSource)
    {
        GuardAgainst.Null(typeSource);

        _typeSource = typeSource;
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

        foreach (var target in _typeSource.Targets)
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

    public void Assert(Action<TTarget, ConventionContext> assert)
    {
        var convention = new ConventionAction(assert);
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
