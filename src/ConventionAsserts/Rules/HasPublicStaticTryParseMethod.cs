namespace ConventionAsserts.Rules;

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
            context.Fail(type, $"must have a public static method with signature: {nameof(Boolean)} {_methodName}({nameof(String)}, out {type.Name})");
        }
    }
}
