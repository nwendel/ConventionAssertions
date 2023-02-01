using ConventionAssertions.Infrastructure.TestFrameworks;

namespace ConventionAssertions.Infrastructure;

internal static class TestFramework
{
    private static IEnumerable<IDetectableTestFramework> _frameworks = new[]
    {
        new XunitTestFramework(),
    };

    private static ITestFramework? _detectedFramework;

    public static ITestFramework Detected
    {
        get
        {
            if (_detectedFramework == null)
            {
                _detectedFramework = _frameworks.SingleOrDefault(x => x.IsAvailable);
                _detectedFramework ??= new UnknownTestFramework();
            }

            return _detectedFramework;
        }
    }

    public static void Reset(IEnumerable<IDetectableTestFramework> frameworks)
    {
        GuardAgainst.Null(frameworks);

        _frameworks = frameworks;
        _detectedFramework = null;
    }

    [DoesNotReturn]
    public static void Throw(string message)
    {
        Detected.Throw(message);
    }
}
