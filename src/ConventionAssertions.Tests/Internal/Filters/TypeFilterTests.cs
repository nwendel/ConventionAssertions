using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Tests.Internal.Filters;

public class TypeFilterTests
{
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
}
