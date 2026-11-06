namespace ConventionAssertions.Reflection;

public static class AssemblyDisplayNameExtensions
{
    public static string DisplayName(this Assembly self)
    {
        GuardAgainst.Null(self);

        var name = self.GetName().Name;
        return name ?? self.FullName ?? string.Empty;
    }
}
