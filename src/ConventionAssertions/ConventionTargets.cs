using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class ConventionTargets
{
    public static IConventionTargets<Type> FromTypes(Action<ITypeScanner> scannerAction)
    {
        GuardAgainst.Null(scannerAction);

        var scanner = new TypeScanner();
        scannerAction(scanner);
        return new ConventionTargetCollection<Type>(((IConventionTargets<Type>)scanner).Targets);
    }

    [Obsolete("Use ConventionTargets.FromTypes(...).Methods(...) instead.")]
    public static IConventionTargets<MethodInfo> FromMethods(
        Action<ITypeScanner> typeScannerAction,
        Action<IMethodScanner> methodScannerAction)
    {
        GuardAgainst.Null(typeScannerAction);
        GuardAgainst.Null(methodScannerAction);

        var typeTargets = FromTypes(typeScannerAction);

        var scanner = new MethodScanner(typeTargets.Targets);
        methodScannerAction(scanner);
        return scanner;
    }
}
