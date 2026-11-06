# IsStatic

Checks that a type is a static class.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types

## Example

Classes that hold extension methods, named `*Extensions`, must be static:

```csharp
Convention.For(
    ConventionTargets.FromTypes(
        a => a.FromAssemblyContaining<OrderService>(),
        t => t.Where(x => x.IsClass && x.Type.Name.EndsWith("Extensions", StringComparison.Ordinal))),
    a => a.Assert<IsStatic>());
```

## Failure message

```text
Type MyApp.Orders.OrderQueryExtensions must be static.
```

## Notes

- A static class is one declared `static`. The runtime marks it abstract and sealed, which is what is checked.
- Interfaces, structs, enums, delegates and non-static classes all fail, so narrow the scan to the types that should be static.
- The `IsStatic` type filter checks the same thing, so `.Where(t => !t.IsStatic)` leaves static classes out of a scan.
