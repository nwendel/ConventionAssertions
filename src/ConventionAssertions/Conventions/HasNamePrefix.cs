namespace ConventionAssertions.Conventions;

public class HasNamePrefix :
    IConfigurableConvention<Type>,
    IConfigurableConvention<MethodInfo>,
    IConfigurableConvention<PropertyInfo>
{
    public string? Prefix { get; set; }

    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        // Generic type names end with an arity marker, for example IHandler`1
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
        if (string.IsNullOrEmpty(Prefix))
        {
            throw new ConventionNotConfiguredException($"{nameof(Prefix)} not specified");
        }

        if (!name.StartsWith(Prefix, StringComparison.Ordinal))
        {
            context.Fail($"name must start with {Prefix}");
        }
    }
}
