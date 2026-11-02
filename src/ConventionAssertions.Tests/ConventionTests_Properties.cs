using System.Reflection;

namespace ConventionAssertions.Tests;

public class ConventionTests_Properties
{
    [Fact]
    public void Throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForProperties(
            null!,
            p => p,
            a => a.Assert((property, context) => { })));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_properties()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForProperties(
            s => s.FromAssemblyContaining<ConventionTests_Properties>(),
            null!,
            a => a.Assert((property, context) => { })));
        Assert.Equal("properties", ex.ParamName);
    }

    [Fact]
    public void Can_scan_and_assert()
    {
        var asserted = new List<PropertyInfo>();

        Convention.ForProperties(
            s => s
                .FromAssemblyContaining<ConventionTests_Properties>()
                .Where(t => t.Type == typeof(SampleId)),
            a => a.Assert((property, context) => asserted.Add(property)));

        var property = Assert.Single(asserted);
        Assert.Equal(nameof(SampleId.Value), property.Name);
    }

    [Fact]
    public void Can_scan_filter_and_assert()
    {
        var asserted = new List<PropertyInfo>();

        Convention.ForProperties(
            s => s
                .FromAssemblyContaining<ConventionTests_Properties>()
                .Where(t => t.Type == typeof(SampleId)),
            p => p.Where(x => x.Property.Name == nameof(SampleId.Value)),
            a => a.Assert((property, context) => asserted.Add(property)));

        var property = Assert.Single(asserted);
        Assert.Equal(nameof(SampleId.Value), property.Name);
    }
}
