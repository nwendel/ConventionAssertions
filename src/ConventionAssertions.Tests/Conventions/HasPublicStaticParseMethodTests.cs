using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasPublicStaticParseMethodTests
{
    private readonly HasPublicStaticParseMethod _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Can_assert()
    {
        _tested.Assert(typeof(int), _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Can_assert_not_public()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(NotPublic), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Can_assert_not_static()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(NotStatic), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Can_assert_wrong_argument_type()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(WrongArgumentType), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Can_assert_wrong_return_type()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(WrongReturnType), _context));
        Assert.Single(_context.Messages);
    }

    private class NotPublic
    {
        private static NotPublic Parse(string value) => throw new NotImplementedException();
    }

    private class NotStatic
    {
        public NotStatic Parse(string value) => throw new NotImplementedException();
    }

    private class WrongArgumentType
    {
        public static WrongArgumentType Parse(int value) => throw new NotImplementedException();
    }

    private class WrongReturnType
    {
        public static object Parse(string value) => throw new NotImplementedException();
    }
}
