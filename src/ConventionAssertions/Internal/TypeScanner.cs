using System.Collections;
using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;
using TypeFilter = ConventionAssertions.Internal.Filters.TypeFilter;

namespace ConventionAssertions.Internal;

public sealed class TypeScanner :
    IConventionTargets<Type>,
    ITypeScanner,
    ITypeScannerFilter
{
    // TODO: How to avoid bang?
    private IEnumerable<Type> _types = null!;

    public ITypeScannerFilter FromAssemblyContaining<T>()
    {
        _types = typeof(T).Assembly.GetTypes();
        return this;
    }

    public ITypeScannerFilter FromConventionTargets(IConventionTargets<Type> targets)
    {
        GuardAgainst.Null(targets);

        _types = targets;
        return this;
    }

    public ITypeScannerFilter FromDefaultDependencyContext()
        => FromDefaultDependencyContext(x => true);

    public ITypeScannerFilter FromDefaultDependencyContext(Func<IAssemblyNameFilter, bool> predicate)
    {
        if (DependencyContext.Default == null)
        {
            throw new InvalidOperationException("No default dependency context");
        }

        return FromDependencyContext(DependencyContext.Default, predicate);
    }

    public ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext)
        => FromDependencyContext(dependencyContext, x => true);

    public ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext, Func<IAssemblyNameFilter, bool> predicate)
    {
        GuardAgainst.Null(dependencyContext);

        var assemblies = dependencyContext.RuntimeLibraries
            .SelectMany(x => x.GetDefaultAssemblyNames(dependencyContext))
            .Select(x => new AssemblyNameFilter(x))
            .Where(x => predicate(x))
            .Select(x => Assembly.Load(x.AssemblyName))
            .ToList();
        _types = assemblies
            .SelectMany(x => x.GetTypes())
            .ToList();
        return this;
    }

    public ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate)
    {
        GuardAgainst.Null(predicate);

        _types = _types
            .Select(x => new TypeFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Type)
            .ToList();
        return this;
    }

    IEnumerator<Type> IEnumerable<Type>.GetEnumerator() => _types.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _types.GetEnumerator();
}
