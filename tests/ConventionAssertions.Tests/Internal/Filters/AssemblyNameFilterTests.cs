using System.Reflection;
using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Tests.Internal.Filters;

public class AssemblyNameFilterTests
{
    [Theory]
    [InlineData("Amni.Appointments")]
    [InlineData("Amni.Appointments.Domain")]
    public void Can_has_name_prefix_true(string assemblyName)
    {
        var tested = new AssemblyNameFilter(new AssemblyName(assemblyName));

        Assert.True(tested.HasNamePrefix("Amni.Appointments"));
    }

    [Theory]
    [InlineData("Amni.AppointmentsLegacy")]
    [InlineData("Amni")]
    [InlineData("Other.Amni.Appointments")]
    [InlineData("amni.appointments")]
    public void Can_has_name_prefix_false(string assemblyName)
    {
        var tested = new AssemblyNameFilter(new AssemblyName(assemblyName));

        Assert.False(tested.HasNamePrefix("Amni.Appointments"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    public void Throws_on_has_name_prefix_null_or_whitespace(string? prefix)
    {
        var tested = new AssemblyNameFilter(new AssemblyName("Amni.Appointments"));

        Assert.Throws<ArgumentNullException>(() => tested.HasNamePrefix(prefix!));
    }

    [Fact]
    public void Can_has_name_segment_true()
    {
        var tested = new AssemblyNameFilter(new AssemblyName("Amni.Appointments.Tests"));

        Assert.True(tested.HasNameSegment("Tests"));
    }

    [Fact]
    public void Can_has_name_segment_false()
    {
        var tested = new AssemblyNameFilter(new AssemblyName("Amni.AppointmentsTests"));

        Assert.False(tested.HasNameSegment("Tests"));
    }

    [Theory]
    [InlineData("Amni.Appointments.Tests")]
    [InlineData("Amni.Appointments.UnitTests")]
    public void Can_has_any_name_segment_true(string assemblyName)
    {
        var tested = new AssemblyNameFilter(new AssemblyName(assemblyName));

        Assert.True(tested.HasAnyNameSegment("Tests", "UnitTests"));
    }

    [Fact]
    public void Can_has_any_name_segment_false()
    {
        var tested = new AssemblyNameFilter(new AssemblyName("Amni.Appointments.Domain"));

        Assert.False(tested.HasAnyNameSegment("Tests", "UnitTests"));
    }

    [Fact]
    public void Throws_on_has_any_name_segment_without_segments()
    {
        var tested = new AssemblyNameFilter(new AssemblyName("Amni.Appointments"));

        Assert.Throws<ArgumentException>(() => tested.HasAnyNameSegment());
    }

    [Fact]
    public void Throws_on_has_any_name_segment_null()
    {
        var tested = new AssemblyNameFilter(new AssemblyName("Amni.Appointments"));

        Assert.Throws<ArgumentNullException>(() => tested.HasAnyNameSegment(null!));
    }
}
