using ConventionAssertions.Infrastructure.TestFrameworks;

namespace ConventionAssertions.Infrastructure;

internal static class TestFramework
{
    private static readonly ITestFramework[] _frameworks = new[]
    {
        new XunitTestFramework(),
    };

    private static ITestFramework? _detectedFramework;

    private static ITestFramework Detected
    {
        get
        {
            if (_detectedFramework == null)
            {
                // TODO: Is Single correct here?
                _detectedFramework = _frameworks.SingleOrDefault(x => x.IsAvailable);
                _detectedFramework ??= new UnknownTestFramework();
            }

            return _detectedFramework;
        }
    }

    [DoesNotReturn]
    public static void Throw(string message)
    {
        Detected.Throw(message);
    }
}
