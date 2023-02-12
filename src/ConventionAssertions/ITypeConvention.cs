namespace ConventionAssertions;

public interface ITypeConvention
{
    string Id => GetType().Name;

    void Assert(Type type, ConventionContext context);
}
