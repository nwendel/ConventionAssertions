namespace ConventionAssertions.Internal.Filters;

public class AssemblyFilter : IAssemblyFilter
{
    public AssemblyFilter(Assembly assembly)
    {
        GuardAgainst.Null(assembly);

        Assembly = assembly;
    }

    public Assembly Assembly { get; }

    public bool HasNameSegment(string segment)
    {
        GuardAgainst.NullOrWhiteSpace(segment);

        var name = Assembly.GetName().Name;
        if (name == null)
        {
            return false;
        }

        var segments = name.Split('.');
        return segments.Contains(segment);
    }
}
