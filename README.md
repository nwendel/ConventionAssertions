# ConventionAssertions ![Build](https://github.com/nwendel/conventionassertions/actions/workflows/build.yml/badge.svg) [![Coverage](https://codecov.io/gh/nwendel/ConventionAssertions/branch/main/graph/badge.svg?token=BMNOSIWUMV)](https://codecov.io/gh/nwendel/ConventionAssertions)

ConventionAssertions is a lightweight way to write your own code conventions for types, methods and properties as small, reusable C# classes, and check them from your unit tests.

```csharp
[Fact]
public void Value_objects_are_sealed()
{
    Convention.For(
        s => s
            .FromAssemblyContaining<OrderId>()
            .Where(t => t.IsAssignableTo<IValueObject>() && t.IsClass),
        a => a.Assert<IsAbstractOrSealed>());
}
```

A failing convention reports every violation in one message:

```text
Found 2.
Type MyApp.Domain.OrderId must be abstract or sealed.
Type MyApp.Domain.CustomerId must be abstract or sealed.
```

Highlights:

- **Member-level targets.** Types, methods and properties are all plain reflection types (`Type`, `MethodInfo`, `PropertyInfo`), so conventions are written against APIs every .NET developer already knows.
- **Conventions are small classes.** Write a class once and reuse it across projects, or write a lambda inline for a one-off rule.
- **Every violation in one failure**, not just the first one.
- **An empty-scan guard.** A convention that matches nothing fails by default, so a typo in a filter can't make a test pass silently.
- **Suppressions with a justification**, right on the test that owns the rule.
- **One dependency** (`Microsoft.Extensions.DependencyModel`).

## Installation

The library targets .NET 8.

```bash
dotnet add package ConventionAssertions
```

Conventions for ASP.NET Core (controllers, authorization) live in a separate package, so the core package doesn't pull in ASP.NET Core:

```bash
dotnet add package ConventionAssertions.AspNetCore
```

ConventionAssertions works with any test framework. When xUnit is loaded, a failing convention throws xUnit's own assertion exception. With any other framework it throws `ConventionAssertions.Infrastructure.TestFrameworks.ConventionException`, which your runner reports as a failed test.

## Getting started

Every convention test has two parts: **which members to check**, and **which convention to check them against**. Both are given to `Convention.For`:

```csharp
using ConventionAssertions;
using ConventionAssertions.Conventions;

public class DomainConventionTests
{
    [Fact]
    public void Value_objects_have_no_public_constructors()
    {
        Convention.For(
            s => s
                .FromAssemblyContaining<OrderId>()
                .Where(t => t.IsAssignableTo<IValueObject>() && t.IsClass),
            a => a.Assert<HasNoPublicConstructors>());
    }
}
```

The first argument receives a type scanner and returns the targets. The second receives an assert object on which you call `Assert` once or more.

### Reusing a set of targets

When several tests check the same set of types, build the targets once with `ConventionTargets.FromTypes` and pass them straight to `Convention.For`:

```csharp
public class ValueObjectConventionTests
{
    private readonly IConventionTargets<Type> _valueObjects = ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<OrderId>()
        .Where(t => t.IsAssignableTo<IValueObject>() && t.IsClass));

    [Fact]
    public void Value_objects_are_sealed()
    {
        Convention.For(
            _valueObjects,
            a => a.Assert<IsAbstractOrSealed>());
    }

    [Fact]
    public void Value_objects_have_try_parse()
    {
        Convention.For(
            _valueObjects,
            a => a.Assert<HasPublicStaticTryParseMethod>());
    }
}
```

A shared set can also be the starting point of a scan and narrowed further with `FromTargets`:

```csharp
Convention.For(
    s => s
        .FromTargets(_valueObjects)
        .Where(t => t.Type == typeof(OrderId)),
    a => a.Assert<HasPublicStaticParseMethod>());
```

## Choosing targets

### Scanners

The scanner decides which assemblies to load types from. Each of these returns a filterable list of types:

| Scanner | Types it starts from |
| --- | --- |
| `FromAssemblyContaining<T>()` | Every type in the assembly that contains `T`. |
| `FromTargets(targets)` | The types in an existing `IConventionTargets<Type>`, for example one made with `ConventionTargets.FromTypes`. |
| `FromDefaultDependencyContext()` | Every type in every assembly in the test project's dependency context (its `.deps.json`). |
| `FromDefaultDependencyContext(a => ...)` | The same, limited to the assemblies that match the predicate. |
| `FromDependencyContext(context)` / `FromDependencyContext(context, a => ...)` | The same, for a `DependencyContext` you supply. |

The dependency-context scanners load every assembly they match, including the framework and third-party packages, so you will usually filter them by name. The assembly filter has `HasNameSegment(segment)`, which matches one dot-separated part of the assembly name, and `AssemblyName` for anything else:

```csharp
Convention.For(
    s => s
        .FromDefaultDependencyContext(a => a.HasNameSegment("MyApp"))
        .Where(t => t.IsConcrete && t.IsAssignableTo<ICommandHandler>()),
    a => a.Assert<IsInternal>());
```

### Type filters

`Where` narrows the types. It can be called more than once, and each call narrows the result of the previous one. The predicate receives a type filter:

| Filter | True when the type |
| --- | --- |
| `Type` | (The `System.Type` itself, for anything not covered below.) |
| `IsClass` | is a class. |
| `IsInterface` | is an interface. |
| `IsAbstract` | is abstract (interfaces count as abstract). |
| `IsConcrete` | is not abstract. |
| `HasAttribute<TAttribute>()` | has the attribute `TAttribute`. |
| `IsAssignableTo<T>()`, `IsAssignableTo(type)` | is assignable to the given type. Open generic types work too, such as `typeof(IRepository<>)` or `typeof(EntityBase<>)`. |
| `IsAssignableToAny(types...)` | is assignable to at least one of the given types. |
| `IsInSameNamespaceAs<T>()` | is in exactly the same namespace as `T`. |
| `IsNot(types...)` | is none of the given types. |

```csharp
.Where(t => t
    .IsConcrete &&
    t.IsAssignableTo(typeof(IRepository<>)) &&
    t.IsNot(typeof(InMemoryRepository<>)))
```

### Methods and properties

Call `.Methods()` or `.Properties()` at the end of a type scan to check the members of those types instead of the types themselves:

```csharp
[Fact]
public void Value_object_properties_have_no_setter()
{
    Convention.For(
        s => s.FromTargets(_valueObjects).Properties(),
        a => a.Assert<HasNoSetter>());
}
```

Both take an optional predicate:

```csharp
Convention.For(
    s => s
        .FromAssemblyContaining<OrderId>()
        .Where(t => t.IsAssignableTo<IValueObject>())
        .Methods(m => m.Method.IsStatic && m.IsPublic),
    a => a.Assert<NameIsSnakeCase>());
```

The method filter has `Method` (the `MethodInfo`), `IsPublic` and `HasCustomAttribute<TAttribute>()` / `HasCustomAttribute(attribute => ...)`. The property filter has `Property` (the `PropertyInfo`) and the same two `HasCustomAttribute` overloads.

`Methods()` returns the public methods of each type, including inherited and static ones. It leaves out methods declared on `object` and compiler-generated special methods such as property accessors and operators. `Properties()` returns the public properties of each type, including inherited ones.

`Methods()` and `Properties()` are extension methods on `IConventionTargets<Type>`, so they also work on a shared set outside the scanner:

```csharp
Convention.For(
    _valueObjects.Methods(),
    a => a.Assert<NameIsSnakeCase>());
```

## Built-in conventions

The conventions in the `ConventionAssertions.Conventions` namespace:

| Convention | Targets | Checks that the target |
| --- | --- | --- |
| `IsAbstractOrSealed` | types | is abstract or sealed. |
| `IsSealed` | types | is sealed. |
| `IsPublic` | types | is public (top-level `public`, or nested `public`). |
| `IsInternal` | types | is internal (top-level `internal`, or nested `internal`). |
| `HasNoPublicConstructors` | types | has no public constructor. |
| `HasPublicStaticParseMethod` | types | has `public static T Parse(string)`. |
| `HasPublicStaticTryParseMethod` | types | has `public static bool TryParse(string, out T)`. |
| `HasNamespaceSegment` | types | has a given segment in its namespace. Configurable (`Segment`, `SegmentPosition`). |
| `HasNoPropertyOfType` | types | has no public property assignable to a given type, such as `DbContext` on a domain type. Configurable (`PropertyType`). |
| `HasCustomAttribute` | types, methods, properties | has a given attribute. Configurable (`CustomAttribute`). |
| `NameHasPrefix` | types, methods, properties | has a name starting with a given prefix. Configurable (`Prefix`). |
| `NameHasSuffix` | types, methods, properties | has a name ending with a given suffix. Configurable (`Suffix`). |
| `NameIsSnakeCase` | types, methods | has a `lower_snake_case` name. |
| `AsyncMethodHasAsyncSuffix` | methods | has a name ending with `Async` if it returns `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>`. Other methods pass. |
| `VoidMethodIsNotAsync` | methods | is not `async void`. |
| `HasNoSetter` | properties | has no public setter. |

For types, `NameHasPrefix` and `NameHasSuffix` ignore the generic arity marker, so `CommandHandler<T>` counts as ending with `Handler`.

The ASP.NET Core package adds, in the `ConventionAssertions.AspNetCore.Conventions` namespace:

| Convention | Targets | Checks that the target |
| --- | --- | --- |
| `ControllerActionHasAuthorization` | methods | is a controller action with an explicit authorization decision: `[Authorize]` or `[AllowAnonymous]` on the action or its controller. Methods marked `[NonAction]` are skipped. An action with `[Authorize]` inside an `[AllowAnonymous]` controller fails, because `[AllowAnonymous]` on the controller overrides it. |

```csharp
using ConventionAssertions.AspNetCore.Conventions;
using Microsoft.AspNetCore.Mvc;

[Fact]
public void Controller_actions_declare_authorization()
{
    Convention.For(
        s => s
            .FromAssemblyContaining<Program>()
            .Where(t => t.IsConcrete && t.IsAssignableTo<ControllerBase>())
            .Methods(m => m.Method.DeclaringType != typeof(ControllerBase)
                && m.Method.DeclaringType != typeof(Controller)),
        a => a.Assert<ControllerActionHasAuthorization>());
}
```

`ConventionAssertions.Conventions.ControllerActionHasAuthorization` in the core package is obsolete and now always fails, telling you to switch to the one in `ConventionAssertions.AspNetCore`.

### Configurable conventions

Some conventions need settings. Pass a configure action to `Assert`:

```csharp
Convention.For(
    _valueObjects,
    a => a.Assert<HasNamespaceSegment>(c => c.Segment = "Domain"));

Convention.For(
    _valueObjects,
    a => a.Assert<HasNamespaceSegment>(c =>
    {
        c.Segment = "ValueObjects";
        c.SegmentPosition = SegmentPosition.Last;
    }));

Convention.For(
    s => s.FromAssemblyContaining<OrderId>().Where(t => t.IsAssignableTo<IValueObject>()),
    a => a.Assert<HasCustomAttribute>(c => c.CustomAttribute = typeof(TypeConverterAttribute)));

Convention.For(
    s => s.FromAssemblyContaining<PlaceOrderHandler>().Where(t => t.IsConcrete && t.IsAssignableTo(typeof(ICommandHandler<>))),
    a => a.Assert<NameHasSuffix>(c => c.Suffix = "Handler"));

Convention.For(
    s => s.FromAssemblyContaining<Order>().Where(t => t.IsInSameNamespaceAs<Order>()),
    a => a.Assert<HasNoPropertyOfType>(c => c.PropertyType = typeof(DbContext)));
```

`SegmentPosition` is `Anywhere` (the default), `First` or `Last`. `HasCustomAttribute` matches the exact attribute type, not attributes derived from it. A configurable convention used without its required settings throws `ConventionNotConfiguredException`.

## Writing your own conventions

A convention is a class with a parameterless constructor that implements `IConvention<TTarget>`, where `TTarget` is `Type`, `MethodInfo` or `PropertyInfo`. Call `context.Fail` once for each problem you find:

```csharp
using System.Reflection;
using ConventionAssertions;

public class ReturnsTask : IConvention<MethodInfo>
{
    public void Assert(MethodInfo target, ConventionContext context)
    {
        if (!typeof(Task).IsAssignableFrom(target.ReturnType))
        {
            context.Fail("must return a Task");
        }
    }
}
```

```csharp
Convention.For(
    s => s
        .FromAssemblyContaining<OrderService>()
        .Where(t => t.IsAssignableTo<ICommandHandler>())
        .Methods(m => m.Method.Name == "Handle"),
    a => a.Assert<ReturnsTask>());
```

The failure message is built from the kind of target, its display name and your text, so `context.Fail("must return a Task")` reads as:

```text
Method MyApp.Orders.PlaceOrderHandler.Handle(MyApp.Orders.PlaceOrder) must return a Task.
```

Write the message as the end of that sentence, without a trailing period.

One class can implement the interface for several target kinds, like `NameIsSnakeCase` does for both `Type` and `MethodInfo`.

### Configurable conventions of your own

A convention with settings implements `IConfigurableConvention<TTarget>` instead and exposes the settings as properties. `Assert<T>()` without a configure action only accepts `IConvention<TTarget>`, so a configurable convention can't be used by mistake without its settings. Throw `ConventionNotConfiguredException` if a required setting is missing anyway:

```csharp
public class HasMaxParameterCount : IConfigurableConvention<MethodInfo>
{
    public int? Max { get; set; }

    public void Assert(MethodInfo target, ConventionContext context)
    {
        if (Max == null)
        {
            throw new ConventionNotConfiguredException($"{nameof(Max)} not specified");
        }

        if (target.GetParameters().Length > Max)
        {
            context.Fail($"must have at most {Max} parameters");
        }
    }
}
```

```csharp
Convention.For(
    s => s.FromAssemblyContaining<OrderService>().Where(t => t.IsConcrete).Methods(),
    a => a.Assert<HasMaxParameterCount>(c => c.Max = 4));
```

## Inline conventions

For a rule that only one test needs, pass a lambda or a method with the same shape as `Assert` instead of writing a class:

```csharp
[Fact]
public void Value_object_properties_are_value_types()
{
    Convention.For(
        _valueObjects.Properties(),
        a => a.Assert((property, context) =>
        {
            if (!property.PropertyType.IsValueType)
            {
                context.Fail("must be a value type");
            }
        }));
}
```

```csharp
[Fact]
public void Value_object_static_methods_return_value()
{
    Convention.For(
        s => s.FromTargets(_valueObjects).Methods(m => m.Method.IsStatic),
        a => a.Assert(ReturnsValue));
}

private static void ReturnsValue(MethodInfo method, ConventionContext context)
{
    if (method.ReturnType == typeof(void))
    {
        context.Fail("must return a value");
    }
}
```

## When nothing matches: `failOnNoTargets`

If a scan finds no targets, the convention fails with `No targets found for convention.` This guards against a test that passes only because a renamed type or a wrong filter left it with nothing to check.

When an empty result is fine, for example a rule for a folder that is empty today, turn the guard off for that assertion:

```csharp
Convention.For(
    s => s.FromAssemblyContaining<OrderId>().Where(t => t.HasAttribute<ObsoleteAttribute>()),
    a => a.Assert<IsInternal>(failOnNoTargets: false));
```

Every `Assert` overload takes `failOnNoTargets`, including the configurable and inline ones.

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

The attribute is read from the method that calls `Convention.For`, so it must be on the test method itself, not on a helper method that the test calls.

## License

Apache-2.0
