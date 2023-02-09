namespace ConventionAssertions.Internal;

public class TypeFilter : ITypeFilter
{
    public TypeFilter(Type type)
    {
        GuardAgainst.Null(type);

        Type = type;
    }

    public Type Type { get; }

    public bool IsAssignableTo<T>() => Type.IsAssignableTo(typeof(T));

    public bool IsInSameNamespaceAs<T>() => Type.Namespace == typeof(T).Namespace;

    public bool IsInterface() => Type.IsInterface;
}
