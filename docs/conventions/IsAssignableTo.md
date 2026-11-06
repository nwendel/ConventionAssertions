# IsAssignableTo

Checks that a type is assignable to a given type, for example that every type in the domain namespace implements `IEntity`.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `Type` | yes | The type that targets must be assignable to: an interface, a base class, or an open generic type such as `typeof(IRepository<>)` or `typeof(EntityBase<>)`. |

## Example

Every concrete type in the domain namespace must implement `IEntity`:

```csharp
Convention.For(
    ConventionTargets.FromTypes(s => s
        .FromAssemblyContaining<MeetingTemplate>()
        .Where(t => t.IsConcrete && t.IsInSameNamespaceAs<MeetingTemplate>())),
    a => a.Assert<IsAssignableTo>(c => c.Type = typeof(IEntity)));
```

## Failure message

```text
Type MyApp.Meetings.Domain.MeetingNote must be assignable to MyApp.Meetings.Domain.IEntity.
```

## Notes

- Matching is the same as the `IsAssignableTo` type filter: a type passes when it implements the interface or derives from the class, directly or through a base class.
- An open generic interface matches any constructed form of it, so `typeof(IRepository<>)` matches a type implementing `IRepository<Order>`. An open generic class matches when it is anywhere among the base classes.
- Leaving `Type` unset throws `ConventionNotConfiguredException`.
