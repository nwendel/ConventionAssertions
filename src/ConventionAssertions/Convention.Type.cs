using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static partial class Convention
{
    public static ConventionTypeSource ForTypes(Action<ITypeScanner> typeScanner)
    {
        GuardAgainst.Null(typeScanner);

        var typeSource = new TypeScanner();
        typeScanner(typeSource);
        return typeSource;
    }

    public static void ForTypes(
        Action<ITypeScanner> typeScanner,
        Action<ITypeAssert> assert)
    {
        var typeSource = ForTypes(typeScanner);
        ForTypes(typeSource, assert);
    }

    public static void ForTypes(
        ConventionTypeSource typeSource,
        Action<ITypeAssert> assert)
    {
        GuardAgainst.Null(assert);

        var typeAssert = new TypeAssert(typeSource);
        assert(typeAssert);
    }
}
