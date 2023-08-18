using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

public interface ITypeScanner : IFluentInterface
{
    ITypeScannerFilter FromAssemblyContaining<T>();

    ITypeScannerFilter FromConventionTargets(IConventionTargets<Type> targets);

    ITypeScannerFilter FromDefaultDependencyContext();

    ITypeScannerFilter FromDefaultDependencyContext(Func<IAssemblyFilter, bool> predicate);

    ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext);

    ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext, Func<IAssemblyFilter, bool> predicate);
}
