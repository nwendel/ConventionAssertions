using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasCustomAttributeTests
{
    private readonly HasCustomAttribute _tested = new();

    [Fact]
    public void Can_assert_type()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var type = typeof(ClassWithTestAttribute);

        var context = new ConventionContext(type);
        _tested.Assert(type, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_type()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var type = typeof(ClassWithoutTestAttribute);

        var context = new ConventionContext(type);
        _tested.Assert(type, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Can_assert_method()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var method = typeof(ClassWithTestAttribute).GetMethod(nameof(ClassWithTestAttribute.MethodWithTestAttribute))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_method()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var method = typeof(ClassWithoutTestAttribute).GetMethod(nameof(ClassWithoutTestAttribute.MethodWithoutTestAttribute))!;

        var context = new ConventionContext(method);
        _tested.Assert(method, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Can_assert_property()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var property = typeof(ClassWithTestAttribute).GetProperty(nameof(ClassWithTestAttribute.PropertyWithTestAttribute))!;

        var context = new ConventionContext(property);
        _tested.Assert(property, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_property()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var property = typeof(ClassWithoutTestAttribute).GetProperty(nameof(ClassWithoutTestAttribute.PropertyWithoutTestAttribute))!;

        var context = new ConventionContext(property);
        _tested.Assert(property, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        var method = typeof(ClassWithTestAttribute).GetMethod(nameof(ClassWithTestAttribute.MethodWithTestAttribute))!;

        var context = new ConventionContext(method);
        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(method, context));
    }

    [Some]
    private class ClassWithTestAttribute
    {
        [Some]
        public required string PropertyWithTestAttribute { get; set; }

        [Some]
        public static void MethodWithTestAttribute()
        {
        }
    }

    private class ClassWithoutTestAttribute
    {
        public required string PropertyWithoutTestAttribute { get; set; }

        public static void MethodWithoutTestAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    private sealed class SomeAttribute : Attribute
    {
    }
}
