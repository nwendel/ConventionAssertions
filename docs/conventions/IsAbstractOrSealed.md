# IsAbstractOrSealed

Checks that a type is abstract or sealed, so nothing outside your design can inherit from a concrete class.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types

## Example

```csharp
Convention.For(
    ConventionTargets.FromTypes(
        a => a.FromAssemblyContaining<OrderId>(),
        t => t.Where(x => x.IsAssignableTo<IValueObject>() && x.IsClass)),
    a => a.Assert<IsAbstractOrSealed>());
```

## Failure message

```text
Type MyApp.Domain.OrderId must be abstract or sealed.
```

## Notes

- Static classes pass, because the compiler marks them both abstract and sealed.
- Interfaces pass, because they are abstract. Filter with `t.IsClass` if you only want classes.
- To require sealed only, use [IsSealed](IsSealed.md).
