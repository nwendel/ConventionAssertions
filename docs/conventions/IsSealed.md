# IsSealed

Checks that a type is sealed.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types

## Example

```csharp
Convention.For(
    s => s
        .FromAssemblyContaining<PlaceOrderHandler>()
        .Where(t => t.IsConcrete && t.IsAssignableTo(typeof(ICommandHandler<>))),
    a => a.Assert<IsSealed>());
```

## Failure message

```text
Type MyApp.Orders.PlaceOrderHandler must be sealed.
```

## Notes

- Abstract classes and interfaces fail, so filter them out with `t.IsConcrete`. To allow abstract base classes as well, use [IsAbstractOrSealed](IsAbstractOrSealed.md).
- Static classes, structs, enums and delegates pass, because the runtime marks them sealed.
