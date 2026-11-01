using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasNoSetterTests
{
    private readonly HasNoSetter _tested = new();

    [Fact]
    public void Can_assert()
    {
        var property = typeof(Something).GetProperty(nameof(Something.NoSetter))!;
        var context = new ConventionContext(property);
        _tested.Assert(property, context);
        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_public_setter()
    {
        var property = typeof(Something).GetProperty(nameof(Something.PublicSetter))!;
        var context = new ConventionContext(property);
        _tested.Assert(property, context);
        Assert.Single(context.Messages);
    }

    private class Something
    {
        public int NoSetter { get; }

        public int PublicSetter { get; set; }
    }
}
