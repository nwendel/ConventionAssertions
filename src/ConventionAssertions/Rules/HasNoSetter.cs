using System.Reflection;

namespace ConventionAssertions.Rules;

public class HasNoSetter : IPropertyConvention
{
    public void Assert(PropertyInfo property, ConventionContext context)
    {
        GuardAgainst.Null(property);
        GuardAgainst.Null(context);

        var setMethod = property.GetSetMethod();
        if (setMethod != null)
        {
            context.Fail(property, "must not have a setter");
        }
    }
}
