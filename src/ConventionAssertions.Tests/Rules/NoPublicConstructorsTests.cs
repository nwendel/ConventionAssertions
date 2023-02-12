using ConventionAssertions.Internal;
using ConventionAssertions.Rules;

namespace ConventionAssertions.Tests.Rules;

public class NoPublicConstructorsTests
{
    private readonly NoPublicConstructors _tested = new();
    private readonly ConventionContext _context = new("some-id");

    [Fact]
    public void Can_assert()
    {
        _tested.Assert(typeof(ProtectedConstructor), _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Can_assert_sealed()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(PublicConstructor), _context));
        Assert.Single(_context.Messages);
    }

    private class ProtectedConstructor
    {
        protected ProtectedConstructor()
        {
        }
    }

    private class PublicConstructor
    {
        public PublicConstructor()
        {
        }
    }
}
