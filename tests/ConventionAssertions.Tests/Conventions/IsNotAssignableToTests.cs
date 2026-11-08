using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class IsNotAssignableToTests
{
    private readonly IsNotAssignableTo _tested = new() { Type = typeof(IValueObject) };

    [Fact]
    public void Can_assert()
    {
        var context = AssertType(typeof(PublicClass));

        Assert.Empty(context.Messages);
    }

    [Theory]
    [InlineData(typeof(GenericOne<>))]
    [InlineData(typeof(IGeneric<>))]
    public void Can_assert_open_generic(Type type)
    {
        _tested.Type = type;
        var context = AssertType(typeof(PublicClass));

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_interface()
    {
        var context = AssertType(typeof(SampleId));

        var message = Assert.Single(context.Messages);
        Assert.Equal(
            "Type ConventionAssertions.Tests.TestHelpers.SampleId must not be assignable to ConventionAssertions.Tests.TestHelpers.IValueObject.",
            message);
    }

    [Fact]
    public void Fails_on_assert_base_class()
    {
        _tested.Type = typeof(GenericOne<int>);
        var context = AssertType(typeof(GenericThree<int>));

        Assert.Single(context.Messages);
    }

    [Theory]
    [InlineData(typeof(GenericOne<>))]
    [InlineData(typeof(IGeneric<>))]
    public void Fails_on_assert_open_generic(Type type)
    {
        _tested.Type = type;
        var context = AssertType(typeof(GenericThree<int>));

        Assert.Single(context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.Type = null;

        Assert.Throws<ConventionNotConfiguredException>(() => AssertType(typeof(PublicClass)));
    }

    private ConventionContext AssertType(Type target)
    {
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        return context;
    }
}
