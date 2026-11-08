using System.Reflection;
using ConventionAssertions.Conventions;

namespace ConventionAssertions.Tests.Conventions;

public class DoesNotExposeTypesTests
{
    private const string EntityName = "ConventionAssertions.Tests.Conventions.DoesNotExposeTypesTests+Entity";
    private const string DtoName = "ConventionAssertions.Tests.Conventions.DoesNotExposeTypesTests+Dto";
    private const string OuterDtoName = "ConventionAssertions.Tests.Conventions.DoesNotExposeTypesTests+OuterDto";

    private readonly DoesNotExposeTypes _tested = new() { Types = TypesOf(typeof(Entity)) };

    [Theory]
    [InlineData(nameof(Something.ReturnsNothing))]
    [InlineData(nameof(Something.ReturnsString))]
    [InlineData(nameof(Something.ReturnsOther))]
    [InlineData(nameof(Something.ReturnsTaskOfOther))]
    public void Can_assert(string methodName)
    {
        var context = AssertMethod(methodName);

        Assert.Empty(context.Messages);
    }

    [Theory]
    [InlineData(nameof(Something.ReturnsEntity))]
    [InlineData(nameof(Something.ReturnsTaskOfEntity))]
    [InlineData(nameof(Something.ReturnsValueTaskOfEntity))]
    [InlineData(nameof(Something.ReturnsListOfEntity))]
    [InlineData(nameof(Something.ReturnsEntityArray))]
    [InlineData(nameof(Something.ReturnsDictionaryOfEntity))]
    [InlineData(nameof(Something.ReturnsNullableOfEntityValue))]
    public void Fails_on_assert_return_type(string methodName)
    {
        if (methodName == nameof(Something.ReturnsNullableOfEntityValue))
        {
            _tested.Types = TypesOf(typeof(EntityValue));
        }

        var context = AssertMethod(methodName);

        var message = Assert.Single(context.Messages);
        Assert.Contains("return type exposes", message, StringComparison.Ordinal);
    }

    [Fact]
    public void Fails_on_assert_derived_type()
    {
        var context = AssertMethod(nameof(Something.ReturnsDerivedEntity));

        var message = Assert.Single(context.Messages);
        Assert.Contains("return type exposes ConventionAssertions.Tests.Conventions.DoesNotExposeTypesTests+DerivedEntity", message, StringComparison.Ordinal);
    }

    [Fact]
    public void Fails_on_assert_nested_property()
    {
        var context = AssertMethod(nameof(Something.ReturnsDto));

        var message = Assert.Single(context.Messages);
        Assert.EndsWith($"return type exposes {EntityName} (via {DtoName}.Template).", message, StringComparison.Ordinal);
    }

    [Fact]
    public void Fails_on_assert_deeply_nested_property()
    {
        var context = AssertMethod(nameof(Something.ReturnsTaskOfOuterDto));

        var message = Assert.Single(context.Messages);
        Assert.EndsWith($"return type exposes {EntityName} (via {OuterDtoName}.Inner.Template).", message, StringComparison.Ordinal);
    }

    [Fact]
    public void Can_assert_cycle()
    {
        var context = AssertMethod(nameof(Something.ReturnsCycle));

        var message = Assert.Single(context.Messages);
        Assert.Contains("return type exposes", message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(nameof(Something.WithEntity))]
    [InlineData(nameof(Something.WithRefEntity))]
    [InlineData(nameof(Something.WithOutEntity))]
    [InlineData(nameof(Something.WithListOfEntity))]
    public void Fails_on_assert_parameter(string methodName)
    {
        var context = AssertMethod(methodName);

        var message = Assert.Single(context.Messages);
        Assert.EndsWith($"parameter entity exposes {EntityName}.", message, StringComparison.Ordinal);
    }

    [Fact]
    public void Fails_on_assert_return_type_and_each_parameter()
    {
        var context = AssertMethod(nameof(Something.ReturnsAndTakesEntities));

        Assert.Equal(3, context.Messages.Count());
    }

    [Fact]
    public void Can_assert_property()
    {
        var target = typeof(Dto).GetProperty(nameof(Dto.Name))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        Assert.Empty(context.Messages);
    }

    [Fact]
    public void Fails_on_assert_property()
    {
        var target = typeof(Dto).GetProperty(nameof(Dto.Template))!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);

        var message = Assert.Single(context.Messages);
        Assert.EndsWith($"exposes {EntityName}.", message, StringComparison.Ordinal);
    }

    [Fact]
    public void Throws_on_assert_not_configured()
    {
        _tested.Types = null;

        Assert.Throws<ConventionNotConfiguredException>(() => AssertMethod(nameof(Something.ReturnsNothing)));
    }

    [Fact]
    public void Throws_on_assert_no_types()
    {
        _tested.Types = ConventionTargets.FromTypes(
            a => a.FromAssemblyContaining<DoesNotExposeTypesTests>(),
            t => t.Where(x => false));

        Assert.Throws<ConventionNotConfiguredException>(() => AssertMethod(nameof(Something.ReturnsNothing)));
    }

    private static IConventionTargets<Type> TypesOf(Type type)
        => ConventionTargets.FromTypes(
            a => a.FromAssemblyContaining<DoesNotExposeTypesTests>(),
            t => t.Where(x => x.Type == type));

    private ConventionContext AssertMethod(string methodName)
    {
        var target = typeof(Something).GetMethod(methodName)!;
        var context = new ConventionContext(target);
        _tested.Assert(target, context);
        return context;
    }

    private struct EntityValue
    {
    }

    private class Entity
    {
        public int Id { get; set; }
    }

    private class DerivedEntity : Entity
    {
    }

    private class Other
    {
        public string? Name { get; set; }
    }

    private class Dto
    {
        public string? Name { get; set; }

        public Entity? Template { get; set; }
    }

    private class OuterDto
    {
        public Dto? Inner { get; set; }
    }

    private class Cycle
    {
        public Cycle? Next { get; set; }

        public Entity? Entity { get; set; }
    }

    private class Something
    {
        public static void ReturnsNothing()
        {
        }

        public static string ReturnsString(int value) => value.ToString(System.Globalization.CultureInfo.InvariantCulture);

        public static Other ReturnsOther() => new();

        public static Task<Other> ReturnsTaskOfOther() => Task.FromResult(new Other());

        public static Entity ReturnsEntity() => new();

        public static Task<Entity> ReturnsTaskOfEntity() => Task.FromResult(new Entity());

        public static ValueTask<Entity> ReturnsValueTaskOfEntity() => ValueTask.FromResult(new Entity());

        public static IReadOnlyList<Entity> ReturnsListOfEntity() => [];

        public static Entity[] ReturnsEntityArray() => [];

        public static Dictionary<int, Entity> ReturnsDictionaryOfEntity() => [];

        public static EntityValue? ReturnsNullableOfEntityValue() => null;

        public static DerivedEntity ReturnsDerivedEntity() => new();

        public static Dto ReturnsDto() => new();

        public static Task<OuterDto> ReturnsTaskOfOuterDto() => Task.FromResult(new OuterDto());

        public static Cycle ReturnsCycle() => new();

        public static void WithEntity(Entity entity)
        {
        }

        public static void WithRefEntity(ref Entity entity)
        {
        }

        public static void WithOutEntity(out Entity entity) => entity = new();

        public static void WithListOfEntity(List<Entity> entity)
        {
        }

        public static Entity ReturnsAndTakesEntities(Entity first, Entity second) => first;
    }
}
