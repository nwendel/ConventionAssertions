using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Internal;

public interface IAssemblyScannerFilter : IFluentInterface
{
    IAssemblyScannerFilter Where(Func<IAssemblyNameFilter, bool> predicate);
}
