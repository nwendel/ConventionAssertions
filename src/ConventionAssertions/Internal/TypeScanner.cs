using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;
using TypeFilter = ConventionAssertions.Internal.Filters.TypeFilter;

namespace ConventionAssertions.Internal;

public sealed class TypeScanner :
    ITypeScanner,
    ITypeScannerFilter
{
    // TODO: How to avoid bang?
    private IEnumerable<Type> _types = null!;

    internal IEnumerable<Type> Types => _types;

    public ITypeScannerFilter FromAssemblyContaining<T>()
    {
        _types = typeof(T).Assembly.GetTypes();
        return this;
    }

    public ITypeScannerFilter FromTargets(IConventionTargets<Type> targets)
    {
        GuardAgainst.Null(targets);

        _types = targets.Targets;
        return this;
    }

    public ITypeScannerFilter FromDefaultDependencyContext()
        => FromDefaultDependencyContext(x => true);

    public ITypeScannerFilter FromDefaultDependencyContext(Func<IAssemblyNameFilter, bool> predicate)
        => FromDependencyContext(DependencyContextAssemblies.Default, predicate);

    public ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext)
        => FromDependencyContext(dependencyContext, x => true);

    public ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext, Func<IAssemblyNameFilter, bool> predicate)
    {
        GuardAgainst.Null(dependencyContext);

        var assemblies = DependencyContextAssemblies.Load(dependencyContext, predicate);
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
}
