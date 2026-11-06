namespace ConventionAssertions.Reflection;

internal static class TypeIsStaticExtensions
{
    // The compiler emits a static class as abstract and sealed
    public static bool IsStatic(this Type self)
    {
        GuardAgainst.Null(self);

        return self.IsClass && self.IsAbstract && self.IsSealed;
    }
}
