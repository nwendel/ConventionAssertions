namespace ConventionAssertions.Internal.Filters;

public interface IAssemblyNameFilter
{
    AssemblyName AssemblyName { get; }

    bool HasNameSegment(string segment);
}
