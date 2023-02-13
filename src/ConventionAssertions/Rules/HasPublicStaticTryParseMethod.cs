using ConventionAssertions.Reflection;

namespace ConventionAssertions.Rules;

// TODO: This rule shold be merged with HasPublicStaticTryParseMethod?
//       Make the rule configurable for scenarios where we only want to check one?
public class HasPublicStaticTryParseMethod : ITypeConvention
{
    private const string _methodName = nameof(int.TryParse);

    public void Assert(Type type, ConventionContext context)
    {
        GuardAgainst.Null(type);
        GuardAgainst.Null(context);

        var method = type.GetMethod(_methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, new[] { typeof(string), type.MakeByRefType() });
        if (method == null || method.ReturnType != typeof(bool))
        {
            context.Fail(type, $"must have a public static method with signature: {typeof(bool).DisplayName()} {_methodName}({typeof(string).DisplayName()}, out {type.DisplayName()})");
        }
    }
}
