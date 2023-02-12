using ConventionAssertions.Internal;
using ConventionAssertions.Rules;

namespace ConventionAssertions.Tests.Rules;

public class HasPublicStaticTryParseMethodTests
{
    private readonly HasPublicStaticTryParseMethod _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Can_assert()
    {
        _tested.Assert(typeof(int), _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_public()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(NotPublic), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_static()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(NotStatic), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_wrong_result_type()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(WrongReturnType), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_wrong_result_modifier()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(WrongResultModififier), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_wrong_argument_type()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(WrongArgumentType), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_wrong_return_type()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(WrongResultType), _context));
        Assert.Single(_context.Messages);
    }

    private class NotPublic
    {
        private static bool TryParse(string value, out NotPublic? result) => throw new NotImplementedException();
    }

    private class NotStatic
    {
        public bool TryParse(string value, out NotStatic? result) => throw new NotImplementedException();
    }

    private class WrongArgumentType
    {
        public static bool Parse(int value, out WrongArgumentType? result) => throw new NotImplementedException();
    }

    private class WrongResultType
    {
        public static bool Parse(int value, out object? result) => throw new NotImplementedException();
    }

    private class WrongResultModififier
    {
        public static bool Parse(int value, object? result) => throw new NotImplementedException();
    }

    private class WrongReturnType
    {
        public static object Parse(string value, out WrongArgumentType? result) => throw new NotImplementedException();
    }
}
