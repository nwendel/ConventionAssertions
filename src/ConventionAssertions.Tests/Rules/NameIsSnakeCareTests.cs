using ConventionAssertions.Internal;
using ConventionAssertions.Rules;

namespace ConventionAssertions.Tests.Rules;

public class NameIsSnakeCareTests
{
    private readonly NameIsSnakeCase _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Can_assert()
    {
        _tested.Assert(typeof(snake_case), _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_pascal_case()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(PascalCase), _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Throws_on_assert_double_undescore()
    {
        Assert.Throws<ConventionFailedException>(() => _tested.Assert(typeof(double__underscore), _context));
        Assert.Single(_context.Messages);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Needed for test")]
    private class snake_case
    {
    }

    private class PascalCase
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Needed for test")]
    private class double__underscore
    {
    }
}
