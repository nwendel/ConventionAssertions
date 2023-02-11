namespace ConventionAssertions.Internal;

public interface ITypeFilter : IFluentInterface
{
    Type Type { get; }

    bool IsAssignableTo<T>();

    public bool IsAssignableTo(Type type);

    public bool IsAssignableToAny(params Type[] types);

    bool IsInSameNamespaceAs<T>();

    bool IsInterface();

    bool HasAttribute<T>()
        where T : Attribute;
}
