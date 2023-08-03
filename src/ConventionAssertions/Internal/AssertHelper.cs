using System.Diagnostics;
using System.Reflection;

namespace ConventionAssertions.Internal;

public static class AssertHelper
{
    public static HashSet<string> FindSuppressions()
    {
        // TODO: This assumes first method found here has the suppression attributes,
        //       not sure what to do long term
        var frame = new StackTrace().GetFrames()
            .SkipWhile(x => x.GetMethod()?.DeclaringType?.Assembly == typeof(Convention).Assembly)
            .First();
        var method = frame.GetMethod();
        if (method == null)
        {
            throw new InvalidOperationException("No method found");
        }

        var attributes = method.GetCustomAttributes<SuppressConventionAttribute>();

        var types = attributes
            .Select(x => x.Target)
            .ToHashSet();
        return types;
    }
}
