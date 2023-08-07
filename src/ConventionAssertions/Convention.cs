using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
{
    public static void ForTypes(
        Action<ITypeScanner> scanner,
        Action<IConventionAssert<Type>> assertAction)
    {
        var targets = ConventionTargets.FromTypes(scanner);
        ForTypes(targets, assertAction);
    }

    public static void ForTypes(
        IConventionTargets<Type> targets,
        Action<IConventionAssert<Type>> assertAction)
    {
        GuardAgainst.Null(assertAction);

        var assert = new ConventionAssert<Type>(targets);
        assertAction(assert);
    }

    public static void ForMethods(
        Action<ITypeScanner> typesScanner,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        var targetTypes = ConventionTargets.FromTypes(typesScanner);
        ForMethods(targetTypes, x => x.Where(c => true), assertAction);
    }

    public static void ForMethods(
        Action<ITypeScanner> typesScanner,
        Action<IMethodScanner> scanner,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        var targetTypes = ConventionTargets.FromTypes(typesScanner);
        ForMethods(targetTypes, scanner, assertAction);
    }

    public static void ForMethods(
        IConventionTargets<Type> targetTypes,
        Action<IMethodScanner> scanner,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(targetTypes);
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        var targets = new MethodScanner(targetTypes);
        scanner(targets);
        var assert = new ConventionAssert<MethodInfo>(targets);
        assertAction(assert);
    }

    public static void ForProperties(
        Action<ITypeScanner> typesScanner,
        Action<IConventionAssert<PropertyInfo>> assert)
    {
        var targetTypes = ConventionTargets.FromTypes(typesScanner);
        ForProperties(targetTypes, x => x.Where(c => true), assert);
    }

    public static void ForProperties(
        Action<ITypeScanner> typesScanner,
        Action<IPropertyScanner> scanner,
        Action<IConventionAssert<PropertyInfo>> assert)
    {
        var targetTypes = ConventionTargets.FromTypes(typesScanner);
        ForProperties(targetTypes, scanner, assert);
    }

    public static void ForProperties(
        IConventionTargets<Type> targetTypes,
        Action<IPropertyScanner> scanner,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(targetTypes);
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        var targets = new PropertyScanner(targetTypes);
        scanner(targets);
        var assert = new ConventionAssert<PropertyInfo>(targets);
        assertAction(assert);
    }
}
