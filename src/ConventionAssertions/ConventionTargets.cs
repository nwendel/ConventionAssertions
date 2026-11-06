using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class ConventionTargets
{
    public static IConventionTargets<Assembly> FromAssemblies(Func<IAssemblyScanner, IAssemblyScannerFilter> assemblies)
    {
        GuardAgainst.Null(assemblies);

        return new ConventionTargetCollection<Assembly>(ScanAssemblies(assemblies));
    }

    public static IConventionTargets<Type> FromTypes(
        Func<IAssemblyScanner, IAssemblyScannerFilter> assemblies,
        Func<ITypeScanner, ITypeScanner>? types = null)
    {
        GuardAgainst.Null(assemblies);

        return new ConventionTargetCollection<Type>(ScanTypes(ScanAssemblies(assemblies), types));
    }

    public static IConventionTargets<Type> FromTypes(
        IConventionTargets<Assembly> assemblies,
        Func<ITypeScanner, ITypeScanner>? types = null)
    {
        GuardAgainst.Null(assemblies);

        return new ConventionTargetCollection<Type>(ScanTypes(assemblies.Targets, types));
    }

    public static IConventionTargets<Type> FromTypes(
        IConventionTargets<Type> targets,
        Func<ITypeScanner, ITypeScanner> types)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(types);

        return new ConventionTargetCollection<Type>(FilterTypes(targets.Targets, types));
    }

    public static IConventionTargets<MethodInfo> FromMethods(
        Func<IAssemblyScanner, IAssemblyScannerFilter> assemblies,
        Func<ITypeScanner, ITypeScanner>? types = null,
        Func<IMethodScanner, IMethodScanner>? methods = null)
    {
        GuardAgainst.Null(assemblies);

        return ScanMethods(ScanTypes(ScanAssemblies(assemblies), types), methods);
    }

    public static IConventionTargets<MethodInfo> FromMethods(
        IConventionTargets<Type> types,
        Func<IMethodScanner, IMethodScanner>? methods = null)
    {
        GuardAgainst.Null(types);

        return ScanMethods(types.Targets, methods);
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
        Func<IAssemblyScanner, IAssemblyScannerFilter> assemblies,
        Func<ITypeScanner, ITypeScanner>? types = null,
        Func<IPropertyScanner, IPropertyScanner>? properties = null)
    {
        GuardAgainst.Null(assemblies);

        return ScanProperties(ScanTypes(ScanAssemblies(assemblies), types), properties);
    }

    public static IConventionTargets<PropertyInfo> FromProperties(
        IConventionTargets<Type> types,
        Func<IPropertyScanner, IPropertyScanner>? properties = null)
    {
        GuardAgainst.Null(types);

        return ScanProperties(types.Targets, properties);
    }

    public static IConventionTargets<PropertyInfo> FromProperties(
        IConventionTargets<PropertyInfo> targets,
        Func<IPropertyScanner, IPropertyScanner> properties)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(properties);

        return ToTargets(properties(new PropertyScanner(targets)));
    }

    private static IEnumerable<Assembly> ScanAssemblies(Func<IAssemblyScanner, IAssemblyScannerFilter> assemblies)
    {
        var scanner = assemblies(new AssemblyScanner()) as AssemblyScanner
            ?? throw new InvalidOperationException("The assembly scan must return the scanner it was given.");
        return scanner.Assemblies;
    }

    private static IEnumerable<Type> ScanTypes(IEnumerable<Assembly> assemblies, Func<ITypeScanner, ITypeScanner>? types)
    {
        var allTypes = assemblies.SelectMany(x => x.GetTypes());
        return types == null
            ? allTypes.ToList()
            : FilterTypes(allTypes, types);
    }

    private static IEnumerable<Type> FilterTypes(IEnumerable<Type> source, Func<ITypeScanner, ITypeScanner> types)
    {
        var scanner = types(new TypeScanner(source)) as TypeScanner
            ?? throw new InvalidOperationException("The type scan must return the scanner it was given.");
        return scanner.Types;
    }

    private static ConventionTargetCollection<MethodInfo> ScanMethods(IEnumerable<Type> types, Func<IMethodScanner, IMethodScanner>? methods)
    {
        IMethodScanner scanner = new MethodScanner(types);
        if (methods != null)
        {
            scanner = methods(scanner);
        }

        return ToTargets(scanner);
    }

    private static ConventionTargetCollection<PropertyInfo> ScanProperties(IEnumerable<Type> types, Func<IPropertyScanner, IPropertyScanner>? properties)
    {
        IPropertyScanner scanner = new PropertyScanner(types);
        if (properties != null)
        {
            scanner = properties(scanner);
        }

        return ToTargets(scanner);
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
