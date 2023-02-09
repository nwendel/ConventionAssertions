namespace ConventionAssertions.Internal;

public interface ITypeFilter : IFluentInterface
{
    Type Type { get; }

    bool IsAssignableTo<T>();

    bool IsInSameNamespaceAs<T>();

    bool IsInterface();
}
