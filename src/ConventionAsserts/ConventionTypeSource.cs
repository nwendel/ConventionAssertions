namespace ConventionAsserts;

public abstract class ConventionTypeSource
{
    public IEnumerable<Type> Types { get; protected set; } = new List<Type>();
}
