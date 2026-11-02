using System.Reflection;

namespace ConventionAssertions.Tests;

public class ConventionTests_Methods
{
    [Fact]
    public void Throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForMethods(
            null!,
            m => m,
            a => a.Assert<NothingMethodConvention>()));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void Throws_on_null_methods()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Convention.ForMethods(
            s => s.FromAssemblyContaining<ConventionTests_Methods>(),
            null!,
            a => a.Assert<NothingMethodConvention>()));
        Assert.Equal("methods", ex.ParamName);
    }

    [Fact]
    public void Can_scan_and_assert()
    {
        var asserted = new List<MethodInfo>();

        Convention.ForMethods(
            s => s
                .FromAssemblyContaining<ConventionTests_Methods>()
                .Where(t => t.Type == typeof(DummyMethodTargets)),
            a => a.Assert((method, context) => asserted.Add(method)));

        var method = Assert.Single(asserted);
        Assert.Equal(nameof(DummyMethodTargets.SomeMethod), method.Name);
    }

    [Fact]
    public void Can_scan_filter_and_assert()
    {
        var asserted = new List<MethodInfo>();

        Convention.ForMethods(
            s => s
                .FromAssemblyContaining<ConventionTests_Methods>()
                .Where(t => t.Type == typeof(DummyMethodTargets)),
            m => m.Where(x => x.Method.Name == "Missing"),
            a => a.Assert((method, context) => asserted.Add(method), failOnNoTargets: false));

        Assert.Empty(asserted);
    }
}
