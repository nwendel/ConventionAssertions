using ConventionAssertions.Reflection;

namespace ConventionAssertions.Conventions;

// TODO: This rule should be merged with HasPublicStaticTryParseMethod?
//       Make the rule configurable for scenarios where we only want to check one?
public class HasPublicStaticTryParseMethod : IConvention<Type>
{
    private const string _methodName = nameof(int.TryParse);

    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        var method = target.GetMethod(_methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, new[] { typeof(string), target.MakeByRefType() });
        if (method == null || method.ReturnType != typeof(bool))
        {
            context.Fail(target, $"must have a public static method with signature: {typeof(bool).DisplayName()} {_methodName}({typeof(string).DisplayName()}, out {target.DisplayName()})");
        }
    }
}
