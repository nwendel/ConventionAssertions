using ConventionAssertions.Internal.Filters;

namespace ConventionAssertions.Tests.Internal.Filters;

public class MethodFilterTests
{
    public static IEnumerable<object[]> IsAwaitableMemberData()
    {
        yield return new[] { nameof(Something.Task) };
        yield return new[] { nameof(Something.GenericTask) };
        yield return new[] { nameof(Something.ValueTask) };
        yield return new[] { nameof(Something.GenericValueTask) };
    }

    public static IEnumerable<object[]> IsNotAwaitableMemberData()
    {
        yield return new[] { nameof(Something.Int) };
        yield return new[] { nameof(Something.Void) };
    }

    [Theory]
    [MemberData(nameof(IsAwaitableMemberData))]
    public void Can_is_awaitable_true(string methodName)
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(methodName)!);

        Assert.True(tested.IsAwaitable);
    }

    [Theory]
    [MemberData(nameof(IsNotAwaitableMemberData))]
    public void Can_is_awaitable_false(string methodName)
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(methodName)!);

        Assert.False(tested.IsAwaitable);
    }

    [Fact]
    public void Can_is_void_true()
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(nameof(Something.Void))!);

        Assert.True(tested.IsVoid);
    }

    [Theory]
    [InlineData(nameof(Something.Int))]
    [InlineData(nameof(Something.Task))]
    public void Can_is_void_false(string methodName)
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(methodName)!);

        Assert.False(tested.IsVoid);
    }

    private class Something
    {
        public static Task Task() => System.Threading.Tasks.Task.CompletedTask;

        public static Task<int> GenericTask() => System.Threading.Tasks.Task.FromResult(0);

        public static ValueTask ValueTask() => default;

        public static ValueTask<int> GenericValueTask() => default;

        public static int Int() => 0;

        public static void Void()
        {
        }
    }
}
