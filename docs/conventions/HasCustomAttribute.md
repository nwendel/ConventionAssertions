# HasCustomAttribute

Checks that a type, method or property has a given attribute.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types, methods, properties
- **Configurable:** yes

## Settings

| Setting | Required | Description |
| --- | --- | --- |
| `CustomAttribute` | yes | The attribute type to require, such as `typeof(TypeConverterAttribute)`. |

## Example

```csharp
Convention.For(
    s => s
        .FromAssemblyContaining<OrderId>()
        .Where(t => t.IsAssignableTo<IValueObject>()),
    a => a.Assert<HasCustomAttribute>(c => c.CustomAttribute = typeof(TypeConverterAttribute)));
```

## Failure message

```text
Type MyApp.Domain.OrderId must have custom attribute System.ComponentModel.TypeConverterAttribute.
```

## Notes

- The attribute type must match exactly. An attribute derived from `CustomAttribute` doesn't count.
- Leaving `CustomAttribute` unset throws `ConventionNotConfiguredException`.
