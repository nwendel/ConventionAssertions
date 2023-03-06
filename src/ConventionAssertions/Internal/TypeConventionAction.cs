namespace ConventionAssertions.Internal;

public class TypeConventionAction : ITypeConvention
{
    private readonly Action<Type, ConventionContext> _action;

    public TypeConventionAction(Action<Type, ConventionContext> action)
    {
        GuardAgainst.Null(action);

        _action = action;
    }

    public void Assert(Type type, ConventionContext context)
    {
        _action(type, context);
    }
}
