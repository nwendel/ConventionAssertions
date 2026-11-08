using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class NameIsSnakeCaseTests
{
    private readonly NameIsSnakeCase _tested = new();

    [Fact]
    public void Can_assert_class()
    {
        var target = typeof(snake_case);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_method()
    {
        var target = typeof(snake_case).GetMethod(nameof(snake_case.snake_case_method))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_pascal_case()
    {
        var target = typeof(PascalCase);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_pascal_case_method()
    {
        var target = typeof(snake_case).GetMethod(nameof(snake_case.PascalCaseMethod))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_double_underscore()
    {
        var target = typeof(double__underscore);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_starts_with_underscore()
    {
        var target = typeof(_starts_with_underscore);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_ends_with_underscore()
    {
        var target = typeof(ends_with_underscore_);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Needed for test")]
    private class snake_case
    {
        public static void snake_case_method()
        {
        }

        public static void PascalCaseMethod()
        {
        }
    }

    private class PascalCase
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Needed for test")]
    private class double__underscore
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Needed for test")]
    private class _starts_with_underscore
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Needed for test")]
    private class ends_with_underscore_
    {
    }
}
