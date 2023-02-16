namespace ConventionAssertions.Internal;

public interface ITypeAssert : IFluentInterface
{
    void Assert<T>()
        where T : ITypeConvention, new();

    void Assert(ITypeConvention convention);

    void Assert(string checkId, Action<Type, ConventionContext> assert);
}
