using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

internal static class DependencyContextAssemblies
{
    public static DependencyContext Default
        => DependencyContext.Default ?? throw new InvalidOperationException("No default dependency context");

    public static List<AssemblyName> GetAssemblyNames(DependencyContext dependencyContext)
    {
        GuardAgainst.Null(dependencyContext);

        return dependencyContext.RuntimeLibraries
            .SelectMany(x => x.GetDefaultAssemblyNames(dependencyContext))
            .ToList();
    }
}
