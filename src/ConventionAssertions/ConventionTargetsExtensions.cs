using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions;

public static class ConventionTargetsExtensions
{
    public static IConventionTargets<MethodInfo> Methods(
        this IConventionTargets<Type> self,
        Func<IMethodFilter, bool>? predicate = null)
    {
        GuardAgainst.Null(self);

        var methodInfos = self.Targets
            .SelectMany(x => x.GetMethods())
            .Where(x => x.DeclaringType != typeof(object))
            .Where(x => !x.IsSpecialName);

        if (predicate != null)
        {
            methodInfos = methodInfos
                .Select(x => new MethodFilter(x))
                .Where(x => predicate(x))
                .Select(x => x.Method);
        }

        return new ConventionTargetCollection<MethodInfo>(methodInfos);
    }

    public static IConventionTargets<PropertyInfo> Properties(
        this IConventionTargets<Type> self,
        Func<IPropertyFilter, bool>? predicate = null)
    {
        GuardAgainst.Null(self);

        var propertyInfos = self.Targets
            .SelectMany(x => x.GetProperties());

        if (predicate != null)
        {
            propertyInfos = propertyInfos
                .Select(x => new PropertyFilter(x))
                .Where(x => predicate(x))
                .Select(x => x.Property);
        }

        return new ConventionTargetCollection<PropertyInfo>(propertyInfos);
    }
}
