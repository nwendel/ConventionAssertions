namespace ConventionAssertions.Internal.Filters;

public class AssemblyNameFilter : IAssemblyNameFilter
{
    public AssemblyNameFilter(AssemblyName assemblyName)
    {
        GuardAgainst.Null(assemblyName);

        AssemblyName = assemblyName;
    }

    public AssemblyName AssemblyName { get; }

    public bool HasNameSegment(string segment)
    {
        GuardAgainst.NullOrWhiteSpace(segment);

        var name = AssemblyName.Name;
        if (name == null)
        {
            return false;
        }

        var segments = name.Split('.');
        return segments.Contains(segment);
    }
}
