namespace ConventionAssertions.Internal;

public class TypeConventionAction : ITypeConvention
{
    private readonly Action<Type, ConventionContext> _action;

    public TypeConventionAction(string conventionId, Action<Type, ConventionContext> action)
    {
        GuardAgainst.NullOrWhiteSpace(conventionId);
        GuardAgainst.Null(action);

        Id = conventionId;
        _action = action;
    }

    public string Id { get; }

    public void Assert(Type type, ConventionContext context)
    {
        _action(type, context);
    }
}
