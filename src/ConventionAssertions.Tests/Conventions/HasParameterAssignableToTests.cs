using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasParameterAssignableToTests
{
    private readonly HasParameterAssignableTo _tested = new() { ParameterType = typeof(DateTime) };

    [Fact]
    public void Can_assert()
    {
        var target = typeof(Something).GetMethod(nameof(Something.WithDateTime))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_count()
    {
        _tested.Count = 1;
        var target = typeof(Something).GetMethod(nameof(Something.WithDateTime))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_count_more_than_one()
    {
        _tested.Count = 2;
        var target = typeof(Something).GetMethod(nameof(Something.WithTwoDateTimes))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_derived_type()
    {
        _tested.ParameterType = typeof(Exception);
        var target = typeof(Something).GetMethod(nameof(Something.WithDerivedType))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_by_ref_parameter()
    {
        var target = typeof(Something).GetMethod(nameof(Something.WithRefDateTime))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_without_parameter_of_type()
    {
        var target = typeof(Something).GetMethod(nameof(Something.WithoutDateTime))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Single(context.Messages);
    }

    [Theory]
    [InlineData(nameof(Something.WithoutDateTime))]
    [InlineData(nameof(Something.WithTwoDateTimes))]
    public void Fails_on_assert_count_mismatch(string methodName)
    {
        _tested.Count = 1;
        var target = typeof(Something).GetMethod(methodName)!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Single(context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.ParameterType = null;
        var target = typeof(Something).GetMethod(nameof(Something.WithDateTime))!;
        var context = new ConventionContext(target);

        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(target, context));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Throws_on_assert_invalid_count(int count)
    {
        _tested.Count = count;
        var target = typeof(Something).GetMethod(nameof(Something.WithDateTime))!;
        var context = new ConventionContext(target);

        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(target, context));
    }

    private class Something
    {
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

        public static void WithDerivedType(InvalidOperationException error)
        {
        }
    }
}
