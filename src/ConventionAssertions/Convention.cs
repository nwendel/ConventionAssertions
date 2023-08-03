using System.Reflection;
using ConventionAssertions.Internal;

namespace ConventionAssertions;

public static class Convention
{
    public static void ForTypes(
        Action<ITypeScanner> typeScanner,
        Action<IConventionAssert<Type>> assert)
    {
        var typeSource = ConventionTargets.FromTypes(typeScanner);
        ForTypes(typeSource, assert);
    }

    public static void ForTypes(
        IConventionTargets<Type> targets,
        Action<IConventionAssert<Type>> assert)
    {
        GuardAgainst.Null(assert);

        var typeAssert = new ConventionAssert<Type>(targets);
        assert(typeAssert);
    }

    public static void ForMethods(
        Action<ITypeScanner> typeScanner,
        Action<IMethodScanner> methodScanner,
        Action<IConventionAssert<MethodInfo>> assert)
    {
        var typeSource = ConventionTargets.FromTypes(typeScanner);
        ForMethods(typeSource, methodScanner, assert);
    }

    public static void ForMethods(
        IConventionTargets<Type> targets,
        Action<IMethodScanner> methodScanner,
        Action<IConventionAssert<MethodInfo>> assert)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(methodScanner);
        GuardAgainst.Null(assert);

        var methodSource = new MethodScanner(targets);
        methodScanner(methodSource);
        var methodAssert = new ConventionAssert<MethodInfo>(methodSource);
        assert(methodAssert);
    }

    public static void ForProperties(
        Action<ITypeScanner> typeScanner,
        Action<IPropertyScanner> propertyScanner,
        Action<IConventionAssert<PropertyInfo>> assert)
    {
        var typeSource = ConventionTargets.FromTypes(typeScanner);
        ForProperties(typeSource, propertyScanner, assert);
    }

    public static void ForProperties(
        IConventionTargets<Type> targets,
        Action<IPropertyScanner> propertyScanner,
        Action<IConventionAssert<PropertyInfo>> assert)
    {
        GuardAgainst.Null(targets);
        GuardAgainst.Null(propertyScanner);
        GuardAgainst.Null(assert);

        var propertySource = new PropertyScanner(targets);
        propertyScanner(propertySource);
        var propertyAssert = new ConventionAssert<PropertyInfo>(propertySource);
        assert(propertyAssert);
    }
}
