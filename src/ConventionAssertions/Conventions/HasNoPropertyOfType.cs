using ConventionAssertions.Reflection;

namespace ConventionAssertions.Conventions;

public class HasNoPropertyOfType : IConfigurableConvention<Type>
{
    public Type? PropertyType { get; set; }

    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (PropertyType == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(PropertyType)} not specified");
        }

        var properties = target.GetProperties()
            .Where(x => PropertyType.IsAssignableFrom(x.PropertyType));
        foreach (var property in properties)
        {
            context.Fail($"must not have property {property.Name} of type {PropertyType.DisplayName()}");
        }
    }
}
