using System.Reflection;

namespace ConventionAssertions.Tests;

public class ConventionTargetsTests_Properties
{
    private readonly IConventionTargets<Type> _types = ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<ConventionTargetsTests_Properties>()
        .Where(t => t.Type == typeof(SampleClass)));

    [Fact]
    public void Throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromProperties(
            (Func<ITypeScanner, ITypeScannerFilter>)null!));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_targets()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromProperties(
            (IConventionTargets<PropertyInfo>)null!,
            p => p));
        Assert.Equal("targets", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_properties_with_targets()
    {
        var targets = ConventionTargets.FromProperties(s => s.FromTargets(_types));

        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromProperties(
            targets,
            null!));
        Assert.Equal("properties", ex.ParamName);
    }

    [Fact]
    public void Includes_declared_properties()
    {
        var names = PropertyNames(ConventionTargets.FromProperties(s => s.FromTargets(_types)));

        Assert.Contains(nameof(SampleClass.Value), names);
        Assert.Contains(nameof(SampleClass.Count), names);
    }

    [Fact]
    public void Includes_inherited_properties()
    {
        var names = PropertyNames(ConventionTargets.FromProperties(s => s.FromTargets(_types)));

        Assert.Contains(nameof(SampleBaseClass.Name), names);
    }

    [Fact]
    public void Can_filter()
    {
        var properties = ConventionTargets.FromProperties(
            s => s.FromTargets(_types),
            p => p.Where(x => x.Property.Name == nameof(SampleClass.Value)));

        var property = Assert.Single(properties.Targets);
        Assert.Equal(nameof(SampleClass.Value), property.Name);
    }

    [Fact]
    public void Can_filter_more_than_once()
    {
        var properties = ConventionTargets.FromProperties(
            s => s.FromTargets(_types),
            p => p
                .Where(x => x.Property.PropertyType == typeof(int))
                .Where(x => x.Property.Name == nameof(SampleClass.Count)));

        var property = Assert.Single(properties.Targets);
        Assert.Equal(nameof(SampleClass.Count), property.Name);
    }

    [Fact]
    public void Can_narrow_property_targets()
    {
        var shared = ConventionTargets.FromProperties(s => s.FromTargets(_types));

        var properties = ConventionTargets.FromProperties(
            shared,
            p => p.Where(x => x.Property.PropertyType == typeof(string)));

        var property = Assert.Single(properties.Targets);
        Assert.Equal(nameof(SampleBaseClass.Name), property.Name);
    }

    private static List<string> PropertyNames(IConventionTargets<PropertyInfo> properties)
        => properties.Targets.Select(x => x.Name).ToList();

    private class SampleBaseClass
    {
        public string Name { get; set; } = string.Empty;
    }

    private class SampleClass : SampleBaseClass
    {
        public int Value { get; set; }

        public int Count { get; set; }
    }
}
