using System.Reflection;

namespace ConventionAssertions.Infrastructure.TestFrameworks;

internal sealed class XunitTestFramework : IDetectableTestFramework
{
    private readonly Assembly? _assembly;
    private readonly Type? _exceptionType;

    public XunitTestFramework()
    {
        _assembly = AppDomain.CurrentDomain.GetAssemblies()
            .SingleOrDefault(x => x.GetName().Name == "xunit.assert");

        _exceptionType = _assembly?.GetType("Xunit.Sdk.XunitException");
    }

    public bool IsAvailable =>
        _assembly != null &&
        _exceptionType != null;

    [DoesNotReturn]
    public void Throw(string message)
    {
        if (_assembly == null)
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
