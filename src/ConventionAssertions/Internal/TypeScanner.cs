using System.Reflection;
using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;
using TypeFilter = ConventionAssertions.Internal.Filters.TypeFilter;

namespace ConventionAssertions.Internal;

public class TypeScanner :
    IConventionTargets<Type>,
    ITypeScanner,
    ITypeScannerFilter
{
    // TODO: How to avoid bang?
    public IEnumerable<Type> Targets { get; private set; } = null!;

    public ITypeScannerFilter FromAssemblyContaining<T>()
    {
        Targets = typeof(T).Assembly.GetTypes();
        return this;
    }

    public ITypeScannerFilter FromConventionTargets(IConventionTargets<Type> targets)
    {
        GuardAgainst.Null(targets);

        Targets = targets.Targets;
        return this;
    }

    public ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext)
    {
        GuardAgainst.Null(dependencyContext);

        var assemblies = dependencyContext.RuntimeLibraries
            .SelectMany(x => x.GetDefaultAssemblyNames(dependencyContext))
            .Select(x => Assembly.Load(x))
            .ToList();
        Targets = assemblies
            .SelectMany(x => x.GetTypes())
            .ToList();
        return this;
    }

    public ITypeScannerFilter FromDefaultDependencyContext()
    {
        if (DependencyContext.Default == null)
        {
            throw new InvalidOperationException("No default dependency context");
        }

        return FromDependencyContext(DependencyContext.Default);
    }

    public ITypeScannerFilter Where(Func<ITypeFilter, bool> predicate)
    {
        GuardAgainst.Null(predicate);

        Targets = Targets
            .Select(x => new TypeFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Type)
            .ToList();
        return this;
    }
}
