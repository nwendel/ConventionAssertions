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

    public void Assert<TConvention>(bool failOnNoTargets = true)
        where TConvention : IConvention<TTarget>, new()
    {
        var convention = new TConvention();
        Assert(convention, failOnNoTargets);
    }

    public void Assert<TConvention>(Action<TConvention> configure, bool failOnNoTargets = true)
        where TConvention : IConfigurableConvention<TTarget>, new()
    {
        GuardAgainst.Null(configure);

        var convention = new TConvention();
        configure(convention);

        Assert(convention, failOnNoTargets);
    }

    public void Assert(Action<TTarget, ConventionContext> assertAction, bool failOnNoTargets = true)
    {
        var convention = new ConventionAction(assertAction);
        Assert(convention, failOnNoTargets);
    }

    private void Assert(IConfigurableConvention<TTarget> convention, bool failOnNoTargets)
    {
        GuardAgainst.Null(convention);

        var context = new ConventionContext();
        var suppressions = AssertHelper.FindSuppressions();

        if (!_targets.Any() && failOnNoTargets)
        {
            TestFramework.Throw($"No targets found for convention.");
        }

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
            var messagesText = string.Join(Environment.NewLine, messages);

            TestFramework.Throw($"Found {context.Messages.Count()}.{Environment.NewLine}{messagesText}");
        }
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
