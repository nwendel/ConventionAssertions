namespace ConventionAssertions.Internal.Filters;

public interface IAssemblyFilter
{
    Assembly Assembly { get; }

    bool HasNameSegment(string segment);
}
