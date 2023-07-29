using System.Reflection;
using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;
using TypeFilter = ConventionAssertions.Internal.Filters.TypeFilter;

namespace ConventionAssertions.Internal;

public class TypeScanner : ConventionTypeSource, ITypeScanner, ITypeScannerFilter
{
    public ITypeScannerFilter FromAssemblyContaining<T>()
    {
        Types = typeof(T).Assembly.GetTypes();
        return this;
    }

    public ITypeScannerFilter FromTypeSource(ConventionTypeSource typeSource)
    {
        GuardAgainst.Null(typeSource);

        Types = typeSource.Types;
        return this;
    }

    public ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext)
    {
        GuardAgainst.Null(dependencyContext);

        var assemblies = dependencyContext.RuntimeLibraries
            .SelectMany(x => x.GetDefaultAssemblyNames(dependencyContext))
            .Select(x => Assembly.Load(x))
            .ToList();
        Types = assemblies
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

        Types = Types
            .Select(x => new TypeFilter(x))
            .Where(x => predicate(x))
            .Select(x => x.Type)
            .ToList();
        return this;
    }
}
