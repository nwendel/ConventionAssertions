namespace ConventionAsserts.Infrastructure.TestFrameworks;

internal class UnknownTestFramework : ITestFramework
{
    public bool IsAvailable => true;

    [DoesNotReturn]
    public void Throw(string message)
    {
        throw new ConventionException(message);
    }
}
