using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasPublicStaticTryParseMethodTests
{
    private readonly HasPublicStaticTryParseMethod _tested = new();

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
    public void Fails_on_assert_wrong_result_type()
    {
        var target = typeof(WrongReturnType);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_wrong_result_modifier()
    {
        var target = typeof(WrongResultModififier);
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
        var target = typeof(WrongResultType);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
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
