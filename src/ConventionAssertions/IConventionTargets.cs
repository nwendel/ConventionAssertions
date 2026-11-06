namespace ConventionAssertions;

public interface IConventionTargets<T>
    where T : notnull
{
    internal IEnumerable<T> Targets { get; }
}
