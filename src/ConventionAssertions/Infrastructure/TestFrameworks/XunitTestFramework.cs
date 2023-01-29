using System.Reflection;

namespace ConventionAssertions.Infrastructure.TestFrameworks;

internal sealed class XunitTestFramework : ITestFramework
{
    private readonly Assembly? _xunitAssembly;
    private readonly Type? _exceptionType;

    public XunitTestFramework()
    {
        _xunitAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.GetName().Name == "xunit.assert")
            .OrderByDescending(x => x.GetName().Version)
            .FirstOrDefault();

        _exceptionType = _xunitAssembly?.GetType("Xunit.Sdk.XunitException");
    }

    public bool IsAvailable => _xunitAssembly != null;

    [DoesNotReturn]
    public void Throw(string message)
    {
        if (_xunitAssembly == null)
        {
            throw new InvalidOperationException("TestFramework assembly not found");
        }

        if (_exceptionType == null)
        {
            throw new InvalidOperationException("TestFramework Exception type not found");
        }

        var ex = (Exception?)Activator.CreateInstance(_exceptionType, message);
        if (ex == null)
        {
            throw new InvalidOperationException("Unable to create exception");
        }

        throw ex;
    }
}
