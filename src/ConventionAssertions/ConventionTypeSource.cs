namespace ConventionAssertions;

public abstract class ConventionTypeSource
{
    // TODO: How to design this to avoid the bang operator?
    public IEnumerable<Type> Types { get; protected set; } = null!;
}
