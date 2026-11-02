# IsPublic

Checks that a type is public.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types

## Example

```csharp
Convention.ForTypes(
    s => s
        .FromAssemblyContaining<OrderId>()
        .Where(t => t.IsAssignableTo<IValueObject>()),
    a => a.Assert<IsPublic>());
```

## Failure message

```text
Type MyApp.Domain.OrderId must be public.
```

## Notes

- A top-level type must be `public`. A nested type must be `public` itself, so a `public` type nested in an `internal` type still passes.
- The opposite check is [IsInternal](IsInternal.md).
