namespace ConventionAssertions.Infrastructure;

internal interface IDetectableTestFramework : ITestFramework
{
    bool IsAvailable { get; }
}
