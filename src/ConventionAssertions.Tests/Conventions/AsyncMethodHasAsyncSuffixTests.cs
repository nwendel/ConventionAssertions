using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class AsyncMethodHasAsyncSuffixTests
{
    private readonly AsyncMethodHasAsyncSuffix _tested = new();

    public static IEnumerable<object[]> CanAssertMemberData()
    {
        yield return new[] { nameof(Something.TaskAsync) };
        yield return new[] { nameof(Something.GenericTaskAsync) };
        yield return new[] { nameof(Something.ValueTaskAsync) };
        yield return new[] { nameof(Something.GenericValueTaskAsync) };
        yield return new[] { nameof(Something.NotAwaitable) };
    }

    public static IEnumerable<object[]> FailsOnAssertMemberData()
    {
        yield return new[] { nameof(Something.Task) };
        yield return new[] { nameof(Something.GenericTask) };
        yield return new[] { nameof(Something.ValueTask) };
        yield return new[] { nameof(Something.GenericValueTask) };
    }

    [Theory]
    [MemberData(nameof(CanAssertMemberData))]
    public void Can_assert(string methodName)
    {
        var target = typeof(Something).GetMethod(methodName)!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Theory]
    [MemberData(nameof(FailsOnAssertMemberData))]
    public void Fails_on_assert(string methodName)
    {
        var target = typeof(Something).GetMethod(methodName)!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    private class Something
    {
        public static Task TaskAsync() => System.Threading.Tasks.Task.CompletedTask;

        public static Task<int> GenericTaskAsync() => System.Threading.Tasks.Task.FromResult(0);

        public static ValueTask ValueTaskAsync() => default;

        public static ValueTask<int> GenericValueTaskAsync() => default;

        public static int NotAwaitable() => 0;

        public static Task Task() => System.Threading.Tasks.Task.CompletedTask;

        public static Task<int> GenericTask() => System.Threading.Tasks.Task.FromResult(0);

        public static ValueTask ValueTask() => default;

        public static ValueTask<int> GenericValueTask() => default;
    }
}
