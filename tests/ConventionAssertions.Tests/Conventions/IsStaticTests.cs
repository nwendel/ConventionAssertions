using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class IsStaticTests
{
    private readonly IsStatic _tested = new();

    private interface ISomething
    {
    }

    [Fact]
    public void Can_assert_static()
    {
        var target = typeof(StaticClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Theory]
    [InlineData(typeof(SealedClass))]
    [InlineData(typeof(AbstractClass))]
    [InlineData(typeof(NotSealedClass))]
    [InlineData(typeof(ISomething))]
    [InlineData(typeof(SomeStruct))]
    public void Fails_on_assert_not_static(Type target)
    {
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        var message = Assert.Single(context.Messages);
        Assert.EndsWith(" must be static.", message, StringComparison.Ordinal);
    }

    private struct SomeStruct
    {
    }

    private static class StaticClass
    {
    }

    private sealed class SealedClass
    {
    }

    private class NotSealedClass
    {
    }

    private abstract class AbstractClass
    {
    }
}
