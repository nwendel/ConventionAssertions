using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

public interface IAssemblyScanner : IFluentInterface
{
    IAssemblyScannerFilter FromAssemblyContaining<T>();

    IAssemblyScannerFilter FromDefaultDependencyContext();

    IAssemblyScannerFilter FromDependencyContext(DependencyContext dependencyContext);
}
