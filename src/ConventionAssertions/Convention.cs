using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
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

    public static void ForMethods(
        Action<ITypeScanner> typeScanner,
        Action<IMethodScanner> methodScanner,
        Action<IMethodAssert> assert)
    {
        var typeSource = ForTypes(typeScanner);
        ForMethods(typeSource, methodScanner, assert);
    }

    public static void ForMethods(
        ConventionTypeSource typeSource,
        Action<IMethodScanner> methodScanner,
        Action<IMethodAssert> assert)
    {
        GuardAgainst.Null(typeSource);
        GuardAgainst.Null(methodScanner);
        GuardAgainst.Null(assert);

        var methodSource = new MethodScanner(typeSource.Types);
        methodScanner(methodSource);
        var methodAssert = new MethodAssert(methodSource);
        assert(methodAssert);
    }
}
