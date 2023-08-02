using ConventionAssertions.Reflection;

namespace ConventionAssertions.Rules;

public class HasPublicStaticParseMethod : IConvention<Type>
{
    private const string _methodName = nameof(int.Parse);

    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        var method = target.GetMethod(_methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, new[] { typeof(string) });
        if (method == null || method.ReturnType != target)
        {
            context.Fail(target, $"must have a public static method with signature: {target.DisplayName()} {_methodName}({typeof(string).DisplayName()})");
        }
    }
}
