using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasNamePrefixTests
{
    private readonly HasNamePrefix _tested = new() { Prefix = "Some" };

    [Fact]
    public void Can_assert_type()
    {
        var target = typeof(SomeClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Can_assert_generic_type()
    {
        var target = typeof(SomeGenericClass<>);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_type()
    {
        var target = typeof(OtherClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Can_assert_method()
    {
        var target = typeof(SomeClass).GetMethod(nameof(SomeClass.SomeMethod))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_method()
    {
        var target = typeof(SomeClass).GetMethod(nameof(SomeClass.OtherMethod))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Can_assert_property()
    {
        var target = typeof(SomeClass).GetProperty(nameof(SomeClass.SomeProperty))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_property()
    {
        var target = typeof(SomeClass).GetProperty(nameof(SomeClass.OtherProperty))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Is_case_sensitive()
    {
        _tested.Prefix = "some";
        var target = typeof(SomeClass);
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.Prefix = null;
        var target = typeof(SomeClass);
        var context = new ConventionContext(target);
        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(target, context));
    }

    private class SomeClass
    {
        public int SomeProperty { get; set; }

        public int OtherProperty { get; set; }

        public static void SomeMethod()
        {
        }

        public static void OtherMethod()
        {
        }
    }

    private class SomeGenericClass<T>
    {
    }

    private class OtherClass
    {
    }
}
