namespace ConventionAssertions.Internal;

public class TypeAssert : ITypeAssert
{
    private readonly ConventionTypeSource _typeSource;

    public TypeAssert(ConventionTypeSource typeSource)
    {
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

    public void Assert(Action<Type, ConventionContext> assert)
    {
        var convention = new TypeConventionAction(assert);
        Assert(convention);
    }
}
