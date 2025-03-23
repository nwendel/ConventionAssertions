using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasCustomAttributeTests
{
    private readonly HasCustomAttribute _tested = new();
    private readonly ConventionContext _context = new();

    [Fact]
    public void Assert_Type_With_CustomAttribute_Should_Pass()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var type = typeof(ClassWithTestAttribute);

        _tested.Assert(type, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Assert_Type_Without_CustomAttribute_Should_Fail()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var type = typeof(ClassWithoutTestAttribute);

        Assert.Throws<ConventionFailedException>(() => _tested.Assert(type, _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Assert_Method_With_CustomAttribute_Should_Pass()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var method = typeof(ClassWithTestAttribute).GetMethod(nameof(ClassWithTestAttribute.MethodWithTestAttribute))!;

        _tested.Assert(method, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Assert_Method_Without_CustomAttribute_Should_Fail()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var method = typeof(ClassWithoutTestAttribute).GetMethod(nameof(ClassWithoutTestAttribute.MethodWithoutTestAttribute))!;

        Assert.Throws<ConventionFailedException>(() => _tested.Assert(method, _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void Assert_Property_With_CustomAttribute_Should_Pass()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var property = typeof(ClassWithTestAttribute).GetProperty(nameof(ClassWithTestAttribute.PropertyWithTestAttribute))!;

        _tested.Assert(property, _context);

        Assert.Empty(_context.Messages);
    }

    [Fact]
    public void Assert_Property_Without_CustomAttribute_Should_Fail()
    {
        _tested.CustomAttribute = typeof(SomeAttribute);
        var property = typeof(ClassWithoutTestAttribute).GetProperty(nameof(ClassWithoutTestAttribute.PropertyWithoutTestAttribute))!;

        Assert.Throws<ConventionFailedException>(() => _tested.Assert(property, _context));
        Assert.Single(_context.Messages);
    }

    [Fact]
    public void HasCustomAttributeCore_Throws_When_CustomAttribute_Not_Specified()
    {
        var method = typeof(ClassWithTestAttribute).GetMethod(nameof(ClassWithTestAttribute.MethodWithTestAttribute))!;

        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(method, _context));
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
