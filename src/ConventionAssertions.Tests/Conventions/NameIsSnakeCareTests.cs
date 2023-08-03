using ConventionAssertions.Conventions;
using ConventionAssertions.Internal;

namespace ConventionAssertions.Tests.Conventions;

public class NameIsSnakeCareTests
{
    private readonly NameIsSnakeCase _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Can_assert_class()
    {
        _tested.Assert(typeof(snake_case), _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Can_assert_method()
    {
        _tested.Assert(typeof(snake_case).GetMethod(nameof(snake_case.snake_case_method))!, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_pascal_case()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(PascalCase), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_pascal_case_method()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(snake_case).GetMethod(nameof(snake_case.PascalCaseMethod))!, _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_double_underscore()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(double__underscore), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_starts_with_underscore()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(_starts_with_underscore), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_ends_with_underscore()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(ends_with_underscore_), _context));
        Assert.Single(_context.Messages);
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
