using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasNoParameterAssignableToTests
{
    private readonly HasNoParameterAssignableTo _tested = new() { ParameterType = typeof(DateTime) };

    [Theory]
    [InlineData(nameof(Something.WithoutParameters))]
    [InlineData(nameof(Something.WithoutDateTime))]
    public void Can_assert(string methodName)
    {
        var target = typeof(Something).GetMethod(methodName)!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_parameter_of_type()
    {
        var target = typeof(Something).GetMethod(nameof(Something.WithDateTime))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_each_parameter_of_type()
    {
        var target = typeof(Something).GetMethod(nameof(Something.WithTwoDateTimes))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Equal(2, context.Messages.Count());
    }

    [Theory]
    [InlineData(nameof(Something.WithRefDateTime))]
    [InlineData(nameof(Something.WithOutDateTime))]
    [InlineData(nameof(Something.WithInDateTime))]
    public void Fails_on_assert_by_ref_parameter_of_type(string methodName)
    {
        var target = typeof(Something).GetMethod(methodName)!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_parameter_of_derived_type()
    {
        _tested.ParameterType = typeof(Exception);
        var target = typeof(Something).GetMethod(nameof(Something.WithDerivedType))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Single(context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.ParameterType = null;
        var target = typeof(Something).GetMethod(nameof(Something.WithoutParameters))!;
        var context = new ConventionContext(target);

        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(target, context));
    }

    private class Something
    {
        public static void WithoutParameters()
        {
        }

        public static void WithoutDateTime(DateTimeOffset created)
        {
        }

        public static void WithDateTime(DateTime created)
        {
        }

        public static void WithTwoDateTimes(DateTime created, DateTime modified)
        {
        }

        public static void WithRefDateTime(ref DateTime created)
        {
        }

        public static void WithOutDateTime(out DateTime created) => created = default;

        public static void WithInDateTime(in DateTime created)
        {
        }

        public static void WithDerivedType(InvalidOperationException error)
        {
        }
    }
}
