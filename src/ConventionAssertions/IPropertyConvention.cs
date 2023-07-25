using System.Reflection;

namespace ConventionAssertions;

public interface IPropertyConvention
{
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Not sure what else to name it")]
    void Assert(PropertyInfo property, ConventionContext context);
}
