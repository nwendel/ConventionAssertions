using System.Reflection;
using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class HasNoAssemblyReferenceWithNameSegmentTests
{
    private static readonly Assembly TestAssembly = typeof(HasNoAssemblyReferenceWithNameSegmentTests).Assembly;
    private static readonly Assembly LibraryAssembly = typeof(Convention).Assembly;

    private readonly HasNoAssemblyReferenceWithNameSegment _tested = new() { Segment = "Blaappen" };

    [Fact]
    public void Can_assert()
    {
        var context = new ConventionContext(TestAssembly);
        _tested.Assert(TestAssembly, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_direct_reference()
    {
        _tested.Segment = "ConventionAssertions";
        _tested.SegmentPosition = SegmentPosition.First;
        var context = new ConventionContext(TestAssembly);
        _tested.Assert(TestAssembly, context);

        var message = Assert.Single(context.Messages);
        Assert.Equal(
            "Assembly ConventionAssertions.Tests must not reference ConventionAssertions (ConventionAssertions.Tests -> ConventionAssertions).",
            message);
    }

    [Fact]
    public void Fails_on_assert_indirect_reference()
    {
        _tested.Segment = "abstractions";
        var context = new ConventionContext(TestAssembly);
        _tested.Assert(TestAssembly, context);

        var message = Assert.Single(context.Messages);
        Assert.Equal(
            "Assembly ConventionAssertions.Tests must not reference xunit.abstractions (ConventionAssertions.Tests -> xunit.core -> xunit.abstractions).",
            message);
    }

    [Fact]
    public void Fails_on_assert_indirect_framework_reference()
    {
        // Only ConventionAssertions references System.Diagnostics.StackTrace, the test assembly doesn't
        _tested.Segment = "StackTrace";
        var context = new ConventionContext(TestAssembly);
        _tested.Assert(TestAssembly, context);

        var message = Assert.Single(context.Messages);
        Assert.Equal(
            "Assembly ConventionAssertions.Tests must not reference System.Diagnostics.StackTrace (ConventionAssertions.Tests -> ConventionAssertions -> System.Diagnostics.StackTrace).",
            message);
    }

    [Fact]
    public void Can_assert_does_not_descend_into_framework_assemblies()
    {
        // System.Text.Json is only referenced by Microsoft.Extensions.DependencyModel, which is not descended into
        _tested.Segment = "Json";
        var context = new ConventionContext(TestAssembly);
        _tested.Assert(TestAssembly, context);

        Assert.Empty(context.Messages);
    }

    [Theory]
    [InlineData(SegmentPosition.Anywhere, "Extensions", 1)]
    [InlineData(SegmentPosition.First, "Extensions", 0)]
    [InlineData(SegmentPosition.Last, "Extensions", 0)]
    [InlineData(SegmentPosition.First, "Microsoft", 1)]
    [InlineData(SegmentPosition.Last, "DependencyModel", 1)]
    [InlineData(SegmentPosition.Last, "Runtime", 1)]
    public void Can_assert_segment_position(SegmentPosition segmentPosition, string segment, int expectedFailures)
    {
        _tested.Segment = segment;
        _tested.SegmentPosition = segmentPosition;
        var context = new ConventionContext(LibraryAssembly);
        _tested.Assert(LibraryAssembly, context);

        Assert.Equal(expectedFailures, context.Messages.Count());
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.Segment = null;
        var context = new ConventionContext(TestAssembly);

        Assert.Throws<ConventionNotConfiguredException>(() => _tested.Assert(TestAssembly, context));
    }
}
