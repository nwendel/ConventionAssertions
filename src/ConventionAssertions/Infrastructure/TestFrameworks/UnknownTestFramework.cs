namespace ConventionAssertions.Infrastructure.TestFrameworks;

internal sealed class UnknownTestFramework : ITestFramework
{
    [DoesNotReturn]
    public void Throw(string message)
    {
        throw new ConventionException(message);
    }
}
