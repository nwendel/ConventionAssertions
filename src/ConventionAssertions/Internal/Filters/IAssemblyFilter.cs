namespace ConventionAssertions.Internal.Filters;

public interface IAssemblyFilter
{
    Assembly Assembly { get; }

    bool HasNamePart(string namePart);
}
