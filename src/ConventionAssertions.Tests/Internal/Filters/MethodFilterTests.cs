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

    [Theory]
    [InlineData(nameof(Something.Get))]
    [InlineData(nameof(Something.GetById))]
    [InlineData(nameof(Something.FindAll))]
    public void Can_has_any_name_prefix_true(string methodName)
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(methodName)!);

        Assert.True(tested.HasAnyNamePrefix("Get", "Find"));
    }

    [Theory]
    [InlineData(nameof(Something.Getaway))]
    [InlineData(nameof(Something.Finder))]
    [InlineData(nameof(Something.Save))]
    [InlineData(nameof(Something.ForGet))]
    public void Can_has_any_name_prefix_false(string methodName)
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(methodName)!);

        Assert.False(tested.HasAnyNamePrefix("Get", "Find"));
    }

    [Fact]
    public void Can_has_any_name_prefix_match_is_case_sensitive()
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(nameof(Something.GetById))!);

        Assert.False(tested.HasAnyNamePrefix("get"));
    }

    [Fact]
    public void Throws_on_has_any_name_prefix_without_prefixes()
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(nameof(Something.Get))!);

        Assert.Throws<ArgumentException>(() => tested.HasAnyNamePrefix());
    }

    [Fact]
    public void Throws_on_has_any_name_prefix_null()
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(nameof(Something.Get))!);

        Assert.Throws<ArgumentNullException>(() => tested.HasAnyNamePrefix(null!));
    }

    [Fact]
    public void Throws_on_has_any_name_prefix_whitespace()
    {
        var tested = new MethodFilter(typeof(Something).GetMethod(nameof(Something.Get))!);

        Assert.Throws<ArgumentNullException>(() => tested.HasAnyNamePrefix(" "));
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

        public static void Get()
        {
        }

        public static void GetById()
        {
        }

        public static void FindAll()
        {
        }

        public static void Getaway()
        {
        }

        public static void Finder()
        {
        }

        public static void Save()
        {
        }

        public static void ForGet()
        {
        }
    }
}
