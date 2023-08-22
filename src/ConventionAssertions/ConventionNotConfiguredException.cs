namespace ConventionAssertions;

public class ConventionNotConfiguredException : Exception
{
    public ConventionNotConfiguredException(string message)
        : base(message)
    {
    }
}
