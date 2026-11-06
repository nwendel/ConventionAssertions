using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

public sealed class AssemblyScanner :
    IAssemblyScanner,
    IAssemblyScannerFilter
{
    // Assemblies are kept by name and only loaded once every Where has run,
    // so filtering on the name never loads an assembly that is filtered out
    private List<(AssemblyName Name, Assembly? Assembly)> _assemblies = new();

    internal IEnumerable<Assembly> Assemblies => _assemblies
        .Select(x => x.Assembly ?? Assembly.Load(x.Name))
        .ToList();

    public IAssemblyScannerFilter FromAssemblyContaining<T>()
    {
        var assembly = typeof(T).Assembly;
        _assemblies = new() { (assembly.GetName(), assembly) };
        return this;
    }

    public IAssemblyScannerFilter FromDefaultDependencyContext()
        => FromDependencyContext(DependencyContextAssemblies.Default);

    public IAssemblyScannerFilter FromDependencyContext(DependencyContext dependencyContext)
    {
        GuardAgainst.Null(dependencyContext);

        _assemblies = DependencyContextAssemblies.GetAssemblyNames(dependencyContext)
            .Select(x => (x, (Assembly?)null))
            .ToList();
        return this;
    }

    public IAssemblyScannerFilter Where(Func<IAssemblyNameFilter, bool> predicate)
    {
        GuardAgainst.Null(predicate);

        _assemblies = _assemblies
            .Where(x => predicate(new AssemblyNameFilter(x.Name)))
            .ToList();
        return this;
    }
}
