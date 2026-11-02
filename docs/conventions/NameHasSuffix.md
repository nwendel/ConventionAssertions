# NameHasSuffix

Checks that the name of a type, method or property ends with a given suffix.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types, methods, properties
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `Suffix` | yes | The text the name must end with. |

## Example

```csharp
Convention.For(
    s => s
        .FromAssemblyContaining<PlaceOrderHandler>()
        .Where(t => t.IsConcrete && t.IsAssignableTo(typeof(ICommandHandler<>))),
    a => a.Assert<NameHasSuffix>(c => c.Suffix = "Handler"));
```

## Failure message

```text
Type MyApp.Orders.PlaceOrder name must end with Handler.
```

## Notes

- The comparison is case-sensitive.
- Only the type's own name is checked, not its namespace. For generic types the arity marker is ignored, so `CommandHandler<T>` is checked as `CommandHandler` and passes.
- Leaving `Suffix` unset or empty throws `ConventionNotConfiguredException`.
- The matching check for the start of a name is [NameHasPrefix](NameHasPrefix.md). For the `Async` suffix on async methods, use [AsyncMethodHasAsyncSuffix](AsyncMethodHasAsyncSuffix.md).
