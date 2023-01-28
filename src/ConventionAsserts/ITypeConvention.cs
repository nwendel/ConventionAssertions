namespace ConventionAsserts;

public interface ITypeConvention
{
    void Assert(Type type, ConventionContext context);
}
