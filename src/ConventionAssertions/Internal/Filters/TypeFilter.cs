using ConventionAssertions.Reflection;

namespace ConventionAssertions.Internal.Filters;

public class TypeFilter : ITypeFilter
{
    public TypeFilter(Type type)
    {
        GuardAgainst.Null(type);

        Type = type;
    }

    public Type Type { get; }

    public bool IsAbstract
        => Type.IsAbstract;

    public bool IsClass
        => Type.IsClass;

    public bool IsConcrete
        => !Type.IsAbstract;

    public bool IsInterface
        => Type.IsInterface;

    public bool HasAttribute<T>()
        where T : Attribute
        => Type.GetCustomAttribute<T>() != null;

    public bool IsAssignableTo<T>()
        => IsAssignableToCore(typeof(T));

    public bool IsAssignableTo(Type type)
        => IsAssignableToCore(type);

    public bool IsAssignableToAny(params Type[] types)
        => types.Any(x => IsAssignableToCore(x));

    public bool IsInSameNamespaceAs<T>()
        => Type.Namespace == typeof(T).Namespace;

    private bool IsAssignableToCore(Type type)
    {
        GuardAgainst.Null(type);

        if (type.IsGenericTypeDefinition)
        {
            if (type.IsInterface)
            {
                return AssignableToGenericInterface(type);
            }

            return Type.TryGetGenericAncestorClass(type, out _);
        }

        return Type.IsAssignableTo(type);
    }

    private bool AssignableToGenericInterface(Type type)
    {
        var interfaces = Type.GetInterfaces();
        foreach (var @interface in interfaces)
        {
            if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == type)
            {
                return true;
            }
        }

        return false;
    }
}
