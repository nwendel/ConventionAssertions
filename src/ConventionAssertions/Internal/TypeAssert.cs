namespace ConventionAssertions.Internal;

public class TypeAssert : ITypeAssert
{
    private readonly ConventionTypeSource _typeSource;

    public TypeAssert(ConventionTypeSource typeSource)
    {
        GuardAgainst.Null(typeSource);

        _typeSource = typeSource;
    }

    public void Assert<T>()
        where T : ITypeConvention, new()
    {
        var convention = new T();
        Assert(convention);
    }

    public void Assert(ITypeConvention convention)
    {
        GuardAgainst.Null(convention);

        var context = new ConventionContext();
        foreach (var type in _typeSource.Types)
        {
            if (HasSuppression(type, convention.CheckId))
            {
                continue;
            }

            try
            {
                convention.Assert(type, context);
            }
            catch (ConventionFailedException)
            {
                // Ignore this exception
            }
        }

        var messages = context.Messages;
        if (messages.Any())
        {
            var message = string.Join(Environment.NewLine, messages);

            TestFramework.Throw(message);
        }
    }

    public void Assert(string checkId, Action<Type, ConventionContext> assert)
    {
        var convention = new TypeConventionAction(checkId, assert);
        Assert(convention);
    }

    private static bool HasSuppression(Type type, string checkId)
    {
        var attributes = type.GetCustomAttributes(false);
        var filtered = attributes
            .Select(x => (Attribute: x, TypeName: x.GetType().Name, PropertyInfo: x.GetType().GetProperty(nameof(ITypeConvention.CheckId))))
            .Where(x => x.TypeName == "SuppressConventionAttribute" && x.PropertyInfo != null)
            .ToList();
        var hasSuppression = filtered
            .Any(x => (string?)x.PropertyInfo!.GetValue(x.Attribute) == checkId);
        return hasSuppression;
    }
}
