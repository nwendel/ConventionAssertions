using ConventionAssertions.Infrastructure;
using ConventionAssertions.Infrastructure.TestFrameworks;
using XunitException = Xunit.Sdk.XunitException;

namespace ConventionAssertions.Tests.XunitFramework.Infrastructure;

public class TestFrameworkTests
{
    [Fact]
    public void Can_detect()
    {
        Assert.Same(typeof(XunitTestFramework), TestFramework.Detected.GetType());
    }

    [Fact]
    public void Can_throw()
    {
        var ex = Assert.Throws<XunitException>(() => TestFramework.Throw("some-message"));
        Assert.Equal("some-message", ex.Message);
    }
}
