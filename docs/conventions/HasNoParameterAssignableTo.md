# HasNoParameterAssignableTo

Checks that a method has no parameter assignable to a given type, for example that query methods on a service don't take the parameter that marks a method as mutating.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** methods
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `ParameterType` | yes | The type that parameters must not be assignable to. |

## Example

Query methods, named `Get*` or `Find*`, must not take an `Actor`:

```csharp
Convention.For(
    ConventionTargets.FromMethods(
        a => a.FromAssemblyContaining<IOrderService>(),
        t => t.Where(x => x.IsInterface),
        m => m.Where(x => x.HasAnyNamePrefix("Get", "Find"))),
    a => a.Assert<HasNoParameterAssignableTo>(c => c.ParameterType = typeof(Actor)));
```

## Failure message

One line for each offending parameter:

```text
Method MyApp.Orders.IOrderService.GetById(System.Guid,MyApp.Actor) must not have parameter actor assignable to MyApp.Actor.
```

## Notes

- A parameter matches when its type can be assigned to `ParameterType`, so a parameter of a class derived from `ParameterType` also fails.
- `ref`, `out` and `in` parameters are checked by their underlying type, so `ref Actor` fails too.
- Leaving `ParameterType` unset throws `ConventionNotConfiguredException`.
- The matching check for properties on a type is [HasNoPropertyAssignableTo](HasNoPropertyAssignableTo.md).
- The positive counterpart, requiring a parameter assignable to a type, is [HasParameterAssignableTo](HasParameterAssignableTo.md).
