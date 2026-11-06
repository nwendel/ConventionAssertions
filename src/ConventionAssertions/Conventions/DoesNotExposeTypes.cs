using ConventionAssertions.Reflection;

namespace ConventionAssertions.Conventions;

public class DoesNotExposeTypes :
    IConfigurableConvention<MethodInfo>,
    IConfigurableConvention<PropertyInfo>
{
    public IConventionTargets<Type>? Types { get; set; }

    public void Assert(MethodInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        var types = GetTypes();
        Check(target.ReturnType, "return type", types, context);
        foreach (var parameter in target.GetParameters())
        {
            Check(parameter.ParameterType, $"parameter {parameter.Name}", types, context);
        }
    }

    public void Assert(PropertyInfo target, ConventionContext context)
    {
        GuardAgainst.Null(target);
        GuardAgainst.Null(context);

        var types = GetTypes();
        Check(target.PropertyType, null, types, context);
    }

    private static void Check(Type start, string? label, List<Type> types, ConventionContext context)
    {
        var visited = new HashSet<Type>();
        var prefix = label == null ? "exposes" : $"{label} exposes";

        Walk(start, null);

        void Walk(Type type, string? via)
        {
            // ref, out and in parameters have a by-ref type, for example Actor&
            if (type.IsByRef)
            {
                type = type.GetElementType()!;
            }

            if (type.IsGenericParameter || !visited.Add(type))
            {
                return;
            }

            if (types.Any(x => x.IsAssignableFrom(type)))
            {
                var path = via == null ? string.Empty : $" (via {via})";
                context.Fail($"{prefix} {type.DisplayName()}{path}");
                return;
            }

            if (type.HasElementType)
            {
                Walk(type.GetElementType()!, via);
            }

            if (type.IsGenericType)
            {
                foreach (var argument in type.GetGenericArguments())
                {
                    Walk(argument, via);
                }
            }

            if (IsFramework(type))
            {
                return;
            }

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyVia = via == null
                    ? $"{type.DisplayName()}.{property.Name}"
                    : $"{via}.{property.Name}";
                Walk(property.PropertyType, propertyVia);
            }
        }
    }

    private static bool IsFramework(Type type)
    {
        if (type.IsPrimitive || type.IsArray || type == typeof(string))
        {
            return true;
        }

        var @namespace = type.Namespace;
        return @namespace != null && (IsNamespace(@namespace, "System") || IsNamespace(@namespace, "Microsoft"));
    }

    private static bool IsNamespace(string @namespace, string root)
        => @namespace == root || @namespace.StartsWith($"{root}.", StringComparison.Ordinal);

    private List<Type> GetTypes()
    {
        if (Types == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(Types)} not specified");
        }

        var types = Types.Targets.ToList();
        if (types.Count == 0)
        {
            throw new ConventionNotConfiguredException($"{nameof(Types)} matched no types");
        }

        return types;
    }
}
