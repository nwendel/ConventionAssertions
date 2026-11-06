using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class IsAssignableToTests
{
    private readonly IsAssignableTo _tested = new() { Type = typeof(IValueObject) };

    [Fact]
    public void Can_assert_interface()
    {
        var context = AssertType(typeof(SampleId));

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_base_class()
    {
        _tested.Type = typeof(GenericOne<int>);
        var context = AssertType(typeof(GenericThree<int>));

        Assert.Empty(context.Messages);
    }

    [Theory]
    [InlineData(typeof(GenericOne<>))]
    [InlineData(typeof(IGeneric<>))]
    public void Can_assert_open_generic(Type type)
    {
        _tested.Type = type;
        var context = AssertType(typeof(GenericThree<int>));

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_not_assignable()
    {
        var context = AssertType(typeof(PublicClass));

        var message = Assert.Single(context.Messages);
        Assert.Equal(
            "Type ConventionAssertions.Tests.TestHelpers.PublicClass must be assignable to ConventionAssertions.Tests.TestHelpers.IValueObject.",
            message);
    }

    [Theory]
    [InlineData(typeof(GenericOne<>))]
    [InlineData(typeof(IGeneric<>))]
    public void Fails_on_assert_not_assignable_to_open_generic(Type type)
    {
        _tested.Type = type;
        var context = AssertType(typeof(PublicClass));

        Assert.Single(context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.Type = null;

        Assert.Throws<ConventionNotConfiguredException>(() => AssertType(typeof(SampleId)));
    }

    private ConventionContext AssertType(Type target)
    {
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        return context;
    }
}
