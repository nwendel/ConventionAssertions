using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static partial class Convention
{
    public static void ForProperties(
        Action<ITypeScanner> typeScanner,
        Action<IPropertyScanner> propertyScanner,
        Action<IPropertyAssert> assert)
    {
        var typeSource = ForTypes(typeScanner);
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
