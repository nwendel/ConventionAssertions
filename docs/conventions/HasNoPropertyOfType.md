# HasNoPropertyOfType

Checks that a type has no public property of a given type, for example that domain types don't hold a `DbContext` or an `HttpClient`.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `PropertyType` | yes | The type that properties must not have. |

## Example

```csharp
Convention.For(
    s => s
        .FromAssemblyContaining<Order>()
        .Where(t => t.IsInSameNamespaceAs<Order>()),
    a => a.Assert<HasNoPropertyOfType>(c => c.PropertyType = typeof(DbContext)));
```

## Failure message

One line for each offending property:

```text
Type MyApp.Domain.Order must not have property Db of type Microsoft.EntityFrameworkCore.DbContext.
```

## Notes

- A property matches when its type can be assigned to `PropertyType`, so a property of a class derived from `DbContext` also fails.
- Only public properties are checked, including inherited ones.
- Leaving `PropertyType` unset throws `ConventionNotConfiguredException`.
