namespace ConventionAssertions.Reflection;

internal static class TypeIsAssignableToExtensions
{
    // Like Type.IsAssignableTo, but an open generic type such as IRepository<> or EntityBase<> also matches
    public static bool IsAssignableToOpenOrClosed(this Type self, Type type)
    {
        GuardAgainst.Null(self);
        GuardAgainst.Null(type);

        if (type.IsGenericTypeDefinition)
        {
            if (type.IsInterface)
            {
                return self.GetInterfaces()
                    .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == type);
            }

            return self.TryGetGenericAncestorClass(type, out _);
        }

        return self.IsAssignableTo(type);
    }
}
