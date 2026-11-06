using ConventionAssertions.Conventions;
using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Tests.Internal.Filters;

public class TypeFilterTests
{
    private interface ISomething
    {
    }

    [Fact]
    public void Can_is_assignable_to_open_generic_class_true()
    {
        var tested = new TypeFilter(typeof(GenericThree<>));

        Assert.True(tested.IsAssignableTo(typeof(GenericOne<>)));
    }

    [Fact]
    public void Can_is_assignable_to_open_generic_class_false()
    {
        var tested = new TypeFilter(typeof(object));

        Assert.False(tested.IsAssignableTo(typeof(GenericOne<>)));
    }

    [Fact]
    public void Can_is_assignable_to_open_generic_interface_true()
    {
        var tested = new TypeFilter(typeof(GenericThree<>));

        Assert.True(tested.IsAssignableTo(typeof(IGeneric<>)));
    }

    [Fact]
    public void Can_is_assignable_to_open_generic_interface_false()
    {
        var tested = new TypeFilter(typeof(object));

        Assert.False(tested.IsAssignableTo(typeof(IGeneric<>)));
    }

    [Theory]
    [InlineData("ConventionAssertions")]
    [InlineData("Tests")]
    [InlineData("TestHelpers")]
    public void Can_has_namespace_segment_true(string segment)
    {
        var tested = new TypeFilter(typeof(PublicClass));

        Assert.True(tested.HasNamespaceSegment(segment));
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("testhelpers")]
    [InlineData("ConventionAssertions.Tests")]
    public void Can_has_namespace_segment_false(string segment)
    {
        var tested = new TypeFilter(typeof(PublicClass));

        Assert.False(tested.HasNamespaceSegment(segment));
    }

    [Theory]
    [InlineData("ConventionAssertions", SegmentPosition.First, true)]
    [InlineData("ConventionAssertions", SegmentPosition.Last, false)]
    [InlineData("TestHelpers", SegmentPosition.Last, true)]
    [InlineData("TestHelpers", SegmentPosition.First, false)]
    [InlineData("Tests", SegmentPosition.Anywhere, true)]
    [InlineData("Tests", SegmentPosition.First, false)]
    public void Can_has_namespace_segment_position(string segment, SegmentPosition segmentPosition, bool expected)
    {
        var tested = new TypeFilter(typeof(PublicClass));

        Assert.Equal(expected, tested.HasNamespaceSegment(segment, segmentPosition));
    }

    [Fact]
    public void Can_has_namespace_segment_global_namespace_false()
    {
        // Anonymous types are in the global namespace
        var tested = new TypeFilter(new { Value = 1 }.GetType());

        Assert.False(tested.HasNamespaceSegment("ConventionAssertions"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    public void Throws_on_has_namespace_segment_null_or_whitespace(string? segment)
    {
        var tested = new TypeFilter(typeof(PublicClass));

        Assert.Throws<ArgumentNullException>(() => tested.HasNamespaceSegment(segment!));
    }

    [Fact]
    public void Can_is_static_true()
    {
        var tested = new TypeFilter(typeof(StaticClass));

        Assert.True(tested.IsStatic);
    }

    [Theory]
    [InlineData(typeof(SealedClass))]
    [InlineData(typeof(AbstractClass))]
    [InlineData(typeof(ISomething))]
    public void Can_is_static_false(Type type)
    {
        var tested = new TypeFilter(type);

        Assert.False(tested.IsStatic);
    }

    [Theory]
    [InlineData(typeof(AbstractClass), true)]
    [InlineData(typeof(ISomething), true)]
    [InlineData(typeof(StaticClass), false)]
    [InlineData(typeof(SealedClass), false)]
    public void Can_is_abstract(Type type, bool expected)
    {
        var tested = new TypeFilter(type);

        Assert.Equal(expected, tested.IsAbstract);
    }

    [Theory]
    [InlineData(typeof(SealedClass), true)]
    [InlineData(typeof(StaticClass), false)]
    [InlineData(typeof(AbstractClass), false)]
    [InlineData(typeof(ISomething), false)]
    public void Can_is_concrete(Type type, bool expected)
    {
        var tested = new TypeFilter(type);

        Assert.Equal(expected, tested.IsConcrete);
    }

    [Fact]
    public void Can_is_visible_true()
    {
        var tested = new TypeFilter(typeof(PublicClass));

        Assert.True(tested.IsVisible);
    }

    [Fact]
    public void Can_is_visible_nested_true()
    {
        var tested = new TypeFilter(PublicDeclaringClass.NestedPublicType);

        Assert.True(tested.IsVisible);
    }

    [Fact]
    public void Can_is_visible_false()
    {
        var tested = new TypeFilter(typeof(InternalClass));

        Assert.False(tested.IsVisible);
    }

    [Fact]
    public void Can_is_visible_nested_in_internal_false()
    {
        var tested = new TypeFilter(InternalDeclaringClass.NestedPublicType);

        Assert.False(tested.IsVisible);
    }

    [Fact]
    public void Can_is_visible_nested_non_public_false()
    {
        var tested = new TypeFilter(PublicDeclaringClass.NestedPrivateType);

        Assert.False(tested.IsVisible);
    }

    private static class StaticClass
    {
    }

    private sealed class SealedClass
    {
    }

    private abstract class AbstractClass
    {
    }
}
