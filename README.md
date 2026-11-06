# ConventionAssertions

Code conventions for .NET, checked from your unit tests.

![Build](https://github.com/nwendel/conventionassertions/actions/workflows/build.yml/badge.svg) [![Coverage](https://codecov.io/gh/nwendel/ConventionAssertions/branch/main/graph/badge.svg?token=BMNOSIWUMV)](https://codecov.io/gh/nwendel/ConventionAssertions)

ConventionAssertions is a lightweight .NET 8 library for writing your own code conventions for types, methods and properties as small, reusable C# classes, and checking them from your unit tests. You pick the members to check with a scan, and every violation is reported in one failure.

## Example

Pick the methods to check with a scan, and give each convention its own test:

```csharp
public class MethodConventionTests
{
    private static readonly IConventionTargets<MethodInfo> _asyncMethods = ConventionTargets.FromMethods(
        s => s.FromAssemblyContaining<OrderService>(),
        m => m.Where(x => x.IsAwaitable));

    private static readonly IConventionTargets<MethodInfo> _voidMethods = ConventionTargets.FromMethods(
        s => s.FromAssemblyContaining<OrderService>(),
        m => m.Where(x => x.IsVoid));

    [Fact]
    public void Async_methods_end_with_Async() =>
        Convention.For(_asyncMethods, a => a.Assert<HasNameSuffix>(c => c.Suffix = "Async"));

    [Fact]
    public void Void_methods_are_not_async() =>
        Convention.For(_voidMethods, a => a.Assert<IsNotAsync>());
}
```

A failing convention lists every method that breaks it, not just the first one:

```text
Found 2.
Method MyApp.Services.OrderService.Load(int) name must end with Async.
Method MyApp.Services.CustomerService.Delete(int) name must end with Async.
```

**Check out the [Quickstart](https://github.com/nwendel/ConventionAssertions/blob/main/docs/quickstart.md) for more examples!**

## Features at a glance

- Checks types, methods, properties and assemblies, all as plain `Type`, `MethodInfo`, `PropertyInfo` and `Assembly`.
- Conventions are small classes you write once and reuse, or a lambda inline for a one-off rule ([writing your own](https://github.com/nwendel/ConventionAssertions/blob/main/docs/writing-conventions.md)).
- Every violation in one failure, not just the first one.
- A scan that matches nothing fails by default, so a typo in a filter can't make a test pass silently.
- Suppress a known violation with a justification, right on the test that owns the rule.
- Works with any test framework, and has a single dependency.

## Built-in conventions

In `ConventionAssertions.Conventions`:

| Convention | Targets | Checks that the target |
| --- | --- | --- |
| [DoesNotExposeTypes](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/DoesNotExposeTypes.md) | methods, properties | doesn't expose any of a set of types, in its signature or nested in the types it exposes (configurable). |
| [HasCustomAttribute](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasCustomAttribute.md) | types, methods, properties | has a given attribute (configurable). |
| [HasNamespaceSegment](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNamespaceSegment.md) | types | has a given segment in its namespace (configurable). |
| [HasNamePrefix](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNamePrefix.md) | types, methods, properties | has a name starting with a given prefix (configurable). |
| [HasNameSuffix](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNameSuffix.md) | types, methods, properties | has a name ending with a given suffix (configurable). |
| [HasNoAssemblyReferenceWithNameSegment](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNoAssemblyReferenceWithNameSegment.md) | assemblies | references no assembly, directly or indirectly, with a given segment in its name (configurable). |
| [HasNoParameterAssignableTo](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNoParameterAssignableTo.md) | methods | has no parameter assignable to a given type (configurable). |
| [HasNoPropertyAssignableTo](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNoPropertyAssignableTo.md) | types | has no public property assignable to a given type (configurable). |
| [HasNoPublicConstructors](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNoPublicConstructors.md) | types | has no public constructor. |
| [HasNoSetter](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasNoSetter.md) | properties | has no public setter. |
| [HasParameterAssignableTo](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasParameterAssignableTo.md) | methods | has a parameter assignable to a given type, optionally an exact number of them (configurable). |
| [HasPublicStaticParseMethod](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasPublicStaticParseMethod.md) | types | has `public static T Parse(string)`. |
| [HasPublicStaticTryParseMethod](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/HasPublicStaticTryParseMethod.md) | types | has `public static bool TryParse(string, out T)`. |
| [IsAbstractOrSealed](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsAbstractOrSealed.md) | types | is abstract or sealed. |
| [IsAssignableTo](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsAssignableTo.md) | types | is assignable to a given type, including open generic types (configurable). |
| [IsInternal](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsInternal.md) | types | is internal. |
| [IsNotAsync](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsNotAsync.md) | methods | is not `async`. |
| [IsNotAssignableTo](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsNotAssignableTo.md) | types | is not assignable to a given type, including open generic types (configurable). |
| [IsPublic](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsPublic.md) | types | is public. |
| [IsSealed](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsSealed.md) | types | is sealed. |
| [IsStatic](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/IsStatic.md) | types | is a static class. |
| [NameIsSnakeCase](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/NameIsSnakeCase.md) | types, methods | has a `lower_snake_case` name. |

In `ConventionAssertions.AspNetCore.Conventions` (the separate, optional ConventionAssertions.AspNetCore package, also for .NET 8):

| Convention | Targets | Checks that the target |
| --- | --- | --- |
| [ControllerActionHasAuthorization](https://github.com/nwendel/ConventionAssertions/blob/main/docs/conventions/ControllerActionHasAuthorization.md) | methods | is a controller action with `[Authorize]` or `[AllowAnonymous]` on it or its controller. |

## License

Apache-2.0
