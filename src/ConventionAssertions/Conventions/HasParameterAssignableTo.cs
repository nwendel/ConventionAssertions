using ConventionAssertions.Reflection;

namespace ConventionAssertions.Conventions;

public class HasParameterAssignableTo : IConfigurableConvention<MethodInfo>
{
    public Type? ParameterType { get; set; }

    public int? Count { get; set; }

    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (ParameterType == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(ParameterType)} not specified");
        }

        if (Count <= 0)
        {
            throw new ConventionNotConfiguredException(
                $"{nameof(Count)} must be at least 1, use {nameof(HasNoParameterAssignableTo)} for no parameters");
        }

        var actual = target.GetParameters()
            .Count(x => ParameterType.IsAssignableFrom(UnwrapByRef(x.ParameterType)));

        if (Count == null)
        {
            if (actual == 0)
            {
                context.Fail($"must have a parameter assignable to {ParameterType.DisplayName()}");
            }

            return;
        }

        if (actual != Count)
        {
            var noun = Count == 1 ? "parameter" : "parameters";
            context.Fail($"must have exactly {Count} {noun} assignable to {ParameterType.DisplayName()}, but has {actual}");
        }
    }

    // ref, out and in parameters have a by-ref type, for example Actor&
    private static Type UnwrapByRef(Type type) =>
        type.IsByRef
            ? type.GetElementType()!
            : type;
}
