using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

public interface IAssemblyScanner : IFluentInterface
{
    IAssemblyScannerFilter FromAssemblyContaining<T>();

    IAssemblyScannerFilter FromDefaultDependencyContext();

    IAssemblyScannerFilter FromDefaultDependencyContext(Func<IAssemblyNameFilter, bool> predicate);

    IAssemblyScannerFilter FromDependencyContext(DependencyContext dependencyContext);

    IAssemblyScannerFilter FromDependencyContext(DependencyContext dependencyContext, Func<IAssemblyNameFilter, bool> predicate);
}
