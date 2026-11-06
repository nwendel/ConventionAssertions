using ConventionAssertions.Reflection;

namespace ConventionAssertions.Conventions;

public class HasNoParameterAssignableTo : IConfigurableConvention<MethodInfo>
{
    public Type? ParameterType { get; set; }

    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (ParameterType == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(ParameterType)} not specified");
        }

        var parameters = target.GetParameters()
            .Where(x => ParameterType.IsAssignableFrom(UnwrapByRef(x.ParameterType)));
        foreach (var parameter in parameters)
        {
            context.Fail($"must not have parameter {parameter.Name} assignable to {ParameterType.DisplayName()}");
        }
    }

    // ref, out and in parameters have a by-ref type, for example Actor&
    private static Type UnwrapByRef(Type type) =>
        type.IsByRef
            ? type.GetElementType()!
            : type;
}
