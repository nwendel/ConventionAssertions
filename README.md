# ConventionAssertions

Code conventions for .NET, checked from your unit tests.

![Build](https://github.com/nwendel/conventionassertions/actions/workflows/build.yml/badge.svg) [![Coverage](https://codecov.io/gh/nwendel/ConventionAssertions/branch/main/graph/badge.svg?token=BMNOSIWUMV)](https://codecov.io/gh/nwendel/ConventionAssertions)

ConventionAssertions is a lightweight .NET 8 library for writing your own code conventions for types, methods and properties as small, reusable C# classes, and checking them from your unit tests. You pick the members to check with a scan, and every violation is reported in one failure.

## Example

Scan an assembly, filter it down to the types you care about, and check them against several conventions at once:

```csharp
[Fact]
public void Value_objects_follow_conventions()
{
    var valueObjects = ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<OrderId>()
        .Where(t => t.IsAssignableTo<IValueObject>() && t.IsConcrete));

    Convention.For(
        valueObjects,
        a =>
        {
            a.Assert<IsSealed>();
            a.Assert<HasNoPublicConstructors>();
            a.Assert<HasPublicStaticParseMethod>();
        });
}
```

A failing convention lists every type that breaks it, not just the first one:

```text
Found 2.
Type MyApp.Domain.OrderId must be sealed.
Type MyApp.Domain.CustomerId must be sealed.
```

**Check out the [Quickstart](https://github.com/nwendel/ConventionAssertions/blob/main/docs/quickstart.md) for more examples!**

## Features at a glance

- Checks types, methods and properties, all as plain `Type`, `MethodInfo` and `PropertyInfo`.
- Conventions are small classes you write once and reuse, or a lambda inline for a one-off rule ([writing your own](https://github.com/nwendel/ConventionAssertions/blob/main/docs/writing-conventions.md)).
- Every violation in one failure, not just the first one.
- A scan that matches nothing fails by default, so a typo in a filter can't make a test pass silently.
- Suppress a known violation with a justification, right on the test that owns the rule.
- Works with any test framework, and has a single dependency.

## Built-in conventions

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

In `ConventionAssertions.AspNetCore.Conventions` (the separate, optional ConventionAssertions.AspNetCore package, also for .NET 8):

| Convention | Targets | Checks that the target |
| --- | --- | --- |
| [ControllerActionHasAuthorization](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/ControllerActionHasAuthorization.md) | methods | is a controller action with `[Authorize]` or `[AllowAnonymous]` on it or its controller. |

## License

Apache-2.0
