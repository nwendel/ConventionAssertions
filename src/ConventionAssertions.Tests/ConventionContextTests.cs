namespace ConventionAssertions.Tests;

public class ConventionContextTests
{
    [Fact]
    public void Throws_on_create_null_target()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new ConventionContext(null!));
        Assert.Equal("target", ex.ParamName);
    }

    [Fact]
    public void Throws_on_fail_null_message()
    {
        var tested = new ConventionContext(typeof(object));

        var ex = Assert.Throws<ArgumentNullException>(() => tested.Fail(null!));
        Assert.Equal("message", ex.ParamName);
    }

    [Fact]
    public void Can_fail_type()
    {
        var tested = new ConventionContext(typeof(object));

        tested.Fail("some-message");

        var message = Assert.Single(tested.Messages);
        Assert.Equal("Type object some-message.", message);
    }

    [Fact]
    public void Can_fail_multiple_times()
    {
        var tested = new ConventionContext(typeof(object));

        tested.Fail("first-message");
        tested.Fail("second-message");

        Assert.Collection(
            tested.Messages,
            x => Assert.Equal("Type object first-message.", x),
            x => Assert.Equal("Type object second-message.", x));
    }
}
