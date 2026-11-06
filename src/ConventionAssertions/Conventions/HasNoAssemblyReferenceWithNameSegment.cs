namespace ConventionAssertions.Conventions;

public class HasNoAssemblyReferenceWithNameSegment : IConfigurableConvention<Assembly>
{
    private static readonly string[] _frameworkNames = ["System", "netstandard", "mscorlib", "WindowsBase"];
    private static readonly string[] _frameworkPrefixes = ["System.", "Microsoft."];

    public string? Segment { get; set; }

    public SegmentPosition SegmentPosition { get; set; } = SegmentPosition.Anywhere;

    public void Assert(Assembly target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        if (Segment == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(Segment)} not specified");
        }

        var rootName = target.GetName().Name ?? string.Empty;
        var visited = new HashSet<string>(StringComparer.Ordinal) { rootName };
        var queue = new Queue<(Assembly Assembly, List<string> Path)>();
        queue.Enqueue((target, [rootName]));

        // Breadth first, so each offending assembly is reported once with the shortest path to it
        while (queue.Count > 0)
        {
            var (assembly, path) = queue.Dequeue();
            foreach (var reference in assembly.GetReferencedAssemblies())
            {
                var name = reference.Name;
                if (name == null || !visited.Add(name))
                {
                    continue;
                }

                var referencePath = new List<string>(path) { name };
                if (HasSegment(name))
                {
                    context.Fail($"must not reference {name} ({FormatPath(referencePath)})");
                    continue;
                }

                if (IsFramework(name))
                {
                    continue;
                }

                var referenced = TryLoad(reference);
                if (referenced == null)
                {
                    context.Fail($"references {name} which could not be loaded ({FormatPath(referencePath)})");
                    continue;
                }

                queue.Enqueue((referenced, referencePath));
            }
        }
    }

    private static bool IsFramework(string name)
        => _frameworkNames.Contains(name, StringComparer.Ordinal) ||
            _frameworkPrefixes.Any(x => name.StartsWith(x, StringComparison.Ordinal));

    private static Assembly? TryLoad(AssemblyName reference)
    {
        try
        {
            return Assembly.Load(reference);
        }
        catch (Exception ex) when (ex is FileNotFoundException or FileLoadException or BadImageFormatException)
        {
            return null;
        }
    }

    private static string FormatPath(List<string> path)
        => string.Join(" -> ", path);

    private bool HasSegment(string name)
    {
        var segments = name.Split('.');
        return SegmentPosition switch
        {
            SegmentPosition.Anywhere => segments.Contains(Segment),
            SegmentPosition.First => segments.First() == Segment,
            SegmentPosition.Last => segments.Last() == Segment,
        };
    }
}
