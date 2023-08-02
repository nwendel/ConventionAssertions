using System.Reflection;

namespace ConventionAssertions.Internal;

public class PropertyAssert : IPropertyAssert
{
    private readonly IConventionTargets<PropertyInfo> _propertySource;

    public PropertyAssert(IConventionTargets<PropertyInfo> propertySource)
    {
        GuardAgainst.Null(propertySource);

        _propertySource = propertySource;
    }

    public void Assert<T>()
        where T : IPropertyConvention, new()
    {
        var convention = new T();
        Assert(convention);
    }

    public void Assert(IPropertyConvention convention)
    {
        GuardAgainst.Null(convention);

        var context = new ConventionContext();
        var suppressions = AssertHelper.FindSuppressions();

        foreach (var property in _propertySource.Targets)
        {
            // TODO: Name? Or is a DisplayName extension needed?
            if (suppressions.Contains(property.Name))
            {
                continue;
            }

            try
            {
                convention.Assert(property, context);
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

    public void Assert(Action<PropertyInfo, ConventionContext> assert)
    {
        var convention = new PropertyConventionAction(assert);
        Assert(convention);
    }

    private sealed class PropertyConventionAction : IPropertyConvention
    {
        private readonly Action<PropertyInfo, ConventionContext> _action;

        public PropertyConventionAction(Action<PropertyInfo, ConventionContext> action)
        {
            GuardAgainst.Null(action);

            _action = action;
        }

        public void Assert(PropertyInfo property, ConventionContext context)
        {
            _action(property, context);
        }
    }
}
