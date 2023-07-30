using ConventionAssertions.Reflection;

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
        var suppressions = AssertHelper.FindSuppressions();

        foreach (var type in _typeSource.Types)
        {
            if (suppressions.Contains(type.DisplayName()))
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

    public void Assert(Action<Type, ConventionContext> assert)
    {
        var convention = new TypeConventionAction(assert);
        Assert(convention);
    }

    private sealed class TypeConventionAction : ITypeConvention
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
}
