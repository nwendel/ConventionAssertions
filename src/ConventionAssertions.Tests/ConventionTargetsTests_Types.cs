namespace ConventionAssertions.Tests;

public class ConventionTargetsTests_Types
{
    [Fact]
    public void Throws_on_null_types()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ConventionTargets.FromTypes(
            (Func<ITypeScanner, ITypeScannerFilter>)null!));
        Assert.Equal("types", ex.ParamName);
    }

    [Fact]
    public void Can_create_type_source()
    {
        var typeSource = ConventionTargets.FromTypes(s => s
            .FromAssemblyContaining<ConventionTargetsTests_Types>()
            .Where(t => t.Type == typeof(ConventionTargetsTests_Types)));

        var type = Assert.Single(typeSource.Targets);
        Assert.Same(typeof(ConventionTargetsTests_Types), type);
    }

    [Fact]
    public void Can_create_type_source_from_targets()
    {
        var shared = ConventionTargets.FromTypes(s => s
            .FromAssemblyContaining<ConventionTargetsTests_Types>()
            .Where(t => t.IsAssignableTo<IValueObject>()));

        var typeSource = ConventionTargets.FromTypes(s => s
            .FromTargets(shared)
            .Where(t => t.Type == typeof(SampleId)));

        var type = Assert.Single(typeSource.Targets);
        Assert.Same(typeof(SampleId), type);
    }
}
