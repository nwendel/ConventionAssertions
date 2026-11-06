using ConventionAssertions.Reflection;

namespace ConventionAssertions.Conventions;

public class HasNamespaceSegment : IConfigurableConvention<Type>
{
    public string? Segment { get; set; }

    public SegmentPosition SegmentPosition { get; set; } = SegmentPosition.Anywhere;

    public void Assert(Type target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (Segment == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(Segment)} not specified");
        }

        if (!target.HasNamespaceSegment(Segment, SegmentPosition))
        {
            Fail(context);
        }
    }

    private void Fail(ConventionContext context)
    {
        var position = SegmentPosition switch
        {
            SegmentPosition.Anywhere => "contains a",
            SegmentPosition.First => "starts with the",
            SegmentPosition.Last => "ends with the",
        };

        context.Fail($"must belong to a namespace which {position} segment {Segment}");
    }
}
