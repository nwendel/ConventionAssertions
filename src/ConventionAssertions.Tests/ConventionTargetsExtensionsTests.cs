namespace ConventionAssertions.Tests;

public class ConventionTargetsExtensionsTests
{
    private readonly IConventionTargets<Type> _types = ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<ConventionTargetsExtensionsTests>()
        .Where(t => t.Type == typeof(SampleClass)));

    [Fact]
    public void Methods_throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ((IConventionTargets<Type>)null!).Methods());
        Assert.Equal("self", ex.ParamName);
    }

    [Fact]
    public void Methods_includes_declared_methods()
    {
        var names = _types.Methods().Targets.Select(x => x.Name).ToList();

        Assert.Contains(nameof(SampleClass.Increment), names);
        Assert.Contains(nameof(SampleClass.Create), names);
    }

    [Fact]
    public void Methods_includes_inherited_methods()
    {
        var names = _types.Methods().Targets.Select(x => x.Name).ToList();

        Assert.Contains(nameof(SampleBaseClass.Reset), names);
    }

    [Fact]
    public void Methods_excludes_object_members_and_accessors()
    {
        var names = _types.Methods().Targets.Select(x => x.Name).ToList();

        Assert.DoesNotContain(nameof(ToString), names);
        Assert.DoesNotContain(nameof(GetHashCode), names);
        Assert.DoesNotContain("get_Value", names);
        Assert.DoesNotContain("set_Value", names);
    }

    [Fact]
    public void Methods_can_filter()
    {
        var method = Assert.Single(_types.Methods(m => m.Method.Name == nameof(SampleClass.Increment)).Targets);

        Assert.Equal(nameof(SampleClass.Increment), method.Name);
    }

    [Fact]
    public void Properties_throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ((IConventionTargets<Type>)null!).Properties());
        Assert.Equal("self", ex.ParamName);
    }

    [Fact]
    public void Properties_includes_public_properties()
    {
        var property = Assert.Single(_types.Properties().Targets);

        Assert.Equal(nameof(SampleClass.Value), property.Name);
    }

    [Fact]
    public void Properties_can_filter()
    {
        Assert.Empty(_types.Properties(p => p.Property.Name == "Missing").Targets);
    }

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
