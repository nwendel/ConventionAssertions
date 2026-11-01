using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
{
    public static void For<TTarget>(
        Func<ITypeScanner, IConventionTargets<TTarget>> scanner,
        Action<IConventionAssert<TTarget>> assertAction)
        where TTarget : MemberInfo
    {
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        var targets = scanner(new TypeScanner());
        For(targets, assertAction);
    }

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

    [Obsolete("Use Convention.For(s => s.From...(), a => a.Assert(...)) instead.")]
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

    [Obsolete("Use Convention.For(s => s.From...().Methods(...), a => a.Assert(...)) instead.")]
    public static void ForMethods(
        Action<ITypeScanner> typeScanner,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(typeScanner);
        GuardAgainst.Null(assertAction);

        var targetTypes = ConventionTargets.FromTypes(typeScanner);
        var targets = new MethodScanner(targetTypes.Targets);
        var assert = new ConventionAssert<MethodInfo>(targets);

        assertAction(assert);
    }

    [Obsolete("Use Convention.For(s => s.From...().Methods(...), a => a.Assert(...)) instead.")]
    public static void ForMethods(
        Action<ITypeScanner> typeScanner,
        Action<IMethodScanner> scanner,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(typeScanner);
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        var targetTypes = ConventionTargets.FromTypes(typeScanner);
        var targets = new MethodScanner(targetTypes.Targets);
        scanner(targets);
        var assert = new ConventionAssert<MethodInfo>(targets);

        assertAction(assert);
    }

    [Obsolete("Use Convention.For(s => s.From...().Methods(...), a => a.Assert(...)) instead.")]
    public static void ForMethods(
        IConventionTargets<MethodInfo> targets,
        Action<IConventionAssert<MethodInfo>> assertAction)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(assertAction);

        var assert = new ConventionAssert<MethodInfo>(targets);

        assertAction(assert);
    }

    [Obsolete("Use Convention.For(s => s.From...().Properties(...), a => a.Assert(...)) instead.")]
    public static void ForProperties(
        Action<ITypeScanner> typeScanner,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(typeScanner);
        GuardAgainst.Null(assertAction);

        var targetTypes = ConventionTargets.FromTypes(typeScanner);
        var targets = new PropertyScanner(targetTypes.Targets);
        var assert = new ConventionAssert<PropertyInfo>(targets);

        assertAction(assert);
    }

    [Obsolete("Use Convention.For(s => s.From...().Properties(...), a => a.Assert(...)) instead.")]
    public static void ForProperties(
        Action<ITypeScanner> typeScanner,
        Action<IPropertyScanner> scanner,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(typeScanner);
        GuardAgainst.Null(scanner);
        GuardAgainst.Null(assertAction);

        var targetTypes = ConventionTargets.FromTypes(typeScanner);
        var targets = new PropertyScanner(targetTypes.Targets);
        scanner(targets);
        var assert = new ConventionAssert<PropertyInfo>(targets);

        assertAction(assert);
    }

    [Obsolete("Use Convention.For(s => s.From...().Properties(...), a => a.Assert(...)) instead.")]
    public static void ForProperties(
        IConventionTargets<PropertyInfo> targets,
        Action<IConventionAssert<PropertyInfo>> assertAction)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(assertAction);

        var assert = new ConventionAssert<PropertyInfo>(targets);

        assertAction(assert);
    }
}
