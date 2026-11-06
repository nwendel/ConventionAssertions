namespace ConventionAssertions.Internal.Filters;

public interface IAssemblyNameFilter
{
    AssemblyName AssemblyName { get; }

    bool HasNamePrefix(string prefix);

    bool HasNameSegment(string segment);

    bool HasAnyNameSegment(params string[] segments);
}
