using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class ConventionSource
{
    public static ConventionTypeSource FromTypes(Action<ITypeScanner> typeScanner)
    {
        GuardAgainst.Null(typeScanner);

        var typeSource = new TypeScanner();
        typeScanner(typeSource);
        return typeSource;
    }
}
