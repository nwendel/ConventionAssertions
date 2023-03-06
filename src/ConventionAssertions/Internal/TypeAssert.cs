using System.Diagnostics;
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

        var suppressions = FindSuppressions();

        var context = new ConventionContext();
        foreach (var type in _typeSource.Types)
        {
            if (suppressions.Contains(type))
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

    private static HashSet<Type> FindSuppressions()
    {
        // TODO: This assumes first method found here has the suppression attributes,
        //       not sure what to do long term
        var frame = new StackTrace().GetFrames()
            .SkipWhile(x => x.GetMethod()?.DeclaringType?.Assembly == typeof(Convention).Assembly)
            .First();
        var method = frame.GetMethod()!;
        var attributes = method.GetCustomAttributes<SuppressConventionAttribute>();

        var types = attributes
            .Select(x => x.TargetType)
            .ToHashSet();
        return types;
    }
}
