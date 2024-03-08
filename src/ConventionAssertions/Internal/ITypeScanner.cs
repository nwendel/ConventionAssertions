using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

public interface ITypeScanner : IFluentInterface
{
    ITypeScannerFilter FromAssemblyContaining<T>();

    ITypeScannerFilter FromTargets(IConventionTargets<Type> targets);

    ITypeScannerFilter FromDefaultDependencyContext();

    ITypeScannerFilter FromDefaultDependencyContext(Func<IAssemblyNameFilter, bool> predicate);

    ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext);

    ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext, Func<IAssemblyNameFilter, bool> predicate);
}
