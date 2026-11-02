namespace ConventionAssertions.Tests;

public class ConventionTargetsTests_Members
{
    private readonly IConventionTargets<Type> _types = ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<ConventionTargetsTests_Members>()
        .Where(t => t.Type == typeof(SampleClass)));

    [Fact]
    public void FromMethods_throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromMethods(
            (Func<ITypeScanner, ITypeScannerFilter>)null!));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void FromMethods_includes_declared_methods()
    {
        var names = MethodNames(ConventionTargets.FromMethods(s => s.FromTargets(_types)));

        Assert.Contains(nameof(SampleClass.Increment), names);
        Assert.Contains(nameof(SampleClass.Create), names);
    }

    [Fact]
    public void FromMethods_includes_inherited_methods()
    {
        var names = MethodNames(ConventionTargets.FromMethods(s => s.FromTargets(_types)));

        Assert.Contains(nameof(SampleBaseClass.Reset), names);
    }

    [Fact]
    public void FromMethods_excludes_object_members_and_accessors()
    {
        var names = MethodNames(ConventionTargets.FromMethods(s => s.FromTargets(_types)));

        Assert.DoesNotContain(nameof(ToString), names);
        Assert.DoesNotContain(nameof(GetHashCode), names);
        Assert.DoesNotContain("get_Value", names);
        Assert.DoesNotContain("set_Value", names);
    }

    [Fact]
    public void FromMethods_can_filter()
    {
        var methods = ConventionTargets.FromMethods(
            s => s.FromTargets(_types),
            m => m.Where(x => x.Method.Name == nameof(SampleClass.Increment)));

        var method = Assert.Single(methods.Targets);
        Assert.Equal(nameof(SampleClass.Increment), method.Name);
    }

    [Fact]
    public void FromMethods_can_filter_more_than_once()
    {
        var methods = ConventionTargets.FromMethods(
            s => s.FromTargets(_types),
            m => m
                .Where(x => x.Method.IsStatic)
                .Where(x => x.Method.Name == nameof(SampleClass.Create)));

        var method = Assert.Single(methods.Targets);
        Assert.Equal(nameof(SampleClass.Create), method.Name);
    }

    [Fact]
    public void FromMethods_throws_on_null_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromMethods(
            (IConventionTargets<System.Reflection.MethodInfo>)null!,
            m => m));
        Assert.Equal("targets", ex.ParamName);
    }

    [Fact]
    public void FromMethods_throws_on_null_methods_with_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromMethods(
            new DummyMethodTargets(),
            null!));
        Assert.Equal("methods", ex.ParamName);
    }

    [Fact]
    public void FromMethods_can_narrow_method_targets()
    {
        var shared = ConventionTargets.FromMethods(s => s.FromTargets(_types));

        var methods = ConventionTargets.FromMethods(
            shared,
            m => m.Where(x => x.Method.IsStatic));

        var method = Assert.Single(methods.Targets);
        Assert.Equal(nameof(SampleClass.Create), method.Name);
    }

    [Fact]
    public void FromProperties_throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromProperties(
            (Func<ITypeScanner, ITypeScannerFilter>)null!));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void FromProperties_includes_public_properties()
    {
        var properties = ConventionTargets.FromProperties(s => s.FromTargets(_types));

        var property = Assert.Single(properties.Targets);
        Assert.Equal(nameof(SampleClass.Value), property.Name);
    }

    [Fact]
    public void FromProperties_can_filter()
    {
        var properties = ConventionTargets.FromProperties(
            s => s.FromTargets(_types),
            p => p.Where(x => x.Property.Name == "Missing"));

        Assert.Empty(properties.Targets);
    }

    [Fact]
    public void FromProperties_can_narrow_property_targets()
    {
        var shared = ConventionTargets.FromProperties(s => s.FromTargets(_types));

        var properties = ConventionTargets.FromProperties(
            shared,
            p => p.Where(x => x.Property.Name == nameof(SampleClass.Value)));

        var property = Assert.Single(properties.Targets);
        Assert.Equal(nameof(SampleClass.Value), property.Name);
    }

    private static List<string> MethodNames(IConventionTargets<System.Reflection.MethodInfo> methods)
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
