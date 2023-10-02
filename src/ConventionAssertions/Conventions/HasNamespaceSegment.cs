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

        var @namespace = target.Namespace;
        if (@namespace == null)
        {
            Fail(target, context);
        }

        var segments = @namespace.Split('.');
        var success = SegmentPosition switch
        {
            SegmentPosition.Anywhere => segments.Contains(Segment),
            SegmentPosition.First => segments.First() == Segment,
            SegmentPosition.Last => segments.Last() == Segment,
        };

        if (!success)
        {
            Fail(target, context);
        }
    }

    [DoesNotReturn]
    private void Fail(Type target, ConventionContext context)
    {
        var position = SegmentPosition switch
        {
            SegmentPosition.Anywhere => "contains a",
            SegmentPosition.First => "starts with the",
            SegmentPosition.Last => "ends with the",
        };

        context.Fail(target, $"must belong to a namespace which {position} segment {Segment}");
    }
}
