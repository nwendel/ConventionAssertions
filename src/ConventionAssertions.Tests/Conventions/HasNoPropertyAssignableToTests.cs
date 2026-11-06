using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasNoPropertyAssignableToTests
{
    private readonly HasNoPropertyAssignableTo _tested = new() { PropertyType = typeof(DateTime) };

    [Fact]
    public void Can_assert()
    {
        var target = typeof(WithoutDateTime);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_property_of_type()
    {
        var target = typeof(WithDateTime);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_each_property_of_type()
    {
        var target = typeof(WithTwoDateTimes);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Equal(2, context.Messages.Count());
    }

    [Fact]
    public void Fails_on_assert_property_of_derived_type()
    {
        _tested.PropertyType = typeof(Exception);
        var target = typeof(WithDerivedType);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.PropertyType = null;
        var target = typeof(WithoutDateTime);
        var context = new ConventionContext(target);
        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(target, context));
    }

    private class WithoutDateTime
    {
        public DateTimeOffset Created { get; set; }
    }

    private class WithDateTime
    {
        public DateTime Created { get; set; }
    }

    private class WithTwoDateTimes
    {
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }

    private class WithDerivedType
    {
        public InvalidOperationException? Error { get; set; }
    }
}
