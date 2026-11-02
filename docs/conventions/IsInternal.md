# IsInternal

Checks that a type is internal, for example to keep implementation classes out of an assembly's public API.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types

## Example

```csharp
Convention.For(
    ConventionTargets.FromTypes(s => s
        .FromDefaultDependencyContext(a => a.HasNameSegment("MyApp"))
        .Where(t => t.IsConcrete && t.IsAssignableTo<ICommandHandler>())),
    a => a.Assert<IsInternal>());
```

## Failure message

```text
Type MyApp.Orders.PlaceOrderHandler must be internal.
```

## Notes

- A top-level type must be `internal`. A nested type must be `internal` itself. Nested `private`, `protected` and `private protected` types fail.
- The opposite check is [IsPublic](IsPublic.md).
