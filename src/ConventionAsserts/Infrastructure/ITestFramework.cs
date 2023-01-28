namespace ConventionAsserts.Infrastructure;

internal interface ITestFramework
{
    bool IsAvailable { get; }

    [DoesNotReturn]
    void Throw(string message);
}
