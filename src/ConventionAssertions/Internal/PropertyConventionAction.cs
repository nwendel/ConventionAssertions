using System.Reflection;

namespace ConventionAssertions.Internal;

public class PropertyConventionAction : IPropertyConvention
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
