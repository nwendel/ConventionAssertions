using ConventionAssertions.Internal.Filters;
using Microsoft.Extensions.DependencyModel;

namespace ConventionAssertions.Internal;

public interface ITypeScanner : IFluentInterface
{
    ITypeScannerFilter FromAssemblyContaining<T>();

    ITypeScannerFilter FromTypeSource(ConventionTypeSource typeSource);

    ITypeScannerFilter FromDependencyContext(DependencyContext dependencyContext);

    ITypeScannerFilter FromDefaultDependencyContext();
}
