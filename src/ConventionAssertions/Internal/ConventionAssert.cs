using ConventionAssertions.Reflection;

namespace ConventionAssertions.Internal;

public class ConventionAssert<TTarget> : IConventionAssert<TTarget>
    where TTarget : notnull
{
    private readonly IConventionTargets<TTarget> _targets;

    public ConventionAssert(IConventionTargets<TTarget> targets)
    {
        GuardAgainst.Null(targets);

        _targets = targets;
    }

    public void Assert<TConvention>(bool failOnNoTargets = true, string? because = null)
        where TConvention : IConvention<TTarget>, new()
    {
        var convention = new TConvention();
        Assert(convention, failOnNoTargets, because);
    }

    public void Assert<TConvention>(Action<TConvention> configure, bool failOnNoTargets = true, string? because = null)
        where TConvention : IConfigurableConvention<TTarget>, new()
    {
        GuardAgainst.Null(configure);

        var convention = new TConvention();
        configure(convention);

        Assert(convention, failOnNoTargets, because);
    }

    public void Assert(Action<TTarget, ConventionContext> assertAction, bool failOnNoTargets = true, string? because = null)
    {
        var convention = new ConventionAction(assertAction);
        Assert(convention, failOnNoTargets, because);
    }

    private void Assert(IConfigurableConvention<TTarget> convention, bool failOnNoTargets, string? because)
    {
        GuardAgainst.Null(convention);
        if (because != null)
        {
            GuardAgainst.NullOrWhiteSpace(because);
        }

        var suppressions = AssertHelper.FindSuppressions();

        if (!_targets.Targets.Any() && failOnNoTargets)
        {
            TestFramework.Throw($"No targets found for convention.");
        }

        var messages = new List<string>();
        foreach (var target in _targets.Targets)
        {
            if (suppressions.Contains(TargetDisplayName.DisplayName(target)))
            {
                continue;
            }

            var context = ConventionContext.Create(target, because);
            convention.Assert(target, context);
            messages.AddRange(context.Messages);
        }

        if (messages.Any())
        {
            var messagesText = string.Join(Environment.NewLine, messages);

            TestFramework.Throw($"Found {messages.Count}.{Environment.NewLine}{messagesText}");
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
