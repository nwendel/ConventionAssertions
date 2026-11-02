using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class IsSealedTests
{
    private readonly IsSealed _tested = new();

    [Fact]
    public void Can_assert_sealed()
    {
        var target = typeof(SealedClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_static()
    {
        var target = typeof(StaticClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_not_sealed()
    {
        var target = typeof(NotSealedClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_abstract()
    {
        var target = typeof(AbstractClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
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
