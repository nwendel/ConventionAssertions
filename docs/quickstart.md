# Quickstart

ConventionAssertions is a lightweight .NET 8 library for writing your own code conventions for types, methods and properties as small, reusable C# classes, and checking them from your unit tests. Conventions for ASP.NET Core (controllers, authorization) live in the separate, optional ConventionAssertions.AspNetCore package, so the core package doesn't pull in ASP.NET Core.

- **Member-level targets.** Types, methods and properties are all plain reflection types (`Type`, `MethodInfo`, `PropertyInfo`), so conventions are written against APIs every .NET developer already knows.
- **Conventions are small classes.** Write a class once and reuse it across projects, or write a lambda inline for a one-off rule.
- **Every violation in one failure**, not just the first one.
- **An empty-scan guard.** A convention that matches nothing fails by default, so a typo in a filter can't make a test pass silently.
- **Suppressions with a justification**, right on the test that owns the rule.
- **One dependency** (`Microsoft.Extensions.DependencyModel`).

ConventionAssertions works with any test framework. When xUnit is loaded, a failing convention throws xUnit's own assertion exception. With any other framework it throws `ConventionAssertions.Infrastructure.TestFrameworks.ConventionException`, which your runner reports as a failed test.

## Getting started

Every convention test has two parts: **which members to check**, and **which convention to check them against**. You pick the members with `ConventionTargets` and check them with `Convention.For`:

```csharp
using ConventionAssertions;
using ConventionAssertions.Conventions;

public class MethodConventionTests
{
    [Fact]
    public void Async_methods_end_with_Async()
    {
        var asyncMethods = ConventionTargets.FromMethods(
            a => a.FromAssemblyContaining<OrderService>(),
            methods: m => m.Where(x => x.IsAwaitable));

        Convention.For(
            asyncMethods,
            a => a.Assert<HasNameSuffix>(c => c.Suffix = "Async"));
    }
}
```

`ConventionTargets.FromTypes`, `FromMethods` and `FromProperties` take one lambda for each level of the scan: first the assemblies, then the types in them, then (for `FromMethods` and `FromProperties`) their members. Every lambda after the assembly one is optional, so `methods:` names the member scan when the type scan is left out. `Convention.For` receives the targets and an assert object on which you call `Assert` once or more.

A failing convention reports every violation in one message:

```text
Found 2.
Method MyApp.Services.OrderService.Load(int) name must end with Async.
Method MyApp.Services.CustomerService.Delete(int) name must end with Async.
```

### Reusing a set of targets

When several tests check the same set of targets, build them once in a field and pass the field to `Convention.For`:

```csharp
public class ValueObjectConventionTests
{
    private static readonly IConventionTargets<Type> ValueObjects = ConventionTargets.FromTypes(
        a => a.FromAssemblyContaining<OrderId>(),
        t => t.Where(x => x.IsAssignableTo<IValueObject>() && x.IsClass));

    [Fact]
    public void Value_objects_are_sealed()
    {
        Convention.For(
            ValueObjects,
            a => a.Assert<IsAbstractOrSealed>());
    }

    [Fact]
    public void Value_objects_have_try_parse()
    {
        Convention.For(
            ValueObjects,
            a => a.Assert<HasPublicStaticTryParseMethod>());
    }
}
```

A shared set can also be the starting point of a scan and narrowed further, by passing it instead of the assembly lambda:

```csharp
private static readonly IConventionTargets<Type> OrderIds = ConventionTargets.FromTypes(
    ValueObjects,
    t => t.Where(x => x.Type == typeof(OrderId)));
```

## Choosing targets

### Assemblies

The assembly lambda decides which assemblies to scan. Start it with one of these:

| Source | Assemblies it starts from |
| --- | --- |
| `FromAssemblyContaining<T>()` | The assembly that contains `T`. |
| `FromDefaultDependencyContext()` | Every assembly in the test project's dependency context (its `.deps.json`). |
| `FromDependencyContext(context)` | The same, for a `DependencyContext` you supply. |

The dependency context includes the framework and every package, so you will usually narrow it by name with `Where`. `Where` can be called more than once, and it only looks at assembly names, so assemblies it leaves out are never loaded and their types are never read. The assembly filter has:

| Filter | True when the assembly name |
| --- | --- |
| `AssemblyName` | (The `System.Reflection.AssemblyName` itself, for anything not covered below.) |
| `HasNamePrefix(prefix)` | starts with the prefix on a segment boundary, so `HasNamePrefix("MyApp.Orders")` matches `MyApp.Orders` and `MyApp.Orders.Domain` but not `MyApp.OrdersLegacy`. |
| `HasNameSegment(segment)` | has the segment as one whole dot-separated part. |
| `HasAnyNameSegment(segments...)` | has at least one of the segments. |

```csharp
Convention.For(
    ConventionTargets.FromTypes(
        a => a.FromDefaultDependencyContext().Where(x => x.HasNamePrefix("MyApp") && !x.HasNameSegment("Tests")),
        t => t.Where(x => x.IsConcrete && x.IsAssignableTo<ICommandHandler>())),
    a => a.Assert<IsInternal>());
```

An existing `IConventionTargets<Assembly>`, made with `ConventionTargets.FromAssemblies`, can be passed instead of the assembly lambda.

### Type filters

The type lambda is optional; without it every type in the assemblies is a target. Its `Where` narrows the types. It can be called more than once, and each call narrows the result of the previous one. The predicate receives a type filter:

| Filter | True when the type |
| --- | --- |
| `Type` | (The `System.Type` itself, for anything not covered below.) |
| `IsClass` | is a class. |
| `IsInterface` | is an interface. |
| `IsEnum` | is an enum. |
| `IsAbstract` | is abstract (interfaces count as abstract, static classes don't). |
| `IsConcrete` | is not abstract, so interfaces, abstract classes and static classes are left out. |
| `IsStatic` | is a static class. |
| `IsVisible` | is visible outside its assembly: public, and for nested types every declaring type is public too. |
| `HasAttribute<TAttribute>()` | has the attribute `TAttribute`. |
| `IsAssignableTo<T>()`, `IsAssignableTo(type)` | is assignable to the given type. Open generic types work too, such as `typeof(IRepository<>)` or `typeof(EntityBase<>)`. |
| `IsAssignableToAny(types...)` | is assignable to at least one of the given types. |
| `IsInSameNamespaceAs<T>()` | is in exactly the same namespace as `T`. |
| `HasNamespaceSegment(segment)`, `HasNamespaceSegment(segment, position)` | has the segment as one whole dot-separated part of its namespace, anywhere or at the given `SegmentPosition` (`First` or `Last`). Matches the same way as the [HasNamespaceSegment](conventions/HasNamespaceSegment.md) convention. |
| `IsNot(types...)` | is none of the given types. |

```csharp
t => t.Where(x => x
    .IsConcrete &&
    x.IsAssignableTo(typeof(IRepository<>)) &&
    x.IsNot(typeof(InMemoryRepository<>)))
```

### Methods and properties

Use `ConventionTargets.FromMethods` or `ConventionTargets.FromProperties` to check the members of a set of types instead of the types themselves:

```csharp
private static readonly IConventionTargets<PropertyInfo> ValueObjectProperties = ConventionTargets.FromProperties(
    ValueObjects);

[Fact]
public void Value_object_properties_have_no_setter()
{
    Convention.For(
        ValueObjectProperties,
        a => a.Assert<HasNoSetter>());
}
```

To narrow the members, pass a member scan after the types. Its `Where` works like the type `Where` and can be called more than once:

```csharp
private static readonly IConventionTargets<MethodInfo> StaticValueObjectMethods = ConventionTargets.FromMethods(
    ValueObjects,
    m => m.Where(x => x.Method.IsStatic && x.IsPublic));
```

For a scan written in one place, `FromMethods` and `FromProperties` also take the assembly, type and member lambdas together:

```csharp
private static readonly IConventionTargets<MethodInfo> StaticValueObjectMethods = ConventionTargets.FromMethods(
    a => a.FromAssemblyContaining<OrderId>(),
    t => t.Where(x => x.IsAssignableTo<IValueObject>()),
    m => m.Where(x => x.Method.IsStatic && x.IsPublic));
```

The method filter has `Method` (the `MethodInfo`), `IsPublic`, `IsAwaitable` (returns `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>`), `IsVoid`, `HasAnyNamePrefix("Get", "Find")` (true when the name starts with one of the prefixes as a whole word, so `GetById` matches but `Getaway` does not) and `HasCustomAttribute<TAttribute>()` / `HasCustomAttribute(attribute => ...)`. The property filter has `Property` (the `PropertyInfo`) and the same two `HasCustomAttribute` overloads.

A method scan starts from the public methods of each type, including inherited and static ones. It leaves out methods declared on `object` and compiler-generated special methods such as property accessors and operators. A property scan starts from the public properties of each type, including inherited ones.

To narrow a shared set of members further, pass it to `FromMethods` or `FromProperties` with a member scan:

```csharp
private static readonly IConventionTargets<MethodInfo> ValueObjectMethods = ConventionTargets.FromMethods(
    ValueObjects);

private static readonly IConventionTargets<MethodInfo> StaticValueObjectMethods = ConventionTargets.FromMethods(
    ValueObjectMethods,
    m => m.Where(x => x.Method.IsStatic));
```

For a one-off check, the targets can also be built inline:

```csharp
Convention.For(
    ConventionTargets.FromMethods(ValueObjects),
    a => a.Assert<NameIsSnakeCase>());
```

### Checking assemblies

Use `ConventionTargets.FromAssemblies` to check assemblies themselves, for example what they reference. It takes the same assembly lambda as the type scan, and the assemblies are the targets:

```csharp
Convention.For(
    ConventionTargets.FromAssemblies(a => a
        .FromDefaultDependencyContext()
        .Where(x => x.HasNamePrefix("MyApp.Orders") && !x.HasNameSegment("Tests"))),
    a => a.Assert<HasNoAssemblyReferenceWithNameSegment>(c => c.Segment = "Legacy"));
```

A failing assembly convention names the assembly by its simple name, such as `Assembly MyApp.Orders.Api must not reference ...`, and `[SuppressConvention]` matches that name too.

## Configuring conventions

The built-in conventions are listed in the [README](../README.md#built-in-conventions), and each one links to a page with its settings, an example and its exact failure message.

Some conventions need settings. Pass them in a configure action:

```csharp
Convention.For(
    ValueObjects,
    a => a.Assert<HasNamespaceSegment>(c => c.Segment = "Domain"));
```

A configurable convention used without its required settings throws `ConventionNotConfiguredException`.

## Writing your own conventions

A convention is a small class that implements `IConvention<TTarget>` for `Type`, `MethodInfo` or `PropertyInfo` and calls `context.Fail` for each problem it finds. Conventions with settings implement `IConfigurableConvention<TTarget>`. For a rule only one test needs, you can pass a lambda to `Assert` instead:

```csharp
Convention.For(
    ValueObjectProperties,
    a => a.Assert((property, context) =>
    {
        if (!property.PropertyType.IsValueType)
        {
            context.Fail("must be a value type");
        }
    }));
```

See [Writing your own conventions](writing-conventions.md) for both options in full.

## When nothing matches: `failOnNoTargets`

If a scan finds no targets, the convention fails with `No targets found for convention.` This guards against a test that passes only because a renamed type or a wrong filter left it with nothing to check.

When an empty result is fine, for example a rule for a folder that is empty today, turn the guard off for that assertion:

```csharp
Convention.For(
    ConventionTargets.FromTypes(a => a.FromAssemblyContaining<OrderId>(), t => t.Where(x => x.HasAttribute<ObsoleteAttribute>())),
    a => a.Assert<IsInternal>(failOnNoTargets: false));
```

Every `Assert` overload takes `failOnNoTargets`, including the configurable and inline ones.

## Saying why: `because`

Every `Assert` overload also takes an optional `because`. It is added to the end of each failure message from that assertion, so the failure can say what to do instead:

```csharp
Convention.For(
    _controllers,
    a => a.Assert<IsNotAssignableTo>(
        c => c.Type = typeof(Controller),
        because: "use ControllerBase instead"));
```

```text
Type MyApp.Api.OrdersController must not be assignable to Microsoft.AspNetCore.Mvc.Controller, use ControllerBase instead.
```

Leaving `because` out keeps the messages as they are. An empty or whitespace-only `because` throws `ArgumentNullException`.

## Suppressing a violation

To accept a known violation, put `[SuppressConvention]` on the test method with the display name of the target and a reason. The attribute can be repeated:

```csharp
[Fact]
[SuppressConvention(Target = "MyApp.Domain.LegacyId", Justification = "Public for the v1 API, remove in v3")]
[SuppressConvention(Target = "MyApp.Domain.OrderId.Parse(string)", Justification = "Name required by ASP.NET Core model binding")]
public void Value_objects_follow_conventions()
{
    // ...
}
```

`Target` is the same display name the failure message shows, so the simplest way to get it right is to copy it from the message:

- **Types:** the full name, such as `MyApp.Domain.OrderId`, or `MyApp.Domain.Result<T>` for a generic type.
- **Methods:** the type, method name and parameter types, such as `MyApp.Domain.OrderId.TryParse(string,MyApp.Domain.OrderId)`. Parameter types are separated by a comma with no space.
- **Properties:** the type and property name, such as `MyApp.Domain.OrderId.Value`.

The attribute is read from the method that calls `Convention`, so it must be on the test method itself, not on a helper method that the test calls.
