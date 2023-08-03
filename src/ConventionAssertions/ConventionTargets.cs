using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class ConventionTargets
{
    public static IConventionTargets<Type> FromTypes(Action<ITypeScanner> scannerAction)
    {
        GuardAgainst.Null(scannerAction);

        var scanner = new TypeScanner();
        scannerAction(scanner);
        return scanner;
    }
}
