using System.Reflection;

namespace ConventionAssertions.Rules;

public class NameIsSnakeCase : ITypeConvention, IMethodConvention
{
    public void Assert(Type type, ConventionContext context)
    {
        GuardAgainst.Null(type);
        GuardAgainst.Null(context);

        if (IsNotSnakeCase(type.Name))
        {
            context.Fail(type, "is not snake case");
        }
    }

    public void Assert(MethodInfo method, ConventionContext context)
    {
        GuardAgainst.Null(method);
        GuardAgainst.Null(context);

        if (IsNotSnakeCase(method.Name))
        {
            context.Fail(method, "is not snake case");
        }
    }

    // TODO: Is this correct?
    private static bool IsNotSnakeCase(string name) =>
        name.Any(x => (!char.IsAsciiLetterOrDigit(x) && x != '_') ||
                      (char.IsLetter(x) && char.IsUpper(x))) ||
        name.Contains("__", StringComparison.Ordinal) ||
        name.First() == '_' ||
        name.Last() == '_';
}
