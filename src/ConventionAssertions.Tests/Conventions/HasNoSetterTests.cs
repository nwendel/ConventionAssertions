using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasNoSetterTests
{
    private readonly HasNoSetter _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Can_assert()
    {
        var property = typeof(Something).GetProperty(nameof(Something.NoSetter))!;
        _tested.Assert(property, _context);
        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_public_setter()
    {
        var property = typeof(Something).GetProperty(nameof(Something.PublicSetter))!;
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(property, _context));
        Assert.Single(_context.Messages);
    }

    private class Something
    {
        public int NoSetter { get; }

        public int PublicSetter { get; set; }
    }
}
