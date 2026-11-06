# HasNamePrefix

Checks that the name of a type, method or property starts with a given prefix.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types, methods, properties
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `Prefix` | yes | The text the name must start with. |

## Example

```csharp
Convention.For(
    ConventionTargets.FromTypes(
        a => a.FromAssemblyContaining<OrderId>(),
        t => t.Where(x => x.IsInterface)),
    a => a.Assert<HasNamePrefix>(c => c.Prefix = "I"));
```

## Failure message

```text
Type MyApp.Domain.ValueObject name must start with I.
```

## Notes

- The comparison is case-sensitive.
- Only the type's own name is checked, not its namespace. For generic types the arity marker is ignored, so `IRepository<T>` is checked as `IRepository`.
- Leaving `Prefix` unset or empty throws `ConventionNotConfiguredException`.
- The matching check for the end of a name is [HasNameSuffix](HasNameSuffix.md).
