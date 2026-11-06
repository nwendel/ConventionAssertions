using ConventionAssertions.Conventions;

namespace ConventionAssertions.Reflection;

internal static class TypeNamespaceSegmentExtensions
{
    // Segments are matched whole and case-sensitively, a type in the global namespace never matches
    public static bool HasNamespaceSegment(this Type self, string segment, SegmentPosition segmentPosition)
    {
        GuardAgainst.Null(self);
        GuardAgainst.NullOrWhiteSpace(segment);

        var @namespace = self.Namespace;
        if (@namespace == null)
        {
            return false;
        }

        var segments = @namespace.Split('.');
        return segmentPosition switch
        {
            SegmentPosition.Anywhere => segments.Contains(segment),
            SegmentPosition.First => segments.First() == segment,
            SegmentPosition.Last => segments.Last() == segment,
            _ => throw new ArgumentOutOfRangeException(nameof(segmentPosition), segmentPosition, "Unknown segment position"),
        };
    }
}
