using ConventionAssertions.Reflection;

namespace ConventionAssertions.Conventions;

public class IsAssignableTo : IConfigurableConvention<Type>
{
    public Type? Type { get; set; }

    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (Type == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(Type)} not specified");
        }

        if (!target.IsAssignableToOpenOrClosed(Type))
        {
            context.Fail($"must be assignable to {Type.DisplayName()}");
        }
    }
}
