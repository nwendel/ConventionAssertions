# IsNotAssignableTo

Checks that a type is not assignable to a given type, for example that API controllers derive from `ControllerBase` rather than the view-enabled `Controller`.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `Type` | yes | The type that targets must not be assignable to: an interface, a base class, or an open generic type such as `typeof(IRepository<>)` or `typeof(EntityBase<>)`. |

## Example

API controllers must derive from `ControllerBase`, not `Controller`. The `because` text is added to each failure, so it says what to do instead:

```csharp
Convention.For(
    _controllers,
    a => a.Assert<IsNotAssignableTo>(
        c => c.Type = typeof(Controller),
        because: "use ControllerBase instead"));
```

## Failure message

```text
Type MyApp.Api.OrdersController must not be assignable to Microsoft.AspNetCore.Mvc.Controller, use ControllerBase instead.
```

Without `because`:

```text
Type MyApp.Api.OrdersController must not be assignable to Microsoft.AspNetCore.Mvc.Controller.
```

## Notes

- Matching is the same as [IsAssignableTo](IsAssignableTo.md) and the `IsAssignableTo` type filter, so a type fails when it implements the interface or derives from the class, directly or through a base class.
- An open generic interface matches any constructed form of it, and an open generic class matches when it is anywhere among the base classes.
- Leaving `Type` unset throws `ConventionNotConfiguredException`.
