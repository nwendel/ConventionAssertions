using System.Reflection;
using ConventionAssertions.Conventions;
using Xunit.Sdk;

namespace ConventionAssertions.Tests;

public class ConventionTests_ForUsage
{
    private readonly IConventionTargets<Type> _valueObjects = ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<ConventionTests_ForUsage>()
        .Where(t => t.IsAssignableTo<IValueObject>() && t.IsClass));

    [Fact]
    public void Value_objects_are_sealed()
    {
        Convention.For(
            s => s
                .FromAssemblyContaining<ConventionTests_ForUsage>()
                .Where(t => t.IsAssignableTo<IValueObject>() && t.IsClass),
            a => a.Assert<IsAbstractOrSealed>());
    }

    [Fact]
    public void Value_objects_have_no_public_constructors()
    {
        Convention.For(
            _valueObjects,
            a => a.Assert<HasNoPublicConstructors>());
    }

    [Fact]
    public void Value_objects_have_try_parse()
    {
        Convention.For(
            _valueObjects,
            a => a.Assert<HasPublicStaticTryParseMethod>());
    }

    [Fact]
    public void Value_objects_are_in_test_helpers_namespace()
    {
        Convention.For(
            _valueObjects,
            a => a.Assert<HasNamespaceSegment>(x => x.Segment = "TestHelpers"));
    }

    [Fact]
    public void Value_object_properties_have_no_setter()
    {
        Convention.For(
            s => s.FromTargets(_valueObjects).Properties(),
            a => a.Assert<HasNoSetter>());
    }

    [Fact]
    public void Value_object_properties_are_value_types()
    {
        Convention.For(
            _valueObjects.Properties(),
            a => a.Assert(IsValueType));
    }

    [Fact]
    public void Value_object_static_methods_return_value()
    {
        Convention.For(
            s => s.FromTargets(_valueObjects).Methods(m => m.Method.IsStatic),
            a => a.Assert(ReturnsValue));
    }

    [Fact]
    public void Can_refine_shared_targets()
    {
        Convention.For(
            s => s
                .FromTargets(_valueObjects)
                .Where(t => t.Type == typeof(SampleId)),
            a => a.Assert<HasPublicStaticParseMethod>());
    }

    [Fact]
    public void Reports_all_violations()
    {
        var ex = Assert.Throws<XunitException>(() => Convention.For(
            _valueObjects.Methods(),
            a => a.Assert<NameIsSnakeCase>()));

        Assert.StartsWith("Found 2.", ex.Message, StringComparison.Ordinal);
        Assert.Contains("Method ConventionAssertions.Tests.TestHelpers.SampleId.Parse(string) is not snake case.", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    [SuppressConvention(Target = "ConventionAssertions.Tests.TestHelpers.SampleId.Parse(string)", Justification = "For testing")]
    public void Can_suppress_violation()
    {
        Convention.For(
            _valueObjects.Methods(m => m.Method.Name == nameof(SampleId.Parse)),
            a => a.Assert<NameIsSnakeCase>());
    }

    private static void IsValueType(PropertyInfo property, ConventionContext context)
    {
        if (!property.PropertyType.IsValueType)
        {
            context.Fail(property, "must be a value type");
        }
    }

    private static void ReturnsValue(MethodInfo method, ConventionContext context)
    {
        if (method.ReturnType == typeof(void))
        {
            context.Fail(method, "must return a value");
        }
    }
}
