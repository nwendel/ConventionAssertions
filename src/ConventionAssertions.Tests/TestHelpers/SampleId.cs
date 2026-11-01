using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ConventionAssertions.Tests.TestHelpers;

public sealed class SampleId : IValueObject
{
    private SampleId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static SampleId Parse(string value)
    {
        return new(int.Parse(value, CultureInfo.InvariantCulture));
    }

    [SuppressMessage("Design", "CA1021:Avoid out parameters", Justification = "TryParse pattern")]
    public static bool TryParse(string value, [NotNullWhen(true)] out SampleId? result)
    {
        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            result = new(parsed);
            return true;
        }

        result = null;
        return false;
    }
}
