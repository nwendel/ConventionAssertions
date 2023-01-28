namespace ConventionAsserts.Internal;

public interface ITypeAssert
{
    void Assert<T>()
        where T : ITypeConvention, new();

    void Assert(ITypeConvention convention);

    void Assert(Action<Type, ConventionContext> assert);
}
