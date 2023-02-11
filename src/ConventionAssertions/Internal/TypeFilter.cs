using System.Reflection;

namespace ConventionAssertions.Internal;

public class TypeFilter : ITypeFilter
{
    public TypeFilter(Type type)
    {
        GuardAgainst.Null(type);

        Type = type;
    }

    public Type Type { get; }

    public bool IsAssignableTo<T>()
        => IsAssignableToCore(typeof(T));

    public bool IsAssignableTo(Type type)
        => IsAssignableToCore(type);

    public bool IsAssignableToAny(params Type[] types)
        => types.Any(x => IsAssignableToCore(x));

    public bool IsInSameNamespaceAs<T>()
        => Type.Namespace == typeof(T).Namespace;

    public bool IsInterface()
        => Type.IsInterface;

    public bool HasAttribute<T>()
        where T : Attribute
        => Type.GetCustomAttribute<T>() != null;

    private bool IsAssignableToCore(Type type)
    {
        if (type.IsGenericTypeDefinition)
        {
            if (type.IsInterface)
            {
                return AssignableToGenericInterface(type);
            }

            return AssignableToGenericClass(type);
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

    private bool AssignableToGenericClass(Type type)
    {
        var candidateType = Type;
        while (candidateType != null)
        {
            if (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == type)
            {
                return true;
            }

            candidateType = candidateType.BaseType;
        }

        return false;
    }
}
