namespace ConventionAssertions.Internal.Filters;

public interface IMemberFilter : IFluentInterface
{
    bool HasCustomAttribute<T>()
        where T : Attribute;

    bool HasCustomAttribute(Func<Attribute, bool> predicate);
}
