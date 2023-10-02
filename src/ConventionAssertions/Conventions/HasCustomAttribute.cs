namespace ConventionAssertions.Conventions;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Follows convention naming standard")]
public class HasCustomAttribute :
    IConfigurableConvention<Type>,
    IConfigurableConvention<MethodInfo>,
    IConfigurableConvention<PropertyInfo>
{
    public Type? CustomAttribute { get; set; }

    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (!HasCustomAttributeCore(target))
        {
            context.Fail(target, $"must have custom attribute {CustomAttribute.FullName}");
        }
    }

    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (!HasCustomAttributeCore(target))
        {
            context.Fail(target, $"must have custom attribute {CustomAttribute.FullName}");
        }
    }

    public void Assert(PropertyInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (!HasCustomAttributeCore(target))
        {
            context.Fail(target, $"must have custom attribute {CustomAttribute.FullName}");
        }
    }

    [MemberNotNull(nameof(CustomAttribute))]
    private bool HasCustomAttributeCore(MemberInfo target)
    {
        if (CustomAttribute == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(CustomAttribute)} not specified");
        }

        var customAttributes = target.GetCustomAttributes();
        var result = customAttributes.Any(x => x.GetType() == CustomAttribute);
        return result;
    }
}
