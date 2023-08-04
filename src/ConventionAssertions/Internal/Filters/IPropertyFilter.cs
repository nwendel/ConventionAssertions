namespace ConventionAssertions.Internal.Filters;

public interface IPropertyFilter : IMemberFilter
{
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Not sure what else to name it")]
    PropertyInfo Property { get; }
}
