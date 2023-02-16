namespace ConventionAssertions;

public interface ITypeConvention
{
    string CheckId => GetType().Name;

    void Assert(Type type, ConventionContext context);
}
