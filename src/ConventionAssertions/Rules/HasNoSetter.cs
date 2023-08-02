using System.Reflection;

namespace ConventionAssertions.Rules;

public class HasNoSetter : IConvention<PropertyInfo>
{
    public void Assert(PropertyInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        var setMethod = target.GetSetMethod();
        if (setMethod != null)
        {
            context.Fail(target, "must not have a setter");
        }
    }
}
