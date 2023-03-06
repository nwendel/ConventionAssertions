namespace ConventionAssertions;

public interface ITypeConvention
{
    void Assert(Type type, ConventionContext context);
}
