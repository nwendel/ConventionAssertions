using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class NameHasSuffixTests
{
    private readonly NameHasSuffix _tested = new() { Suffix = "Handler" };

    [Fact]
    public void Can_assert_type()
    {
        var target = typeof(CommandHandler);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_generic_type()
    {
        var target = typeof(GenericHandler<>);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_type()
    {
        var target = typeof(CommandProcessor);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Can_assert_method()
    {
        var target = typeof(CommandHandler).GetMethod(nameof(CommandHandler.EventHandler))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_method()
    {
        var target = typeof(CommandHandler).GetMethod(nameof(CommandHandler.Process))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Can_assert_property()
    {
        var target = typeof(CommandHandler).GetProperty(nameof(CommandHandler.NextHandler))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_property()
    {
        var target = typeof(CommandHandler).GetProperty(nameof(CommandHandler.Name))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.Suffix = null;
        var target = typeof(CommandHandler);
        var context = new ConventionContext(target);
        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(target, context));
    }

    private class CommandHandler
    {
        public CommandHandler? NextHandler { get; set; }

        public string? Name { get; set; }

        public static void EventHandler()
        {
        }

        public static void Process()
        {
        }
    }

    private class GenericHandler<T>
    {
    }

    private class CommandProcessor
    {
    }
}
