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

Click a convention for its settings, an example and its exact failure message.

In `ConventionAssertions.Conventions`:

| Convention | Targets | Checks that the target |
| --- | --- | --- |
| [AsyncMethodHasAsyncSuffix](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/AsyncMethodHasAsyncSuffix.md) | methods | has a name ending with `Async` if it returns a task. |
| [HasCustomAttribute](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasCustomAttribute.md) | types, methods, properties | has a given attribute (configurable). |
| [HasNamespaceSegment](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNamespaceSegment.md) | types | has a given segment in its namespace (configurable). |
| [HasNoPropertyOfType](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNoPropertyOfType.md) | types | has no public property of a given type (configurable). |
| [HasNoPublicConstructors](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNoPublicConstructors.md) | types | has no public constructor. |
| [HasNoSetter](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNoSetter.md) | properties | has no public setter. |
| [HasPublicStaticParseMethod](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasPublicStaticParseMethod.md) | types | has `public static T Parse(string)`. |
| [HasPublicStaticTryParseMethod](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasPublicStaticTryParseMethod.md) | types | has `public static bool TryParse(string, out T)`. |
| [IsAbstractOrSealed](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsAbstractOrSealed.md) | types | is abstract or sealed. |
| [IsInternal](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsInternal.md) | types | is internal. |
| [IsPublic](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsPublic.md) | types | is public. |
| [IsSealed](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsSealed.md) | types | is sealed. |
| [NameHasPrefix](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/NameHasPrefix.md) | types, methods, properties | has a name starting with a given prefix (configurable). |
| [NameHasSuffix](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/NameHasSuffix.md) | types, methods, properties | has a name ending with a given suffix (configurable). |
| [NameIsSnakeCase](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/NameIsSnakeCase.md) | types, methods | has a `lower_snake_case` name. |
| [VoidMethodIsNotAsync](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/VoidMethodIsNotAsync.md) | methods | is not `async void`. |

In `ConventionAssertions.AspNetCore.Conventions` (ConventionAssertions.AspNetCore package):

| Convention | Targets | Checks that the target |
| --- | --- | --- |
| [ControllerActionHasAuthorization](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/ControllerActionHasAuthorization.md) | methods | is a controller action with `[Authorize]` or `[AllowAnonymous]` on it or its controller. |

Some conventions need settings. Pass them in a configure action:

```csharp
Convention.For(
    _valueObjects,
    a => a.Assert<HasNamespaceSegment>(c => c.Segment = "Domain"));
```

A configurable convention used without its required settings throws `ConventionNotConfiguredException`.

## Writing your own conventions

A convention is a small class that implements `IConvention<TTarget>` for `Type`, `MethodInfo` or `PropertyInfo` and calls `context.Fail` for each problem it finds. Conventions with settings implement `IConfigurableConvention<TTarget>`. For a rule only one test needs, you can pass a lambda to `Assert` instead:

```csharp
Convention.For(
    _valueObjects.Properties(),
    a => a.Assert((property, context) =>
    {
        if (!property.PropertyType.IsValueType)
        {
            context.Fail("must be a value type");
        }
    }));
```

See [Writing your own conventions](https://github.com/nwendel/ConventionAssertions/blob/main/docs/writing-conventions.md) for both options in full.

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
