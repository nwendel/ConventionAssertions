namespace ConventionAssertions.Infrastructure;

internal static class StringExtensions
{
    public static string TakeUntil(this string self, char @char)
    {
        GuardAgainst.Null(self);

        var index = self.IndexOf(@char, StringComparison.Ordinal);
        return index == -1
            ? self
            : self[0..index];
    }
}
