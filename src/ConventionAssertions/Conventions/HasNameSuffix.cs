namespace ConventionAssertions.Conventions;

public class HasNameSuffix :
    IConfigurableConvention<Type>,
    IConfigurableConvention<MethodInfo>,
    IConfigurableConvention<PropertyInfo>
{
    public string? Suffix { get; set; }

    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        // Generic type names end with an arity marker, for example CommandHandler`1
        AssertCore(target.Name.TakeUntil('`'), context);
    }

    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        AssertCore(target.Name, context);
    }

    public void Assert(PropertyInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        AssertCore(target.Name, context);
    }

    private void AssertCore(string name, ConventionContext context)
    {
        if (string.IsNullOrEmpty(Suffix))
        {
            throw new ConventionNotConfiguredException($"{nameof(Suffix)} not specified");
        }

        if (!name.EndsWith(Suffix, StringComparison.Ordinal))
        {
            context.Fail($"name must end with {Suffix}");
        }
    }
}
