using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

internal static class DependencyContextAssemblies
{
    public static DependencyContext Default
        => DependencyContext.Default ?? throw new InvalidOperationException("No default dependency context");

    public static List<Assembly> Load(DependencyContext dependencyContext, Func<IAssemblyNameFilter, bool> predicate)
    {
        GuardAgainst.Null(dependencyContext);
        GuardAgainst.Null(predicate);

        return dependencyContext.RuntimeLibraries
            .SelectMany(x => x.GetDefaultAssemblyNames(dependencyContext))
            .Select(x => new AssemblyNameFilter(x))
            .Where(x => predicate(x))
            .Select(x => Assembly.Load(x.AssemblyName))
            .ToList();
    }
}
