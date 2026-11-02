# HasNoPublicConstructors

Checks that a type has no public constructor, for example so value objects can only be created through a factory method such as `Parse` or `Create`.

- **Package:** ConventionAssertions
- **Namespace:** `ConventionAssertions.Conventions`
- **Targets:** types

## Example

```csharp
Convention.For(
    s => s
        .FromAssemblyContaining<OrderId>()
        .Where(t => t.IsAssignableTo<IValueObject>() && t.IsClass),
    a => a.Assert<HasNoPublicConstructors>());
```

## Failure message

```text
Type MyApp.Domain.OrderId must not have a public constructor.
```

## Notes

- Only public instance constructors count. Static constructors and `private`, `protected` and `internal` constructors are allowed.
- A class with no constructor in source gets a public parameterless one from the compiler, so it fails. Declare a `private` constructor to pass.
- Structs always have a public parameterless constructor in C#, but reflection doesn't report it unless you declare one, so a struct with only non-public constructors passes.
