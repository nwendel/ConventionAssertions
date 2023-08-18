namespace ConventionAssertions.Internal.Filters;

public class AssemblyFilter : IAssemblyFilter
{
    public AssemblyFilter(Assembly assembly)
    {
        GuardAgainst.Null(assembly);

        Assembly = assembly;
    }

    public Assembly Assembly { get; }

    public bool HasNamePart(string namePart)
    {
        GuardAgainst.NullOrWhiteSpace(namePart);

        var name = Assembly.GetName().Name;
        if (name == null)
        {
            return false;
        }

        var nameParts = name.Split('.');
        return nameParts.Contains(namePart);
    }
}
