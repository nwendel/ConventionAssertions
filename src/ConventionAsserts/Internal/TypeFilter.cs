namespace ConventionAsserts.Internal;

public class TypeFilter : ITypeFilter
{
    public TypeFilter(Type type)
    {
        GuardAgainst.Null(type);

        Type = type;
    }

    public Type Type { get; }

    public bool AssignableTo<T>() => Type.IsAssignableTo(typeof(T));
}
