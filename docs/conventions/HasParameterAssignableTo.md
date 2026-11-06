# HasParameterAssignableTo

Checks that a method has a parameter assignable to a given type, optionally an exact number of them, for example that mutating methods on a service take the parameter that identifies who is making the change.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** methods
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `ParameterType` | yes | The type that parameters must be assignable to. |
| `Count` | no | The exact number of parameters assignable to `ParameterType`. When unset, at least one is required. |

## Example

Mutating methods, those not named `Get*` or `Find*`, must take exactly one `Actor`:

```csharp
Convention.For(
    ConventionTargets.FromMethods(
        s => s
            .FromAssemblyContaining<IOrderService>()
            .Where(t => t.IsInterface),
        m => m.Where(x => !x.HasAnyNamePrefix("Get", "Find"))),
    a => a.Assert<HasParameterAssignableTo>(c =>
    {
        c.ParameterType = typeof(Actor);
        c.Count = 1;
    }));
```

## Failure message

Without `Count`, one line when no parameter matches:

```text
Method MyApp.Orders.IOrderService.Cancel(System.Guid) must have a parameter assignable to MyApp.Actor.
```

With `Count`, one line when the number of matching parameters differs:

```text
Method MyApp.Orders.IOrderService.Cancel(System.Guid) must have exactly 1 parameter assignable to MyApp.Actor, but has 0.
```

## Notes

- A parameter matches when its type can be assigned to `ParameterType`, so a parameter of a class derived from `ParameterType` counts too.
- `ref`, `out` and `in` parameters are checked by their underlying type, so `ref Actor` counts too.
- Leaving `ParameterType` unset throws `ConventionNotConfiguredException`.
- Setting `Count` to 0 or less throws `ConventionNotConfiguredException`; to require no parameter assignable to a type, use [HasNoParameterAssignableTo](HasNoParameterAssignableTo.md).
