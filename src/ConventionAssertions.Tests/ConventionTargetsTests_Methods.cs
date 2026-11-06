using System.Reflection;

namespace ConventionAssertions.Tests;

public class ConventionTargetsTests_Methods
{
    private readonly IConventionTargets<Type> _types = ConventionTargets.FromTypes(
        a => a.FromAssemblyContaining<ConventionTargetsTests_Methods>(),
        t => t.Where(x => x.Type == typeof(SampleClass)));

    [Fact]
    public void Throws_on_null_assemblies()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromMethods(
            (Func<IAssemblyScanner, IAssemblyScannerFilter>)null!));
        Assert.Equal("assemblies", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_type_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromMethods(
            (IConventionTargets<Type>)null!));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void Can_scan_assemblies_types_and_methods()
    {
        var methods = ConventionTargets.FromMethods(
            a => a.FromAssemblyContaining<ConventionTargetsTests_Methods>(),
            t => t.Where(x => x.Type == typeof(SampleClass)),
            m => m.Where(x => x.Method.IsStatic));

        var method = Assert.Single(methods.Targets);
        Assert.Equal(nameof(SampleClass.Create), method.Name);
    }

    [Fact]
    public void Can_scan_assemblies_and_methods_without_type_scan()
    {
        var methods = ConventionTargets.FromMethods(
            a => a.FromAssemblyContaining<ConventionTargetsTests_Methods>(),
            methods: m => m.Where(x => x.Method.Name == nameof(SampleClass.Increment)));

        var method = Assert.Single(methods.Targets);
        Assert.Equal(nameof(SampleClass.Increment), method.Name);
    }

    [Fact]
    public void Throws_on_null_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromMethods(
            (IConventionTargets<MethodInfo>)null!,
            m => m));
        Assert.Equal("targets", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_methods_with_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromMethods(
            new DummyMethodTargets(),
            null!));
        Assert.Equal("methods", ex.ParamName);
    }

    [Fact]
    public void Includes_declared_methods()
    {
        var names = MethodNames(ConventionTargets.FromMethods(_types));

        Assert.Contains(nameof(SampleClass.Increment), names);
        Assert.Contains(nameof(SampleClass.Create), names);
    }

    [Fact]
    public void Includes_inherited_methods()
    {
        var names = MethodNames(ConventionTargets.FromMethods(_types));

        Assert.Contains(nameof(SampleBaseClass.Reset), names);
    }

    [Fact]
    public void Excludes_object_members_and_accessors()
    {
        var names = MethodNames(ConventionTargets.FromMethods(_types));

        Assert.DoesNotContain(nameof(ToString), names);
        Assert.DoesNotContain(nameof(GetHashCode), names);
        Assert.DoesNotContain("get_Value", names);
        Assert.DoesNotContain("set_Value", names);
    }

    [Fact]
    public void Can_filter()
    {
        var methods = ConventionTargets.FromMethods(
            _types,
            m => m.Where(x => x.Method.Name == nameof(SampleClass.Increment)));

        var method = Assert.Single(methods.Targets);
        Assert.Equal(nameof(SampleClass.Increment), method.Name);
    }

    [Fact]
    public void Can_filter_more_than_once()
    {
        var methods = ConventionTargets.FromMethods(
            _types,
            m => m
                .Where(x => x.Method.IsStatic)
                .Where(x => x.Method.Name == nameof(SampleClass.Create)));

        var method = Assert.Single(methods.Targets);
        Assert.Equal(nameof(SampleClass.Create), method.Name);
    }

    [Fact]
    public void Can_narrow_method_targets()
    {
        var shared = ConventionTargets.FromMethods(_types);

        var methods = ConventionTargets.FromMethods(
            shared,
            m => m.Where(x => x.Method.IsStatic));

        var method = Assert.Single(methods.Targets);
        Assert.Equal(nameof(SampleClass.Create), method.Name);
    }

    private static List<string> MethodNames(IConventionTargets<MethodInfo> methods)
        => methods.Targets.Select(x => x.Name).ToList();

    private class SampleBaseClass
    {
        public virtual void Reset()
        {
            // Never called, used for testing
        }
    }

    private class SampleClass : SampleBaseClass
    {
        public int Value { get; set; }

        public static SampleClass Create()
        {
            return new();
        }

        public void Increment()
        {
            Value++;
        }
    }
}
