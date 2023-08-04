namespace ConventionAssertions.Internal.Filters;

public class PropertyFilter : MemberFilter, IPropertyFilter
{
    public PropertyFilter(PropertyInfo property)
    {
        GuardAgainst.Null(property);

        Property = property;
    }

    public PropertyInfo Property { get; }

    protected override MemberInfo Member => Property;
}
