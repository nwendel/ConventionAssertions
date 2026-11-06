using ConventionAssertions.Conventions;
using ConventionAssertions.Reflection;

namespace ConventionAssertions.Internal.Filters;

public class TypeFilter : ITypeFilter
{
    public TypeFilter(Type type)
    {
        GuardAgainst.Null(type);

        Type = type;
    }

    public Type Type { get; }

    public bool IsAbstract
        => Type.IsAbstract && !Type.IsStatic();

    public bool IsClass
        => Type.IsClass;

    public bool IsConcrete
        => !Type.IsAbstract;

    public bool IsEnum
        => Type.IsEnum;

    public bool IsInterface
        => Type.IsInterface;

    public bool IsStatic
        => Type.IsStatic();

    public bool IsVisible
        => Type.IsVisible;

    public bool HasAttribute<T>()
        where T : Attribute
        => Type.GetCustomAttribute<T>() != null;

    public bool IsAssignableTo<T>()
        => IsAssignableToCore(typeof(T));

    public bool IsAssignableTo(Type type)
        => IsAssignableToCore(type);

    public bool IsAssignableToAny(params Type[] types)
        => types.Any(x => IsAssignableToCore(x));

    public bool IsInSameNamespaceAs<T>()
        => Type.Namespace == typeof(T).Namespace;

    public bool HasNamespaceSegment(string segment)
        => Type.HasNamespaceSegment(segment, SegmentPosition.Anywhere);

    public bool HasNamespaceSegment(string segment, SegmentPosition segmentPosition)
        => Type.HasNamespaceSegment(segment, segmentPosition);

    public bool IsNot(params Type[] types)
        => !types.Any(x => x == Type);

    private bool IsAssignableToCore(Type type)
    {
        GuardAgainst.Null(type);

        return Type.IsAssignableToOpenOrClosed(type);
    }
}
