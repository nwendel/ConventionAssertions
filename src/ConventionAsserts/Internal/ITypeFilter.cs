namespace ConventionAsserts.Internal;

public interface ITypeFilter
{
    Type Type { get; }

    bool AssignableTo<T>();
}
