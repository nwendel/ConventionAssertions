using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class IsAbstractOrSealedTests
{
    private readonly IsAbstractOrSealed _tested = new();

    [Fact]
    public void Can_assert_abstract()
    {
        var target = typeof(AbstractClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_sealed()
    {
        var target = typeof(SealedClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_non_abstract_or_sealed()
    {
        var target = typeof(NonAbstractOrSealedClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    private abstract class AbstractClass
    {
    }

    private sealed class SealedClass
    {
    }

    private class NonAbstractOrSealedClass
    {
    }
}
