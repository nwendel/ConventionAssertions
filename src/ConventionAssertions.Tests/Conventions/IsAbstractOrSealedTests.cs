using ConventionAssertions.Conventions;
using ConventionAssertions.Internal;

namespace ConventionAssertions.Tests.Conventions;

public class IsAbstractOrSealedTests
{
    private readonly IsAbstractOrSealed _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Can_assert_abstract()
    {
        _tested.Assert(typeof(AbstractClass), _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Can_assert_sealed()
    {
        _tested.Assert(typeof(SealedClass), _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_non_abstract_or_sealed()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(NonAbstractOrSealedClass), _context));
        Assert.Single(_context.Messages);
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
