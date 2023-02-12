using System.Reflection;

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

        var context = new ConventionContext(convention.Id);
        foreach (var type in _typeSource.Types)
        {
            var suppressions = type.GetCustomAttributes<SuppressConventionAttribute>();
            if (suppressions.Any(x => x.Id == convention.Id))
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

    public void Assert(string conventionId, Action<Type, ConventionContext> assert)
    {
        var convention = new TypeConventionAction(conventionId, assert);
        Assert(convention);
    }
}
