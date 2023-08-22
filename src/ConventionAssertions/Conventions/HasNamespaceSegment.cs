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
            context.Fail(target, "wrong namespace");
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
            context.Fail(target, "wrong namespace");
        }
    }
}
