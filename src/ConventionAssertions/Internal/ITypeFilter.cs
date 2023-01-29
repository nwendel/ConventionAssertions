namespace ConventionAssertions.Internal;

public interface ITypeFilter : IFluentInterface
{
    Type Type { get; }

    bool AssignableTo<T>();
}
