using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
{
    public static void ForTypes(
        Action<ITypeScanner> typeScanner,
        Action<ITypeAssert> assert)
    {
        var typeSource = ConventionSource.FromTypes(typeScanner);
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
        var typeSource = ConventionSource.FromTypes(typeScanner);
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

    public static void ForProperties(
        Action<ITypeScanner> typeScanner,
        Action<IPropertyScanner> propertyScanner,
        Action<IPropertyAssert> assert)
    {
        var typeSource = ConventionSource.FromTypes(typeScanner);
        ForProperties(typeSource, propertyScanner, assert);
    }

    public static void ForProperties(
        ConventionTypeSource typeSource,
        Action<IPropertyScanner> propertyScanner,
        Action<IPropertyAssert> assert)
    {
        GuardAgainst.Null(typeSource);
        GuardAgainst.Null(propertyScanner);
        GuardAgainst.Null(assert);

        var propertySource = new PropertyScanner(typeSource.Types);
        propertyScanner(propertySource);
        var propertyAssert = new PropertyAssert(propertySource);
        assert(propertyAssert);
    }
}
