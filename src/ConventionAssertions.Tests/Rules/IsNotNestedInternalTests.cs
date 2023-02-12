using ConventionAssertions.Internal;
using ConventionAssertions.Rules;
using ConventionAssertions.Tests.TestHelpers;

namespace ConventionAssertions.Tests.Rules;

public class IsNotNestedInternalTests
{
    private readonly IsNotNestedInternal _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Can_assert_internal()
    {
        _tested.Assert(typeof(InternalClass), _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_public()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(PublicClass), _context));
        Assert.Single(_context.Messages);
    }
}
