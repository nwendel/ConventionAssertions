namespace ConventionAssertions.Internal;

public interface ITypeFilter
{
    Type Type { get; }

    bool AssignableTo<T>();
}
