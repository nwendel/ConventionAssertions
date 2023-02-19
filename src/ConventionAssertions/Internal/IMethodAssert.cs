namespace ConventionAssertions.Internal;

public interface IMethodAssert : IFluentInterface
{
    void Assert<T>()
        where T : IMethodConvention, new();
}
