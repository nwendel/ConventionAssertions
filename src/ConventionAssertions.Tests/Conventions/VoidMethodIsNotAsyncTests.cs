using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class VoidMethodIsNotAsyncTests
{
    private readonly VoidMethodIsNotAsync _tested = new();

    [Theory]
    [InlineData(nameof(Something.NotAsyncVoid))]
    [InlineData(nameof(Something.AsyncTask))]
    public void Can_assert(string methodName)
    {
        var target = typeof(Something).GetMethod(methodName)!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_async_void()
    {
        var target = typeof(Something).GetMethod(nameof(Something.AsyncVoid))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        Assert.Single(context.Messages);
    }

    private class Something
    {
        public static void NotAsyncVoid()
        {
        }

        public static async Task AsyncTask() => await Task.Yield();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Needed for test")]
        public static async void AsyncVoid() => await Task.Yield();
    }
}
