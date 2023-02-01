using System.Reflection;
using ConventionAssertions.Infrastructure.TestFrameworks;

namespace ConventionAssertions.Tests.Infrastructure.TestFrameworks;
public class XunitTestFrameworkTests
{
    private readonly XunitTestFramework _tested = new();

    [Fact]
    public void Can_is_available_null_assembly()
    {
        SetAssembly(null);

        Assert.False(_tested.IsAvailable);
    }

    [Fact]
    public void Can_is_available_null_exception_type()
    {
        SetExceptionType(null);

        Assert.False(_tested.IsAvailable);
    }

    [Fact]
    public void Throws_on_throw_no_assembly()
    {
        SetAssembly(null);

        Assert.Throws<InvalidOperationException>(() => _tested.Throw("some-message"));
    }

    [Fact]
    public void Throws_on_throw_no_exception_type()
    {
        SetExceptionType(null);

        Assert.Throws<InvalidOperationException>(() => _tested.Throw("some-message"));
    }

    private void SetAssembly(Assembly? assembly)
    {
        var fieldInfo = typeof(XunitTestFramework).GetField("_assembly", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo == null)
        {
            throw new InvalidOperationException();
        }

        fieldInfo.SetValue(_tested, assembly);
        SetExceptionType(null);
    }

    private void SetExceptionType(Assembly? assembly)
    {
        var fieldInfo = typeof(XunitTestFramework).GetField("_exceptionType", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo == null)
        {
            throw new InvalidOperationException();
        }

        fieldInfo.SetValue(_tested, assembly);
    }
}
