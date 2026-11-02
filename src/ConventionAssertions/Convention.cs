using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
{
    public static void For<TTarget>(
        IConventionTargets<TTarget> targets,
        Action<IConventionAssert<TTarget>> assertAction)
        where TTarget : MemberInfo
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(assertAction);

        var assert = new ConventionAssert<TTarget>(targets);
        assertAction(assert);
    }

    public static void ForTypes(
        Func<ITypeScanner, ITypeScannerFilter> types,
        Action<IConventionAssert<Type>> assertAction)
    {
        GuardAgainst.Null(types);
        GuardAgainst.Null(assertAction);

        var targets = ConventionTargets.FromTypes(types);
        For(targets, assertAction);
    }

    public static void ForMethods(
        Func<ITypeScanner, ITypeScannerFilter> types,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(types);
        GuardAgainst.Null(assertAction);

        var targets = ConventionTargets.FromMethods(types);
        For(targets, assertAction);
    }

    public static void ForMethods(
        Func<ITypeScanner, ITypeScannerFilter> types,
        Func<IMethodScanner, IMethodScanner> methods,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(types);
        GuardAgainst.Null(methods);
        GuardAgainst.Null(assertAction);

        var targets = ConventionTargets.FromMethods(types, methods);
        For(targets, assertAction);
    }

    public static void ForProperties(
        Func<ITypeScanner, ITypeScannerFilter> types,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(types);
        GuardAgainst.Null(assertAction);

        var targets = ConventionTargets.FromProperties(types);
        For(targets, assertAction);
    }

    public static void ForProperties(
        Func<ITypeScanner, ITypeScannerFilter> types,
        Func<IPropertyScanner, IPropertyScanner> properties,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(types);
        GuardAgainst.Null(properties);
        GuardAgainst.Null(assertAction);

        var targets = ConventionTargets.FromProperties(types, properties);
        For(targets, assertAction);
    }

    [Obsolete("Use Convention.ForTypes(s => s.From...(), a => a.Assert(...)) instead.")]
    public static void For(
        Func<ITypeScanner, ITypeScannerFilter> scanner,
        Action<IConventionAssert<Type>> assertAction)
    {
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        ForTypes(scanner, assertAction);
    }

    [Obsolete("Use Convention.For(targets, a => a.Assert(...)) instead.")]
    public static void ForMethods(
        IConventionTargets<MethodInfo> targets,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(assertAction);

        For(targets, assertAction);
    }

    [Obsolete("Use Convention.For(targets, a => a.Assert(...)) instead.")]
    public static void ForProperties(
        IConventionTargets<PropertyInfo> targets,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(assertAction);

        For(targets, assertAction);
    }
}
