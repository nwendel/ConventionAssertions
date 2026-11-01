using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class IsInternalTests
{
    private readonly IsInternal _tested = new();

    public static IEnumerable<object[]> CanAssertMemberData()
    {
        yield return new[] { typeof(InternalClass) };
        yield return new[] { PublicDeclaringClass.NestedInternalType };
        yield return new[] { InternalDeclaringClass.NestedInternalType };
    }

    public static IEnumerable<object[]> ThrowsOnAssertMemberData()
    {
        yield return new[] { typeof(PublicClass) };
        yield return new[] { PublicDeclaringClass.NestedPublicType };
        yield return new[] { PublicDeclaringClass.NestedProtectedInternalType };
        yield return new[] { PublicDeclaringClass.NestedProtectedType };
        yield return new[] { PublicDeclaringClass.NestedPrivateProtectedType };
        yield return new[] { PublicDeclaringClass.NestedPrivateType };
        yield return new[] { InternalDeclaringClass.NestedPublicType };
        yield return new[] { InternalDeclaringClass.NestedProtectedInternalType };
        yield return new[] { InternalDeclaringClass.NestedProtectedType };
        yield return new[] { InternalDeclaringClass.NestedPrivateProtectedType };
        yield return new[] { InternalDeclaringClass.NestedPrivateType };
    }

    [Theory]
    [MemberData(nameof(CanAssertMemberData))]
    public void Can_assert(Type type)
    {
        var context = new ConventionContext(type);
        _tested.Assert(type, context);

        Assert.Empty(context.Messages);
    }

    [Theory]
    [MemberData(nameof(ThrowsOnAssertMemberData))]
    public void Fails_on_assert(Type type)
    {
        var context = new ConventionContext(type);
        _tested.Assert(type, context);
        Assert.Single(context.Messages);
    }
}
