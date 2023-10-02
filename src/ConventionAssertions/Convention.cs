using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
{
    public static void ForTypes(
        Action<ITypeScanner> scanner,
        Action<IConventionAssert<Type>> assertAction)
    {
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        var targets = ConventionTargets.FromTypes(scanner);
        var assert = new ConventionAssert<Type>(targets);
        assertAction(assert);
    }

    public static void ForMethods(
        Action<ITypeScanner> typeScanner,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(typeScanner);
        GuardAgainst.Null(assertAction);

        var targetTypes = ConventionTargets.FromTypes(typeScanner);
        var targets = new MethodScanner(targetTypes);
        var assert = new ConventionAssert<MethodInfo>(targets);

        assertAction(assert);
    }

    public static void ForMethods(
        Action<ITypeScanner> typeScanner,
        Action<IMethodScanner> scanner,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(typeScanner);
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        var targetTypes = ConventionTargets.FromTypes(typeScanner);
        var targets = new MethodScanner(targetTypes);
        scanner(targets);
        var assert = new ConventionAssert<MethodInfo>(targets);

        assertAction(assert);
    }

    public static void ForProperties(
        Action<ITypeScanner> typeScanner,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(typeScanner);
        GuardAgainst.Null(assertAction);

        var targetTypes = ConventionTargets.FromTypes(typeScanner);
        var targets = new PropertyScanner(targetTypes);
        var assert = new ConventionAssert<PropertyInfo>(targets);

        assertAction(assert);
    }

    public static void ForProperties(
        Action<ITypeScanner> typeScanner,
        Action<IPropertyScanner> scanner,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(typeScanner);
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        var targetTypes = ConventionTargets.FromTypes(typeScanner);
        var targets = new PropertyScanner(targetTypes);
        scanner(targets);
        var assert = new ConventionAssert<PropertyInfo>(targets);

        assertAction(assert);
    }
}
