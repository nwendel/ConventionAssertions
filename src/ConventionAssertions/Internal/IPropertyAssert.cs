using System.Reflection;

namespace ConventionAssertions.Internal;

public interface IPropertyAssert : IFluentInterface
{
    void Assert<T>()
        where T : IPropertyConvention, new();

    void Assert(IPropertyConvention convention);

    void Assert(Action<PropertyInfo, ConventionContext> assert);
}
