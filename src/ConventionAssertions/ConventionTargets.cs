using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class ConventionTargets
{
    public static IConventionTargets<Type> FromTypes(Action<ITypeScanner> typeScanner)
    {
        GuardAgainst.Null(typeScanner);

        var targets = new TypeScanner();
        typeScanner(targets);
        return targets;
    }
}
