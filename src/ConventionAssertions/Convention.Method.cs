using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static partial class Convention
{
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
