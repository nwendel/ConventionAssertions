using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class ConventionTargets
{
    public static IConventionTargets<Type> FromTypes(Func<ITypeScanner, ITypeScannerFilter> types)
    {
        GuardAgainst.Null(types);

        return new ConventionTargetCollection<Type>(ScanTypes(types));
    }

    public static IConventionTargets<MethodInfo> FromMethods(
        Func<ITypeScanner, ITypeScannerFilter> types,
        Func<IMethodScanner, IMethodScanner>? methods = null)
    {
        GuardAgainst.Null(types);

        IMethodScanner scanner = new MethodScanner(ScanTypes(types));
        if (methods != null)
        {
            scanner = methods(scanner);
        }

        return ToTargets(scanner);
    }

    public static IConventionTargets<MethodInfo> FromMethods(
        IConventionTargets<MethodInfo> targets,
        Func<IMethodScanner, IMethodScanner> methods)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(methods);

        return ToTargets(methods(new MethodScanner(targets)));
    }

    public static IConventionTargets<PropertyInfo> FromProperties(
        Func<ITypeScanner, ITypeScannerFilter> types,
        Func<IPropertyScanner, IPropertyScanner>? properties = null)
    {
        GuardAgainst.Null(types);

        IPropertyScanner scanner = new PropertyScanner(ScanTypes(types));
        if (properties != null)
        {
            scanner = properties(scanner);
        }

        return ToTargets(scanner);
    }

    public static IConventionTargets<PropertyInfo> FromProperties(
        IConventionTargets<PropertyInfo> targets,
        Func<IPropertyScanner, IPropertyScanner> properties)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(properties);

        return ToTargets(properties(new PropertyScanner(targets)));
    }

    [Obsolete("Use ConventionTargets.FromTypes(s => s.From...()) instead.")]
    public static IConventionTargets<Type> FromTypes(Action<ITypeScanner> scannerAction)
    {
        GuardAgainst.Null(scannerAction);

        var scanner = new TypeScanner();
        scannerAction(scanner);
        return new ConventionTargetCollection<Type>(scanner.Types);
    }

    [Obsolete("Use ConventionTargets.FromMethods(s => s.From...(), m => m.Where(...)) instead.")]
    public static IConventionTargets<MethodInfo> FromMethods(
        Action<ITypeScanner> typeScannerAction,
        Action<IMethodScanner> methodScannerAction)
    {
        GuardAgainst.Null(typeScannerAction);
        GuardAgainst.Null(methodScannerAction);

        var typeTargets = FromTypes(typeScannerAction);

        var scanner = new MethodScanner(typeTargets.Targets);
        methodScannerAction(scanner);
        return new ConventionTargetCollection<MethodInfo>(scanner.Methods);
    }

    private static IEnumerable<Type> ScanTypes(Func<ITypeScanner, ITypeScannerFilter> types)
    {
        var scanner = types(new TypeScanner()) as TypeScanner
            ?? throw new InvalidOperationException("The type scan must return the scanner it was given.");
        return scanner.Types;
    }

    private static ConventionTargetCollection<MethodInfo> ToTargets(IMethodScanner scanner)
    {
        var methodScanner = scanner as MethodScanner
            ?? throw new InvalidOperationException("The method scan must return the scanner it was given.");
        return new ConventionTargetCollection<MethodInfo>(methodScanner.Methods);
    }

    private static ConventionTargetCollection<PropertyInfo> ToTargets(IPropertyScanner scanner)
    {
        var propertyScanner = scanner as PropertyScanner
            ?? throw new InvalidOperationException("The property scan must return the scanner it was given.");
        return new ConventionTargetCollection<PropertyInfo>(propertyScanner.Properties);
    }
}
