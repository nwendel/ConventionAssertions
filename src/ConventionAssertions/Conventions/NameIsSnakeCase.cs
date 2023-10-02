namespace ConventionAssertions.Conventions;

public class NameIsSnakeCase :
    IConvention<Type>,
    IConvention<MethodInfo>
{
    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (IsNotSnakeCase(target.Name))
        {
            context.Fail(target, "is not snake case");
        }
    }

    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (IsNotSnakeCase(target.Name))
        {
            context.Fail(target, "is not snake case");
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
