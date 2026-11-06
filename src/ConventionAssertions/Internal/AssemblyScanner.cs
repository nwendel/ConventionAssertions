using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

public sealed class AssemblyScanner :
    IAssemblyScanner,
    IAssemblyScannerFilter
{
    private List<Assembly> _assemblies = new();

    internal IEnumerable<Assembly> Assemblies => _assemblies;

    public IAssemblyScannerFilter FromAssemblyContaining<T>()
    {
        _assemblies = new List<Assembly> { typeof(T).Assembly };
        return this;
    }

    public IAssemblyScannerFilter FromDefaultDependencyContext()
        => FromDefaultDependencyContext(x => true);

    public IAssemblyScannerFilter FromDefaultDependencyContext(Func<IAssemblyNameFilter, bool> predicate)
        => FromDependencyContext(DependencyContextAssemblies.Default, predicate);

    public IAssemblyScannerFilter FromDependencyContext(DependencyContext dependencyContext)
        => FromDependencyContext(dependencyContext, x => true);

    public IAssemblyScannerFilter FromDependencyContext(DependencyContext dependencyContext, Func<IAssemblyNameFilter, bool> predicate)
    {
        GuardAgainst.Null(dependencyContext);

        _assemblies = DependencyContextAssemblies.Load(dependencyContext, predicate);
        return this;
    }

    public IAssemblyScannerFilter Where(Func<IAssemblyNameFilter, bool> predicate)
    {
        GuardAgainst.Null(predicate);

        _assemblies = _assemblies
            .Where(x => predicate(new AssemblyNameFilter(x.GetName())))
            .ToList();
        return this;
    }
}
