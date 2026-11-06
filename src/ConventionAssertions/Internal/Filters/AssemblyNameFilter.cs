namespace ConventionAssertions.Internal.Filters;

public class AssemblyNameFilter : IAssemblyNameFilter
{
    public AssemblyNameFilter(AssemblyName assemblyName)
    {
        GuardAgainst.Null(assemblyName);

        AssemblyName = assemblyName;
    }

    public AssemblyName AssemblyName { get; }

    public bool HasNamePrefix(string prefix)
    {
        GuardAgainst.NullOrWhiteSpace(prefix);

        var name = AssemblyName.Name;
        if (name == null || !name.StartsWith(prefix, StringComparison.Ordinal))
        {
            return false;
        }

        // Only match on segment boundaries, so Amni.Appointments doesn't match Amni.AppointmentsLegacy
        return name.Length == prefix.Length || name[prefix.Length] == '.';
    }

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

    public bool HasAnyNameSegment(params string[] segments)
    {
        GuardAgainst.Null(segments);
        GuardAgainst.Condition(
            segments.Length == 0,
            "At least one segment must be specified",
            nameof(segments));

        return segments.Any(HasNameSegment);
    }
}
