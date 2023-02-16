namespace ConventionAssertions.Internal;

public class TypeConventionAction : ITypeConvention
{
    private readonly Action<Type, ConventionContext> _action;

    public TypeConventionAction(string checkId, Action<Type, ConventionContext> action)
    {
        GuardAgainst.NullOrWhiteSpace(checkId);
        GuardAgainst.Null(action);

        CheckId = checkId;
        _action = action;
    }

    public string CheckId { get; }

    public void Assert(Type type, ConventionContext context)
    {
        _action(type, context);
    }
}
