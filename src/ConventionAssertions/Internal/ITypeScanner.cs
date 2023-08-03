using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

public interface ITypeScanner : IFluentInterface
{
    ITypeScannerFilter FromAssemblyContaining<T>();

    ITypeScannerFilter FromConventionTargets(IConventionTargets<Type> targets);

    ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext);

    ITypeScannerFilter FromDefaultDependencyContext();
}
