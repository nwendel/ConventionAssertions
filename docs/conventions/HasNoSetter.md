# HasNoSetter

Checks that a property has no public setter, for example to keep value objects immutable.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** properties

## Example

```csharp
Convention.For(
    ConventionTargets.FromProperties(
        a => a.FromAssemblyContaining<OrderId>(),
        t => t.Where(x => x.IsAssignableTo<IValueObject>())),
    a => a.Assert<HasNoSetter>());
```

## Failure message

```text
Property MyApp.Domain.OrderId.Value must not have a setter.
```

## Notes

- Only public setters count. `private set`, `protected set` and `internal set` pass.
- An `init` accessor is a public setter to reflection, so `{ get; init; }` fails.
