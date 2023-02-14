namespace ConventionAssertions.Reflection;

public static class TypeGetGenericAncestorExtensions
{
    public static Type GetGenericAncestorClass(this Type self, Type genericAncestorType)
        => self.TryGetGenericAncestorClass(genericAncestorType, out var ancestorType)
            ? ancestorType
            : throw new ArgumentException($"{self.DisplayName()} does not implement {genericAncestorType.DisplayName()}");

    public static bool TryGetGenericAncestorClass(this Type self, Type genericAncestorType, [NotNullWhen(true)] out Type? ancestorType)
    {
        GuardAgainst.Null(self);
        GuardAgainst.Null(genericAncestorType);
        GuardAgainst.Condition(!genericAncestorType.IsGenericTypeDefinition, $"{genericAncestorType.DisplayName()} is not a generic type definition", nameof(genericAncestorType));

        ancestorType = self;
        while (ancestorType != null)
        {
            if (ancestorType.IsGenericType && ancestorType.GetGenericTypeDefinition() == genericAncestorType)
            {
                return true;
            }

            ancestorType = ancestorType.BaseType;
        }

        return false;
    }
}
