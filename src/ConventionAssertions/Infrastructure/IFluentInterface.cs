using System.ComponentModel;

namespace ConventionAssertions.Infrastructure;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IFluentInterface
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Must match object.GetType()")]
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Must match object.GetType()")]
    Type GetType();

    [EditorBrowsable(EditorBrowsableState.Never)]
    int GetHashCode();

    [EditorBrowsable(EditorBrowsableState.Never)]
    string? ToString();

    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Equals(object obj);
}
