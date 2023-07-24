using ConventionAssertions.Internal.Filters;
using ConventionAssertions.Tests.TestHelpers;

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
}
