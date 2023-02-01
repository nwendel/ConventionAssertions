namespace ConventionAssertions.Infrastructure;

internal interface ITestFramework
{
    [DoesNotReturn]
    void Throw(string message);
}
