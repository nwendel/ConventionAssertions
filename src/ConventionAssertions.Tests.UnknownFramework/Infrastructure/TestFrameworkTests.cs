using ConventionAssertions.Infrastructure;
using ConventionAssertions.Infrastructure.TestFrameworks;

namespace ConventionAssertions.Tests.UnknownFramework.Infrastructure;

public class TestFrameworkTests
{
    public TestFrameworkTests()
    {
        TestFramework.Reset(Array.Empty<ITestFramework>());
    }

    [Fact]
    public void Can_detect()
    {
        Assert.Same(typeof(UnknownTestFramework), TestFramework.Detected.GetType());
    }

    [Fact]
    public void Can_throw()
    {
        var ex = Assert.Throws<ConventionException>(() => TestFramework.Throw("some-message"));
        Assert.Equal("some-message", ex.Message);
    }
}
