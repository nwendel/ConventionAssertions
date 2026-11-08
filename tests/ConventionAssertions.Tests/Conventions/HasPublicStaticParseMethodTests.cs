using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasPublicStaticParseMethodTests
{
    private readonly HasPublicStaticParseMethod _tested = new();

    [Fact]
    public void Can_assert()
    {
        var target = typeof(int);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_not_public()
    {
        var target = typeof(NotPublic);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_not_static()
    {
        var target = typeof(NotStatic);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_wrong_argument_type()
    {
        var target = typeof(WrongArgumentType);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_wrong_return_type()
    {
        var target = typeof(WrongReturnType);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
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
