namespace ConventionAsserts.Infrastructure.TestFrameworks;

internal sealed class UnknownTestFramework : ITestFramework
{
    public bool IsAvailable => true;

    [DoesNotReturn]
    public void Throw(string message)
    {
        throw new ConventionException(message);
    }
}
